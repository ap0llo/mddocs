using Mono.Cecil;

namespace Grynwald.MdDocs.CommandLineHelp.Model
{
    //TODO: MetaValue, Min, Max
    public abstract class ParameterDocumentation
    {
        public string HelpText { get; }

        public bool Hidden { get; }

        public object Default { get; }


        public ParameterDocumentation(string helpText, bool hidden, object @default)
        {
            HelpText = helpText;
            Hidden = hidden;
            Default = @default;
        }

        protected ParameterDocumentation(CustomAttribute attribute)
        {
            Hidden = attribute.GetPropertyValueOrDefault<bool>("Hidden");
            Default = attribute.GetPropertyValueOrDefault<object>("Default");
            HelpText = attribute.GetPropertyValueOrDefault<string>("HelpText");
        }
    }
}
