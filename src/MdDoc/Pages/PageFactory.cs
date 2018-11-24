using Grynwald.Utilities.Collections;
using MdDoc.Model;
using System;
using System.Collections.Generic;

namespace MdDoc.Pages
{
    public class PageFactory
    {
        private readonly string m_RootOutputPath;        
        private readonly AssemblyDocumentation m_Model;
        private readonly IDictionary<TypeDocumentation, TypePage> m_TypePages = new Dictionary<TypeDocumentation, TypePage>();
        private readonly IDictionary<PropertyDocumentation, PropertyPage> m_PropertyPages = new Dictionary<PropertyDocumentation, PropertyPage>();
        private readonly IDictionary<ConstructorDocumentation, ConstructorsPage> m_ConstructorPages = new Dictionary<ConstructorDocumentation, ConstructorsPage>();
        private readonly IDictionary<MethodDocumentation, MethodPage> m_MethodPages = new Dictionary<MethodDocumentation, MethodPage>();


        public IEnumerable<IPage> AllPages
        {
            get
            {
                foreach (var page in m_TypePages.Values)
                    yield return page;

                foreach (var page in m_PropertyPages.Values)
                    yield return page;

                foreach (var page in m_ConstructorPages.Values)
                    yield return page;

                foreach (var page in m_MethodPages.Values)
                    yield return page;
            }
        }


        public PageFactory(AssemblyDocumentation assemblyDocumentation, string outDir)
        {
            if (string.IsNullOrEmpty(outDir))
                throw new ArgumentException("Value must not be null or empty", nameof(outDir));

            m_RootOutputPath = outDir;
            m_Model = assemblyDocumentation ?? throw new ArgumentNullException(nameof(assemblyDocumentation));


            LoadPages();
        }


        public IPage TryGetPage(TypeDocumentation type) => m_TypePages.GetValueOrDefault(type);

        public IPage TryGetPage(PropertyDocumentation property) => m_PropertyPages.GetValueOrDefault(property);

        public IPage TryGetPage(ConstructorDocumentation constructor) => m_ConstructorPages.GetValueOrDefault(constructor);

        public IPage TryGetPage(MethodDocumentation method) => m_MethodPages.GetValueOrDefault(method);

        public IPage TryGetPage(OperatorDocumentation op) => null; //TODO


        private void LoadPages()
        {
            foreach (var type in m_Model.MainModuleDocumentation.Types)
            {
                m_TypePages.Add(type, new TypePage(this, m_RootOutputPath, type));
                
                foreach (var property in type.Properties)
                {
                    m_PropertyPages.Add(property, new PropertyPage(this, m_RootOutputPath, property));
                }

                if (type.Constructors != null)
                {
                    m_ConstructorPages.Add(type.Constructors, new ConstructorsPage(this, m_RootOutputPath, type.Constructors));
                }

                foreach (var method in type.Methods)
                {
                    m_MethodPages.Add(method, new MethodPage(this, m_RootOutputPath, method));
                }

                //TODO: Events, Fields, Operators
            }
        }
    }
}
