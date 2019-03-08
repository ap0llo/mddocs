using System;

namespace Grynwald.MdDocs.ApiReference.Model
{
    public abstract class TypeMemberId : MemberId
    {
        public TypeId DefiningType { get; }

        public string Name { get; }


        protected TypeMemberId(TypeId definingType, string name)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentException("Value must not be empty", nameof(name));

            DefiningType = definingType ?? throw new ArgumentNullException(nameof(definingType));
            Name = name;
        }
    }
}
