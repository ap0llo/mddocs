﻿using MdDoc.Test.TestData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MdDoc.Test.Model
{
    public class FieldDocumentationTest : MemberDocumentationTest
    {
        protected override MdDoc.Model.MemberDocumentation GetMemberDocumentationInstance()
        {
            return GetTypeDocumentation(typeof(TestClass_Fields)).Fields.First();
        }
    }
}