using System;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.Extensions.Logging;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Grynwald.MdDocs.MSBuild
{
    internal class MSBuildLogger : ILogger
    {
        private readonly TaskLoggingHelper m_TaskloggingHelper;

        private class NullDisposable : IDisposable
        {
            public static readonly NullDisposable Instance = new NullDisposable();

            private NullDisposable()
            { }

            public void Dispose()
            { }
        }


        public MSBuildLogger(TaskLoggingHelper taskloggingHelper)
        {
            m_TaskloggingHelper = taskloggingHelper ?? throw new ArgumentNullException(nameof(taskloggingHelper));
        }


        public IDisposable BeginScope<TState>(TState state) => NullDisposable.Instance;

        public bool IsEnabled(LogLevel _) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
                return;

            formatter = formatter ?? throw new ArgumentNullException(nameof(formatter));

            var message = formatter(state, exception);

            switch (logLevel)
            {
                case LogLevel.Error:
                case LogLevel.Critical:
                    m_TaskloggingHelper.LogError(message);
                    break;

                case LogLevel.Trace:
                case LogLevel.Debug:
                case LogLevel.None:
                    m_TaskloggingHelper.LogMessage(MessageImportance.Low, message);
                    break;

                case LogLevel.Information:
                    m_TaskloggingHelper.LogMessage(MessageImportance.Normal, message);
                    break;

                case LogLevel.Warning:
                    m_TaskloggingHelper.LogWarning(message);
                    break;

                default:
                    throw new NotImplementedException($"Unexpected log level '{logLevel}'");
            }
        }
    }
}
