using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Grynwald.MarkdownGenerator;
using MdDoc.Model;
using MdDoc.Model.XmlDocs;

using static Grynwald.MarkdownGenerator.FactoryMethods;

namespace MdDoc.Pages
{
    class TypePage : PageBase<TypeDocumentation>
    {
        public override OutputPath OutputPath { get; }
        
        protected override TypeDocumentation Model { get; }


        public TypePage(PageFactory pageFactory, string rootOutputPath, TypeDocumentation model)
            : base(pageFactory, rootOutputPath)
        {
            Model = model ?? throw new ArgumentNullException(nameof(model));
            OutputPath = new OutputPath(GetTypeDir(Model), "Type.md");
        }


        public override void Save()
        {
            var document = new MdDocument(
                Heading($"{Model.DisplayName} {Model.Kind}", 1)
            );

            AddDefinitionSection(document.Root);
           
            AddRemarksSection(document.Root);

            //TODO: Skip constructors when it is compiler-generated, i.e. only the implict default constructor
            AddOverloadableMembersSection(
                document.Root,
                "Constructors",
                (IEnumerable<OverloadDocumentation>)Model.Constructors?.Overloads ?? Array.Empty<OverloadDocumentation>()
            );

            //TODO: Example

            AddSimpleMembersSection(document.Root, "Fields", Model.Fields);

            AddSimpleMembersSection(document.Root, "Events", Model.Events);

            AddSimpleMembersSection(document.Root, "Properties", Model.Properties);

            AddOverloadableMembersSection(document.Root, "Indexers", Model.Indexers.SelectMany(m => m.Overloads));

            AddOverloadableMembersSection(document.Root, "Methods", Model.Methods.SelectMany(m => m.Overloads));

            AddOverloadableMembersSection(document.Root, "Operators", Model.Operators.SelectMany(x => x.Overloads));

            //TODO: Explicit interface implementations

            //TODO: Extension methods

            AddSeeAlsoSection(document.Root);

            Directory.CreateDirectory(Path.GetDirectoryName(OutputPath));
            document.Save(OutputPath);
        }


        private void AddDefinitionSection(MdContainerBlock block)
        {
            // Add Namespace 
            block.Add(
                Paragraph(Bold("Namespace:"), " " + Model.Namespace)
            );

            // Add Assembly
            block.Add(
                Paragraph(Bold("Assembly:"), " " + Model.AssemblyName)
            );

            if (Model.Summary != null)
            {
                block.Add(TextBlockToMarkdownConverter.ConvertToBlock(Model.Summary, this));
            }

            block.Add(CodeBlock(Model.CSharpDefinition, "csharp"));

            // Add list of base types            
            if (Model.InheritanceHierarchy.Count > 1)
            {
                block.Add(
                    Paragraph(
                        Bold("Inheritance:"),
                        " ",
                        Model.InheritanceHierarchy.Select(GetTypeNameSpan).Join(" → ")
                ));
            }

            // Add class attributes
            if (Model.Attributes.Count > 0)
            {
                block.Add(
                    Paragraph(
                        Bold("Attributes:"),
                        " ",
                        Model.Attributes.Select(GetTypeNameSpan).Join(",")
                ));
            }

            // Add list of implemented interfaces
            if (Model.ImplementedInterfaces.Count > 0)
            {
                block.Add(
                    Paragraph(
                        Bold("Implements:"),
                        " ",
                        Model.ImplementedInterfaces.Select(GetTypeNameSpan).Join(","))
                );
            }
        }
   
        private void AddRemarksSection(MdContainerBlock block)
        {
            if (Model.Remarks != null)
            {
                block.Add(Heading(2, "Remarks"));
                block.Add(TextBlockToMarkdownConverter.ConvertToBlock(Model.Remarks, this));
            }
        }

        private void AddOverloadableMembersSection(MdContainerBlock block, string sectionHeading, IEnumerable<OverloadDocumentation> overloads)
        {            
            if (overloads.Any())
            {
                var table = Table(Row("Name", "Description"));

                foreach (var ctor in overloads.OrderBy(x => x.Signature))
                {
                    table.Add(
                        Row(
                            CreateLink(ctor.MemberId, ctor.Signature),
                            ConvertToSpan(ctor.Summary)
                    ));
                }

                block.Add(
                    Heading(sectionHeading, 2),
                    table
                );
            }
        }

        private void AddSimpleMembersSection(MdContainerBlock block, string sectionHeading, IEnumerable<SimpleMemberDocumentation> members)
        {
            if (members.Any())
            {
                block.Add(Heading(sectionHeading, 2));
                
                var table = Table(Row("Name", "Description"));
                foreach(var member in members.OrderBy(x => x.Name))
                {
                    table.Add(
                        Row(
                            CreateLink(member.MemberId, member.Name),
                            ConvertToSpan(member.Summary)
                    ));
                }
                block.Add(table);
            }
        }
        
        private void AddSeeAlsoSection(MdContainerBlock block)
        {
            if(Model.SeeAlso.Count > 0)
            {
                block.Add(Heading(2, "See Also"));
                block.Add(
                    BulletList(
                        Model.SeeAlso.Select(seeAlso => ListItem(ConvertToSpan(seeAlso)))
                ));
            }
        }              
    }
}
