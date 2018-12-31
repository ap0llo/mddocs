using System;

namespace MdDoc.Model.XmlDocs
{
    /// <summary>
    /// Represents a <c>seealso</c> xml docs element used to provide useful links to other members
    /// </summary>
    /// <remarks>
    /// <c>seealso</c> allows specifying links that will show up in the "See Also" section.
    /// <para>
    /// Supported for types, fields, event, properties and methods
    /// </para>
    /// </remarks>
    public class SeeAlsoElement
    {
        public MemberId MemberId { get; }

        public TextBlock Text { get; }


        public SeeAlsoElement(MemberId memberId, TextBlock text)
        {
            MemberId = memberId ?? throw new ArgumentNullException(nameof(memberId));
            Text = text ?? throw new ArgumentNullException(nameof(text));
        }
    }
}
