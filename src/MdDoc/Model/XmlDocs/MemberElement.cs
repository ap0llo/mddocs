using System;
using System.Collections.Generic;
using System.Text;

namespace MdDoc.Model.XmlDocs
{
    public sealed class MemberElement
    {
        public string Id { get;}


        public SummaryElement Summary { get; }

        public RemarksElement Remarks { get; }

        public ExampleElement Example { get; }

        public IReadOnlyList<ExceptionElement> Exceptions { get; }

        public IReadOnlyList<TypeParamElement> TypeParameters { get; }

        public IReadOnlyList<ParamElement> Parameters { get; }

        public ValueElement Value { get; }

        public ReturnsElement Returns { get; }


        public MemberElement(
            string id, 
            SummaryElement summary,
            RemarksElement remarks,
            ExampleElement example,
            IReadOnlyList<ExceptionElement> exceptions, 
            IReadOnlyList<TypeParamElement> typeParameters,
            IReadOnlyList<ParamElement> parameters,
            ValueElement value,
            ReturnsElement returns)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentException("Value must not be empty", nameof(id));

            Id = id;
            Summary = summary;
            Remarks = remarks;
            Example = example;
            Exceptions = exceptions ?? Array.Empty<ExceptionElement>();
            TypeParameters = typeParameters ?? Array.Empty<TypeParamElement>();
            Parameters = parameters ?? Array.Empty<ParamElement>();
            Value = value;
            Returns = returns;
        }
        
    }
}
