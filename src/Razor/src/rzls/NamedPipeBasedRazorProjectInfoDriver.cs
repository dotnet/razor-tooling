﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.IO.Pipes;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Utilities;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.VisualStudio.Threading;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal sealed class NamedPipeBasedRazorProjectInfoDriver : AbstractRazorProjectInfoDriver, IConnectionBasedRazorProjectInfoDriver<string>
{
    private NamedPipeClientStream? _namedPipe;

    public NamedPipeBasedRazorProjectInfoDriver(ILoggerFactory loggerFactory) : base(loggerFactory)
    {
        StartInitialization();
    }

    public async Task ConnectAsync(string pipeName, CancellationToken cancellationToken)
    {
        Assumed.True(_namedPipe is null);
        Logger.LogTrace($"Connecting to named pipe {pipeName} on PID: {Process.GetCurrentProcess().Id}");

        _namedPipe = new NamedPipeClientStream(".", pipeName, PipeDirection.In, PipeOptions.CurrentUserOnly | PipeOptions.Asynchronous);
        await _namedPipe.ConnectAsync(cancellationToken).ConfigureAwait(false);

        // Don't block reading the stream on the caller of this
        ReadFromStreamAsync(CancellationToken.None).Forget();
    }

    protected override Task InitializeAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    protected override void OnDispose()
    {
        _namedPipe?.Dispose();
        _namedPipe = null;
    }

    private async Task ReadFromStreamAsync(CancellationToken cancellationToken = default)
    {
        Logger?.LogTrace($"Starting read from named pipe.");

        while (
            _namedPipe is not null &&
            _namedPipe.IsConnected
            && !cancellationToken.IsCancellationRequested)
        {

            try
            {
                switch (_namedPipe.ReadProjectInfoAction())
                {
                    case ProjectInfoAction.Remove:
                        Logger?.LogTrace($"Attempting to read project id for removal");
                        var id = await _namedPipe.ReadProjectInfoRemovalAsync(cancellationToken).ConfigureAwait(false);
                        EnqueueRemove(new ProjectKey(id));

                        break;

                    case ProjectInfoAction.Update:
                        Logger?.LogTrace($"Attempting to read project info for update");
                        var projectInfo = await _namedPipe.ReadProjectInfoAsync(cancellationToken).ConfigureAwait(false);
                        if (projectInfo is not null)
                        {
                            EnqueueUpdate(projectInfo);
                        }

                        break;

                    default:
                        throw Assumes.NotReachable();
                }
            }
            catch (Exception ex)
            {
                Logger?.LogError(ex, $"{ex.Message}");
            }
        }
    }
}
