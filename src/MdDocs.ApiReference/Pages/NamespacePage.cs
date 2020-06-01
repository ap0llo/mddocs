﻿using System.Collections.Generic;
using System.Linq;
using Grynwald.MarkdownGenerator;
using Grynwald.MdDocs.ApiReference.Model;
using Grynwald.MdDocs.Common.Pages;
using Microsoft.Extensions.Logging;

namespace Grynwald.MdDocs.ApiReference.Pages
{
    public sealed class NamespacePage : PageBase<NamespaceDocumentation>
    {
        private readonly ILogger m_Logger;


        internal NamespacePage(ILinkProvider linkProvider, NamespaceDocumentation model, ILogger logger)
            : base(linkProvider, model)
        {
            m_Logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
        }

        public sealed override void Save(string path) => GetDocument().Save(path);

        public sealed override void Save(string path, MdSerializationOptions markdownOptions) => GetDocument().Save(path, markdownOptions);


        private MdDocument GetDocument()
        {
            var document = new MdDocument(
               new MdHeading($"{Model.Name} Namespace", 1)
            );

            // Add parent Namespace
            if (Model.ParentNamespaceDocumentation != null)
            {
                document.Root.Add(
                    new MdParagraph(new MdStrongEmphasisSpan("Namespace:"), " ", GetMdSpan(Model.ParentNamespaceDocumentation.NamespaceId))
                );
            }

            AddNamespacesList(document.Root);

            AddTypeTable(document.Root, "Classes", Model.Types.Where(x => x.Kind == TypeKind.Class));
            AddTypeTable(document.Root, "Structs", Model.Types.Where(x => x.Kind == TypeKind.Struct));
            AddTypeTable(document.Root, "Interfaces", Model.Types.Where(x => x.Kind == TypeKind.Interface));
            AddTypeTable(document.Root, "Enums", Model.Types.Where(x => x.Kind == TypeKind.Enum));

            document.Root.Add(new PageFooter());

            return document;
        }

        private void AddNamespacesList(MdContainerBlock block)
        {
            if (!Model.Namespaces.Any())
                return;

            block.Add(new MdHeading(2, "Namespaces"));

            block.Add(
                new MdBulletList(
                    Model.Namespaces
                        .OrderBy(x => x.Name)
                        .Select(@namespace => new MdListItem(GetMdSpan(@namespace.NamespaceId))
            )));

        }

        private void AddTypeTable(MdContainerBlock block, string heading, IEnumerable<TypeDocumentation> types)
        {
            if (!types.Any())
                return;

            block.Add(new MdHeading(2, heading));

            var table = new MdTable(new MdTableRow("Name", "Description"));
            foreach (var type in types.OrderBy(x => x.DisplayName))
            {
                table.Add(
                    new MdTableRow(
                        CreateLink(type.MemberId, type.DisplayName),
                        ConvertToSpan(type.Summary)
                ));
            }
            block.Add(table);
        }
    }
}