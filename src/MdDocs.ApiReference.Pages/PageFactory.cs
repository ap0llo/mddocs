using System;
using System.Collections.Generic;
using System.IO;
using Grynwald.MarkdownGenerator;
using Grynwald.MdDocs.ApiReference.Model;
using Grynwald.Utilities.Collections;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Grynwald.MdDocs.ApiReference.Pages
{
    public class PageFactory
    {
        private readonly ILogger m_Logger;
        private readonly IPathProvider m_PathProvider;
        private readonly AssemblyDocumentation m_Model;
        private readonly IDictionary<IDocumentation, IPage> m_PagesByModel = new Dictionary<IDocumentation, IPage>();
        private readonly DocumentSet<IDocument> m_DocumentSet = new DocumentSet<IDocument>();


        public PageFactory(IPathProvider pathProvider, AssemblyDocumentation assemblyDocumentation)
            : this(pathProvider, assemblyDocumentation, NullLogger.Instance)
        { }

        public PageFactory(IPathProvider pathProvider, AssemblyDocumentation assemblyDocumentation, ILogger logger)
        {
            m_Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            m_PathProvider = pathProvider ?? throw new ArgumentNullException(nameof(pathProvider));
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
                    return m_PagesByModel.GetValueOrDefault(item);
            }
        }

        public DocumentSet<IDocument> GetPages() => m_DocumentSet;


        private void LoadPages()
        {
            var linkProvider = new CompositeLinkProvider(new InternalLinkProvider(m_Model, this, m_DocumentSet));

            m_Logger.LogInformation("Loading pages");

            foreach (var @namespace in m_Model.MainModuleDocumentation.Namespaces)
            {
                var page = new NamespacePage(linkProvider, @namespace, m_Logger);
                m_PagesByModel.Add(@namespace, page);
                m_DocumentSet.Add(m_PathProvider.GetPath(page), page);
            }

            foreach (var type in m_Model.MainModuleDocumentation.Types)
            {
                var typePage = new TypePage(linkProvider, type, m_Logger);
                m_PagesByModel.Add(type, typePage);
                m_DocumentSet.Add(m_PathProvider.GetPath(typePage), typePage);

                foreach (var property in type.Properties)
                {
                    var page = new PropertyPage(linkProvider, property, m_Logger);
                    m_PagesByModel.Add(property, page);
                    m_DocumentSet.Add(m_PathProvider.GetPath(page), page);
                }

                foreach (var indexer in type.Indexers)
                {
                    var page = new IndexerPage(linkProvider, indexer, m_Logger);
                    m_PagesByModel.Add(indexer, page);
                    m_DocumentSet.Add(m_PathProvider.GetPath(page), page);
                }

                if (type.Constructors != null)
                {
                    var page = new ConstructorsPage(linkProvider, type.Constructors, m_Logger);
                    m_PagesByModel.Add(type.Constructors, page);
                    m_DocumentSet.Add(m_PathProvider.GetPath(page), page);
                }

                foreach (var method in type.Methods)
                {
                    var page = new MethodPage(linkProvider, method, m_Logger);
                    m_PagesByModel.Add(method, page);
                    m_DocumentSet.Add(m_PathProvider.GetPath(page), page);
                }

                if (type.Kind != TypeKind.Enum)
                {
                    foreach (var field in type.Fields)
                    {
                        var page = new FieldPage(linkProvider, field, m_Logger);
                        m_PagesByModel.Add(field, page);
                        m_DocumentSet.Add(m_PathProvider.GetPath(page), page);
                    }
                }

                foreach (var ev in type.Events)
                {
                    var page = new EventPage(linkProvider, ev, m_Logger);
                    m_PagesByModel.Add(ev, page);
                    m_DocumentSet.Add(m_PathProvider.GetPath(page), page);
                }

                foreach (var op in type.Operators)
                {
                    var page = new OperatorPage(linkProvider, op, m_Logger);
                    m_PagesByModel.Add(op, page);
                    m_DocumentSet.Add(m_PathProvider.GetPath(page), page);
                }
            }
        }
    }
}
