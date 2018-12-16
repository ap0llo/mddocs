using NuDoq;
using System;
using System.Collections.Generic;
using System.Text;

namespace MdDoc.Model.XmlDocs
{
    static class ModelConverter
    {
        public static IReadOnlyList<MemberElement> ConvertModel(DocumentMembers nuDoqModel)
        {
            var result = new List<MemberElement>();
            foreach(Member member in nuDoqModel.Elements)
            {
                var visitor = new MemberVisitor();
                member.Accept(visitor);

                var element = new MemberElement(member.Id,
                    visitor.Summary,
                    visitor.Remarks,
                    visitor.Example,
                    visitor.Exceptions,
                    visitor.TypeParameters,
                    visitor.Parameters,
                    visitor.Value,
                    visitor.Returns);

                result.Add(element);
            }

            return result;
        }
    }


    class MemberVisitor : Visitor
    {
        public SummaryElement Summary { get; private set; }

        public RemarksElement Remarks { get; private set; }

        public ExampleElement Example { get; private set; }

        public List<ExceptionElement> Exceptions { get; } = new List<ExceptionElement>();

        public List<TypeParamElement> TypeParameters { get; } = new List<TypeParamElement>();

        public List<ParamElement> Parameters { get; } = new List<ParamElement>();

        public ValueElement Value { get; private set; }

        public ReturnsElement Returns { get; private set; }



        public override void VisitSummary(Summary summary)
        {
            Summary = new SummaryElement(GetElements(summary));
        }

        public override void VisitExample(Example example)
        {
            Example = new ExampleElement(GetElements(example));
        }

        public override void VisitRemarks(Remarks remarks)
        {
            Remarks = new RemarksElement(GetElements(remarks));
        }

        public override void VisitException(NuDoq.Exception exception)
        {
            Exceptions.Add(new ExceptionElement(exception, GetElements(exception)));
        }

        public override void VisitTypeParam(TypeParam typeParam)
        {
            TypeParameters.Add(new TypeParamElement(typeParam, GetElements(typeParam)));
        }

        public override void VisitParam(Param param)
        {
            Parameters.Add(new ParamElement(param, GetElements(param)));
        }

        public override void VisitValue(Value value)
        {
            Value = new ValueElement(GetElements(value));
        }

        public override void VisitReturns(Returns returns)
        {
            Returns = new ReturnsElement(GetElements(returns));
        }

        private IEnumerable<Element> GetElements(Container container)
        {
            var visitor = new ContainerVisitor();

            foreach (var element in container.Elements)
            {
                element.Accept(visitor);
            }

            return visitor.Elements;
        }

    }

    class ContainerVisitor : Visitor
    {
        public List<Element> Elements { get; } = new List<Element>();


        //public override void VisitMember(Member member) => Result.Add(new MemberElement(member, GetElements(member)));

        public override void VisitCode(Code code) => Elements.Add(new CodeElement(code));

        public override void VisitC(C code) => Elements.Add(new CElement(code));

        public override void VisitParamRef(ParamRef paramRef) => Elements.Add(new ParamRefElement(paramRef));

        public override void VisitTypeParamRef(TypeParamRef typeParamRef) => Elements.Add(new TypeParamRefElement(typeParamRef));

        public override void VisitText(Text text) => Elements.Add(new TextElement(text));

        public override void VisitSeeAlso(SeeAlso seeAlso) => Elements.Add(new SeeAlsoElement(seeAlso));

        public override void VisitSee(See see) => Elements.Add(new SeeElement(see));


        public override void VisitPara(Para para)
        {
            var visitor = new ContainerVisitor();

            foreach (var element in para.Elements)
            {
                element.Accept(visitor);
            }
            
            Elements.Add(new ParaElement(visitor.Elements));
        }
    }
}
