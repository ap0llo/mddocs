using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
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

    }
}
