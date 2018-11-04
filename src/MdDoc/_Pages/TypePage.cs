using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Mono.Cecil;
using Grynwald.MarkdownGenerator;

using static Grynwald.MarkdownGenerator.FactoryMethods;

namespace MdDoc
{
    class TypePage : PageBase
    {
        private readonly TypeDefinition m_Type;


        public override string Name => $"Type {m_Type.Name}";

        protected override OutputPath OutputPath => m_PathProvider.GetOutputPath(m_Type);


        public TypePage(DocumentationContext context, PathProvider pathProvider, TypeDefinition type)
            : base(context, pathProvider)
        {
            m_Type = type ?? throw new ArgumentNullException(nameof(type));            
        }


        public override void Save()
        {
            var document = new MdDocument(
                Heading($"{m_Type.Name} {m_Type.Kind()}", 1)
            );

            AddTypeInfoSection(document.Root);

            document.Root.Add(
                Paragraph(
                    m_Context.XmlDocProvider.TryGetDocumentation(m_Type).Summary
            ));

            AddConstructorsSection(document.Root);

            AddFieldsSection(document.Root);

            AddEventsSection(document.Root);

            AddPropertiesSection(document.Root);

            AddMethodsSection(document.Root);

            //TODO: Separate methods and operators

            Directory.CreateDirectory(Path.GetDirectoryName(OutputPath));
            document.Save(OutputPath);
        }


        private void AddTypeInfoSection(MdContainerBlock block)
        {
            // Add Namespace 
            block.Add(
                Paragraph(Bold("Namespace:"), " " + m_Type.Namespace)
            );

            // Add Assembly
            block.Add(
                Paragraph(Bold("Assembly:"), " " + m_Type.Module.Assembly.Name.Name)
            );


            // Add list of base types
            var inheritance = GetInheritanceHierarchy();
            if (inheritance.Count > 1)
            {
                block.Add(
                    Paragraph(
                        Bold("Inheritance:"),
                        " ",
                        inheritance.Select(GetTypeNameSpan).Join(" → ")
                ));
            }

            // Add class attributes
            if (m_Type.CustomAttributes.Any())
            {
                block.Add(
                    Paragraph(
                        Bold("Attributes:"),
                        " ",
                        m_Type.CustomAttributes.Select(x => x.AttributeType).Select(GetTypeNameSpan).Join(",")
                ));
            }

            // Add list of implemented interfaces
            if (!m_Type.IsInterface && m_Type.HasInterfaces)
            {
                block.Add(
                    Paragraph(
                        Bold("Implements:"),
                        " ",
                        m_Type.Interfaces.Select(x => x.InterfaceType).Select(GetTypeNameSpan).Join(","))
                );
            }
        }

        private void AddConstructorsSection(MdContainerBlock block)
        {
            var constructors = m_Type.GetDocumentedConstrutors(m_Context);
         
            if (constructors.Any())
            {
                var table = Table(Row("Name", "Description"));
                foreach(var ctor in constructors)
                {
                    var ctorPage = m_PathProvider.GetConstructorsOutputPath(m_Type);
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
            var publicFields = m_Type
                .Fields
                .Where(m_Context.IsDocumentedItem);

            if (publicFields.Any())
            {
                block.Add(
                    Heading("Fields", 2),
                    Table(
                        Row("Name", "Description"),
                        publicFields.Select(x => Row(x.Name))
                    )
                );
            }

        }

        private void AddEventsSection(MdContainerBlock block)
        {
            if (m_Type.Kind() != TypeKind.Class && m_Type.Kind() != TypeKind.Struct && m_Type.Kind() != TypeKind.Interface)
                return;

            var events = m_Type
                .Events
                .Where(m_Context.IsDocumentedItem);

            if (events.Any())
            {
                block.Add(
                    Heading("Events", 2),
                    Table(
                        Row("Name", "Description"),
                        events.Select(x => Row(x.Name))
                ));
            }
        }
        
        private void AddPropertiesSection(MdContainerBlock block)
        {
            if (m_Type.Kind() != TypeKind.Class && m_Type.Kind() != TypeKind.Struct && m_Type.Kind() != TypeKind.Interface)
                return;

            var properties = m_Type
                .Properties
                .Where(m_Context.IsDocumentedItem);

            if (properties.Any())
            {
                var table = Table(Row("Name", "Description"));

                foreach(var property in properties)
                {
                    var propertyDocumentationPath = m_PathProvider.GetOutputPath(property);
                    var link = Link(property.Name, OutputPath.GetRelativePathTo(propertyDocumentationPath));

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
            var methods = m_Type.GetDocumentedMethods(m_Context);         

            if (methods.Any())
            {
                var table = Table(Row("Name", "Description"));

                foreach(var method in methods)
                {
                    var methodDocumentationPath = m_PathProvider.GetMethodOutputPath(method);
                    var link = Link(GetSignature(method), OutputPath.GetRelativePathTo(methodDocumentationPath));

                    table.Add(Row(link));
                }

                block.Add(
                    Heading("Methods", 2),
                    table
                );
            }
        }
        
        private LinkedList<TypeDefinition> GetInheritanceHierarchy()
        {
            var inheritance = new LinkedList<TypeDefinition>();
            inheritance.AddFirst(m_Type);
            var currentBaseType = m_Type.BaseType.Resolve();
            while (currentBaseType != null)
            {
                inheritance.AddFirst(currentBaseType);
                currentBaseType = currentBaseType.BaseType?.Resolve();
            }

            return inheritance;
        }

        
        protected override MdSpan GetTypeNameSpan(TypeReference type, bool noLink)
        {
            if (type.Equals(m_Type))
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
