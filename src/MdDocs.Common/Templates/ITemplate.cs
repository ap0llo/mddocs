using Grynwald.MarkdownGenerator;

namespace Grynwald.MdDocs.Common.Templates
{
    /// <summary>
    /// Represents a template for converting a model of type <typeparamref name="T"/> to a set of documents
    /// </summary>    
    public interface ITemplate<T>
    {
        /// <summary>
        /// Gets the output documents for the specified model
        /// </summary>
        DocumentSet<IDocument> Render(T model);
    }
}
