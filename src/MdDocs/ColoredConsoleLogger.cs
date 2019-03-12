using System;
using Microsoft.Extensions.Logging;

namespace Grynwald.MdDocs
{
    /// <summary>
    /// Console logger that writes colored log message to the output.
    /// </summary>
    internal class ColoredConsoleLogger : ILogger
    {
        private readonly LogLevel m_MinLogLevel;

        private class NullDisposable : IDisposable
        {
            public static readonly NullDisposable Instance = new NullDisposable();

            private NullDisposable()
            { }

            public void Dispose()
            { }
        }

        /// <summary>
        /// Initializes a new instance of <see cref="ColoredConsoleLogger"/>.
        /// </summary>
        /// <param name="minLogLevel">The minimum log level. Log messages if a lower log level will be ignored.</param>
        public ColoredConsoleLogger(LogLevel minLogLevel)
        {
            m_MinLogLevel = minLogLevel;
        }


        public IDisposable BeginScope<TState>(TState state) => NullDisposable.Instance;

        public bool IsEnabled(LogLevel logLevel) => logLevel >= m_MinLogLevel;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
                return;

            formatter = formatter ?? throw new ArgumentNullException(nameof(formatter));

            var message = formatter(state, exception);

            var messageColor = GetConsoleColor(logLevel);

            var previousColor = Console.ForegroundColor;
            Console.ForegroundColor = messageColor;
            try
            {
                Console.Write(logLevel.ToString().ToUpper());
                Console.Write(": ");

                if (!String.IsNullOrEmpty(message))
                {
                    Console.WriteLine(message);
                }

                if (exception != null)
                {
                    Console.WriteLine(exception.ToString());
                }
            }
            finally
            {
                Console.ForegroundColor = previousColor;
            }
        }


        private static ConsoleColor GetConsoleColor(LogLevel logLevel)
        {
            ConsoleColor color;
            switch (logLevel)
            {
                case LogLevel.Warning:
                    color = ConsoleColor.Yellow;
                    break;

                case LogLevel.Error:
                case LogLevel.Critical:
                    color = ConsoleColor.Red;
                    break;

                default:
                    color = Console.ForegroundColor;
                    break;
            }

            return color;
        }
    }
}
