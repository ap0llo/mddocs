using System;

namespace MdDoc.Model.XmlDocs
{
    public class FieldId : MemberId
    {
        public TypeId DefiningType { get; }

        public string Name { get; }


        public FieldId(TypeId definingType, string name)
        {
            if (definingType == null)
                throw new ArgumentNullException(nameof(definingType));

            if (String.IsNullOrEmpty(name))
                throw new ArgumentException("Value must not be empty", nameof(name));

            DefiningType = definingType;
            Name = name;
        }
    }
}
