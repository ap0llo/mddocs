using System;
using Grynwald.MarkdownGenerator;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.Extensions.Logging;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Grynwald.MdDocs.MSBuild
{
    public abstract class TaskBase : Task
    {
        private ILogger m_Logger;


        [Required]
        public ITaskItem Assembly { get; set; }

        [Required]
        public ITaskItem OutputDirectory { get; set; }

        public string MarkdownPreset { get; set; }

        protected ILogger Logger
        {
            get
            {
                m_Logger = m_Logger ?? new MSBuildLogger(Log);
                return m_Logger;
            }
        }

        protected string AssemblyPath => Assembly.GetFullPath();

        protected string OutputDirectoryPath => OutputDirectory.GetFullPath();


        protected bool ValidateParameters()
        {
            if (Assembly is null)
                Log.LogError($"Required task parameter '{nameof(Assembly)}' is null");

            if (OutputDirectory is null)
                Log.LogError($"Required task parameter '{nameof(OutputDirectory)}' is null");

            return Log.HasLoggedErrors == false;
        }


        protected MdSerializationOptions GetSerializationOptions()
        {
            if (String.IsNullOrEmpty(MarkdownPreset))
            {
                return MdSerializationOptions.Presets.Default;
            }

            try
            {
                var preset = MdSerializationOptions.Presets.Get(MarkdownPreset);
                Logger.LogInformation($"Using preset '{MarkdownPreset}' for generating markdown");
                return preset;
            }
            catch (PresetNotFoundException)
            {
                Logger.LogInformation($"Preset '{MarkdownPreset}' not found. Using default serialization options");
                return MdSerializationOptions.Presets.Default;
            }
        }
    }
}
