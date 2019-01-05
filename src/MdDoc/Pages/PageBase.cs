using System;
using System.IO;
using System.Linq;
using Grynwald.MarkdownGenerator;
using MdDoc.Model;
using MdDoc.Model.XmlDocs;
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

        public MdSpan GetMdSpan(MemberId id) => GetMdSpan(id, false);

        public MdSpan GetMdSpan(MemberId id, bool noLink = false)
        {
            switch (id)
            {
                case TypeId typeId:
                    return GetMdSpan(typeId, noLink);

                case MethodId methodId:
                    if (noLink)
                        return MethodFormatter.Instance.GetSignature(methodId);
                    else
                        return CreateLink(methodId, MethodFormatter.Instance.GetSignature(methodId));

                case PropertyId propertyId:
                    if (noLink)
                        return MethodFormatter.Instance.GetSignature(propertyId);
                    else
                        return CreateLink(propertyId, MethodFormatter.Instance.GetSignature(propertyId));

                case TypeMemberId typeMemberId:
                    if (noLink)
                        return typeMemberId.Name;
                    else
                        return CreateLink(typeMemberId, typeMemberId.Name);

                default:
                    return MdEmptySpan.Instance;
            }
        }

        public MdSpan CreateLink(MemberId target, MdSpan text)
        {
            var modelItem = Model.TryGetDocumentation(target);
            var page = modelItem != null ? PageFactory.TryGetPage(modelItem) : null;

            if (page == null)
                return text;

            // do not create self-links
            if (OutputPath.Equals(page.OutputPath))
                return text;

            return new MdLinkSpan(text, OutputPath.GetRelativePathTo(page.OutputPath));
        }


        protected string GetTypeDir(TypeDocumentation type)
        {
            var namespaceDir = Path.Combine(m_RootOutputPath, String.Join('/', type.Namespace.Split(s_SplitChars)));

            var dirName = type.TypeId.Name;
            if (type.TypeId is GenericTypeInstanceId genericTypeInstance)
            {
                dirName += "-" + genericTypeInstance.TypeArguments.Count;
            }
            else if (type.TypeId is GenericTypeId genericType)
            {
                dirName += "-" + genericType.Arity;
            }

            return Path.Combine(namespaceDir, dirName);
        }        
        
        protected MdSpan ConvertToSpan(TextBlock textBlock)
        {
            return textBlock == null ? MdEmptySpan.Instance : TextBlockToMarkdownConverter.ConvertToSpan(textBlock, this);
        }

        protected MdSpan ConvertToSpan(SeeAlsoElement seeAlso)
        {
            if (seeAlso.Text.Elements.Count > 0)
            {
                var text = TextBlockToMarkdownConverter.ConvertToSpan(seeAlso.Text, this);
                return CreateLink(seeAlso.MemberId, text);
            }
            else
            {
                return GetMdSpan(seeAlso.MemberId);
            }
        }

        protected MdBlock ConvertToBlock(TextBlock text) => TextBlockToMarkdownConverter.ConvertToBlock(text, this);


        private MdSpan GetMdSpan(TypeId type, bool noLink)
        {
            if (type is ArrayTypeId arrayType)
            {
                var elementTypeSpan = GetMdSpan(arrayType.ElementType, noLink);
                return new MdCompositeSpan(elementTypeSpan, $"[]");
            }

            if (type is GenericTypeInstanceId genericType)
            {
                return CompositeSpan(
                    genericType.Name,
                    "<",
                    genericType.TypeArguments.Select(t => GetMdSpan(t, noLink)).Join(", "),
                    ">"
                );
            }

            if (noLink)
            {
                return new MdTextSpan(type.Name);
            }
            else
            {
                return CreateLink(type, type.Name);
            }
        }

    }
}
