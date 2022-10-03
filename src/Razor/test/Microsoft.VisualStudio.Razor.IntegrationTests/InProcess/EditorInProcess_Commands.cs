﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Razor.IntegrationTests.InProcess;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Threading;
using static Microsoft.VisualStudio.VSConstants;

namespace Microsoft.VisualStudio.Extensibility.Testing
{
    internal partial class EditorInProcess
    {
        public async Task InvokeFormatDocumentAsync(CancellationToken cancellationToken)
        {
            var commandGuid = typeof(VSStd2KCmdID).GUID;
            var commandId = VSStd2KCmdID.FORMATDOCUMENT;
            await ExecuteCommandAsync(commandGuid, (uint)commandId, cancellationToken);
        }

        public async Task InvokeGoToDefinitionAsync(CancellationToken cancellationToken)
        {
            var commandGuid = typeof(VSStd97CmdID).GUID;
            var commandId = VSStd97CmdID.GotoDefn;
            await ExecuteCommandAsync(commandGuid, (uint)commandId, cancellationToken);
        }

        public async Task GoToImplementationAsync(CancellationToken cancellationToken)
        {
            await TestServices.Shell.ExecuteCommandAsync(WellKnownCommands.GoToImplementation, cancellationToken);
            await TestServices.Workspace.WaitForAsyncOperationsAsync(
                FeatureAttribute.Workspace,
                cancellationToken);
        }

        public async Task InvokeGoToImplementationAsync(CancellationToken cancellationToken)
        {
            var commandGuid = typeof(VSStd97CmdID).GUID;
            var commandId = VSStd97CmdID.GotoDecl;
            await ExecuteCommandAsync(commandGuid, (uint)commandId, cancellationToken);
        }

        public async Task CloseDocumentWindowAsync(CancellationToken cancellationToken)
        {
            var commandGuid = typeof(VSStd97CmdID).GUID;
            var commandId = VSStd97CmdID.CloseDocument;
            await ExecuteCommandAsync(commandGuid, (uint)commandId, cancellationToken);
        }

        private async Task ExecuteCommandAsync(Guid commandGuid, uint commandId, CancellationToken cancellationToken)
        {
            await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

            var dispatcher = await GetRequiredGlobalServiceAsync<SUIHostCommandDispatcher, IOleCommandTarget>(cancellationToken);
            ErrorHandler.ThrowOnFailure(dispatcher.Exec(commandGuid, commandId, (uint)OLECMDEXECOPT.OLECMDEXECOPT_DODEFAULT, IntPtr.Zero, IntPtr.Zero));
        }
    }
}
