using MdDoc.Model;
using MdDoc.Test.TestData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace MdDoc.Test.Model
{
    public class MethodDocumentationTest : MemberDocumentationTest
    {
        [Fact]
        public void Name_returns_the_expected_value_for_generic_overloads()
        {
            var methodName = "TestMethod1";
            
            // get methods, use StartsWith() as generic overloads are suffixed with the numer
            // of type parameters
            var methodOverloads = GetTypeDefinition(typeof(TestClass_MethodOverloads))
                .Methods
                .Where(x => x.Name.StartsWith(methodName))
                .Select(x => new MethodOverloadDocumentation(x));

            var sut = new MethodDocumentation(GetTypeDocumentation(typeof(TestClass_MethodOverloads)), methodOverloads);

            Assert.Equal(methodName, sut.Name);
        }



        protected override MdDoc.Model.MemberDocumentation GetMemberDocumentationInstance()
        {
            return GetTypeDocumentation(typeof(TestClass_Methods)).Methods.First();
        }

    }
}
