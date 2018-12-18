namespace MdDoc.Model.XmlDocs
{
    public abstract class MemberId
    {

        // force re-implementation of equality members

        public abstract override bool Equals(object obj);

        public abstract override int GetHashCode();

    }
}
