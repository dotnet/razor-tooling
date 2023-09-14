﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.Telemetry;
using Microsoft.VisualStudio.Telemetry.Razor;

#if DEBUG
using System.Linq;
#endif

namespace Microsoft.AspNetCore.Razor.Telemetry;

[Shared]
[Export(typeof(ITelemetryReporter))]
internal class TelemetryReporter : ITelemetryReporter
{
    private readonly ImmutableArray<TelemetrySession> _telemetrySessions;
    private readonly IEnumerable<IFaultExceptionHandler> _faultExceptionHandlers;
    private readonly ILogger? _logger;

    [ImportingConstructor]
    public TelemetryReporter(
        [Import(AllowDefault = true)] ILoggerFactory? loggerFactory = null,
        [ImportMany] IEnumerable<IFaultExceptionHandler>? faultExceptionHandlers = null)
    {
        // Get the DefaultSession for telemetry. This is set by VS with
        // TelemetryService.SetDefaultSession and provides the correct
        // appinsights keys etc
        _telemetrySessions = ImmutableArray.Create(TelemetryService.DefaultSession);
        _faultExceptionHandlers = faultExceptionHandlers ?? Array.Empty<IFaultExceptionHandler>();
        _logger = loggerFactory?.CreateLogger<TelemetryReporter>();
    }

    public void ReportEvent(string name, Severity severity)
    {
        var telemetryEvent = new TelemetryEvent(TelemetryHelpers.GetTelemetryName(name), TelemetryHelpers.ToTelemetrySeverity(severity));
        Report(telemetryEvent);
    }

    public void ReportEvent(string name, Severity severity, ImmutableDictionary<string, object?> values)
    {
        var telemetryEvent = new TelemetryEvent(TelemetryHelpers.GetTelemetryName(name), TelemetryHelpers.ToTelemetrySeverity(severity));
        foreach (var (propertyName, propertyValue) in values)
        {
            if (TelemetryHelpers.IsNumeric(propertyValue))
            {
                telemetryEvent.Properties.Add(TelemetryHelpers.GetPropertyName(propertyName), propertyValue);
            }
            else
            {
                telemetryEvent.Properties.Add(TelemetryHelpers.GetPropertyName(propertyName), new TelemetryComplexProperty(propertyValue));
            }
        }

        Report(telemetryEvent);
    }

    public void ReportFault(Exception exception, string? message, params object?[] @params)
    {
        try
        {
            if (exception is OperationCanceledException { InnerException: { } oceInnerException })
            {
                ReportFault(oceInnerException, message, @params);
                return;
            }

            if (exception is AggregateException aggregateException)
            {
                // We (potentially) have multiple exceptions; let's just report each of them
                foreach (var innerException in aggregateException.Flatten().InnerExceptions)
                {
                    ReportFault(innerException, message, @params);
                }

                return;
            }

            var handled = false;
            foreach (var handler in _faultExceptionHandlers)
            {
                if (handler.HandleException(this, exception, message, @params))
                {
                    // This behavior means that each handler still gets a chance
                    // to respond to the exception. There's no real reason for this other
                    // than best guess. When it was added, there was only one handler but
                    // it was intended to be easy to add more.
                    handled = true;
                }
            }

            if (handled)
            {
                return;
            }

            var currentProcess = Process.GetCurrentProcess();

            var faultEvent = new FaultEvent(
                eventName: TelemetryHelpers.GetTelemetryName("fault"),
                description: TelemetryHelpers.GetDescription(exception),
                FaultSeverity.General,
                exceptionObject: exception,
                gatherEventDetails: faultUtility =>
                {
                    foreach (var data in @params)
                    {
                        if (data is null)
                        {
                            continue;
                        }

                        faultUtility.AddErrorInformation(data.ToString());
                    }

                    // Returning "0" signals that, if sampled, we should send data to Watson.
                    // Any other value will cancel the Watson report. We never want to trigger a process dump manually,
                    // we'll let TargetedNotifications determine if a dump should be collected.
                    // See https://aka.ms/roslynnfwdocs for more details
                    return 0;
                });

            Report(faultEvent);
        }
        catch (Exception)
        {
        }
    }

    private void Report(TelemetryEvent telemetryEvent)
    {
        try
        {
#if !DEBUG
            foreach (var session in _telemetrySessions)
            {
                session.PostEvent(telemetryEvent);
            }
#else
            // In debug we only log to normal logging. This makes it much easier to add and debug telemetry events
            // before we're ready to send them to the cloud
            var name = telemetryEvent.Name;
            var propertyString = string.Join(",", telemetryEvent.Properties.Select(kvp => $"[ {kvp.Key}:{kvp.Value} ]"));
            _logger?.LogTrace("Telemetry Event: {name} \n Properties: {propertyString}\n", name, propertyString);

            if (telemetryEvent is FaultEvent)
            {
                var eventType = telemetryEvent.GetType();
                var description = eventType.GetProperty("Description", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(telemetryEvent, null);
                var exception = eventType.GetProperty("ExceptionObject", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(telemetryEvent, null);
                var message = $"Fault Event: {name} \n Exception Info: {exception ?? description} \n Properties: {propertyString}";

                Debug.Assert(true, message);
            }
#endif
        }
        catch (Exception e)
        {
            // No need to do anything here. We failed to report telemetry
            // which isn't good, but not catastrophic for a user
            _logger?.LogError(e, "Failed logging telemetry event");
        }
    }

    public IDisposable BeginBlock(string name, Severity severity)
    {
        return BeginBlock(name, severity, ImmutableDictionary<string, object?>.Empty);
    }

    public IDisposable BeginBlock(string name, Severity severity, ImmutableDictionary<string, object?> values)
    {
        return new TelemetryScope(this, name, severity, values.ToImmutableDictionary((tuple) => tuple.Key, (tuple) => (object?)tuple.Value));
    }

    public IDisposable TrackLspRequest(string lspMethodName, string languageServerName, Guid correlationId)
    {
        if (correlationId == Guid.Empty)
        {
            return NullTelemetryScope.Instance;
        }

        return BeginBlock("TrackLspRequest", Severity.Normal, ImmutableDictionary.CreateRange(new KeyValuePair<string, object?>[]
        {
            new("eventscope.method", lspMethodName),
            new("eventscope.languageservername", languageServerName),
            new("eventscope.correlationid", correlationId),
        }));
    }
}
