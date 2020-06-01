using System;
using System.IO;
using Grynwald.MdDocs.Common.Configuration;
using Grynwald.MdDocs.MSBuild;
using Grynwald.MdDocs.MSBuild.Test;
using Grynwald.Utilities.IO;
using Microsoft.Build.Utilities;
using Xunit;

namespace MdDocs.MSBuild.Test
{
    public class TaskBaseTest
    {
        private class TestableTaskBase : TaskBase
        {
            public override bool Execute() => throw new NotImplementedException();
        }

        [Fact]
        public void ValidateParameters_returns_false_is_Assembly_is_null()
        {
            // ARRANGE
            var sut = new TestableTaskBase()
            {
                Assembly = null!,
                BuildEngine = new BuildEngineMock()
            };

            // ACT
            var valid = sut.ValidateParameters();

            // ASSERT
            Assert.False(valid);
        }

        [Fact]
        public void ValidateParameters_returns_true_if_OutputDirectory_is_null()
        {
            // ARRANGE
            var sut = new TestableTaskBase()
            {
                Assembly = new TaskItem("myAssembly.dll"),
                BuildEngine = new BuildEngineMock(),
                OutputDirectory = null!
            };

            // ACT
            var valid = sut.ValidateParameters();

            // ASSERT
            Assert.True(valid);
        }

        [Fact]
        public void ValidateParameters_returns_true_if_ConfigurationFile_is_null()
        {
            // ARRANGE
            var sut = new TestableTaskBase()
            {
                Assembly = new TaskItem("myAssembly.dll"),
                BuildEngine = new BuildEngineMock(),
                OutputDirectory = new TaskItem("my-output-directory"),
                ConfigurationFile = null
            };

            // ACT
            var valid = sut.ValidateParameters();

            // ASSERT
            Assert.True(valid);
        }


    }
}
