using System;
using System.Collections.Generic;
using Grynwald.MdDocs.ApiReference.Model;
using Grynwald.Utilities.Collections;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Grynwald.MdDocs.ApiReference.Pages
{
    //TODO: Use DocumentSet
    public class PageFactory
    {
        private readonly string m_RootOutputPath;
        private readonly ILogger m_Logger;
        private readonly AssemblyDocumentation m_Model;
        private readonly IDictionary<IDocumentation, IPage> m_Pages = new Dictionary<IDocumentation, IPage>();


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

        public void SaveAll()
        {
            m_Logger.LogInformation($"Saving pages to output directory '{m_RootOutputPath}'");
            foreach (var page in AllPages)
            {
                page.Save();
            }
        }


        private void LoadPages()
        {
            m_Logger.LogInformation("Loading pages");

            foreach (var @namespace in m_Model.MainModuleDocumentation.Namespaces)
            {
                m_Pages.Add(@namespace, new NamespacePage(this, m_RootOutputPath, @namespace, m_Logger));
            }

            foreach (var type in m_Model.MainModuleDocumentation.Types)
            {
                m_Pages.Add(type, new TypePage(this, m_RootOutputPath, type, m_Logger));

                foreach (var property in type.Properties)
                {
                    m_Pages.Add(property, new PropertyPage(this, m_RootOutputPath, property, m_Logger));
                }

                foreach (var indexer in type.Indexers)
                {
                    m_Pages.Add(indexer, new IndexerPage(this, m_RootOutputPath, indexer, m_Logger));
                }

                if (type.Constructors != null)
                {
                    m_Pages.Add(type.Constructors, new ConstructorsPage(this, m_RootOutputPath, type.Constructors, m_Logger));
                }

                foreach (var method in type.Methods)
                {
                    m_Pages.Add(method, new MethodPage(this, m_RootOutputPath, method, m_Logger));
                }

                if (type.Kind != TypeKind.Enum)
                {
                    foreach (var field in type.Fields)
                    {
                        m_Pages.Add(field, new FieldPage(this, m_RootOutputPath, field, m_Logger));
                    }
                }

                foreach (var ev in type.Events)
                {
                    m_Pages.Add(ev, new EventPage(this, m_RootOutputPath, ev, m_Logger));
                }

                foreach (var op in type.Operators)
                {
                    m_Pages.Add(op, new OperatorPage(this, m_RootOutputPath, op, m_Logger));
                }
            }
        }
    }
}
