using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Mono.Cecil;
using Grynwald.MarkdownGenerator;

using static Grynwald.MarkdownGenerator.FactoryMethods;

namespace MdDoc
{
    class TypeDocumentationWriter : DocumentationWriterBase
    {
        private readonly TypeDefinition m_Type;

        protected override OutputPath OutputPath => m_PathProvider.GetOutputPath(m_Type);


        public TypeDocumentationWriter(DocumentationContext context, PathProvider pathProvider, TypeDefinition type)
            : base(context, pathProvider)
        {
            m_Type = type ?? throw new ArgumentNullException(nameof(type));            
        }


        public void SaveDocumentation()
        {
            var document = new MdDocument(
                Heading($"{m_Type.Name} {m_Type.Kind()}", 1)
            );

            AddTypeInfoSection(document.Root);

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
            if (m_Type.Kind() != TypeKind.Class && m_Type.Kind() != TypeKind.Struct)
                return;

            var constructors = m_Type
                .Methods
                .Where(m => m.IsConstructor)
                .Where(m_Context.IsDocumentedItem);

            if (constructors.Any())
            {
                block.Add(
                    Heading("Constructors", 2),
                    Table(
                        Row("Name", "Description"),
                        constructors.Select(x => Row(GetSignature(x)))
                ));
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
            if (m_Type.Kind() != TypeKind.Class && m_Type.Kind() != TypeKind.Struct && m_Type.Kind() != TypeKind.Interface)
                return;
            

            var methods = m_Type.Methods
                .Where(m => !m.IsConstructor)
                .Where(m_Context.IsDocumentedItem)                
                .ToArray();                

            if (methods.Any())
            {
                block.Add(
                    Heading("Methods", 2),
                    Table(
                        Row("Name", "Description"),
                        methods.Select(m => Row(GetSignature(m)))
                ));
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

        private MdSpan GetSignature(MethodDefinition method)
        {            
            var methodName = method.IsConstructor
                ? method.DeclaringType.Name
                : method.Name;

            var parameters = method
                .Parameters
                .Select(x => x.ParameterType)
                .Select(t => GetTypeNameSpan(t, true))
                .Join(", ");
            
            return CompositeSpan(methodName, "(", parameters, ")");            
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
