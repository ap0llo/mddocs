using System;
using System.IO;
using System.Linq;
using Grynwald.MarkdownGenerator;
using MdDoc.Model;

using static Grynwald.MarkdownGenerator.FactoryMethods;

namespace MdDoc.Pages
{
    abstract class PageBase<TModel> : IMdSpanFactory, IPage where TModel : IDocumentation
    {
        private static readonly char[] s_SplitChars = ".".ToCharArray();
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

        
        protected virtual MdSpan GetTypeNameSpan(TypeId type) => GetTypeNameSpan(type, false);

        //TODO: Add tests
        protected MdSpan GetTypeNameSpan(TypeId type, bool noLink)
        {
            if (type is ArrayTypeId arrayType)
            {
                var elementTypeSpan = GetTypeNameSpan(arrayType.ElementType, noLink);
                return new MdCompositeSpan(elementTypeSpan, $"[]");
            }

            if (type is GenericTypeInstanceId genericType)
            {                
                return CompositeSpan(
                    genericType.Name,
                    "<",
                    genericType.TypeArguments.Select(t => GetTypeNameSpan(t, noLink)).Join(", "),
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
                else if(OutputPath.Equals(typePage.OutputPath))
                {
                    // do not create self-links
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
            var namespaceDir = Path.Combine(m_RootOutputPath, String.Join('/', type.Namespace.Split(s_SplitChars)));

            var dirName = type.TypeId.Name;
            if(type.TypeId is GenericTypeInstanceId genericTypeInstance)
            {
                dirName += "-" + genericTypeInstance.TypeArguments.Count;
            }
            else if(type.TypeId is GenericTypeId genericType)
            {
                dirName += "-" + genericType.Arity;
            }

            return Path.Combine(namespaceDir, dirName);
        }

        public MdSpan GetMdSpan(MemberId id) => GetMdSpan(id, false);

        public MdSpan GetMdSpan(MemberId id, bool noLink)
        {
            switch (id)
            {
                case TypeId typeId:
                    return GetTypeNameSpan(typeId);

                case TypeMemberId typeMemberId:
                    var modelItem = Model.TryGetDocumentation(typeMemberId);
                    var page = modelItem != null ? PageFactory.TryGetPage(modelItem) : null;
                    if(page == null || noLink)
                    {
                        return new MdTextSpan(typeMemberId.Name);
                    }
                    else
                    {
                        return new MdLinkSpan(
                            typeMemberId.Name,
                            OutputPath.GetRelativePathTo(page.OutputPath)
                        );  
                    }               

                default:
                    return MdEmptySpan.Instance;                    
            }
        }       
    }


}
