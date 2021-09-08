﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Test.Common;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Formatting
{
    public class CodeDirectiveOnTypeFormattingTest : FormattingTestBase
    {
        public CodeDirectiveOnTypeFormattingTest(ITestOutputHelper output)
            : base(output)
        {
        }

        [Fact]
        public async Task CloseCurly_Class_SingleLineAsync()
        {
            await RunOnTypeFormattingTestAsync(
input: @"
@code {
 public class Foo{}$$
}
",
expected: @"
@code {
    public class Foo { }
}
", triggerCharacter: '}');
        }

        [Fact]
        public async Task CloseCurly_Class_MultiLineAsync()
        {
            await RunOnTypeFormattingTestAsync(
input: @"
@code {
 public class Foo{
}$$
}
",
expected: @"
@code {
    public class Foo
    {
    }
}
", triggerCharacter: '}');
        }

        [Fact]
        public async Task CloseCurly_Method_SingleLineAsync()
        {
            await RunOnTypeFormattingTestAsync(
input: @"
@code {
 public void Foo{}$$
}
",
expected: @"
@code {
    public void Foo { }
}
", triggerCharacter: '}');
        }

        [Fact]
        public async Task CloseCurly_Method_MultiLineAsync()
        {
            await RunOnTypeFormattingTestAsync(
input: @"
@code {
 public void Foo{
}$$
}
",
expected: @"
@code {
    public void Foo
    {
    }
}
", triggerCharacter: '}');
        }

        [Fact]
        public async Task CloseCurly_Property_SingleLineAsync()
        {
            await RunOnTypeFormattingTestAsync(
input: @"
@code {
 public string Foo{ get;set;}$$
}
",
expected: @"
@code {
    public string Foo { get; set; }
}
", triggerCharacter: '}');
        }

        [Fact]
        public async Task CloseCurly_Property_MultiLineAsync()
        {
            await RunOnTypeFormattingTestAsync(
input: @"
@code {
 public string Foo{
get;set;}$$
}
",
expected: @"
@code {
    public string Foo
    {
        get; set;
    }
}
", triggerCharacter: '}');
        }

        [Fact]
        public async Task CloseCurly_Property_StartOfBlockAsync()
        {
            await RunOnTypeFormattingTestAsync(
input: @"
@code { public string Foo{ get;set;}$$
}
",
expected: @"
@code {
    public string Foo { get; set; }
}
", triggerCharacter: '}');
        }

        [Fact]
        public async Task Semicolon_ClassField_SingleLineAsync()
        {
            await RunOnTypeFormattingTestAsync(
input: @"
@code {
 public class Foo {private int _hello = 0;$$}
}
",
expected: @"
@code {
    public class Foo { private int _hello = 0; }
}
", triggerCharacter: ';');
        }

        [Fact]
        public async Task Semicolon_ClassField_MultiLineAsync()
        {
            await RunOnTypeFormattingTestAsync(
input: @"
@code {
    public class Foo{
private int _hello = 0;$$ }
}
",
expected: @"
@code {
    public class Foo{
        private int _hello = 0; }
}
", triggerCharacter: ';');
        }

        [Fact]
        public async Task Semicolon_MethodVariableAsync()
        {
            await RunOnTypeFormattingTestAsync(
input: @"
@code {
    public void Foo()
    {
                            var hello = 0;$$
    }
}
",
expected: @"
@code {
    public void Foo()
    {
        var hello = 0;
    }
}
", triggerCharacter: ';');
        }

        [Fact]
        [WorkItem("https://github.com/dotnet/aspnetcore/issues/27135")]
        public async Task Semicolon_Fluent_CallAsync()
        {
            await RunOnTypeFormattingTestAsync(
input: @"@implements IDisposable

@code{
    protected override async Task OnInitializedAsync()
    {
        hubConnection = new HubConnectionBuilder()
            .WithUrl(NavigationManager.ToAbsoluteUri(""/chathub""))
            .Build();$$
    }
}
",
expected: @"@implements IDisposable

@code{
    protected override async Task OnInitializedAsync()
    {
        hubConnection = new HubConnectionBuilder()
            .WithUrl(NavigationManager.ToAbsoluteUri(""/chathub""))
            .Build();
    }
}
", triggerCharacter: ';');
        }

        [Fact]
        public async Task ClosingBrace_MatchesCSharpIndentationAsync()
        {
            await RunOnTypeFormattingTestAsync(
input: @"
@page ""/counter""

<h1>Counter</h1>

<p>Current count: @currentCount</p>

<button class=""btn btn-primary"" @onclick=""IncrementCount"">Click me</button>

@code {
    private int currentCount = 0;

    private void IncrementCount()
    {
        currentCount++;
        if (true){
            }$$
    }
}
",
expected: @"
@page ""/counter""

<h1>Counter</h1>

<p>Current count: @currentCount</p>

<button class=""btn btn-primary"" @onclick=""IncrementCount"">Click me</button>

@code {
    private int currentCount = 0;

    private void IncrementCount()
    {
        currentCount++;
        if (true)
        {
        }
    }
}
", triggerCharacter: '}');
        }

        [Fact]
        public async Task ClosingBrace_DoesntMatchCSharpIndentationAsync()
        {
            await RunOnTypeFormattingTestAsync(
input: @"
@page ""/counter""

<h1>Counter</h1>

<p>Current count: @currentCount</p>

<button class=""btn btn-primary"" @onclick=""IncrementCount"">Click me</button>

@code {
    private int currentCount = 0;

    private void IncrementCount()
    {
        currentCount++;
        if (true){
                }$$
    }
}
",
expected: @"
@page ""/counter""

<h1>Counter</h1>

<p>Current count: @currentCount</p>

<button class=""btn btn-primary"" @onclick=""IncrementCount"">Click me</button>

@code {
    private int currentCount = 0;

    private void IncrementCount()
    {
        currentCount++;
        if (true)
        {
        }
    }
}
", triggerCharacter: '}');
        }

        [Fact]
        [WorkItem("https://github.com/dotnet/aspnetcore/issues/27102")]
        public async Task CodeBlock_SemiColon_SingleLine1Async()
        {
            await RunOnTypeFormattingTestAsync(
input: @"
<div></div>
@{ Debugger.Launch();$$}
<div></div>
",
expected: @"
<div></div>
@{
    Debugger.Launch();
}
<div></div>
", triggerCharacter: ';');
        }

        [Fact]
        [WorkItem("https://github.com/dotnet/aspnetcore/issues/27102")]
        public async Task CodeBlock_SemiColon_SingleLine2Async()
        {
            await RunOnTypeFormattingTestAsync(
input: @"
<div></div>
@{     Debugger.Launch(   )     ;$$ }
<div></div>
",
expected: @"
<div></div>
@{
    Debugger.Launch(); 
}
<div></div>
", triggerCharacter: ';');
        }

        [Fact]
        [WorkItem("https://github.com/dotnet/aspnetcore/issues/27102")]
        public async Task CodeBlock_SemiColon_SingleLine3Async()
        {
            await RunOnTypeFormattingTestAsync(
input: @"
<div>
    @{     Debugger.Launch(   )     ;$$ }
</div>
",
expected: @"
<div>
    @{
        Debugger.Launch(); 
    }
</div>
", triggerCharacter: ';');
        }

        [Fact]
        [WorkItem("https://github.com/dotnet/aspnetcore/issues/27102")]
        public async Task CodeBlock_SemiColon_MultiLineAsync()
        {
            await RunOnTypeFormattingTestAsync(
input: @"
<div></div>
@{
    var abc = 123;$$
}
<div></div>
",
expected: @"
<div></div>
@{
    var abc = 123;
}
<div></div>
", triggerCharacter: ';');
        }
    }
}
