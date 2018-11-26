using Grynwald.MarkdownGenerator;
using MdDoc.Model;
using Mono.Cecil;
using System;
using System.IO;
using System.Linq;

using static Grynwald.MarkdownGenerator.FactoryMethods;

namespace MdDoc.Pages
{
    abstract class PageBase<TModel> : IPage where TModel : IDocumentation
    {
        private static char[] s_SplitChars = ".".ToCharArray();
        private readonly string m_RootOutputPath;


        public abstract OutputPath OutputPath { get; }

        protected PageFactory PageFactory { get; }

        protected abstract TModel Model { get; }


        public PageBase(PageFactory pageFactory, string rootOutputPath)
        {
            PageFactory = pageFactory ?? throw new ArgumentNullException(nameof(pageFactory));
            m_RootOutputPath = rootOutputPath ?? throw new ArgumentNullException(nameof(rootOutputPath));
        }


        public abstract void Save();


        protected virtual MdSpan GetTypeNameSpan(TypeDocumentation type) => GetTypeNameSpan(type.Definition, false);

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

            if (noLink)
            {
                return new MdTextSpan(type.Name);
            }
            else
            {
                IPage typePage = default;
                var documentation = Model.TryGetDocumentation(type);
                if(documentation != null)
                {
                    typePage = PageFactory.TryGetPage(documentation);
                }

                if(typePage == default)
                {
                    return new MdTextSpan(type.Name);
                }
                else
                {
                                        
                    return new MdLinkSpan(
                        type.Name,
                        OutputPath.GetRelativePathTo(typePage.OutputPath)
                    );
                }


            }
        }

        protected string GetTypeDir(TypeDocumentation type)
        {
            var dir = Path.Combine(m_RootOutputPath, String.Join('/', type.Namespace.Split(s_SplitChars)));
            return Path.Combine(dir, type.Name);
        }
    }
}
