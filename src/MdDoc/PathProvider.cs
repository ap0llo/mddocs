using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MdDoc
{
    class PathProvider
    {
        private static char[] s_SplitChars = ".".ToCharArray();
        private readonly string m_RootOutputPath;

        public PathProvider(string rootOutputPath)
        {
            m_RootOutputPath = rootOutputPath ?? throw new ArgumentNullException(nameof(rootOutputPath));
        }

        public OutputPath GetOutputPath(TypeReference type)
        {            
            var path = Path.Combine(GetTypeDir(type), $"{type.Name}.md");
            return new OutputPath(path);
        }

        public OutputPath GetOutputPath(PropertyDefinition property)
        {            
            var path = Path.Combine(GetTypeDir(property.DeclaringType), "properties", $"{property.DeclaringType.Name}.{property.Name}.md");
            return new OutputPath(path);
        }

        public OutputPath GetConstructorsOutputPath(TypeReference type)
        {
            var path = Path.Combine(GetTypeDir(type), $"{type.Name}-constructors.md");
            return new OutputPath(path);
        }

        public OutputPath GetMethodOutputPath(MethodDefinition method) => GetMethodOutputPath(method.DeclaringType, method.Name);

        public OutputPath GetMethodOutputPath(TypeReference type, string methodName)
        {
            var path = Path.Combine(GetTypeDir(type), "methods", $"{type.Name}.{methodName}.md");
            return new OutputPath(path);
        }

        private string GetTypeDir(TypeReference type)
        {
            var dir = Path.Combine(m_RootOutputPath, String.Join('/', type.Namespace.Split(s_SplitChars)));
            return Path.Combine(dir, type.Name);
        }
    }
}
