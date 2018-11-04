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

        public string GetOutputPath(TypeReference type)
        {
            var dir = Path.Combine(m_RootOutputPath, String.Join('/', type.Namespace.Split(s_SplitChars)));
            return Path.Combine(dir, type.Name + ".md");
        }
    }
}
