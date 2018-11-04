using System;
using System.Collections.Generic;
using System.Text;

namespace MdDoc
{
    static class StringExtensions
    {
        public static string TrimEmptyLines(this string value)
        {
            
            if(value.Contains('\r') || value.Contains('\n'))
            {
                var index1 = value.IndexOf('\r');
                var index2 = value.IndexOf('\n');

                int index;
                if(index1 >= 0 && index2 >= 0)
                {
                    index = Math.Min(index1, index2);
                }
                else
                {
                    index = Math.Max(index1, index2);
                }

                var firstLine = value.Substring(0, index);
                if(String.IsNullOrWhiteSpace(firstLine))
                {
                    value = value.Remove(0, index).TrimStart('\r', '\n');                    
                }
            }

            //TODO: Trailing empty lines

            return value;
        }




        public static string GetLeadingWhitespace(this string value)
        {
            int i;
            for (i = 0; i < value.Length && char.IsWhiteSpace(value[i]); i++) ;

            return value.Substring(0, i);
        }

    }
}
