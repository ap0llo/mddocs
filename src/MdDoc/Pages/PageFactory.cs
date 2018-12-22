using System;
using System.Collections.Generic;
using Grynwald.Utilities.Collections;
using MdDoc.Model;

namespace MdDoc.Pages
{
    public class PageFactory
    {
        private readonly string m_RootOutputPath;        
        private readonly AssemblyDocumentation m_Model;
        private readonly IDictionary<IDocumentation, IPage> m_Pages = new Dictionary<IDocumentation, IPage>();


        public IEnumerable<IPage> AllPages => m_Pages.Values;


        public PageFactory(AssemblyDocumentation assemblyDocumentation, string outDir)
        {
            if (string.IsNullOrEmpty(outDir))
                throw new ArgumentException("Value must not be null or empty", nameof(outDir));

            m_RootOutputPath = outDir;
            m_Model = assemblyDocumentation ?? throw new ArgumentNullException(nameof(assemblyDocumentation));


            LoadPages();
        }


        public IPage TryGetPage(IDocumentation item)
        {
            switch (item)
            {
                // all overloads of an method / operator are combined to a single page
                // so when the page of an overlaod is requested, return the combined page                

                case MethodOverloadDocumentation methodOverload:
                    return TryGetPage(methodOverload.MethodDocumentation);

                case OperatorOverloadDocumentation operatorOverload:
                    return TryGetPage(operatorOverload.OperatorDocumentation);
                    
                default:
                    return m_Pages.GetValueOrDefault(item);
            }
        }

        private void LoadPages()
        {
            foreach (var type in m_Model.MainModuleDocumentation.Types)
            {
                m_Pages.Add(type, new TypePage(this, m_RootOutputPath, type));
                
                foreach (var property in type.Properties)
                {
                    m_Pages.Add(property, new PropertyPage(this, m_RootOutputPath, property));
                }

                if (type.Constructors != null)
                {
                    m_Pages.Add(type.Constructors, new ConstructorsPage(this, m_RootOutputPath, type.Constructors));
                }

                foreach (var method in type.Methods)
                {
                    m_Pages.Add(method, new MethodPage(this, m_RootOutputPath, method));
                }

                foreach(var field in type.Fields)
                {
                    m_Pages.Add(field, new FieldPage(this, m_RootOutputPath, field));
                }

                foreach(var ev in type.Events)
                {
                    m_Pages.Add(ev, new EventPage(this, m_RootOutputPath, ev));
                }

                foreach(var op in type.Operators)
                {
                    m_Pages.Add(op, new OperatorPage(this, m_RootOutputPath, op));
                }                
            }
        }
    }
}
