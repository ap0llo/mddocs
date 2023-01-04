using System;
using Microsoft.Build.Logging.StructuredLogger;

namespace Grynwald.MdDocs.MSBuild.IntegrationTest
{
    /// <summary>
    /// Encapsulates the result of a MSBuild build
    /// </summary>
    internal class MSBuildExecutionResult
    {
        private readonly Lazy<Build> m_BinaryLog;

        /// <summary>
        /// Gets the exit code of the MSBuild process
        /// </summary>
        public required int ExitCode { get; init; }

        /// <summary>
        /// Gets the path of the MSBuild binary log file of the build.
        /// </summary>
        public required string BinaryLogFilePath { get; init; }

        /// <summary>
        /// Gets the parsed binary lof file of the build .
        /// </summary>
        /// <seealso href="https://msbuildlog.com/#api"/>
        public Build BinaryLog => m_BinaryLog.Value;


        public MSBuildExecutionResult()
        {
            m_BinaryLog = new Lazy<Build>(() => Microsoft.Build.Logging.StructuredLogger.BinaryLog.ReadBuild(BinaryLogFilePath));
        }
    }
}
