using System;
using System.Collections.Generic;
using System.Text;

namespace Grynwald.MdDocs.CommandLineHelp
{
    internal static class StringBuilderExtensions
    {
        public static StringBuilder AppendIf(this StringBuilder stringBuilder, bool condition, string value) =>
            condition ? stringBuilder.Append(value) : stringBuilder;

        public static StringBuilder AppendIf(this StringBuilder stringBuilder, bool condition, char value) =>
            condition ? stringBuilder.Append(value) : stringBuilder;


        public static StringBuilder AppendIf(this StringBuilder stringBuilder, bool condition, params string?[] values)
        {
            if (condition)
            {
                foreach (var value in values)
                {
                    stringBuilder.Append(value);
                }
            }
            return stringBuilder;
        }
        public static StringBuilder Apply<T>(this StringBuilder stringBuilder, Action<StringBuilder, T> action, T parameter)
        {
            action(stringBuilder, parameter);
            return stringBuilder;
        }
    }
}
