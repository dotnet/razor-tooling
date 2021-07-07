﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.Editor;

namespace Microsoft.VisualStudio.Editor.Razor
{
    [System.Composition.Shared]
    [Export(typeof(EditorSettingsManager))]
    internal class DefaultEditorSettingsManager : EditorSettingsManager
    {
        public override event EventHandler<EditorSettingsChangedEventArgs> Changed;

        private readonly object SettingsAccessorLock = new object();
        private readonly SingleThreadedDispatcher _singleThreadedDispatcher;
        private EditorSettings _settings;

        [ImportingConstructor]
        public DefaultEditorSettingsManager(SingleThreadedDispatcher singleThreadedDispatcher)
        {
            _singleThreadedDispatcher = singleThreadedDispatcher;
            _settings = EditorSettings.Default;
        }

        public override EditorSettings Current
        {
            get
            {
                lock (SettingsAccessorLock)
                {
                    return _settings;
                }
            }
        }

        public override void Update(EditorSettings updatedSettings)
        {
            if (updatedSettings == null)
            {
                throw new ArgumentNullException(nameof(updatedSettings));
            }

            _singleThreadedDispatcher.AssertDispatcherThread();

            lock (SettingsAccessorLock)
            {
                if (!_settings.Equals(updatedSettings))
                {
                    _settings = updatedSettings;
                    OnChanged();
                }
            }
        }

        private void OnChanged()
        {
            _singleThreadedDispatcher.AssertDispatcherThread();

            var args = new EditorSettingsChangedEventArgs(Current);
            Changed?.Invoke(this, args);
        }
    }
}
