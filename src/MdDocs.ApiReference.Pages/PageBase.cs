using System;
using System.IO;
using System.Linq;
using Grynwald.MarkdownGenerator;
using Grynwald.MdDocs.ApiReference.Model;
using Grynwald.MdDocs.ApiReference.Model.XmlDocs;

namespace Grynwald.MdDocs.ApiReference.Pages
{
    internal abstract class PageBase<TModel> : IMdSpanFactory, IPage where TModel : class, IDocumentation
    {
        private static readonly char[] s_SplitChars = ".".ToCharArray();
        private readonly ILinkProvider m_LinkProvider;


        public abstract string RelativeOutputPath { get; }

        protected PageFactory PageFactory { get; }

        protected TModel Model { get; }


        public PageBase(ILinkProvider linkProvider, PageFactory pageFactory, TModel model)
        {
            PageFactory = pageFactory ?? throw new ArgumentNullException(nameof(pageFactory));
            Model = model ?? throw new ArgumentNullException(nameof(model));

            m_LinkProvider = linkProvider ?? throw new ArgumentNullException(nameof(linkProvider));
        }


        public abstract void Save(string path);


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
                        return SignatureFormatter.GetSignature(methodId);
                    else
                        return CreateLink(methodId, SignatureFormatter.GetSignature(methodId));

                case PropertyId propertyId:
                    if (noLink)
                        return SignatureFormatter.GetSignature(propertyId);
                    else
                        return CreateLink(propertyId, SignatureFormatter.GetSignature(propertyId));

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

            if (m_LinkProvider.TryGetLink(this, target, out var link))
            {
                if (String.IsNullOrEmpty(link.RelativePath))
                {
                    // link in same file, but there is an anchor => link to anchor
                    if (link.HasAnchor)
                        return new MdLinkSpan(text, "#" + link.Anchor);
                }
                // link to different file and link has an anchor
                else
                {                    
                    return link.HasAnchor
                        ? new MdLinkSpan(text, link.RelativePath + "#" + link.Anchor)
                        : new MdLinkSpan(text, link.RelativePath);
                }
            }

            // no link could be created => return text without link
            return text;
        }

        public virtual bool TryGetAnchor(MemberId id, out string anchor)
        {
            anchor = default;
            return false;
        }


        protected string GetTypeDirRelative(TypeDocumentation type)
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

            return Path.Combine(GetNamespaceDirRelative(type.NamespaceDocumentation), dirName);
        }

        protected string GetNamespaceDirRelative(NamespaceDocumentation namespaceDocumentation) =>
           String.Join("/", namespaceDocumentation.Name.Split(s_SplitChars));

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

            block.Add(new MdParagraph(
                "⚠️ ",
                new MdStrongEmphasisSpan("Warning:"),
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
                return new MdCompositeSpan(
                    genericType.Name,
                    "<",
                    genericType.TypeArguments.Select(t => GetMdSpan(t, noLink)).Join(", "),
                    ">"
                );
            }

            if (noLink)
            {
                return new MdTextSpan(type.DisplayName);
            }
            else
            {
                return CreateLink(type, type.DisplayName);
            }
        }

    }
}
