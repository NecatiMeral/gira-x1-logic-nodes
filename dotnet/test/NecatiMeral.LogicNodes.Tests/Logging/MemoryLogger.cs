using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Necati_Meral_Yahoo_De.LogicNodes.Tests.Core;

namespace Necati_Meral_Yahoo_De.LogicNodes.Tests.Logging;
public class MemoryLogger : ILogger
{
    public List<(LogLevel, Exception, string)> LogLines = [];

    public IDisposable BeginScope<TState>(TState state) where TState : notnull => NullDisposable.Instance;
    public bool IsEnabled(LogLevel logLevel) => true;
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
        LogLines.Add((logLevel, exception, formatter(state, exception)));
    }
}
