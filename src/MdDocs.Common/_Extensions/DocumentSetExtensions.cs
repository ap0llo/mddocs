using Grynwald.MarkdownGenerator;

namespace Grynwald.MdDocs.Common
{
    public static class DocumentSetExtensions
    {
        public static void Save<T>(this DocumentSet<T> documentSet, string directoryPath, bool cleanOutputDirectory, MdSerializationOptions markdownOptions) where T : IDocument
        {
            documentSet.Save(directoryPath, cleanOutputDirectory, (document, path) =>
            {
                if (document is MdDocument mdDocument)
                {
                    mdDocument.Save(path, markdownOptions);
                }
                else
                {
                    document.Save(path);
                }
            });
        }
    }
}
