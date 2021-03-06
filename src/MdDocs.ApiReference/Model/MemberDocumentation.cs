﻿using System;

namespace Grynwald.MdDocs.ApiReference.Model
{
    /// <summary>
    /// Base documentation model for all type members (field, events, methods...)
    /// </summary>
    public abstract class MemberDocumentation : IAssemblyMemberDocumentation
    {
        /// <summary>
        /// Gets the documentation model for the type defining the member.
        /// </summary>
        public TypeDocumentation TypeDocumentation { get; }

        /// <inheritdoc />
        public AssemblyDocumentation AssemblyDocumentation => TypeDocumentation.GetAssemblyDocumentation();


        // private protected constructor => prevent implementation outside of this assembly
        private protected MemberDocumentation(TypeDocumentation typeDocumentation)
        {
            TypeDocumentation = typeDocumentation ?? throw new ArgumentNullException(nameof(typeDocumentation));
        }


        /// <inheritdoc />
        public abstract IDocumentation? TryGetDocumentation(MemberId id);
    }
}
