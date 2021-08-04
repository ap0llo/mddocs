using System;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Grynwald.MdDocs.TestHelpers
{
    public class XunitLogger : ILogger
    {
        private readonly ITestOutputHelper m_TestOutputHelper;
        private readonly string? m_CategoryName;

        private class LoggerScope : IDisposable
        {
            public void Dispose()
            { }
        }

        /// <summary>
        /// Initializes a new instance of <see cref="XunitLogger"/>
        /// </summary>
        public XunitLogger(ITestOutputHelper testOutputHelper) : this(testOutputHelper, null)
        { }

        /// <summary>
        /// Initializes a new instance of <see cref="XunitLogger"/>
        /// </summary>
        public XunitLogger(ITestOutputHelper testOutputHelper, string? categoryName)
        {
            m_TestOutputHelper = testOutputHelper ?? throw new ArgumentNullException(nameof(testOutputHelper));
            m_CategoryName = String.IsNullOrEmpty(categoryName) ? null : categoryName;
        }

        /// <inheritdoc />
        public IDisposable BeginScope<TState>(TState state) => new LoggerScope();

        /// <inheritdoc />
        public bool IsEnabled(LogLevel logLevel) => true;

        /// <inheritdoc />
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
                return;

            if (m_CategoryName is null)
            {
                m_TestOutputHelper.WriteLine($"{logLevel.ToString().ToUpper()} - {formatter(state, exception)}");
            }
            else
            {
                m_TestOutputHelper.WriteLine($"{logLevel.ToString().ToUpper()} - {m_CategoryName} - {formatter(state, exception)}");
            }

        }
    }


    public class XunitLogger<T> : XunitLogger, ILogger<T>
    {
        public XunitLogger(ITestOutputHelper testOutputHelper) : base(testOutputHelper, typeof(T).Name)
        { }
    }
}
