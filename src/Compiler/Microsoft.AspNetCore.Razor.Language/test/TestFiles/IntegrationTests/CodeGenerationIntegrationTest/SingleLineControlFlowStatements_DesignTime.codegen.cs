﻿// <auto-generated/>
#pragma warning disable 1591
namespace Microsoft.AspNetCore.Razor.Language.IntegrationTests.TestFiles
{
    #line hidden
    public class TestFiles_IntegrationTests_CodeGenerationIntegrationTest_SingleLineControlFlowStatements_DesignTime
    {
        #pragma warning disable 219
        private void __RazorDirectiveTokenHelpers__() {
        }
        #pragma warning restore 219
        #pragma warning disable 0414
        private static object __o = null;
        #pragma warning restore 0414
        #pragma warning disable 1998
        public async System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 3 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/SingleLineControlFlowStatements.cshtml"
  
    if (DateTime.Now.ToBinary() % 2 == 0) 

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/SingleLineControlFlowStatements.cshtml"
                                      __o = "Current time is divisible by 2";

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/SingleLineControlFlowStatements.cshtml"
                                                                              else 

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/SingleLineControlFlowStatements.cshtml"
                                                                              __o = DateTime.Now;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/SingleLineControlFlowStatements.cshtml"
                                                                                                

    object Bar()
    {
        if (DateTime.Now.ToBinary() % 2 == 0)
            return "Current time is divisible by 2";
        else if (DateTime.Now.ToBinary() % 3 == 0)
            return "Current time is divisible by 3";
        else
            return DateTime.Now;
    }

    for (var i = 0; i < 10; i++)
        // Incrementing a number
        i--;

    

#line default
#line hidden
#nullable disable
#nullable restore
#line 20 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/SingleLineControlFlowStatements.cshtml"
     foreach (var item in new[] {"hello"})
        

#line default
#line hidden
#nullable disable
#nullable restore
#line 21 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/SingleLineControlFlowStatements.cshtml"
   __o = item;

#line default
#line hidden
#nullable disable
#nullable restore
#line 21 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/SingleLineControlFlowStatements.cshtml"
             

#line default
#line hidden
#nullable disable
#nullable restore
#line 21 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/SingleLineControlFlowStatements.cshtml"
             

    do
        

#line default
#line hidden
#nullable disable
#nullable restore
#line 24 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/SingleLineControlFlowStatements.cshtml"
   __o = currentCount;

#line default
#line hidden
#nullable disable
#nullable restore
#line 24 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/SingleLineControlFlowStatements.cshtml"
                     
    while (--currentCount >= 0);

    while (--currentCount <= 10)
        currentCount++;

    using (var reader = new System.IO.StreamReader("/something"))
        

#line default
#line hidden
#nullable disable
#nullable restore
#line 31 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/SingleLineControlFlowStatements.cshtml"
   __o = reader.ReadToEnd();

#line default
#line hidden
#nullable disable
#nullable restore
#line 31 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/SingleLineControlFlowStatements.cshtml"
                           

    

#line default
#line hidden
#nullable disable
#nullable restore
#line 33 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/SingleLineControlFlowStatements.cshtml"
     lock (this)
        currentCount++;

#line default
#line hidden
#nullable disable
#nullable restore
#line 34 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/SingleLineControlFlowStatements.cshtml"
                       

#line default
#line hidden
#nullable disable
#nullable restore
#line 77 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/SingleLineControlFlowStatements.cshtml"
 for (var i = 0; i < 10; i++)
    

#line default
#line hidden
#nullable disable
#nullable restore
#line 78 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/SingleLineControlFlowStatements.cshtml"
__o = i;

#line default
#line hidden
#nullable disable
#nullable restore
#line 78 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/SingleLineControlFlowStatements.cshtml"
      

#line default
#line hidden
#nullable disable
#nullable restore
#line 80 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/SingleLineControlFlowStatements.cshtml"
 foreach (var item in new[] {"hello"})
    

#line default
#line hidden
#nullable disable
#nullable restore
#line 81 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/SingleLineControlFlowStatements.cshtml"
__o = item;

#line default
#line hidden
#nullable disable
#nullable restore
#line 81 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/SingleLineControlFlowStatements.cshtml"
         

#line default
#line hidden
#nullable disable
#nullable restore
#line 83 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/SingleLineControlFlowStatements.cshtml"
 do
    

#line default
#line hidden
#nullable disable
#nullable restore
#line 84 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/SingleLineControlFlowStatements.cshtml"
__o = currentCount;

#line default
#line hidden
#nullable disable
#nullable restore
#line 84 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/SingleLineControlFlowStatements.cshtml"
                 
while (--currentCount >= 0);

#line default
#line hidden
#nullable disable
#nullable restore
#line 87 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/SingleLineControlFlowStatements.cshtml"
 while (--currentCount <= 10)
    currentCount++;

#line default
#line hidden
#nullable disable
#nullable restore
#line 90 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/SingleLineControlFlowStatements.cshtml"
 using (var reader = new System.IO.StreamReader("/something"))
    // Reading the entire file
    

#line default
#line hidden
#nullable disable
#nullable restore
#line 92 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/SingleLineControlFlowStatements.cshtml"
__o = reader.ReadToEnd();

#line default
#line hidden
#nullable disable
#nullable restore
#line 92 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/SingleLineControlFlowStatements.cshtml"
                       

#line default
#line hidden
#nullable disable
#nullable restore
#line 94 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/SingleLineControlFlowStatements.cshtml"
 lock (this)
    currentCount++;

#line default
#line hidden
#nullable disable
#nullable restore
#line 97 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/SingleLineControlFlowStatements.cshtml"
 if (true) 

#line default
#line hidden
#nullable disable
#nullable restore
#line 97 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/SingleLineControlFlowStatements.cshtml"
            @GitHubUserName <p>Hello!</p>


#line default
#line hidden
#nullable disable
#nullable restore
#line 99 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/SingleLineControlFlowStatements.cshtml"
 if (true) 
    

#line default
#line hidden
#nullable disable
#nullable restore
#line 100 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/SingleLineControlFlowStatements.cshtml"
                 __o = DateTime.Now;

#line default
#line hidden
#nullable disable
#nullable restore
#line 101 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/SingleLineControlFlowStatements.cshtml"

#line default
#line hidden
#nullable disable
        }
        #pragma warning restore 1998
#nullable restore
#line 37 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/SingleLineControlFlowStatements.cshtml"
            
    public string Foo()
    {
        var x = "";

        if (DateTime.Now.ToBinary() % 2 == 0)
            return "Current time is divisible by 2";
        else
            return "It isn't divisible by two";
        
        for (var i = 0; i < 10; i++)
            // Incrementing a number
            i--;

        foreach (var item in new[] {"hello"})
            

#line default
#line hidden
#nullable disable
#nullable restore
#line 52 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/SingleLineControlFlowStatements.cshtml"
       __o = item;

#line default
#line hidden
#nullable disable
#nullable restore
#line 52 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/SingleLineControlFlowStatements.cshtml"
                 

        do
            

#line default
#line hidden
#nullable disable
#nullable restore
#line 55 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/SingleLineControlFlowStatements.cshtml"
       __o = currentCount;

#line default
#line hidden
#nullable disable
#nullable restore
#line 55 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/SingleLineControlFlowStatements.cshtml"
                         
        while (--currentCount >= 0);

        while (--currentCount <= 10)
            currentCount++;

        using (var reader = new System.IO.StreamReader("/something"))
            

#line default
#line hidden
#nullable disable
#nullable restore
#line 62 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/SingleLineControlFlowStatements.cshtml"
       __o = reader.ReadToEnd();

#line default
#line hidden
#nullable disable
#nullable restore
#line 62 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/SingleLineControlFlowStatements.cshtml"
                               

        lock (this)
            currentCount++;
    }

    int currentCount = 0;

    public void IncrementCount()
    {
        if (true) currentCount++;
    }


#line default
#line hidden
#nullable disable
    }
}
#pragma warning restore 1591
