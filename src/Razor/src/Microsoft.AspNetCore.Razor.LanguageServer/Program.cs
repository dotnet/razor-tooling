﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.Common.Serialization;
using Microsoft.AspNetCore.Razor.LanguageServer.Completion;
using Microsoft.AspNetCore.Razor.LanguageServer.Converters;
using Microsoft.AspNetCore.Razor.LanguageServer.Hover;
using Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.Completion;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.Editor.Razor;
using System.Linq;
using OmniSharp.Extensions.LanguageServer.Protocol.Serialization;
using OmniSharp.Extensions.LanguageServer.Server;
using OmniSharp.Extensions.JsonRpc.Serialization.Converters;

namespace Microsoft.AspNetCore.Razor.LanguageServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            MainAsync(args).Wait();
        }

        public static async Task MainAsync(string[] args)
        {
            var logLevel = LogLevel.Information;
            for (var i = 0; i < args.Length; i++)
            {
                if (args[i].IndexOf("debug", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    while (!Debugger.IsAttached)
                    {
                        Thread.Sleep(1000);
                    }

                    Debugger.Break();
                    continue;
                }

                if (args[i] == "--logLevel" && i + 1 < args.Length)
                {
                    var logLevelString = args[++i];
                    if (!Enum.TryParse(logLevelString, out logLevel))
                    {
                        logLevel = LogLevel.Information;
                        Console.WriteLine($"Invalid log level '{logLevelString}'. Defaulting to {logLevel.ToString()}.");
                    }
                }
            }

            // ClientResponseConverter will not serializer the "result" property in the case that it is null (which is legal in a couple spots)
            // For now we've written our own serializer which handler this scenario.
            var responseConverter = Serializer.Instance.JsonSerializer.Converters.FirstOrDefault(converter =>
            {
                return converter.GetType() == typeof(ClientResponseConverter);
            });

            if (responseConverter != null)
            {
                Serializer.Instance.Settings.Converters.Remove(responseConverter);
            }

            Serializer.Instance.Settings.Converters.Add(new ResponseRazorConverter());
            Serializer.Instance.JsonSerializer.Converters.RegisterRazorConverters();

            ILanguageServer server = null;
            server = await OmniSharp.Extensions.LanguageServer.Server.LanguageServer.From(options =>
                options
                    .WithInput(Console.OpenStandardInput())
                    .WithOutput(Console.OpenStandardOutput())
                    .ConfigureLogging(builder => builder
                        .AddLanguageServer()
                        .SetMinimumLevel(logLevel))
                    .OnInitialized(async (languageServer, request, response) =>
                    {
                        var fileChangeDetectorManager = languageServer.Services.GetRequiredService<RazorFileChangeDetectorManager>();
                        await fileChangeDetectorManager.InitializedAsync(languageServer);
                    })
                    .WithHandler<RazorDocumentSynchronizationEndpoint>()
                    .WithHandler<RazorCompletionEndpoint>()
                    .WithHandler<RazorHoverEndpoint>()
                    .WithHandler<RazorLanguageEndpoint>()
                    .WithHandler<RazorProjectEndpoint>()
                    .WithServices(services =>
                    {
                        services.AddSingleton<RemoteTextLoaderFactory, DefaultRemoteTextLoaderFactory>();
                        services.AddSingleton<ProjectResolver, DefaultProjectResolver>();
                        services.AddSingleton<DocumentResolver, DefaultDocumentResolver>();
                        services.AddSingleton<FilePathNormalizer>();
                        services.AddSingleton<RazorProjectService, DefaultRazorProjectService>();
                        services.AddSingleton<ProjectSnapshotChangeTrigger, BackgroundDocumentGenerator>();
                        services.AddSingleton<RazorFileChangeDetectorManager>();

                        // File change listeners
                        services.AddSingleton<IProjectConfigurationFileChangeListener, ProjectConfigurationStateSynchronizer>();
                        services.AddSingleton<IProjectFileChangeListener, ProjectFileSynchronizer>();

                        // File Change detectors
                        services.AddSingleton<IRazorFileChangeDetector, ProjectConfigurationFileChangeDetector>();
                        services.AddSingleton<IRazorFileChangeDetector, ProjectFileChangeDetector>();

                        // Document processed listeners
                        services.AddSingleton<DocumentProcessedListener, RazorDiagnosticsPublisher>();
                        services.AddSingleton<DocumentProcessedListener, UnsynchronizableContentDocumentProcessedListener>();

                        services.AddSingleton<HostDocumentFactory, DefaultHostDocumentFactory>();
                        services.AddSingleton<ProjectSnapshotManagerAccessor, DefaultProjectSnapshotManagerAccessor>();
                        services.AddSingleton<TagHelperFactsService, DefaultTagHelperFactsService>();
                        services.AddSingleton<VisualStudio.Editor.Razor.TagHelperCompletionService, VisualStudio.Editor.Razor.DefaultTagHelperCompletionService>();
                        services.AddSingleton<TagHelperDescriptionFactory, DefaultTagHelperDescriptionFactory>();

                        // Completion
                        services.AddSingleton<Completion.TagHelperCompletionService, Completion.DefaultTagHelperCompletionService>();
                        services.AddSingleton<RazorCompletionItemProvider, DirectiveCompletionItemProvider>();
                        services.AddSingleton<RazorCompletionItemProvider, DirectiveAttributeCompletionItemProvider>();
                        services.AddSingleton<RazorCompletionItemProvider, DirectiveAttributeParameterCompletionItemProvider>();
                        services.AddSingleton<RazorCompletionItemProvider, DirectiveAttributeTransitionCompletionItemProvider>();

                        var foregroundDispatcher = new VSCodeForegroundDispatcher();
                        services.AddSingleton<ForegroundDispatcher>(foregroundDispatcher);

                        var csharpPublisher = new DefaultCSharpPublisher(foregroundDispatcher, new Lazy<OmniSharp.Extensions.LanguageServer.Protocol.Server.ILanguageServer>(() => server));
                        services.AddSingleton<ProjectSnapshotChangeTrigger>(csharpPublisher);
                        services.AddSingleton<CSharpPublisher>(csharpPublisher);

                        services.AddSingleton<RazorCompletionFactsService, DefaultRazorCompletionFactsService>();
                        services.AddSingleton<RazorHoverInfoService, DefaultRazorHoverInfoService>();
                        services.AddSingleton<HtmlFactsService, DefaultHtmlFactsService>();
                        var documentVersionCache = new DefaultDocumentVersionCache(foregroundDispatcher);
                        services.AddSingleton<DocumentVersionCache>(documentVersionCache);
                        services.AddSingleton<ProjectSnapshotChangeTrigger>(documentVersionCache);
                        var containerStore = new DefaultGeneratedCodeContainerStore(
                            foregroundDispatcher,
                            documentVersionCache,
                            csharpPublisher);
                        services.AddSingleton<GeneratedCodeContainerStore>(containerStore);
                        services.AddSingleton<ProjectSnapshotChangeTrigger>(containerStore);
                    }));

            // Workaround for https://github.com/OmniSharp/csharp-language-server-protocol/issues/106
            var languageServer = (OmniSharp.Extensions.LanguageServer.Server.LanguageServer)server;

            try
            {
                var factory = new LoggerFactory();
                var logger = factory.CreateLogger<Program>();
                var assemblyInformationAttribute = typeof(Program).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();
                logger.LogInformation("Razor Language Server version " + assemblyInformationAttribute.InformationalVersion);
            }
            catch
            {
                // Swallow exceptions from determining assembly information.
            }

            await server.WaitForExit;

            TempDirectory.Instance.Dispose();
        }
    }
}
