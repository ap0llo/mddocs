using Grynwald.MarkdownGenerator;
using System;
using System.Collections.Generic;
using System.Text;
using Mono.Cecil;
using System.Linq;

using static Grynwald.MarkdownGenerator.FactoryMethods;

namespace MdDoc.Pages
{
    abstract class MemberPage : PageBase
    {
        protected abstract TypeReference DeclaringType { get; }

        public MemberPage(PageFactory pageFactory, string rootOutputPath) : base(pageFactory, rootOutputPath)
        { }


        protected void AddDeclaringTypeSection(MdContainerBlock block)
        {
            block.Add(
                Paragraph(
                    Bold("Declaring Type:"), " ", GetTypeNameSpan(DeclaringType)
            ));
        }

        protected void AddOverloadsSection(MdContainerBlock block, IEnumerable<MethodDefinition> methods)
        {
            var table = Table(Row("Signature", "Description"));
            foreach (var method in methods)
            {
                var signature = GetSignature(method);

                table.Add(
                    Row(Link(signature, $"#{signature}"))
                );
            }

            block.Add(
                Heading("Overloads", 2),
                table
            );
        }

        protected void AddDetailSections(MdContainerBlock block, IEnumerable<MethodDefinition> methods)
        {
            foreach (var method in methods)
            {
                block.Add(
                    Heading(GetSignature(method), 2)
                );

                //block.Add(
                //    Paragraph(
                //        m_Context.XmlDocProvider.TryGetDocumentation(method).Summary
                //));

                //TODO: Attributes

                if (method.Parameters.Any())
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
