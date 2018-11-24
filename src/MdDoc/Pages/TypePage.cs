using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Mono.Cecil;
using Grynwald.MarkdownGenerator;
using MdDoc.Model;

using static Grynwald.MarkdownGenerator.FactoryMethods;

namespace MdDoc.Pages
{
    class TypePage : PageBase<TypeDocumentation>
    {
        
        public override OutputPath OutputPath =>
            new OutputPath(Path.Combine(GetTypeDir(Model.Definition), $"{Model.Name}.md"));

        protected override TypeDocumentation Model { get; }


        public TypePage(PageFactory pageFactory, string rootOutputPath, TypeDocumentation model)
            : base(pageFactory, rootOutputPath)
        {
            Model = model ?? throw new ArgumentNullException(nameof(model));            
        }


        public override void Save()
        {
            var document = new MdDocument(
                Heading($"{Model.Name} {Model.Kind}", 1)
            );

            AddTypeInfoSection(document.Root);

            //TODO: Include info from XML docs

            AddConstructorsSection(document.Root);

            AddFieldsSection(document.Root);

            AddEventsSection(document.Root);

            AddPropertiesSection(document.Root);

            AddMethodsSection(document.Root);

            AddOperatorsSection(document.Root);

            //TODO: Separate methods and operators

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
            if (!Model.Definition.IsInterface && Model.Definition.HasInterfaces)
            {
                block.Add(
                    Paragraph(
                        Bold("Implements:"),
                        " ",
                        Model.Definition.Interfaces.Select(x => x.InterfaceType).Select(GetTypeNameSpan).Join(","))
                );
            }
        }

        private void AddConstructorsSection(MdContainerBlock block)
        {         
            if (Model.Constructors != null)
            {
                var table = Table(Row("Name", "Description"));
                foreach(var ctor in Model.Constructors.Definitions)
                {
                    var ctorPagePath = PageFactory.TryGetPage(Model.Constructors)?.OutputPath;

                    if(ctorPagePath != null)
                    {
                        var link = Link(GetSignature(ctor), OutputPath.GetRelativePathTo(ctorPagePath));
                        table.Add(Row(link));
                    }
                    else
                    {                        
                        table.Add(Row(GetSignature(ctor)));
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
            if (Model.Fields.Any())
            {
                block.Add(
                    Heading("Fields", 2),
                    Table(
                        Row("Name", "Description"),
                        Model.Fields.Select(x => Row(x.Definition.Name))
                    )
                );
            }

        }

        private void AddEventsSection(MdContainerBlock block)
        {
            if (Model.Kind != TypeKind.Class && Model.Kind != TypeKind.Struct && Model.Kind != TypeKind.Interface)
                return;
            
            if (Model.Events.Any())
            {
                block.Add(
                    Heading("Events", 2),
                    Table(
                        Row("Name", "Description"),
                        Model.Events.Select(x => Row(x.Definition.Name))
                ));
            }
        }
        
        private void AddPropertiesSection(MdContainerBlock block)
        {
            if (Model.Kind != TypeKind.Class && Model.Kind != TypeKind.Struct && Model.Kind != TypeKind.Interface)
                return;
            

            if (Model.Properties.Any())
            {
                var table = Table(Row("Name", "Description"));

                foreach(var property in Model.Properties)
                {
                    var propertyPage = PageFactory.TryGetPage(property);

                    if(propertyPage != null)
                    {                        
                        var link = Link(property.Definition.Name, OutputPath.GetRelativePathTo(propertyPage.OutputPath));
                        table.Add(Row(link));
                    }
                    else
                    {
                        table.Add(Row(property.Definition.Name));
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

                    foreach(var overload in method.Definitions)
                    {
                        if(methodPage != null)
                        {                            
                            var link = Link(GetSignature(overload), OutputPath.GetRelativePathTo(methodPage.OutputPath));
                            table.Add(Row(link));
                        }
                        else
                        {                            
                            table.Add(Row(GetSignature(overload)));
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
                    foreach (var overload in operatorOverload.Definitions)
                    {
                        if(operatorPage != null)
                        {                            
                            var link = Link(GetSignature(overload), OutputPath.GetRelativePathTo(operatorPage.OutputPath));
                            table.Add(Row(link));
                        }
                        else
                        {                            
                            table.Add(Row(GetSignature(overload)));
                        }

                    }
                }

                block.Add(
                    Heading("Operators", 2),
                    table
                );
            }
        }
        
        
        protected override MdSpan GetTypeNameSpan(TypeReference type, bool noLink)
        {
            if (type.Equals(Model))
            {
                return new MdTextSpan(type.Name);
            }
            else
            {
                return base.GetTypeNameSpan(type, noLink);
            }
        }

    }
}
