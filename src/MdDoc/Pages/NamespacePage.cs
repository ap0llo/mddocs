using System.Collections.Generic;
using System.IO;
using System.Linq;
using Grynwald.MarkdownGenerator;
using MdDoc.Model;

using static Grynwald.MarkdownGenerator.FactoryMethods;

namespace MdDoc.Pages
{
    class NamespacePage : PageBase<NamespaceDocumentation>
    {
        public override OutputPath OutputPath { get; }


        public NamespacePage(PageFactory pageFactory, string rootOutputPath, NamespaceDocumentation model)
            : base(pageFactory, rootOutputPath, model)
        {
            OutputPath = new OutputPath(GetNamespaceDir(Model), "Namespace.md");
        }



        public override void Save()
        {
            var document = new MdDocument(
               Heading($"{Model.Name} Namespace", 1)
            );

            AddTypeTable(document.Root, "Classes", Model.Types.Where(x => x.Kind == TypeKind.Class));
            AddTypeTable(document.Root, "Structs", Model.Types.Where(x => x.Kind == TypeKind.Struct));
            AddTypeTable(document.Root, "Interfaces", Model.Types.Where(x => x.Kind == TypeKind.Interface));
            AddTypeTable(document.Root, "Enums", Model.Types.Where(x => x.Kind == TypeKind.Enum));

            //TODO: Sub-namespaces

            Directory.CreateDirectory(Path.GetDirectoryName(OutputPath));
            document.Save(OutputPath);
        }

        private void AddTypeTable(MdContainerBlock block, string heading, IEnumerable<TypeDocumentation> types)
        {
            if (!types.Any())
                return;

            block.Add(Heading(2, heading));

            var table = Table(Row("Name", "Description"));
            foreach (var type in types.OrderBy(x => x.DisplayName))
            {
                table.Add(
                    Row(
                        CreateLink(type.MemberId, type.DisplayName),
                        ConvertToSpan(type.Summary)
                ));
            }
            block.Add(table);
        }
    }
}
