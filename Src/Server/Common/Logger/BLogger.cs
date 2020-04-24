using BTB.Application.Common.Interfaces;
using BTB.Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BTB.Server.Common.Logger
{
    public class BLogger : ILogger
    {
        private readonly LoggerProviderBase _provider;
        private readonly string _logCategory;

        public BLogger(LoggerProviderBase provider, string logCategory)
        {
            _provider = provider;
            _logCategory = logCategory;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return _provider.ScopeProvider.Push(state);
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return _provider.IsEnabled(logLevel);
        }

        public async void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (IsEnabled(logLevel))
            {
                LogEntryVO entry = new LogEntryVO(exception, _provider.IsStackTraceEnabled());
                entry.Category = this._logCategory;
                entry.Level = logLevel;
                entry.Text = exception?.Message ?? exception?.Source ?? state?.ToString();


                await _provider.WriteLogAsync(entry);
            }
        }
    }
}
