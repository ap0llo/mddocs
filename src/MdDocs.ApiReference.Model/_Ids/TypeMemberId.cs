using System;

namespace Grynwald.MdDocs.ApiReference.Model
{
    /// <summary>
    /// Identifies a type's member (methods, properties, fields ...)
    /// </summary>
    public abstract class TypeMemberId : MemberId
    {
        /// <summary>
        /// Gets the id of the type defining the member.
        /// </summary>
        public TypeId DefiningType { get; }

        /// <summary>
        /// Gets the member's name.
        /// </summary>
        public string Name { get; }


        // private protected constructor => prevent implementation outside of this assembly
        private protected TypeMemberId(TypeId definingType, string name)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentException("Value must not be empty", nameof(name));

            DefiningType = definingType ?? throw new ArgumentNullException(nameof(definingType));
            Name = name;
        }
    }
}
