using System;

namespace MdDoc.Model
{
    public abstract class TypeMemberId : MemberId
    {
        public TypeId DefiningType { get; }


        protected TypeMemberId(TypeId definingType)
        {
            DefiningType = definingType ?? throw new ArgumentNullException(nameof(definingType));
        }

    }
}
