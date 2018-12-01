using System;
using System.Collections.Generic;
using System.Text;

namespace MdDoc.Model.XmlDocs
{
    internal static class StringExtensions
    {    
        /// <summary>
        /// Removes leading and trailing empty lines from the string
        /// </summary>
        public static string TrimEmptyLines(this string value)
        {
            // trim start
            {
                // find end of leading whitespace 
                // band the last index of a new line character within that whitespace
                var endOfWhiteSpace = -1;
                var lastNewLine = -1;
                for (var i = 0; i < value.Length; i++)
                {
                    if (value[i] == '\r' || value[i] == '\n')
                    {
                        lastNewLine = i;
                    }

                    if (!char.IsWhiteSpace(value[i]))
                    {
                        endOfWhiteSpace = i;
                        break;
                    }
                }

                // no leading whitespace => keep orignal value
                // no newline in whitespace => keep orignal value
                // otherwise:
                //   remove leading whitespace until the last newline character                 
                if (endOfWhiteSpace >= 0 && lastNewLine >= 0)
                {
                    value = value.Remove(0, lastNewLine + 1);
                }
            }

            // trim end
            {
                // find start of trailing whitespace (search from the end)
                // and the first new line within that whitespace
                var startOfWhiteSpace = -1;
                var firstNewLine = -1;
                for (var i = value.Length - 1; i > 0; i--)
                {
                    if (value[i] == '\r' || value[i] == '\n')
                    {
                        firstNewLine = i;
                    }

                    if (!char.IsWhiteSpace(value[i]))
                    {
                        startOfWhiteSpace = i;
                        break;
                    }
                }

                if(startOfWhiteSpace >= 0 && firstNewLine >= 0)
                {
                    // if the first new line is a '\r' folowed by '\n', 
                    // move increment by one character and treat \r\n as the line break
                    if (value[firstNewLine] == '\r' && firstNewLine +1 < value.Length && value[firstNewLine + 1] == '\n')
                        firstNewLine += 1;

                    // remove all chars after the line break
                    // do nothign if the line break is the last character of the string
                    if (firstNewLine < value.Length  - 1)
                    {
                        value = value.Remove(firstNewLine + 1);                                        
                    }
                }
            }
        
            return value;
        }

    }
}
