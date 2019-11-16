/* --------------------------------------------------------------------------------------------
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License. See License.txt in the project root for license information.
 * ------------------------------------------------------------------------------------------ */

import * as vscode from 'vscode';
import { getRazorDocumentUri, isRazorCSharpFile } from './RazorConventions';
import { RazorDocumentManager } from './RazorDocumentManager';
import { RazorDocumentSynchronizer } from './RazorDocumentSynchronizer';
import { RazorLanguage } from './RazorLanguage';
import { RazorLanguageServiceClient } from './RazorLanguageServiceClient';
import { RazorLogger } from './RazorLogger';
import { RazorRenameProvider } from './RazorRenameProvider';
import { LanguageKind } from './RPC/LanguageKind';

export class RazorRenameFeature {
    public readonly serviceClient: RazorLanguageServiceClient;
    public readonly renameProvider: RazorRenameProvider;

    constructor(
        documentSynchronizer: RazorDocumentSynchronizer,
        documentManager: RazorDocumentManager,
        serviceClient: RazorLanguageServiceClient,
        logger: RazorLogger) {
            this.serviceClient = serviceClient;
            this.renameProvider = new RazorRenameProvider(
                documentSynchronizer,
                documentManager,
                serviceClient,
                logger);
    }

    public register() {
        const registrations = [
            vscode.languages.registerRenameProvider(RazorLanguage.id, this.renameProvider),
            vscode.commands.registerCommand(
                'razor.remapRenameEdit', async (
                    document: vscode.TextDocument,
                    position: vscode.Position,
                    newText: string,
                    cSharpEdit: vscode.WorkspaceEdit) => {
                    const remappedEdit = await this.remapRazorCSharpEdits(
                    cSharpEdit,
                    this.serviceClient);
                    return remappedEdit;
                }),
        ];

        return vscode.Disposable.from(...registrations);
    }

    // Given a set of C# edits, we need to map edits to Razor CSharp documents back to the original Razor documents.
    private async remapRazorCSharpEdits(
        cSharpEdit: vscode.WorkspaceEdit,
        serviceClient: RazorLanguageServiceClient) {

        const result = new vscode.WorkspaceEdit();

        // The returned edits will be for the projected C# documents. We now need to re-map that to the original document.
        for (const entry of cSharpEdit.entries()) {
            const uri = entry[0];
            const edits = entry[1];

            if (!isRazorCSharpFile(uri)) {
                // This edit happens outside of a Razor document. Let the edit go through as is.
                result.set(uri, edits);
                continue;
            }

            // We're now working with a Razor CSharp document.
            const documentUri = getRazorDocumentUri(uri);
            const newEdits = new Array<vscode.TextEdit>();

            // Re-map each edit to its position in the original Razor document.
            for (const edit of edits) {
                const remappedResponse = await serviceClient.mapToDocumentRange(
                    LanguageKind.CSharp,
                    edit.range,
                    documentUri);

                if (!remappedResponse) {
                    // Something went wrong when re-mapping to the original document. Accept the edit as is.
                    newEdits.push(edit);
                    continue;
                }

                const newEdit = new vscode.TextEdit(remappedResponse.range, edit.newText);
                newEdits.push(newEdit);
            }

            result.set(documentUri, newEdits);
        }

        return result;
    }
}
