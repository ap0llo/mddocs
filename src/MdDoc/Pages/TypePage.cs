using System;
using System.IO;
using System.Linq;
using Grynwald.MarkdownGenerator;
using MdDoc.Model;

using static Grynwald.MarkdownGenerator.FactoryMethods;

namespace MdDoc.Pages
{
    //TODO: Add documentation from XML comments
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

            AddSummarySection(document.Root);

            AddConstructorsSection(document.Root);

            AddFieldsSection(document.Root);

            AddEventsSection(document.Root);

            AddPropertiesSection(document.Root);

            AddMethodsSection(document.Root);

            AddOperatorsSection(document.Root);

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
    
        private void AddSummarySection(MdContainerBlock block)
        {
            if(Model.Summary != null)
            {
                block.Add(Heading("Summary", 2));                
                block.Add(XmlDocToMarkdownConverter.ConvertToBlock(Model.Summary));
            }
        }

        private void AddConstructorsSection(MdContainerBlock block)
        {         
            if (Model.Constructors != null)
            {
                var table = Table(Row("Name", "Description"));
                var ctorPagePath = PageFactory.TryGetPage(Model.Constructors)?.OutputPath;

                foreach(var ctor in Model.Constructors.Overloads)
                {
                    if(ctorPagePath != null)
                    {
                        var link = Link(ctor.Signature, OutputPath.GetRelativePathTo(ctorPagePath));
                        table.Add(Row(link));
                    }
                    else
                    {                        
                        table.Add(Row(ctor.Signature));
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
                block.Add(
                    Heading("Fields", 2),
                    Table(
                        Row("Name", "Description"),
                        Model.Fields.Select(field => Row(field.Name))
                    )
                );
            }

        }

        private void AddEventsSection(MdContainerBlock block)
        {            
            if (Model.Events.Count > 0)
            {
                block.Add(
                    Heading("Events", 2),
                    Table(
                        Row("Name", "Description"),
                        Model.Events.Select(x => Row(x.Name))
                ));
            }
        }
        
        private void AddPropertiesSection(MdContainerBlock block)
        {            
            if (Model.Properties.Count > 0)
            {
                var table = Table(Row("Name", "Description"));

                foreach(var property in Model.Properties)
                {
                    var propertyPage = PageFactory.TryGetPage(property);

                    if(propertyPage != null)
                    {                        
                        var link = Link(property.Name, OutputPath.GetRelativePathTo(propertyPage.OutputPath));
                        table.Add(Row(link));
                    }
                    else
                    {
                        table.Add(Row(property.Name));
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

                foreach(var method in Model.Methods)
                {
                    var methodPage = PageFactory.TryGetPage(method);

                    foreach(var overload in method.Overloads)
                    {
                        var summary = overload.Summary != null
                            ? XmlDocToMarkdownConverter.ConvertToSpan(overload.Summary)
                            : new MdTextSpan("No summary found");

                        if(methodPage != null)
                        {                            
                            var link = Link(overload.Signature, OutputPath.GetRelativePathTo(methodPage.OutputPath));                           
                            table.Add(Row(link, summary));
                        }
                        else
                        {                            
                            table.Add(Row(overload.Signature, summary));
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

                foreach (var operatorOverload in Model.Operators)
                {
                    var operatorPage = PageFactory.TryGetPage(operatorOverload);

                    foreach (var overload in operatorOverload.Overloads)
                    {
                        if(operatorPage != null)
                        {                            
                            var link = Link(overload.Signature, OutputPath.GetRelativePathTo(operatorPage.OutputPath));
                            table.Add(Row(link));
                        }
                        else
                        {                            
                            table.Add(Row(overload.Signature));
                        }
                    }
                }

                block.Add(
                    Heading("Operators", 2),
                    table
                );
            }
        }
    }
}
