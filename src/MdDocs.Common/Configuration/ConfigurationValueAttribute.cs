using System;

namespace Grynwald.MdDocs.Common.Configuration
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class ConfigurationValueAttribute : Attribute
    {
        public string Key { get; }

        public ConfigurationValueAttribute(string key)
        {
            Key = key;
        }
    }
}
