﻿using System;

namespace Grynwald.MdDocs.ApiReference.Model.XmlDocs
{
    /// <summary>
    /// Represents a <c><![CDATA[<c>]]></c> element in XML documentation comments.
    /// </summary>
    /// <remarks>
    /// For a list of tags in documentation comments, see
    /// https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/xmldoc/recommended-tags-for-documentation-comments
    /// </remarks>
    public sealed class CElement : Element
    {
        /// <summary>
        /// Gets the content of the element
        /// </summary>
        public string Content { get; }


        /// <summary>
        /// Initializes a new instance of <see cref="CElement"/>
        /// </summary>
        /// <param name="content">The content of the element.</param>
        public CElement(string content)
        {
            Content = content ?? throw new ArgumentNullException(nameof(content));
        }


        /// <inheritdoc />
        public override void Accept(IVisitor visitor) => visitor.Visit(this);
    }
}
