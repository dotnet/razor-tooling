﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.AspNetCore.Razor.Telemetry;
using Microsoft.CommonLanguageServerProtocol.Framework;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal class CLaSPTelemetryService(ITelemetryReporter telemetryReporter) : AbstractTelemetryService
{
    public override AbstractRequestScope CreateRequestScope(string lspMethodName)
        => new RequestTelemetryScope(lspMethodName, telemetryReporter);

    private class RequestTelemetryScope : AbstractRequestScope
    {
        private readonly Stopwatch _stopWatch;
        private readonly ITelemetryReporter _telemetryReporter;
        private TelemetryResult _result = TelemetryResult.Succeeded;
        private TimeSpan _queuedDuration;
        private Exception? _exception;

        public RequestTelemetryScope(string lspMethodName, ITelemetryReporter telemetryReporter) : base(lspMethodName)
        {
            _stopWatch = Stopwatch.StartNew();
            _telemetryReporter = telemetryReporter;
        }

        public override void Dispose()
        {
            var requestDuration = _stopWatch.Elapsed;
            _telemetryReporter.UpdateRequestTelemetry(Name, Language, _queuedDuration, requestDuration, _result, _exception);
        }

        public override void RecordCancellation()
        {
            _result = TelemetryResult.Canceled;
        }

        public override void RecordException(Exception exception)
        {
            _exception = exception;
            _result = TelemetryResult.Failed;
        }

        public override void RecordExecutionStart()
        {
            _queuedDuration = _stopWatch.Elapsed;
        }

        public override void RecordWarning(string message)
        {
            _exception = new Exception(message);
            _result = TelemetryResult.Failed;
        }
    }
}
