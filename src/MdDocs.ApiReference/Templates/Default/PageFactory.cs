using System;
using System.Collections.Generic;
using Grynwald.MarkdownGenerator;
using Grynwald.MdDocs.ApiReference.Configuration;
using Grynwald.MdDocs.ApiReference.Model;
using Grynwald.Utilities.Collections;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Grynwald.MdDocs.ApiReference.Templates.Default
{
    public class PageFactory
    {
        private readonly ILogger m_Logger;
        private readonly IApiReferencePathProvider m_PathProvider;
        private readonly ApiReferenceConfiguration m_Configuration;
        private readonly AssemblySetDocumentation m_Model;
        private readonly IDictionary<IDocumentation, IPage> m_PagesByModel = new Dictionary<IDocumentation, IPage>();
        private readonly DocumentSet<IDocument> m_DocumentSet = new DocumentSet<IDocument>();


        public PageFactory(IApiReferencePathProvider pathProvider, ApiReferenceConfiguration configuration, AssemblySetDocumentation assemblySet)
            : this(pathProvider, configuration, assemblySet, NullLogger.Instance)
        { }

        public PageFactory(IApiReferencePathProvider pathProvider, ApiReferenceConfiguration configuration, AssemblySetDocumentation assemblySet, ILogger logger)
        {
            m_Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            m_PathProvider = pathProvider ?? throw new ArgumentNullException(nameof(pathProvider));
            m_Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            m_Model = assemblySet ?? throw new ArgumentNullException(nameof(assemblySet));

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

            foreach (var @namespace in m_Model.Namespaces)
            {
                var page = new NamespacePage(linkProvider, m_Configuration, @namespace, m_Logger);
                m_PagesByModel.Add(@namespace, page);
                m_DocumentSet.Add(m_PathProvider.GetPath(page), page);
            }

            foreach (var assembly in m_Model.Assemblies)
            {
                foreach (var type in assembly.Types)
                {
                    var typePage = new TypePage(linkProvider, m_Configuration, type, m_Logger);
                    m_PagesByModel.Add(type, typePage);
                    m_DocumentSet.Add(m_PathProvider.GetPath(typePage), typePage);

                    foreach (var property in type.Properties)
                    {
                        var page = new PropertyPage(linkProvider, m_Configuration, property, m_Logger);
                        m_PagesByModel.Add(property, page);
                        m_DocumentSet.Add(m_PathProvider.GetPath(page), page);
                    }

                    foreach (var indexer in type.Indexers)
                    {
                        var page = new IndexerPage(linkProvider, m_Configuration, indexer, m_Logger);
                        m_PagesByModel.Add(indexer, page);
                        m_DocumentSet.Add(m_PathProvider.GetPath(page), page);
                    }

                    if (type.Constructors != null)
                    {
                        var page = new ConstructorsPage(linkProvider, m_Configuration, type.Constructors, m_Logger);
                        m_PagesByModel.Add(type.Constructors, page);
                        m_DocumentSet.Add(m_PathProvider.GetPath(page), page);
                    }

                    foreach (var method in type.Methods)
                    {
                        var page = new MethodPage(linkProvider, m_Configuration, method, m_Logger);
                        m_PagesByModel.Add(method, page);
                        m_DocumentSet.Add(m_PathProvider.GetPath(page), page);
                    }

                    if (type.Kind != TypeKind.Enum)
                    {
                        foreach (var field in type.Fields)
                        {
                            var page = new FieldPage(linkProvider, m_Configuration, field, m_Logger);
                            m_PagesByModel.Add(field, page);
                            m_DocumentSet.Add(m_PathProvider.GetPath(page), page);
                        }
                    }

                    foreach (var ev in type.Events)
                    {
                        var page = new EventPage(linkProvider, m_Configuration, ev, m_Logger);
                        m_PagesByModel.Add(ev, page);
                        m_DocumentSet.Add(m_PathProvider.GetPath(page), page);
                    }

                    foreach (var op in type.Operators)
                    {
                        var page = new OperatorPage(linkProvider, m_Configuration, op, m_Logger);
                        m_PagesByModel.Add(op, page);
                        m_DocumentSet.Add(m_PathProvider.GetPath(page), page);
                    }
                }
            }

        }
    }
}
