using System;
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
            OutputPath = new OutputPath(Path.Combine(GetTypeDir(Model), $"{Model.TypeId.Name}.md"));
        }


        public override void Save()
        {
            var document = new MdDocument(
                Heading($"{Model.DisplayName} {Model.Kind}", 1)
            );

            AddTypeInfoSection(document.Root);
            
            //TODO: CSharpDefinition

            AddRemarksSection(document.Root);

            AddConstructorsSection(document.Root);

            AddFieldsSection(document.Root);

            AddEventsSection(document.Root);

            AddPropertiesSection(document.Root);

            AddMethodsSection(document.Root);

            AddOperatorsSection(document.Root);

            //TODO: Explicit interface implementations

            //TODO: Extension methods

            AddSeeAlsoSection(document.Root);

            Directory.CreateDirectory(Path.GetDirectoryName(OutputPath));
            document.Save(OutputPath);
        }


        private void AddTypeInfoSection(MdContainerBlock block)
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
                block.Add(XmlDocToMarkdownConverter.ConvertToBlock(Model.Summary, this));
            }

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
                block.Add(XmlDocToMarkdownConverter.ConvertToBlock(Model.Remarks, this));
            }
        }

        private void AddConstructorsSection(MdContainerBlock block)
        {
            //TODO: Skip constructors when it is compiler-generated, i.e. only the implict default constructor
            if (Model.Constructors != null)
            {
                var table = Table(Row("Name", "Description"));
                var ctorPagePath = PageFactory.TryGetPage(Model.Constructors)?.OutputPath;

                //TODO: Sort (unsure by what)
                foreach (var ctor in Model.Constructors.Overloads)
                {
                    if (ctorPagePath != null)
                    {
                        var link = Link(ctor.Signature, OutputPath.GetRelativePathTo(ctorPagePath));
                        table.Add(Row(link, ConvertToSpan(ctor.Summary)));
                    }
                    else
                    {
                        table.Add(Row(ctor.Signature, ConvertToSpan(ctor.Summary)));
                    }
                }

                block.Add(
                    Heading("Constructors", 2),
                    table
                );
            }
        }

        private void AddFieldsSection(MdContainerBlock block)
        {
            if (Model.Fields.Count > 0)
            {
                //TODO: Add page for field, insert link
                //TODO: Sort by name
                block.Add(
                    Heading("Fields", 2),
                    Table(
                        Row("Name", "Description"),
                        Model.Fields.Select(field => Row(field.Name, ConvertToSpan(field.Summary)))
                    )
                );
            }

        }

        private void AddEventsSection(MdContainerBlock block)
        {            
            if (Model.Events.Count > 0)
            {
                //TODO: Add event page, insert link to page
                //TODO: Sort by name
                block.Add(
                    Heading("Events", 2),
                    Table(
                        Row("Name", "Description"),
                        Model.Events.Select(ev => Row(ev.Name, ConvertToSpan(ev.Summary)))
                ));
            }
        }
        
        private void AddPropertiesSection(MdContainerBlock block)
        {            
            if (Model.Properties.Count > 0)
            {
                var table = Table(Row("Name", "Description"));

                //TODO: Sort by name
                foreach (var property in Model.Properties)
                {
                    var propertyPage = PageFactory.TryGetPage(property);

                    if(propertyPage != null)
                    {                        
                        var link = Link(property.Name, OutputPath.GetRelativePathTo(propertyPage.OutputPath));
                        table.Add(Row(link, ConvertToSpan(property.Summary)));
                    }
                    else
                    {
                        table.Add(Row(property.Name, ConvertToSpan(property.Summary)));
                    }
                }
             
                block.Add(
                    Heading("Properties", 2),
                    table
                );
            }
        }
        
        private void AddMethodsSection(MdContainerBlock block)
        {
            if (Model.Methods.Count > 0)
            {
                var table = Table(Row("Name", "Description"));

                //TODO: Sort methods by name
                foreach(var method in Model.Methods)
                {
                    var methodPage = PageFactory.TryGetPage(method);

                    foreach(var overload in method.Overloads)
                    {                        
                        if(methodPage != null)
                        {                            
                            var link = Link(overload.Signature, OutputPath.GetRelativePathTo(methodPage.OutputPath));                           
                            table.Add(Row(link, ConvertToSpan(overload.Summary)));
                        }
                        else
                        {                            
                            table.Add(Row(overload.Signature, ConvertToSpan(overload.Summary)));
                        }
                    }
                }

                block.Add(
                    Heading("Methods", 2),
                    table
                );
            }
        }
        
        private void AddOperatorsSection(MdContainerBlock block)
        {
            if (Model.Operators.Count > 0)
            {
                var table = Table(Row("Name", "Description"));

                // TODO: Sort by operator
                foreach (var operatorOverload in Model.Operators)
                {
                    var operatorPage = PageFactory.TryGetPage(operatorOverload);

                    foreach (var overload in operatorOverload.Overloads)
                    {
                        if(operatorPage != null)
                        {                            
                            var link = Link(overload.Signature, OutputPath.GetRelativePathTo(operatorPage.OutputPath));
                            table.Add(Row(link, ConvertToSpan(overload.Summary)));
                        }
                        else
                        {                            
                            table.Add(Row(overload.Signature, ConvertToSpan(overload.Summary)));
                        }
                    }
                }

                block.Add(
                    Heading("Operators", 2),
                    table
                );
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

        private MdSpan ConvertToSpan(SummaryElement summary)
        {
            return summary == null ? MdEmptySpan.Instance : XmlDocToMarkdownConverter.ConvertToSpan(summary, this);
        }

        private MdSpan ConvertToSpan(SeeAlsoElement seeAlso)
        {            
            return XmlDocToMarkdownConverter.ConvertToSpan(seeAlso, this);
        }
    }
}
