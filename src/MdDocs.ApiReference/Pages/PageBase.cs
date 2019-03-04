using System;
using System.IO;
using System.Linq;
using Grynwald.MarkdownGenerator;
using Grynwald.MdDocs.ApiReference.Model;
using Grynwald.MdDocs.ApiReference.Model.XmlDocs;

using static Grynwald.MarkdownGenerator.FactoryMethods;

namespace Grynwald.MdDocs.ApiReference.Pages
{
    internal abstract class PageBase<TModel> : IMdSpanFactory, IPage where TModel : class, IDocumentation
    {
        private static readonly char[] s_SplitChars = ".".ToCharArray();
        private readonly string m_RootOutputPath;
        private readonly ILinkProvider m_LinkProvider;

        public abstract OutputPath OutputPath { get; }

        protected PageFactory PageFactory { get; }

        protected TModel Model { get; }


        public PageBase(PageFactory pageFactory, string rootOutputPath, TModel model)
        {
            PageFactory = pageFactory ?? throw new ArgumentNullException(nameof(pageFactory));
            m_RootOutputPath = rootOutputPath ?? throw new ArgumentNullException(nameof(rootOutputPath));
            Model = model ?? throw new ArgumentNullException(nameof(model));

            m_LinkProvider = new CompositeLinkProvider(
                new InternalLinkProvider(model, pageFactory)
            );
        }


        public abstract void Save();

        public MdParagraph GetMdParagraph(MemberId id) => new MdParagraph(GetMdSpan(id, false));

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

                case NamespaceId namespaceId:
                    if (noLink)
                        return namespaceId.Name;
                    else
                        return CreateLink(namespaceId, namespaceId.Name);

                default:
                    return MdEmptySpan.Instance;
            }
        }

        public MdSpan CreateLink(MemberId target, MdSpan text)
        {
            // try to generate a link to the target but avoid self-links
            if (m_LinkProvider.TryGetLink(target, out var link) && !OutputPath.Equals(link))
            {
                return new MdLinkSpan(text, OutputPath.GetRelativePathTo(link));
            }
            else
            {
                return text;
            }
        }


        protected string GetTypeDir(TypeDocumentation type)
        {
            var dirName = type.TypeId.Name;
            if (type.TypeId is GenericTypeInstanceId genericTypeInstance)
            {
                dirName += "-" + genericTypeInstance.TypeArguments.Count;
            }
            else if (type.TypeId is GenericTypeId genericType)
            {
                dirName += "-" + genericType.Arity;
            }

            return Path.Combine(GetNamespaceDir(type.NamespaceDocumentation), dirName);
        }

        protected string GetNamespaceDir(NamespaceDocumentation namespaceDocumentation) =>
            Path.Combine(m_RootOutputPath, String.Join('/', namespaceDocumentation.Name.Split(s_SplitChars)));

        protected MdSpan ConvertToSpan(TextBlock textBlock)
        {
            return textBlock == null ? MdEmptySpan.Instance : TextBlockToMarkdownConverter.ConvertToSpan(textBlock, this);
        }

        protected MdSpan ConvertToSpan(SeeAlsoElement seeAlso)
        {
            if (seeAlso.Text.IsEmpty)
            {
                return GetMdSpan(seeAlso.MemberId);
            }
            else
            {
                var text = ConvertToSpan(seeAlso.Text);
                return CreateLink(seeAlso.MemberId, text);
            }
        }

        protected MdBlock ConvertToBlock(TextBlock text) => TextBlockToMarkdownConverter.ConvertToBlock(text, this);

        protected virtual void AddObsoleteWarning(MdContainerBlock block, IObsoleteableDocumentation memberDocumentation)
        {
            if (!memberDocumentation.IsObsolete)
                return;

            var message = memberDocumentation.ObsoleteMessage;
            if (String.IsNullOrEmpty(message))
                message = "This API is obsolete.";

            block.Add(Paragraph(
                "⚠️ ",
                Bold("Warning:"),
                " ",
                message
            ));
        }
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
