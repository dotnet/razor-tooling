﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Telemetry;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis.LanguageServer;
using Microsoft.VisualStudio.Editor.Razor.Test.Shared;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.VisualStudio.Telemetry;
using Microsoft.VisualStudio.Telemetry.Metrics;
using StreamJsonRpc;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.Razor.Telemetry;

public class TelemetryReporterTests(ITestOutputHelper testOutput) : ToolingTestBase(testOutput)
{
    [Fact]
    public void NoArgument()
    {
        using var reporter = new TestTelemetryReporter(LoggerFactory);
        reporter.ReportEvent("EventName", Severity.Normal);
        Assert.Collection(reporter.Events,
            e1 =>
            {
                Assert.Equal(TelemetrySeverity.Normal, e1.Severity);
                Assert.Equal("dotnet/razor/eventname", e1.Name);
                Assert.False(e1.HasProperties);
            });
    }

    [Fact]
    public void OneArgument()
    {
        using var reporter = new TestTelemetryReporter(LoggerFactory);
        reporter.ReportEvent("EventName", Severity.Normal, new Property("P1", false));
        Assert.Collection(reporter.Events,
            e1 =>
            {
                Assert.Equal(TelemetrySeverity.Normal, e1.Severity);
                Assert.Equal("dotnet/razor/eventname", e1.Name);
                Assert.True(e1.HasProperties);
                Assert.Single(e1.Properties);

                Assert.Equal(false, e1.Properties["dotnet.razor.p1"]);
            });
    }

    [Fact]
    public void TwoArguments()
    {
        using var reporter = new TestTelemetryReporter(LoggerFactory);
        reporter.ReportEvent("EventName", Severity.Normal, new("P1", false), new("P2", "test"));
        Assert.Collection(reporter.Events,
            e1 =>
            {
                Assert.Equal(TelemetrySeverity.Normal, e1.Severity);
                Assert.Equal("dotnet/razor/eventname", e1.Name);
                Assert.True(e1.HasProperties);

                Assert.Equal(false, e1.Properties["dotnet.razor.p1"]);
                Assert.Equal("test", e1.Properties["dotnet.razor.p2"]);
            });
    }

    [Fact]
    public void ThreeArguments()
    {
        using var reporter = new TestTelemetryReporter(LoggerFactory);
        var p3Value = Guid.NewGuid();
        reporter.ReportEvent("EventName",
            Severity.Normal,
            new("P1", false),
            new("P2", "test"),
            new("P3", p3Value));

        Assert.Collection(reporter.Events,
            e1 =>
            {
                Assert.Equal(TelemetrySeverity.Normal, e1.Severity);
                Assert.Equal("dotnet/razor/eventname", e1.Name);
                Assert.True(e1.HasProperties);

                Assert.Equal(false, e1.Properties["dotnet.razor.p1"]);
                Assert.Equal("test", e1.Properties["dotnet.razor.p2"]);

                var p3 = e1.Properties["dotnet.razor.p3"] as TelemetryComplexProperty;
                Assert.NotNull(p3);
                Assert.Equal(p3Value, p3.Value);
            });
    }

    [Fact]
    public void FourArguments()
    {
        using var reporter = new TestTelemetryReporter(LoggerFactory);
        var p3Value = Guid.NewGuid();
        reporter.ReportEvent("EventName",
            Severity.Normal,
            new("P1", false),
            new("P2", "test"),
            new("P3", p3Value),
            new("P4", 100));

        Assert.Collection(reporter.Events,
            e1 =>
            {
                Assert.Equal(TelemetrySeverity.Normal, e1.Severity);
                Assert.Equal("dotnet/razor/eventname", e1.Name);
                Assert.True(e1.HasProperties);

                Assert.Equal(false, e1.Properties["dotnet.razor.p1"]);
                Assert.Equal("test", e1.Properties["dotnet.razor.p2"]);

                var p3 = e1.Properties["dotnet.razor.p3"] as TelemetryComplexProperty;
                Assert.NotNull(p3);
                Assert.Equal(p3Value, p3.Value);

                Assert.Equal(100, e1.Properties["dotnet.razor.p4"]);
            });
    }

    [Fact]
    public void Block_NoArguments()
    {
        using var reporter = new TestTelemetryReporter(LoggerFactory);
        using (var scope = reporter.BeginBlock("EventName", Severity.Normal))
        {
        }

        Assert.Collection(reporter.Events,
            e1 =>
            {
                Assert.Equal("dotnet/razor/eventname", e1.Name);
                Assert.Equal(TelemetrySeverity.Normal, e1.Severity);
                Assert.True(e1.HasProperties);

                Assert.True(e1.Properties["dotnet.razor.eventscope.ellapsedms"] is long);
            });
    }

    [Fact]
    public void Block_OneArgument()
    {
        using var reporter = new TestTelemetryReporter(LoggerFactory);
        using (reporter.BeginBlock("EventName", Severity.Normal, new Property("P1", false)))
        {
        }

        Assert.Collection(reporter.Events,
            e1 =>
            {
                Assert.Equal(TelemetrySeverity.Normal, e1.Severity);
                Assert.Equal("dotnet/razor/eventname", e1.Name);
                Assert.True(e1.HasProperties);

                Assert.True(e1.Properties["dotnet.razor.eventscope.ellapsedms"] is long);
                Assert.Equal(false, e1.Properties["dotnet.razor.p1"]);
            });
    }

    [Fact]
    public void Block_TwoArguments()
    {
        using var reporter = new TestTelemetryReporter(LoggerFactory);
        using (reporter.BeginBlock("EventName", Severity.Normal, TimeSpan.Zero, new("P1", false), new("P2", "test")))
        {
        }

        Assert.Collection(reporter.Events,
            e1 =>
            {
                Assert.Equal(TelemetrySeverity.Normal, e1.Severity);
                Assert.Equal("dotnet/razor/eventname", e1.Name);
                Assert.True(e1.HasProperties);

                Assert.True(e1.Properties["dotnet.razor.eventscope.ellapsedms"] is long);
                Assert.Equal(false, e1.Properties["dotnet.razor.p1"]);
                Assert.Equal("test", e1.Properties["dotnet.razor.p2"]);
            });
    }

    [Fact]
    public void Block_ThreeArguments()
    {
        using var reporter = new TestTelemetryReporter(LoggerFactory);
        var p3Value = Guid.NewGuid();
        using (reporter.BeginBlock("EventName",
            Severity.Normal,
            TimeSpan.Zero,
            new("P1", false),
            new("P2", "test"),
            new("P3", p3Value)))
        {
        }

        Assert.Collection(reporter.Events,
            e1 =>
            {
                Assert.Equal(TelemetrySeverity.Normal, e1.Severity);
                Assert.Equal("dotnet/razor/eventname", e1.Name);
                Assert.True(e1.HasProperties);

                Assert.True(e1.Properties["dotnet.razor.eventscope.ellapsedms"] is long);
                Assert.Equal(false, e1.Properties["dotnet.razor.p1"]);
                Assert.Equal("test", e1.Properties["dotnet.razor.p2"]);

                var p3 = e1.Properties["dotnet.razor.p3"] as TelemetryComplexProperty;
                Assert.NotNull(p3);
                Assert.Equal(p3Value, p3.Value);
            });
    }

    [Fact]
    public void Block_FourArguments()
    {
        using var reporter = new TestTelemetryReporter(LoggerFactory);
        var p3Value = Guid.NewGuid();
        using (reporter.BeginBlock("EventName",
            Severity.Normal,
            TimeSpan.Zero,
            new("P1", false),
            new("P2", "test"),
            new("P3", p3Value),
            new("P4", 100)))
        {
        }

        Assert.Collection(reporter.Events,
            e1 =>
            {
                Assert.Equal(TelemetrySeverity.Normal, e1.Severity);
                Assert.Equal("dotnet/razor/eventname", e1.Name);
                Assert.True(e1.HasProperties);

                Assert.True(e1.Properties["dotnet.razor.eventscope.ellapsedms"] is long);
                Assert.Equal(false, e1.Properties["dotnet.razor.p1"]);
                Assert.Equal("test", e1.Properties["dotnet.razor.p2"]);

                var p3 = e1.Properties["dotnet.razor.p3"] as TelemetryComplexProperty;
                Assert.NotNull(p3);
                Assert.Equal(p3Value, p3.Value);

                Assert.Equal(100, e1.Properties["dotnet.razor.p4"]);
            });
    }

    [Fact]
    public void HandleRIEWithInnerException()
    {
        using var reporter = new TestTelemetryReporter(LoggerFactory);

        var ae = new ApplicationException("expectedText");
        var rie = new RemoteInvocationException("a", 0, ae);

        reporter.ReportFault(rie, rie.Message);

        Assert.Collection(reporter.Events,
            e1 =>
            {
                Assert.Equal(TelemetrySeverity.High, e1.Severity);
                Assert.Equal("dotnet/razor/fault", e1.Name);
                // faultEvent doesn't expose any interesting properties,
                // like the ExceptionObject, or the resulting Description,
                // or really anything we would explicitly want to verify against.
                Assert.IsType<FaultEvent>(e1);
            });
    }

    [Fact]
    public void HandleRIEWithNoInnerException()
    {
        using var reporter = new TestTelemetryReporter(LoggerFactory);

        var rie = new RemoteInvocationException("a", 0, errorData: null);

        reporter.ReportFault(rie, rie.Message);

        Assert.Collection(reporter.Events,
            e1 =>
            {
                Assert.Equal(TelemetrySeverity.High, e1.Severity);
                Assert.Equal("dotnet/razor/fault", e1.Name);
                // faultEvent doesn't expose any interesting properties,
                // like the ExceptionObject, or the resulting Description,
                // or really anything we would explicitly want to verify against.
                Assert.IsType<FaultEvent>(e1);
            });
    }

    [Fact]
    public void TrackLspRequest()
    {
        using var reporter = new TestTelemetryReporter(LoggerFactory);
        var correlationId = Guid.NewGuid();
        using (reporter.TrackLspRequest("MethodName", "ServerName", TimeSpan.Zero, correlationId))
        {
        }

        Assert.Collection(reporter.Events,
            e1 =>
            {
                Assert.Equal(TelemetrySeverity.Normal, e1.Severity);
                Assert.Equal("dotnet/razor/tracklsprequest", e1.Name);
                Assert.True(e1.HasProperties);

                Assert.True(e1.Properties["dotnet.razor.eventscope.ellapsedms"] is long);
                Assert.Equal("MethodName", e1.Properties["dotnet.razor.eventscope.method"]);
                Assert.Equal("ServerName", e1.Properties["dotnet.razor.eventscope.languageservername"]);

                var correlationProperty = e1.Properties["dotnet.razor.eventscope.correlationid"] as TelemetryComplexProperty;
                Assert.NotNull(correlationProperty);
                Assert.Equal(correlationId, correlationProperty.Value);
            });
    }

    [Fact]
    public void ReportFault_OperationCanceledExceptionWithoutInnerException_SkipsFaultReport()
    {
        // Arrange
        using var reporter = new TestTelemetryReporter(LoggerFactory);
        var exception = new OperationCanceledException("OCE", innerException: null);

        // Act
        reporter.ReportFault(exception, "Test message");

        // Assert
        Assert.Empty(reporter.Events);
    }

    [Fact]
    public void ReportFault_TaskCanceledExceptionWithoutInnerException_SkipsFaultReport()
    {
        // Arrange
        using var reporter = new TestTelemetryReporter(LoggerFactory);
        var exception = new TaskCanceledException("TCE", innerException: null);

        // Act
        reporter.ReportFault(exception, "Test message");

        // Assert
        Assert.Empty(reporter.Events);
    }

    [Fact]
    public void ReportFault_InnerExceptionOfOCEIsNotAnOCE_ReportsFault()
    {
        // Arrange
        var depth = 3;
        using var reporter = new TestTelemetryReporter(LoggerFactory);
        var innerMostException = new Exception();
        var exception = new OperationCanceledException("Test", innerMostException);
        for (var i = 0; i < depth; i++)
        {
            exception = new OperationCanceledException("Test", exception);
        }

        // Act
        reporter.ReportFault(exception, "Test message");

        // Assert
        Assert.NotEmpty(reporter.Events);
    }

    [Fact]
    public void ReportFault_InnerMostExceptionIsOperationCanceledException_SkipsFaultReport()
    {
        // Arrange
        var depth = 3;
        using var reporter = new TestTelemetryReporter(LoggerFactory);
        var innerMostException = new OperationCanceledException();
        var exception = new OperationCanceledException("Test", innerMostException);
        for (var i = 0; i < depth; i++)
        {
            exception = new OperationCanceledException("Test", exception);
        }

        // Act
        reporter.ReportFault(exception, "Test message");

        // Assert
        Assert.Empty(reporter.Events);
    }

    [Fact]
    public void ReportHistogram()
    {
        // Arrange
        var reporter = new TestTelemetryReporter(LoggerFactory);

        // Act
        reporter.ReportRequestTiming(
            Methods.TextDocumentCodeActionName,
            WellKnownLspServerKinds.RazorLspServer.GetContractName(),
            TimeSpan.FromMilliseconds(100),
            TimeSpan.FromMilliseconds(100),
            AspNetCore.Razor.Telemetry.TelemetryResult.Succeeded);

        reporter.ReportRequestTiming(
            Methods.TextDocumentCodeActionName,
            WellKnownLspServerKinds.RazorLspServer.GetContractName(),
            TimeSpan.FromMilliseconds(200),
            TimeSpan.FromMilliseconds(200),
            AspNetCore.Razor.Telemetry.TelemetryResult.Cancelled);

        reporter.ReportRequestTiming(
            Methods.TextDocumentCodeActionName,
            WellKnownLspServerKinds.RazorLspServer.GetContractName(),
            TimeSpan.FromMilliseconds(300),
            TimeSpan.FromMilliseconds(300),
            AspNetCore.Razor.Telemetry.TelemetryResult.Failed);

        reporter.ReportRequestTiming(
            Methods.TextDocumentCompletionName,
             WellKnownLspServerKinds.RazorLspServer.GetContractName(),
            TimeSpan.FromMilliseconds(100),
            TimeSpan.FromMilliseconds(100),
            AspNetCore.Razor.Telemetry.TelemetryResult.Succeeded);

        reporter.ReportRequestTiming(
            Methods.TextDocumentCompletionName,
             WellKnownLspServerKinds.RazorLspServer.GetContractName(),
            TimeSpan.FromMilliseconds(200),
            TimeSpan.FromMilliseconds(200),
            AspNetCore.Razor.Telemetry.TelemetryResult.Cancelled);

        reporter.ReportRequestTiming(
            Methods.TextDocumentCompletionName,
             WellKnownLspServerKinds.RazorLspServer.GetContractName(),
            TimeSpan.FromMilliseconds(300),
            TimeSpan.FromMilliseconds(300),
            AspNetCore.Razor.Telemetry.TelemetryResult.Failed);

        reporter.Dispose();

        // Assert
        reporter.AssertMetrics(
            static evt =>
            {
                var histogram = Assert.IsAssignableFrom<IHistogram<long>>(evt.Instrument);
                Assert.Equal("TimeInQueue", histogram.Name);

                var telemetryEvent = evt.Event;
                Assert.Equal("dotnet/razor/lsp_timeinqueue", telemetryEvent.Name);
                Assert.Collection(telemetryEvent.Properties,
                    static prop =>
                    {
                        Assert.Equal("dotnet.razor.method", prop.Key);
                        Assert.Equal(Methods.TextDocumentCodeActionName, prop.Value);
                    });
            },
            static evt =>
            {
                var histogram = Assert.IsAssignableFrom<IHistogram<long>>(evt.Instrument);
                Assert.Equal(Methods.TextDocumentCodeActionName, histogram.Name);

                var telemetryEvent = evt.Event;
                Assert.Equal("dotnet/razor/lsp_requestduration", telemetryEvent.Name);
                Assert.Collection(telemetryEvent.Properties,
                    static prop =>
                    {
                        Assert.Equal("dotnet.razor.method", prop.Key);
                        Assert.Equal(Methods.TextDocumentCodeActionName, prop.Value);
                    });
            },
            static evt =>
            {
                var histogram = Assert.IsAssignableFrom<IHistogram<long>>(evt.Instrument);
                Assert.Equal(Methods.TextDocumentCompletionName, histogram.Name);

                var telemetryEvent = evt.Event;
                Assert.Equal("dotnet/razor/lsp_requestduration", telemetryEvent.Name);
                Assert.Collection(telemetryEvent.Properties,
                    static prop =>
                    {
                        Assert.Equal("dotnet.razor.method", prop.Key);
                        Assert.Equal(Methods.TextDocumentCompletionName, prop.Value);
                    });
            });

        Assert.Collection(reporter.Events,
            static evt =>
            {
                Assert.Equal("dotnet/razor/lsp_requestcounter", evt.Name);
                Assert.Collection(evt.Properties,
                    static prop =>
                    {
                        Assert.Equal("dotnet.razor.method", prop.Key);
                        Assert.Equal(Methods.TextDocumentCodeActionName, prop.Value);
                    },
                    static prop =>
                    {
                        Assert.Equal("dotnet.razor.successful", prop.Key);
                        Assert.Equal(1, prop.Value);
                    },
                    static prop =>
                    {
                        Assert.Equal("dotnet.razor.failed", prop.Key);
                        Assert.Equal(1, prop.Value);
                    },
                    static prop =>
                    {
                        Assert.Equal("dotnet.razor.cancelled", prop.Key);
                        Assert.Equal(1, prop.Value);
                    });
            },
            static evt =>
            {
                Assert.Equal("dotnet/razor/lsp_requestcounter", evt.Name);
                Assert.Collection(evt.Properties,
                    static prop =>
                    {
                        Assert.Equal("dotnet.razor.method", prop.Key);
                        Assert.Equal(Methods.TextDocumentCompletionName, prop.Value);
                    },
                    static prop =>
                    {
                        Assert.Equal("dotnet.razor.successful", prop.Key);
                        Assert.Equal(1, prop.Value);
                    },
                    static prop =>
                    {
                        Assert.Equal("dotnet.razor.failed", prop.Key);
                        Assert.Equal(1, prop.Value);
                    },
                    static prop =>
                    {
                        Assert.Equal("dotnet.razor.cancelled", prop.Key);
                        Assert.Equal(1, prop.Value);
                    });
            });
    }

    [Theory, MemberData(nameof(s_throwFunctions))]
    public void GetModifiedFaultParameters_FiltersCorrectly(Func<object> throwAction)
    {
        try
        {
            throwAction();
        }
        catch (Exception ex)
        {
            var (param1, param2) = TestTelemetryReporter.GetModifiedFaultParameters(ex);

            Assert.Equal("Microsoft.VisualStudio.LanguageServices.Razor.Test", param1);
            Assert.NotNull(param2);

            // Depending on debug/release the stack can contain a constructor or
            // a call to this method. We expect one or the other and both
            // are valid
#if DEBUG
            Assert.StartsWith("<.cctor>", param2);
#else 
            Assert.Equal("GetModifiedFaultParameters_FiltersCorrectly", param2);
#endif
        }
    }

    public static readonly IEnumerable<object[]> s_throwFunctions = [
        [() => ((object?)null).AssumeNotNull()]
    ];
}
