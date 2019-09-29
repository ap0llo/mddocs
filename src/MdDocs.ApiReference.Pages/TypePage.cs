using System;
using System.Collections.Generic;
using System.Linq;
using Grynwald.MarkdownGenerator;
using Grynwald.MdDocs.ApiReference.Model;
using Grynwald.MdDocs.Common.Pages;
using Microsoft.Extensions.Logging;

namespace Grynwald.MdDocs.ApiReference.Pages
{
    public sealed class TypePage : PageBase<TypeDocumentation>
    {
        private readonly ILogger m_Logger;


        internal TypePage(ILinkProvider linkProvider, TypeDocumentation model, ILogger logger)
            : base(linkProvider, model)
        {
            m_Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }



        public override void Save(string path)
        {
            m_Logger.LogInformation($"Saving page '{path}'");

            var document = new MdDocument(
                new MdHeading($"{Model.DisplayName} {Model.Kind}", 1)
            );

            AddObsoleteWarning(document.Root, Model);

            AddDefinitionSection(document.Root);

            AddTypeParametersSection(document.Root);

            AddRemarksSection(document.Root);

            //TODO: Skip constructors when it is compiler-generated, i.e. only the implicit default constructor
            AddOverloadableMembersSection(
                document.Root,
                "Constructors",
                (IEnumerable<OverloadDocumentation>)Model.Constructors?.Overloads ?? Array.Empty<OverloadDocumentation>()
            );

            AddSimpleMembersSection(document.Root, "Fields", Model.Fields);

            AddSimpleMembersSection(document.Root, "Events", Model.Events);

            AddSimpleMembersSection(document.Root, "Properties", Model.Properties);

            AddOverloadableMembersSection(document.Root, "Indexers", Model.Indexers.SelectMany(m => m.Overloads));

            AddOverloadableMembersSection(document.Root, "Methods", Model.Methods.SelectMany(m => m.Overloads));

            AddOverloadableMembersSection(document.Root, "Operators", Model.Operators.SelectMany(x => x.Overloads));

            //TODO: Explicit interface implementations

            //TODO: Extension methods

            AddExampleSection(document.Root);

            AddSeeAlsoSection(document.Root);

            document.Root.Add(new PageFooter());

            document.Save(path);
        }


        private void AddDefinitionSection(MdContainerBlock block)
        {
            // Add Namespace            
            block.Add(
                new MdParagraph(new MdStrongEmphasisSpan("Namespace:"), " ", GetMdSpan(Model.NamespaceDocumentation.NamespaceId))
            );

            // Add Assembly
            block.Add(
                new MdParagraph(new MdStrongEmphasisSpan("Assembly:"), " " + Model.AssemblyName)
            );

            if (!Model.Summary.IsEmpty)
            {
                block.Add(ConvertToBlock(Model.Summary));
            }

            // Definition as code
            block.Add(new MdCodeBlock(Model.CSharpDefinition, "csharp"));

            // Add list of base types            
            if (Model.InheritanceHierarchy.Count > 1)
            {
                block.Add(
                    new MdParagraph(
                        new MdStrongEmphasisSpan("Inheritance:"),
                        " ",
                        Model.InheritanceHierarchy.Select(GetMdSpan).Join(" → ")
                ));
            }

            // Add class attributes
            if (Model.Attributes.Count > 0)
            {
                block.Add(
                    new MdParagraph(
                        new MdStrongEmphasisSpan("Attributes:"),
                        " ",
                        Model.Attributes.Select(GetMdSpan).Join(",")
                ));
            }

            // Add list of implemented interfaces
            if (Model.ImplementedInterfaces.Count > 0)
            {
                block.Add(
                    new MdParagraph(
                        new MdStrongEmphasisSpan("Implements:"),
                        " ",
                        Model.ImplementedInterfaces.Select(GetMdSpan).Join(","))
                );
            }
        }

        private void AddRemarksSection(MdContainerBlock block)
        {
            if (!Model.Remarks.IsEmpty)
            {
                block.Add(new MdHeading(2, "Remarks"));
                block.Add(ConvertToBlock(Model.Remarks));
            }
        }

        private void AddOverloadableMembersSection(MdContainerBlock block, string sectionHeading, IEnumerable<OverloadDocumentation> overloads)
        {
            if (overloads.Any())
            {
                var table = new MdTable(new MdTableRow("Name", "Description"));

                foreach (var ctor in overloads.OrderBy(x => x.Signature))
                {
                    table.Add(
                        new MdTableRow(
                            CreateLink(ctor.MemberId, ctor.Signature),
                            ConvertToSpan(ctor.Summary)
                    ));
                }

                block.Add(
                    new MdHeading(sectionHeading, 2),
                    table
                );
            }
        }

        private void AddSimpleMembersSection(MdContainerBlock block, string sectionHeading, IEnumerable<SimpleMemberDocumentation> members)
        {
            if (members.Any())
            {
                block.Add(new MdHeading(sectionHeading, 2));

                var table = new MdTable(new MdTableRow("Name", "Description"));
                foreach (var member in members.OrderBy(x => x.Name))
                {
                    table.Add(
                        new MdTableRow(
                            CreateLink(member.MemberId, member.Name),
                            ConvertToSpan(member.Summary)
                    ));
                }
                block.Add(table);
            }
        }

        private void AddExampleSection(MdContainerBlock block)
        {
            if (Model.Example.IsEmpty)
                return;

            block.Add(new MdHeading("Example", 2));
            block.Add(ConvertToBlock(Model.Example));
        }

        private void AddSeeAlsoSection(MdContainerBlock block)
        {
            if (Model.SeeAlso.Count > 0)
            {
                block.Add(new MdHeading(2, "See Also"));
                block.Add(
                    new MdBulletList(
                        Model.SeeAlso.Select(seeAlso => new MdListItem(ConvertToSpan(seeAlso)))
                ));
            }
        }

        private void AddTypeParametersSection(MdContainerBlock block)
        {
            if (Model.TypeParameters.Count == 0)
                return;


            block.Add(new MdHeading("Type Parameters", 2));

            foreach (var typeParameter in Model.TypeParameters)
            {
                block.Add(
                    new MdParagraph(new MdCodeSpan(typeParameter.Name)
                ));

                if (typeParameter.Description != null)
                {
                    block.Add(ConvertToBlock(typeParameter.Description));
                }
            }
        }
    }
}
