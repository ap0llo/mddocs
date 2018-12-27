using System;
using NuDoq;

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
        private readonly SeeAlso m_NuDoqModel;


        public string Cref => m_NuDoqModel.Cref;

        public MemberId MemberId { get; }

        public TextBlock Text { get; }


        public SeeAlsoElement(SeeAlso nuDoqModel, TextBlock text)
        {
            m_NuDoqModel = nuDoqModel ?? throw new ArgumentNullException(nameof(nuDoqModel));
            Text = text ?? throw new ArgumentNullException(nameof(text));
            MemberId = new MemberIdParser(nuDoqModel.Cref).Parse();
        }
    }
}
