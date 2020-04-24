using BTB.Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BTB.Server.Common.Logger
{
    public abstract class LoggerProviderBase : IDisposable, ILoggerProvider, ISupportExternalScope
    {
        private ConcurrentDictionary<string, BLogger> loggers = new ConcurrentDictionary<string, BLogger>();
        private IExternalScopeProvider _fScopeProvider;
        protected IDisposable SettingsChangeToken;

        private LoggerConfig _loggerConfig;

        public delegate Task ErrorLogOccuredAsync(CancellationToken cancellationToken);
        public static event ErrorLogOccuredAsync ErrorLogOccuredEventAsync;

        public bool IsDisposed { get; protected set; }

        public LoggerProviderBase(LoggerConfig loggerConfig)
        {
            _loggerConfig = loggerConfig;
        }

        ~LoggerProviderBase()
        {
            if (!this.IsDisposed)
            {
                Dispose(false);
            }
        }

        void ISupportExternalScope.SetScopeProvider(IExternalScopeProvider scopeProvider)
        {
            _fScopeProvider = scopeProvider;
        }

        ILogger ILoggerProvider.CreateLogger(string category)
        {
            return loggers.GetOrAdd(category, (category) => {
                return new BLogger(this, category);
            });
        }

        void IDisposable.Dispose()
        {
            try 
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            } 
            catch
            {
            }            
        }

        protected virtual void Dispose(bool disposing)
        {
            if (SettingsChangeToken != null)
            {
                SettingsChangeToken.Dispose();
                SettingsChangeToken = null;
            }
        }

        public abstract bool IsEnabled(LogLevel level);

        public bool IsStackTraceEnabled()
        {
            return _loggerConfig.StackTrace; ;
        }

        public abstract Task WriteLogAsync(LogEntryVO Info);

        internal IExternalScopeProvider ScopeProvider
        {
            get
            {
                if (_fScopeProvider == null)
                    _fScopeProvider = new LoggerExternalScopeProvider();
                return _fScopeProvider;
            }
        }

        public Task RunErrorLogOccuredEvent()
        {
            if (ErrorLogOccuredEventAsync != null)
            {
                return ErrorLogOccuredEventAsync(CancellationToken.None);
            }

            return Task.CompletedTask;
        }
    }
}
