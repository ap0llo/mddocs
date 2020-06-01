using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Microsoft.Build.Framework;

namespace Grynwald.MdDocs.MSBuild.Test
{
    internal class BuildEngineMock : IBuildEngine
    {
        public bool ContinueOnError => false;

        public int LineNumberOfTaskNode => 1;

        public int ColumnNumberOfTaskNode => 1;

        public string ProjectFileOfTaskNode => "mock.proj";


        public bool BuildProjectFile(string projectFileName, string[] targetNames, IDictionary globalProperties, IDictionary targetOutputs)
        {
            throw new NotImplementedException();
        }

        public void LogCustomEvent(CustomBuildEventArgs e)
        { }

        public void LogErrorEvent(BuildErrorEventArgs e)
        { }

        public void LogMessageEvent(BuildMessageEventArgs e)
        { }

        public void LogWarningEvent(BuildWarningEventArgs e)
        { }
    }
}
