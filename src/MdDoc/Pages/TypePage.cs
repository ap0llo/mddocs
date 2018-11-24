﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Mono.Cecil;
using Grynwald.MarkdownGenerator;
using MdDoc.Model;

using static Grynwald.MarkdownGenerator.FactoryMethods;

namespace MdDoc.Pages
{
    class TypePage : PageBase
    {
        private readonly TypeDocumentation m_Model;
        
        protected override OutputPath OutputPath => m_PathProvider.GetOutputPath(m_Model.Definition);

        protected override IDocumentation Model => m_Model;


        public TypePage(PageFactory pageFactory, PathProvider pathProvider, TypeDocumentation model)
            : base(pageFactory, pathProvider)
        {
            m_Model = model ?? throw new ArgumentNullException(nameof(model));            
        }


        public override void Save()
        {
            var document = new MdDocument(
                Heading($"{m_Model.Name} {m_Model.Kind}", 1)
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
                Paragraph(Bold("Namespace:"), " " + m_Model.Namespace)
            );

            // Add Assembly
            block.Add(
                Paragraph(Bold("Assembly:"), " " + m_Model.AssemblyName)
            );


            // Add list of base types            
            if (m_Model.InheritanceHierarchy.Count > 1)
            {
                block.Add(
                    Paragraph(
                        Bold("Inheritance:"),
                        " ",
                        m_Model.InheritanceHierarchy.Select(GetTypeNameSpan).Join(" → ")
                ));
            }

            // Add class attributes
            if (m_Model.Attributes.Count > 0)
            {
                block.Add(
                    Paragraph(
                        Bold("Attributes:"),
                        " ",
                        m_Model.Attributes.Select(GetTypeNameSpan).Join(",")
                ));
            }

            // Add list of implemented interfaces
            if (!m_Model.Definition.IsInterface && m_Model.Definition.HasInterfaces)
            {
                block.Add(
                    Paragraph(
                        Bold("Implements:"),
                        " ",
                        m_Model.Definition.Interfaces.Select(x => x.InterfaceType).Select(GetTypeNameSpan).Join(","))
                );
            }
        }

        private void AddConstructorsSection(MdContainerBlock block)
        {         
            if (m_Model.Constructors != null)
            {
                var table = Table(Row("Name", "Description"));
                foreach(var ctor in m_Model.Constructors.Definitions)
                {
                    var ctorPage = m_PathProvider.GetConstructorsOutputPath(m_Model.Definition);
                    var link = Link(GetSignature(ctor), OutputPath.GetRelativePathTo(ctorPage));

                    table.Add(Row(link));
                }

                block.Add(
                    Heading("Constructors", 2),
                    table
                );
            }
        }

        private void AddFieldsSection(MdContainerBlock block)
        {
            if (m_Model.Fields.Any())
            {
                block.Add(
                    Heading("Fields", 2),
                    Table(
                        Row("Name", "Description"),
                        m_Model.Fields.Select(x => Row(x.Definition.Name))
                    )
                );
            }

        }

        private void AddEventsSection(MdContainerBlock block)
        {
            if (m_Model.Kind != TypeKind.Class && m_Model.Kind != TypeKind.Struct && m_Model.Kind != TypeKind.Interface)
                return;
            
            if (m_Model.Events.Any())
            {
                block.Add(
                    Heading("Events", 2),
                    Table(
                        Row("Name", "Description"),
                        m_Model.Events.Select(x => Row(x.Definition.Name))
                ));
            }
        }
        
        private void AddPropertiesSection(MdContainerBlock block)
        {
            if (m_Model.Kind != TypeKind.Class && m_Model.Kind != TypeKind.Struct && m_Model.Kind != TypeKind.Interface)
                return;
            

            if (m_Model.Properties.Any())
            {
                var table = Table(Row("Name", "Description"));

                foreach(var property in m_Model.Properties)
                {
                    var propertyDocumentationPath = m_PathProvider.GetOutputPath(property.Definition);
                    var link = Link(property.Definition.Name, OutputPath.GetRelativePathTo(propertyDocumentationPath));

                    table.Add(Row(link));
                }
             
                block.Add(
                    Heading("Properties", 2),
                    table
                );
            }
        }
        
        private void AddMethodsSection(MdContainerBlock block)
        {   
            if (m_Model.Methods.Count > 0)
            {
                var table = Table(Row("Name", "Description"));

                foreach(var method in m_Model.Methods)
                {
                    foreach(var overload in method.Definitions)
                    {
                        var methodDocumentationPath = m_PathProvider.GetMethodOutputPath(overload);
                        var link = Link(GetSignature(overload), OutputPath.GetRelativePathTo(methodDocumentationPath));

                        table.Add(Row(link));
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
            if (m_Model.Operators.Count > 0)
            {
                var table = Table(Row("Name", "Description"));

                foreach (var method in m_Model.Operators)
                {
                    foreach (var overload in method.Definitions)
                    {
                        var methodDocumentationPath = m_PathProvider.GetMethodOutputPath(overload);
                        var link = Link(GetSignature(overload), OutputPath.GetRelativePathTo(methodDocumentationPath));

                        table.Add(Row(link));
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
            if (type.Equals(m_Model))
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
