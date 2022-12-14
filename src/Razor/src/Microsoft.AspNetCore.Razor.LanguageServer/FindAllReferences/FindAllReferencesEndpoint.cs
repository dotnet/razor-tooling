﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.LanguageServer.Extensions;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.Editor.Razor;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.VisualStudio.Text.Adornments;

namespace Microsoft.AspNetCore.Razor.LanguageServer.FindAllReferences
{
    internal class FindAllReferencesEndpoint : AbstractRazorDelegatingEndpoint<ReferenceParamsBridge, VSInternalReferenceItem[]>, IVSFindAllReferencesEndpoint
    {
        private readonly LanguageServerFeatureOptions _featureOptions;
        private readonly RazorDocumentMappingService _documentMappingService;

        public FindAllReferencesEndpoint(
            LanguageServerFeatureOptions languageServerFeatureOptions,
            RazorDocumentMappingService documentMappingService,
            ClientNotifierServiceBase languageServer,
            ILoggerFactory loggerFactory,
            LanguageServerFeatureOptions featureOptions,
            TagHelperFactsService tagHelperFactsService,
            HtmlFactsService htmlFactsService)
            : base(languageServerFeatureOptions, documentMappingService, languageServer, loggerFactory.CreateLogger<FindAllReferencesEndpoint>())
        {
            _featureOptions = featureOptions;
            _documentMappingService = documentMappingService ?? throw new ArgumentNullException(nameof(documentMappingService));
        }

        public RegistrationExtensionResult GetRegistration(VSInternalClientCapabilities clientCapabilities)
        {
            const string AssociatedServerCapability = "referencesProvider";

            var registrationOptions = new ReferenceOptions()
            {
                // TODO: GH issue link to support progress 
                WorkDoneProgress = false,
            };

            return new RegistrationExtensionResult(AssociatedServerCapability, new SumType<bool, ReferenceOptions>(registrationOptions));
        }

        protected override string CustomMessageTarget => RazorLanguageServerCustomMessageTargets.RazorReferencesEndpointName;

        protected override Task<IDelegatedParams?> CreateDelegatedParamsAsync(ReferenceParamsBridge request, RazorRequestContext requestContext, Projection projection, CancellationToken cancellationToken)
        {
            var documentContext = requestContext.GetRequiredDocumentContext();
            return Task.FromResult<IDelegatedParams?>(new DelegatedPositionParams(
                    documentContext.Identifier,
                    projection.Position,
                    projection.LanguageKind));
        }

        protected override async Task<VSInternalReferenceItem[]> HandleDelegatedResponseAsync(VSInternalReferenceItem[] delegatedResponse, ReferenceParamsBridge originalRequest, RazorRequestContext requestContext, Projection projection, CancellationToken cancellationToken)
        {
            var remappedLocations = new List<VSInternalReferenceItem>();

            foreach (var referenceItem in delegatedResponse)
            {
                if (referenceItem?.Location is null || referenceItem.Text is null)
                {
                    continue;
                }

                // Temporary fix for codebehind leaking through
                // Revert when https://github.com/dotnet/aspnetcore/issues/22512 is resolved
                referenceItem.DefinitionText = FilterReferenceDisplayText(referenceItem.DefinitionText);
                referenceItem.Text = FilterReferenceDisplayText(referenceItem.Text);

                // Indicates the reference item is directly available in the code
                referenceItem.Origin = VSInternalItemOrigin.Exact;

                if (!_featureOptions.IsVirtualCSharpFile(referenceItem.Location.Uri) &&
                    !_featureOptions.IsVirtualHtmlFile(referenceItem.Location.Uri))
                {
                    // This location doesn't point to a virtual file. No need to remap.
                    remappedLocations.Add(referenceItem);
                    continue;
                }

                var documentContext = requestContext.GetRequiredDocumentContext();
                var codeDocument = await documentContext.GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);

                if (!_documentMappingService.TryMapFromProjectedDocumentRange(codeDocument, referenceItem.Location.Range, out var documentRange))
                {
                    continue;
                }

                var razorDocumentUri = _featureOptions.GetRazorDocumentUri(referenceItem.Location.Uri);
                referenceItem.Location.Uri = razorDocumentUri;
                referenceItem.DisplayPath = razorDocumentUri.AbsolutePath;
                referenceItem.Location.Range = documentRange;

                remappedLocations.Add(referenceItem);
            }

            return remappedLocations.ToArray();
        }

        private static object? FilterReferenceDisplayText(object? referenceText)
        {
            const string CodeBehindObjectPrefix = "__o = ";
            const string CodeBehindBackingFieldSuffix = "k__BackingField";

            if (referenceText is string text)
            {
                if (text.StartsWith(CodeBehindObjectPrefix, StringComparison.Ordinal))
                {
                    return text
                        .Substring(CodeBehindObjectPrefix.Length, text.Length - CodeBehindObjectPrefix.Length - 1); // -1 for trailing `;`
                }

                return text.Replace(CodeBehindBackingFieldSuffix, string.Empty);
            }

            if (referenceText is ClassifiedTextElement textElement &&
                FilterReferenceClassifiedRuns(textElement.Runs.ToArray()))
            {
                var filteredRuns = textElement.Runs.Skip(4); // `__o`, ` `, `=`, ` `
                filteredRuns = filteredRuns.Take(filteredRuns.Count() - 1); // Trailing `;`
                return new ClassifiedTextElement(filteredRuns);
            }

            return referenceText;
        }

        private static bool FilterReferenceClassifiedRuns(IReadOnlyList<ClassifiedTextRun> runs)
        {
            if (runs.Count < 5)
            {
                return false;
            }

            return VerifyRunMatches(runs[0], "field name", "__o") &&
                VerifyRunMatches(runs[1], "text", " ") &&
                VerifyRunMatches(runs[2], "operator", "=") &&
                VerifyRunMatches(runs[3], "text", " ") &&
                VerifyRunMatches(runs[runs.Count - 1], "punctuation", ";");

            static bool VerifyRunMatches(ClassifiedTextRun run, string expectedClassificationType, string expectedText)
            {
                return run.ClassificationTypeName == expectedClassificationType &&
                    run.Text == expectedText;
            }
        }
    }
}
