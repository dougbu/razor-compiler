// <auto-generated/>
#pragma warning disable 1591
namespace Microsoft.AspNetCore.Razor.Language.IntegrationTests.TestFiles
{
    #line hidden
    public class TestFiles_IntegrationTests_CodeGenerationIntegrationTest_Markup_InCodeBlocksWithTagHelper_DesignTime
    {
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        private global::DivTagHelper __DivTagHelper;
        #pragma warning disable 219
        private void __RazorDirectiveTokenHelpers__() {
        ((System.Action)(() => {
#nullable restore
#line 1 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/Markup_InCodeBlocksWithTagHelper.cshtml"
global::System.Object __typeHelper = "*, TestAssembly";

#line default
#line hidden
#nullable disable
        }
        ))();
        }
        #pragma warning restore 219
        #pragma warning disable 0414
        private static System.Object __o = null;
        #pragma warning restore 0414
        #pragma warning disable 1998
        public async System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 2 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/Markup_InCodeBlocksWithTagHelper.cshtml"
  
    var people = new Person[]
    {
        new Person() { Name = "Taylor", Age = 95, },
    };

    void PrintName(Person person)
    {
        

#line default
#line hidden
#nullable disable
#nullable restore
#line 10 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/Markup_InCodeBlocksWithTagHelper.cshtml"
        __o = person.Name;

#line default
#line hidden
#nullable disable
            __DivTagHelper = CreateTagHelper<global::DivTagHelper>();
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
#nullable restore
#line 10 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/Markup_InCodeBlocksWithTagHelper.cshtml"
                               
    }

#line default
#line hidden
#nullable disable
#nullable restore
#line 14 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/Markup_InCodeBlocksWithTagHelper.cshtml"
   PrintName(people[0]); 

#line default
#line hidden
#nullable disable
#nullable restore
#line 15 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/Markup_InCodeBlocksWithTagHelper.cshtml"
   await AnnounceBirthday(people[0]); 

#line default
#line hidden
#nullable disable
        }
        #pragma warning restore 1998
#nullable restore
#line 17 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/Markup_InCodeBlocksWithTagHelper.cshtml"
            
    Task AnnounceBirthday(Person person)
    {
        var formatted = $"Mr. {person.Name}";
        

#line default
#line hidden
#nullable disable
#nullable restore
#line 22 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/Markup_InCodeBlocksWithTagHelper.cshtml"
                           __o = formatted;

#line default
#line hidden
#nullable disable
        __DivTagHelper = CreateTagHelper<global::DivTagHelper>();
        await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
#nullable restore
#line 23 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/Markup_InCodeBlocksWithTagHelper.cshtml"
              

        

#line default
#line hidden
#nullable disable
#nullable restore
#line 26 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/Markup_InCodeBlocksWithTagHelper.cshtml"
         for (var i = 0; i < person.Age / 10; i++)
        {
            

#line default
#line hidden
#nullable disable
#nullable restore
#line 28 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/Markup_InCodeBlocksWithTagHelper.cshtml"
            __o = i;

#line default
#line hidden
#nullable disable
#nullable restore
#line 28 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/Markup_InCodeBlocksWithTagHelper.cshtml"
                                         
        }

#line default
#line hidden
#nullable disable
#nullable restore
#line 30 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/Markup_InCodeBlocksWithTagHelper.cshtml"
             

        if (person.Age < 20)
        {
            return Task.CompletedTask;
        }

        

#line default
#line hidden
#nullable disable
#nullable restore
#line 37 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/Markup_InCodeBlocksWithTagHelper.cshtml"
                               
        return Task.CompletedTask;
    }


    class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }

#line default
#line hidden
#nullable disable
    }
}
#pragma warning restore 1591
