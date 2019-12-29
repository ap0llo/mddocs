using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Mono.Cecil;

namespace Grynwald.MdDocs.CommandLineHelp.Model
{
    /// <summary>
    /// Base class for all command documentation classes
    /// </summary>
    public abstract class CommandDocumentationBase
    {
        /// <summary>
        /// Gets the application the command belongs to.
        /// </summary>
        public ApplicationDocumentation Application { get; }

        /// <summary>
        /// Gets the command's options (named parameters)
        /// </summary>
        public IReadOnlyList<OptionDocumentation> Options { get; } = Array.Empty<OptionDocumentation>();

        /// <summary>
        /// Gets the commands's values (unnamed parameters)
        /// </summary>
        public IReadOnlyList<ValueDocumentation> Values { get; } = Array.Empty<ValueDocumentation>();

        /// <summary>
        /// Gets the commands parameters (both named and unnamed). 
        /// </summary>
        /// <remarks>
        /// Values is equivalent to <c>Options.Union(Values)</c>
        /// </remarks>
        public IReadOnlyList<ParameterDocumentation> Parameters { get; } = Array.Empty<ParameterDocumentation>();


        // protected internal: Prevent implementations outside the assembly (abstract classes cannot be sealed)
        protected internal CommandDocumentationBase(
            ApplicationDocumentation application,
            IEnumerable<OptionDocumentation>? options = null,
            IEnumerable<ValueDocumentation>? values = null)
        {
            Application = application ?? throw new ArgumentNullException(nameof(application));

            Options = options?.Where(x => !x.Hidden)?.OrderBy(x => x.Name ?? x.ShortName?.ToString())?.ToArray() ?? Array.Empty<OptionDocumentation>();
            Values = values?.Where(x => !x.Hidden)?.OrderBy(x => x.Index)?.ToArray() ?? Array.Empty<ValueDocumentation>();
            Parameters = Values.Cast<ParameterDocumentation>().Concat(Options).ToArray();
        }



        protected static IReadOnlyList<OptionDocumentation> LoadOptions(TypeDefinition definition, ILogger logger)
        {
            return definition.GetAllProperties()
                .WithAttribute(Constants.OptionAttributeFullName)
                .Select(property => OptionDocumentation.FromPropertyDefinition(property, logger))
                .Where(option => !option.Hidden)
                .ToArray();
        }

        protected static IReadOnlyList<ValueDocumentation> LoadValues(TypeDefinition definition, ILogger logger)
        {
            return definition.GetAllProperties()
                .WithAttribute(Constants.ValueAttributeFullName)
                .Select(property => ValueDocumentation.FromPropertyDefinition(property, logger))
                .Where(value => !value.Hidden)
                .ToArray();
        }
    }
}
