/* --------------------------------------------------------------------------------------------
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License. See License.txt in the project root for license information.
 * ------------------------------------------------------------------------------------------ */

import * as vscode from 'vscode';
import { LanguageKind } from '../RPC/LanguageKind';

export class SemanticTokensEditRequest {
    public readonly razorDocumentUri: string;

    constructor(
        public readonly kind: LanguageKind,
        razorDocumentUri: vscode.Uri,
        public readonly previousResultId: string,
    ) {
        this.razorDocumentUri = razorDocumentUri.toString();
    }
}
