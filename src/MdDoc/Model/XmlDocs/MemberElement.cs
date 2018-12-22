using System;
using System.Collections.Generic;

namespace MdDoc.Model.XmlDocs
{
    public sealed class MemberElement
    {
        public MemberId MemberId { get; }

        public TextBlock Summary { get; }

        public TextBlock Remarks { get; }

        public TextBlock Example { get; }

        public IReadOnlyList<ExceptionElement> Exceptions { get; }

        public IReadOnlyList<TypeParamElement> TypeParameters { get; }

        public IReadOnlyList<ParamElement> Parameters { get; }

        public TextBlock Value { get; }

        public TextBlock Returns { get; }

        public IReadOnlyList<SeeAlsoElement> SeeAlso { get; }


        public MemberElement(
            MemberId memberId,
            TextBlock summary,
            TextBlock remarks,
            TextBlock example,
            IReadOnlyList<ExceptionElement> exceptions, 
            IReadOnlyList<TypeParamElement> typeParameters,
            IReadOnlyList<ParamElement> parameters,
            TextBlock value,
            TextBlock returns,
            IReadOnlyList<SeeAlsoElement> seeAlso)
        {
            MemberId = memberId ?? throw new ArgumentNullException(nameof(memberId));
            Summary = summary;
            Remarks = remarks;
            Example = example;
            Exceptions = exceptions ?? Array.Empty<ExceptionElement>();
            TypeParameters = typeParameters ?? Array.Empty<TypeParamElement>();
            Parameters = parameters ?? Array.Empty<ParamElement>();
            Value = value;
            Returns = returns;
            SeeAlso = seeAlso ?? Array.Empty<SeeAlsoElement>();
        }
        
    }
}
