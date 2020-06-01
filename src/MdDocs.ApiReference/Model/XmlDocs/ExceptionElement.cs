using System;

namespace Grynwald.MdDocs.ApiReference.Model.XmlDocs
{
    /// <summary>
    /// Represents a <c><![CDATA[<exception>]]></c> element in XML documentation comments.
    /// </summary>
    /// <remarks>
    /// The <c>exception</c> tag can be used to indicate possible exceptions thrown by a method.
    /// <para>
    /// For a list of tags in documentation comments, see
    /// https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/xmldoc/recommended-tags-for-documentation-comments
    /// </para>
    /// </remarks>
    public sealed class ExceptionElement
    {
        /// <summary>
        /// Gets the type of the exception being referred to.
        /// </summary>
        public TypeId Type { get; }

        /// <summary>
        /// Gets the description for the exception.
        /// </summary>
        public TextBlock Text { get; }


        /// <summary>
        /// Initializes a new instance of <see cref="ExceptionElement"/>.
        /// </summary>
        /// <param name="type">The type of exception being referred to.</param>
        /// <param name="text">The descriptions for the exception</param>
        /// <exception cref="ArgumentNullException">Thrown when either <paramref name="type"/> or <paramref name="text"/> is <c>null</c>.</exception>
        public ExceptionElement(TypeId type, TextBlock text)
        {
            Type = type ?? throw new ArgumentNullException(nameof(type));
            Text = text ?? throw new ArgumentNullException(nameof(text));
        }
    }
}
