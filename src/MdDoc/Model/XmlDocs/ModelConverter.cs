using System.Collections.Generic;
using NuDoq;

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

                var memberId = new MemberIdParser(member.Id).Parse();

                var element = new MemberElement(
                        memberId,
                        visitor.Summary,
                        visitor.Remarks,
                        visitor.Example,
                        visitor.Exceptions,
                        visitor.TypeParameters,
                        visitor.Parameters,
                        visitor.Value,
                        visitor.Returns,
                        visitor.SeeAlso);

                result.Add(element);               
            }

            return result;
        }
    }


    class MemberVisitor : Visitor
    {
        public TextBlock Summary { get; private set; }

        public TextBlock Remarks { get; private set; }

        public TextBlock Example { get; private set; }

        public List<ExceptionElement> Exceptions { get; } = new List<ExceptionElement>();

        public List<TypeParamElement> TypeParameters { get; } = new List<TypeParamElement>();

        public Dictionary<string, TextBlock> Parameters { get; } = new Dictionary<string, TextBlock>();

        public TextBlock Value { get; private set; }

        public TextBlock Returns { get; private set; }

        public List<SeeAlsoElement> SeeAlso { get; } = new List<SeeAlsoElement>();


        public override void VisitSummary(Summary summary)
        {
            Summary = GetTextBlock(summary);
        }

        public override void VisitExample(Example example)
        {
            Example = GetTextBlock(example);
        }

        public override void VisitRemarks(Remarks remarks)
        {
            Remarks = GetTextBlock(remarks);
        }

        public override void VisitException(NuDoq.Exception exception)
        {
            Exceptions.Add(new ExceptionElement(exception, GetTextBlock(exception)));
        }

        public override void VisitTypeParam(TypeParam typeParam)
        {
            TypeParameters.Add(new TypeParamElement(typeParam, GetTextBlock(typeParam)));
        }

        public override void VisitParam(Param param)
        {
            Parameters[param.Name] = GetTextBlock(param);
        }

        public override void VisitValue(Value value)
        {
            Value = GetTextBlock(value);
        }

        public override void VisitReturns(Returns returns)
        {
            Returns = GetTextBlock(returns);
        }

        public override void VisitSeeAlso(SeeAlso seeAlso)
        {
            SeeAlso.Add(new SeeAlsoElement(seeAlso, GetTextBlock(seeAlso)));
        }

        private TextBlock GetTextBlock(Container container)
        {
            var visitor = new ContainerVisitor();

            foreach (var element in container.Elements)
            {
                element.Accept(visitor);
            }

            return new TextBlock(visitor.Elements);
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
        
        public override void VisitSee(See see) => Elements.Add(new SeeElement(see));


        public override void VisitPara(Para para)
        {
            var visitor = new ContainerVisitor();

            foreach (var element in para.Elements)
            {
                element.Accept(visitor);
            }
            
            Elements.Add(new ParaElement(new TextBlock(visitor.Elements)));
        }
    }
}
