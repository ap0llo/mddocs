namespace Grynwald.MdDocs.ApiReference.Model.XmlDocs
{
    internal struct MemberIdToken
    {
        public string Value { get; set; }

        public MemberIdTokenKind Kind { get; set; }


        public MemberIdToken(MemberIdTokenKind kind, char value)
        {
            Kind = kind;
            Value = value.ToString();
        }

        public MemberIdToken(MemberIdTokenKind kind, string value)
        {
            Kind = kind;
            Value = value;
        }


        public override string ToString() => $"{nameof(MemberIdTokenKind)}.{Kind}: '{Value}'";
    }
}
