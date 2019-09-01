using Mono.Cecil;

namespace Grynwald.MdDocs.CommandLineHelp.Model
{
    //TODO: Min, Max
    public abstract class ParameterDocumentation
    {
        public bool Required { get; }

        public string HelpText { get; }

        public bool Hidden { get; }

        public object Default { get; }

        public string MetaValue { get; }


        public ParameterDocumentation(bool required, string helpText, bool hidden, object @default, string metaValue)
        {
            Required = required;
            HelpText = helpText;
            Hidden = hidden;
            Default = @default;
            MetaValue = metaValue;
        }


        protected ParameterDocumentation(CustomAttribute attribute)
        {
            Required = attribute.GetPropertyValueOrDefault<bool>("Required");
            Hidden = attribute.GetPropertyValueOrDefault<bool>("Hidden");
            Default = attribute.GetPropertyValueOrDefault<object>("Default");
            HelpText = attribute.GetPropertyValueOrDefault<string>("HelpText");
            MetaValue = attribute.GetPropertyValueOrDefault<string>("MetaValue");
        }
    }
}
