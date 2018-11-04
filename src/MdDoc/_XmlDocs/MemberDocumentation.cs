using Grynwald.MarkdownGenerator;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace MdDoc
{
    class MemberDocumentation
    {
        public static readonly MemberDocumentation Empty = new MemberDocumentation();


        public MdSpan Summary { get; } = MdEmptySpan.Instance;


        private MemberDocumentation()
        { }

        public MemberDocumentation(XElement xml)
        {
            Summary = ConvertToMarkdown(xml?.Element("summary")?.Value);
        }





        private MdSpan ConvertToMarkdown(string value)
        {
            if (String.IsNullOrEmpty(value))
                return MdEmptySpan.Instance;


            var trimmed = value.TrimEmptyLines();


            var lines = trimmed.Split(new[] { '\r', '\n' });

            var prefix = lines[0].GetLeadingWhitespace();


            var stringBuilder = new StringBuilder();


            foreach(var line in lines)
            {
                if(line.StartsWith(prefix))
                {
                    stringBuilder.AppendLine(line.Remove(0, prefix.Length));
                }
                else
                {
                    stringBuilder.AppendLine(line);
                }
            }


            var result = stringBuilder.ToString();
            return new MdTextSpan(result);

        }


    }
}
