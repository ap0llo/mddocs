using MdDoc.Model;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MdDoc
{
    public class PageFactory
    {
        private readonly PathProvider m_PathProvider;        
        private readonly AssemblyDocumentation m_Model;

        public PageFactory(AssemblyDocumentation assemblyDocumentation, string outDir)
        {
            if (string.IsNullOrEmpty(outDir))
                throw new ArgumentException("Value must not be null or empty", nameof(outDir));

            m_PathProvider = new PathProvider(outDir);
            m_Model = assemblyDocumentation ?? throw new ArgumentNullException(nameof(assemblyDocumentation));
        }

                

        public IEnumerable<IPage> GetPages()
        {
            foreach (var type in m_Model.MainModuleDocumentation.Types)
            {                
                yield return new TypePage(m_Model.Context, m_PathProvider, type);                

                foreach (var property in type.Properties)
                {
                    yield return new PropertyPage(m_Model.Context, m_PathProvider, property);                    
                }

                if(type.Constructors != null)
                {
                    yield return new ConstructorsPage(m_Model.Context, m_PathProvider, type.Constructors);
                }
                                                
                foreach(var method in type.Methods)
                {
                    yield return new MethodPage(m_Model.Context, m_PathProvider, method);
                }

                //TODO: Events, Fields, Operators
            }
        }
    }
}
