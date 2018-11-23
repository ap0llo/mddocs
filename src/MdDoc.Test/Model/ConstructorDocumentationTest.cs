using System;
using System.Collections.Generic;
using System.Text;
using MdDoc.Model;
using MdDoc.Test.TestData;

namespace MdDoc.Test.Model
{
    public class ConstructorDocumentationTest : MemberDocumentationTest
    {
        protected override MdDoc.Model.MemberDocumentation GetMemberDocumentationInstance()
        {
            return GetTypeDocumentation(typeof(TestClass_Constructors)).Constructors;
        }
    }
}
