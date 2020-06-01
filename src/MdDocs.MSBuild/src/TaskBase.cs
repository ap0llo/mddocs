using Grynwald.MdDocs.Common.Configuration;
using Grynwald.Utilities.Configuration;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Grynwald.MdDocs.MSBuild
{
    public abstract class TaskBase : Task
    {
        private ILogger? m_Logger;


        [Required]
        public ITaskItem Assembly { get; set; } = null!; // MSBuild ensures that value is set

        public ITaskItem? OutputDirectory { get; set; } = null;

        public ITaskItem? ConfigurationFile { get; set; } = null;


        [ConfigurationValue("mddocs:markdown:preset")]
        public string? MarkdownPreset { get; set; }


        protected ILogger Logger
        {
            get
            {
                m_Logger = m_Logger ?? new MSBuildLogger(Log);
                return m_Logger;
            }
        }


        // should be protected but is internal for testing
        internal bool ValidateParameters()
        {
            if (Assembly is null)
                Log.LogError($"Required task parameter '{nameof(Assembly)}' is null");

            return Log.HasLoggedErrors == false;
        }

        // should be protected but is internal for testing
        internal DocsConfiguration LoadConfiguration()
        {
            var configuration = DocsConfigurationLoader.GetConfiguration(ConfigurationFile?.GetFullPath() ?? "", this);
            return configuration;
        }
    }
}
