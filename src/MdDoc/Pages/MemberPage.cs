using System.Collections.Generic;
using Grynwald.MarkdownGenerator;
using MdDoc.Model;

using static Grynwald.MarkdownGenerator.FactoryMethods;

namespace MdDoc.Pages
{
    abstract class MemberPage<TModel> : PageBase<TModel> where TModel : MemberDocumentation
    {        
        public MemberPage(PageFactory pageFactory, string rootOutputPath) : base(pageFactory, rootOutputPath)
        { }


        protected void AddDeclaringTypeSection(MdContainerBlock block)
        {
            block.Add(
                Paragraph(
                    Bold("Declaring Type:"), " ", GetTypeNameSpan(Model.TypeDocumentation.TypeId)
            ));
        }

        protected void AddOverloadsSection(MdContainerBlock block, IEnumerable<MethodOverloadDocumentation> methods)
        {
            var table = Table(Row("Signature", "Description"));
            foreach (var method in methods)
            {
                table.Add(
                    Row(method.Signature)
                );
            }

            block.Add(
                Heading("Overloads", 2),
                table
            );
        }

        protected void AddDetailSections(MdContainerBlock block, IEnumerable<MethodOverloadDocumentation> methods)
        {
            foreach (var method in methods)
            {
                block.Add(
                    Heading(method.Signature, 2)
                );

                if (method.Parameters.Count > 0)
                {
                    var table = Table(Row("Name", "Type", "Description"));
                    foreach (var parameter in method.Parameters)
                    {
                        table.Add(
                            Row(
                                CodeSpan(parameter.Name),
                                GetTypeNameSpan(parameter.ParameterType),
                                ""
                        ));
                    }

                    block.Add(
                        Heading("Parameters", 3),
                        table
                    );
                }
            }
        }
    }
}
