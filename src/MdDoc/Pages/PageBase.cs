using Grynwald.MarkdownGenerator;
using MdDoc.Model;
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

        
        protected virtual MdSpan GetTypeNameSpan(TypeName type) => GetTypeNameSpan(type, false);

        //TODO: Add tests
        protected virtual MdSpan GetTypeNameSpan(TypeName type, bool noLink)
        {
            if (type.IsArray)
            {
                var elementTypeSpan = GetTypeNameSpan(type.ElementType, noLink);
                return new MdCompositeSpan(elementTypeSpan, $"[]");
            }

            if (type.TypeArguments.Count > 0)
            {                
                return CompositeSpan(
                    type.BaseName,
                    "<",
                    type.TypeArguments.Select(t => GetTypeNameSpan(t, noLink)).Join(", "),
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
            var namespaceDir = Path.Combine(m_RootOutputPath, String.Join('/', type.Name.Namespace.Split(s_SplitChars)));

            var dirName = type.Name.BaseName;
            if(type.Name.TypeArguments.Count > 0)
            {
                dirName += "-" + type.Name.TypeArguments.Count;
            }

            return Path.Combine(namespaceDir, dirName);
        }
    }


}
