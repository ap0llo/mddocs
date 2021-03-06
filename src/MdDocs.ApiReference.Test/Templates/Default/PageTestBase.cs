﻿using ApprovalTests;
using ApprovalTests.Reporters;
using Grynwald.MdDocs.ApiReference.Configuration;
using Grynwald.MdDocs.ApiReference.Model;
using Grynwald.MdDocs.ApiReference.Templates.Default;
using Grynwald.MdDocs.Common.Configuration;
using Grynwald.MdDocs.TestHelpers;
using Moq;
using Xunit;

namespace Grynwald.MdDocs.ApiReference.Test.Templates.Default
{
    /// <summary>
    /// Base class for all page tests in this assembly.
    /// Defines test cases that apply to all pages.
    /// </summary>
    [Trait("Category", "SkipWhenLiveUnitTesting")]
    [UseReporter(typeof(DiffReporter))]
    public abstract class PageTestBase<TModel, TPage> : DynamicCompilationTestBase where TPage : PageBase<TModel> where TModel : class, IDocumentation
    {
        protected abstract TModel CreateSampleModel();

        protected abstract TPage CreatePage(TModel model, ApiReferenceConfiguration configuration);

        protected void Approve(TModel model, ApiReferenceConfiguration? configuration = null)
        {
            configuration ??= new ConfigurationProvider().GetDefaultApiReferenceConfiguration();

            var page = CreatePage(model, configuration);

            var doc = page.GetDocument();

            Assert.NotNull(doc);
            var writer = new ApprovalTextWriter(doc.ToString());
            Approvals.Verify(writer, new ApprovalNamer(GetType().Name, relativeOutputDirectory: "../../_referenceResults"), Approvals.GetReporter());
        }


        [Fact]
        public void GetDocument_returns_expected_Markdown_for_default_settings()
        {
            var configuration = new ConfigurationProvider().GetDefaultApiReferenceConfiguration();
            var model = CreateSampleModel();

            Approve(model, configuration);
        }

        [Fact]
        public void GetDocument_does_not_include_AutoGenerated_notice_if_the_includeAutoGeneratedNotice_setting_is_false()
        {
            var configuration = new ConfigurationProvider().GetDefaultApiReferenceConfiguration();
            configuration.Template.Default.IncludeAutoGeneratedNotice = false;
            var model = CreateSampleModel();
            Approve(model, configuration);
        }

        [Fact]
        public void GetDocument_returns_document_that_does_not_include_the_assembly_version_if_the_includeVersion_setting_is_false()
        {
            var configuration = new ConfigurationProvider().GetDefaultApiReferenceConfiguration();
            configuration.Template.Default.IncludeVersion = false;
            var model = CreateSampleModel();
            Approve(model, configuration);
        }
    }
}
