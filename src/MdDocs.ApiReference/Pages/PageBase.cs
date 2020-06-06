using System;
using System.Linq;
using Grynwald.MarkdownGenerator;
using Grynwald.MdDocs.ApiReference.Configuration;
using Grynwald.MdDocs.ApiReference.Model;
using Grynwald.MdDocs.ApiReference.Model.XmlDocs;

namespace Grynwald.MdDocs.ApiReference.Pages
{
    public abstract class PageBase<TModel> : IMdSpanFactory, IPage where TModel : class, IDocumentation
    {
        private readonly ILinkProvider m_LinkProvider;
        protected readonly ApiReferenceConfiguration m_Configuration;

        public TModel Model { get; }


        internal PageBase(ILinkProvider linkProvider, ApiReferenceConfiguration configuration, TModel model)
        {
            Model = model ?? throw new ArgumentNullException(nameof(model));

            m_LinkProvider = linkProvider ?? throw new ArgumentNullException(nameof(linkProvider));
            m_Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }


        public abstract void Save(string path);

        public abstract void Save(string path, MdSerializationOptions options);


        internal abstract MdDocument GetDocument();


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
                if (String.IsNullOrEmpty(link!.RelativePath))
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

        public virtual bool TryGetAnchor(MemberId id, out string? anchor)
        {
            anchor = default;
            return false;
        }

        protected MdSpan ConvertToSpan(TextBlock textBlock)
        {
            return textBlock == null ? MdEmptySpan.Instance : TextBlockToMarkdownConverter.ConvertToSpan(textBlock, this);
        }

        protected MdSpan ConvertToSpan(SeeAlsoElement seeAlso)
        {
            // While Visual Studio only allows referring to other code elements using the <c>cref</c> attribute,
            // linking to external resources (e.g. websites) is supported by as well using the <c>href</c> attribute.
            //
            // When a both attributes are present, the external link is ignored.

            // <seealso /> references another assembly member
            if (seeAlso.MemberId != null)
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
            // <seealso /> references an external resource
            else if (seeAlso.Target != null)
            {
                if (seeAlso.Text.IsEmpty)
                {
                    return new MdLinkSpan(seeAlso.Target.ToString(), seeAlso.Target);
                }
                else
                {
                    var text = ConvertToSpan(seeAlso.Text);
                    return new MdLinkSpan(text, seeAlso.Target);
                }
            }
            else
            {
                throw new InvalidOperationException($"Encountered instance of {nameof(SeeAlsoElement)} where both {nameof(SeeAlsoElement.MemberId)} and {nameof(SeeAlsoElement.Target)} were null.");
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
