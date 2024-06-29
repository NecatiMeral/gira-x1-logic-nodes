using System;
using Microsoft.Extensions.Logging;
using Necati_Meral_Yahoo_De.LogicNodes.Tests.Core;
using Xunit.Abstractions;

namespace Necati_Meral_Yahoo_De.LogicNodes.Tests.Logging;
public class XUnitLogger : ILogger
{
    protected ITestOutputHelper TestOutputHelper { get; }

    public XUnitLogger(ITestOutputHelper testOutputHelper)
    {
        TestOutputHelper = testOutputHelper;
    }

    public IDisposable BeginScope<TState>(TState state) where TState : notnull => NullDisposable.Instance;
    public bool IsEnabled(LogLevel logLevel) => true;
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
        TestOutputHelper.WriteLine($"{logLevel} {formatter(state, exception)}");
    }
}
