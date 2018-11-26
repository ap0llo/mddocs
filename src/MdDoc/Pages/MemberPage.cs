using System.Collections.Generic;
using System.Linq;
using Grynwald.MarkdownGenerator;
using MdDoc.Model;
using Mono.Cecil;

using static Grynwald.MarkdownGenerator.FactoryMethods;

namespace MdDoc.Pages
{
    abstract class MemberPage<TModel> : PageBase<TModel> where TModel : Model.MemberDocumentation
    {        
        public MemberPage(PageFactory pageFactory, string rootOutputPath) : base(pageFactory, rootOutputPath)
        { }


        protected void AddDeclaringTypeSection(MdContainerBlock block)
        {
            block.Add(
                Paragraph(
                    Bold("Declaring Type:"), " ", GetTypeNameSpan(Model.TypeDocumentation)
            ));
        }

        protected void AddOverloadsSection(MdContainerBlock block, IEnumerable<MethodOverload> methods)
        {
            var table = Table(Row("Signature", "Description"));
            foreach (var method in methods)
            {
                var signature = GetSignature(method.Definition);

                table.Add(
                    Row(Link(signature, $"#{signature}"))
                );
            }

            block.Add(
                Heading("Overloads", 2),
                table
            );
        }

        protected void AddDetailSections(MdContainerBlock block, IEnumerable<MethodOverload> methods)
        {
            foreach (var method in methods)
            {
                block.Add(
                    Heading(GetSignature(method.Definition), 2)
                );

                if (method.Definition.Parameters.Any())
                {
                    var table = Table(Row("Name", "Type", "Description"));
                    foreach (var parameter in method.Definition.Parameters)
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
