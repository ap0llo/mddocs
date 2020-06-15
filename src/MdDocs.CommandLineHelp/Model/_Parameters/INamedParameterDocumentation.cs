namespace Grynwald.MdDocs.CommandLineHelp.Model
{
    // TODO: Documentation comments
    // TODO: Reconsider naming to emphasize difference between INamedParameterDocumentation (switch or named parameter) and NamedParameterDocumentation (named parameter)
    public interface INamedParameterDocumentation
    {
        bool HasName { get; }

        bool HasShortName { get; }

        string? Name { get; }

        string? ShortName { get; }
    }
}
