using Grynwald.MarkdownGenerator;
using Mono.Cecil;
using System;
using System.Linq;

using static Grynwald.MarkdownGenerator.FactoryMethods;

namespace MdDoc.Pages
{
    abstract class PageBase : IPage
    {
        protected readonly DocumentationContext m_Context;
        protected readonly PathProvider m_PathProvider;        


        protected abstract OutputPath OutputPath { get; }

        protected PageFactory PageFactory { get; }


        public PageBase(PageFactory pageFactory, DocumentationContext context, PathProvider pathProvider)
        {
            PageFactory = pageFactory ?? throw new ArgumentNullException(nameof(pageFactory));
            m_Context = context ?? throw new ArgumentNullException(nameof(context));
            m_PathProvider = pathProvider ?? throw new ArgumentNullException(nameof(pathProvider));
        }


        public abstract void Save();


        protected virtual MdSpan GetTypeNameSpan(TypeReference type) => GetTypeNameSpan(type, false);

        protected virtual MdSpan GetTypeNameSpan(TypeReference type, bool noLink)
        {
            if (type.IsArray)
            {
                var elementTypeSpan = GetTypeNameSpan(type.GetElementType(), noLink);
                return new MdCompositeSpan(elementTypeSpan, $"[]");
            }

            if (type is GenericInstanceType genericType && genericType.HasGenericArguments)
            {
                var arguments = genericType.GenericArguments;

                var typeName = genericType.Name.Replace($"`{arguments.Count}", "");

                return CompositeSpan(
                    typeName,
                    "<",
                    arguments.Select(t => GetTypeNameSpan(t, noLink)).Join(", "),
                    ">"
                );
            }

            if (noLink || !m_Context.IsDocumentedItem(type))
            {
                return new MdTextSpan(type.Name);
            }
            else
            {
                var typeOutputPath = m_PathProvider.GetOutputPath(type);
                return new MdLinkSpan(
                    type.Name,
                    OutputPath.GetRelativePathTo(typeOutputPath)
                );
            }
        }

        protected MdSpan GetSignature(MethodDefinition method)
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
    }
}
