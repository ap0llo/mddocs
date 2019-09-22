using System;
using System.Collections.Generic;
using Grynwald.MarkdownGenerator;
using Grynwald.MdDocs.ApiReference.Model;
using Grynwald.Utilities.Collections;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Grynwald.MdDocs.ApiReference.Pages
{
    public class PageFactory
    {
        private readonly string m_RootOutputPath;
        private readonly ILogger m_Logger;
        private readonly AssemblyDocumentation m_Model;
        private readonly IDictionary<IDocumentation, IPage> m_Pages = new Dictionary<IDocumentation, IPage>();
        private readonly DocumentSet<IDocument> m_DocumentSet = new DocumentSet<IDocument>();


        public IEnumerable<IPage> AllPages => m_Pages.Values;


        public PageFactory(AssemblyDocumentation assemblyDocumentation, string outDir)
            : this(assemblyDocumentation, outDir, NullLogger.Instance)
        { }

        public PageFactory(AssemblyDocumentation assemblyDocumentation, string outDir, ILogger logger)
        {
            if (String.IsNullOrEmpty(outDir))
                throw new ArgumentException("Value must not be null or empty", nameof(outDir));

            m_RootOutputPath = outDir;
            m_Logger = logger ?? throw new ArgumentNullException(nameof(logger));

            m_Model = assemblyDocumentation ?? throw new ArgumentNullException(nameof(assemblyDocumentation));

            LoadPages();
        }


        public IPage TryGetPage(IDocumentation item)
        {
            switch (item)
            {
                // all overloads of an method / operator are combined to a single page
                // so when the page of an overload is requested, return the combined page                

                case MethodOverloadDocumentation methodOverload:
                    return TryGetPage(methodOverload.MethodDocumentation);

                case ConstructorOverloadDocumentation construtorOverload:
                    return TryGetPage(construtorOverload.ConstructorDocumentation);

                case OperatorOverloadDocumentation operatorOverload:
                    return TryGetPage(operatorOverload.OperatorDocumentation);

                default:
                    return m_Pages.GetValueOrDefault(item);
            }
        }

        public DocumentSet<IDocument> GetPages() => m_DocumentSet;


        private void LoadPages()
        {
            var linkProvider = new CompositeLinkProvider(new InternalLinkProvider(m_Model, this, m_DocumentSet));

            m_Logger.LogInformation("Loading pages");

            foreach (var @namespace in m_Model.MainModuleDocumentation.Namespaces)
            {
                var page = new NamespacePage(linkProvider, this, m_RootOutputPath, @namespace, m_Logger);
                m_Pages.Add(@namespace, page);
                m_DocumentSet.Add(page.RelativeOutputPath, page);
            }

            foreach (var type in m_Model.MainModuleDocumentation.Types)
            {
                var typePage = new TypePage(linkProvider, this, m_RootOutputPath, type, m_Logger);
                m_Pages.Add(type, typePage);
                m_DocumentSet.Add(typePage.RelativeOutputPath, typePage);

                foreach (var property in type.Properties)
                {
                    var page = new PropertyPage(linkProvider, this, m_RootOutputPath, property, m_Logger);
                    m_Pages.Add(property, page);
                    m_DocumentSet.Add(page.RelativeOutputPath, page);
                }

                foreach (var indexer in type.Indexers)
                {
                    var page = new IndexerPage(linkProvider, this, m_RootOutputPath, indexer, m_Logger);
                    m_Pages.Add(indexer, page);
                    m_DocumentSet.Add(page.RelativeOutputPath, page);
                }

                if (type.Constructors != null)
                {
                    var page = new ConstructorsPage(linkProvider, this, m_RootOutputPath, type.Constructors, m_Logger);
                    m_Pages.Add(type.Constructors, page);
                    m_DocumentSet.Add(page.RelativeOutputPath, page);
                }

                foreach (var method in type.Methods)
                {
                    var page = new MethodPage(linkProvider, this, m_RootOutputPath, method, m_Logger);
                    m_Pages.Add(method, page);
                    m_DocumentSet.Add(page.RelativeOutputPath, page);
                }

                if (type.Kind != TypeKind.Enum)
                {
                    foreach (var field in type.Fields)
                    {
                        var page = new FieldPage(linkProvider, this, m_RootOutputPath, field, m_Logger);
                        m_Pages.Add(field, page);
                        m_DocumentSet.Add(page.RelativeOutputPath, page);
                    }
                }

                foreach (var ev in type.Events)
                {
                    var page = new EventPage(linkProvider, this, m_RootOutputPath, ev, m_Logger);
                    m_Pages.Add(ev, page);
                    m_DocumentSet.Add(page.RelativeOutputPath, page);
                }

                foreach (var op in type.Operators)
                {
                    var page = new OperatorPage(linkProvider, this, m_RootOutputPath, op, m_Logger);
                    m_Pages.Add(op, page);
                    m_DocumentSet.Add(page.RelativeOutputPath, page);
                }
            }
        }
    }
}
