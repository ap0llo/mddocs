using System;
using System.Collections.Generic;

namespace Grynwald.MdDocs.MSBuild.IntegrationTest
{
    internal class ProcessArgumentsBuilder
    {
        private readonly List<string> m_Arguments = new List<string>();


        public ProcessArgumentsBuilder Append(string argument)
        {
            if (String.IsNullOrWhiteSpace(argument))
                throw new ArgumentException("Value must not be null or whitespace", nameof(argument));

            m_Arguments.Add(argument);
            return this;
        }

        public ProcessArgumentsBuilder AppendQuoted(string argument)
        {
            if (String.IsNullOrWhiteSpace(argument))
                throw new ArgumentException("Value must not be null or whitespace", nameof(argument));

            m_Arguments.Add($"\"{argument}\"");
            return this;
        }


        public override string ToString() => String.Join(" ", m_Arguments);
    }
}
