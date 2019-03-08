using Grynwald.MdDocs.ApiReference.Model.XmlDocs;

namespace Grynwald.MdDocs.ApiReference.Model
{
    public abstract class MemberId
    {

        // force re-implementation of equality members

        public abstract override bool Equals(object obj);

        public abstract override int GetHashCode();


        public static MemberId Parse(string value)
        {
            var parser = new MemberIdParser(value);
            return parser.Parse();
        }
    }
}
