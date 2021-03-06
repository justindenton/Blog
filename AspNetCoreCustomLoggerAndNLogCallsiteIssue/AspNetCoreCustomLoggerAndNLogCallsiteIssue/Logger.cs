﻿using System;
using Microsoft.Extensions.Logging;

namespace AspNetCoreCustomLoggerAndNLogCallsiteIssue
{
    public class Logger<T> : ILogger<T>
    {
        private readonly ILogger _logger;

        public Logger(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<T>();
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception,
            Func<TState, Exception, string> formatter)
        {
            string Formatter(TState innserState, Exception innerException)
            {
                // additional logic goes here, in my case that was extracting additional information from custom exceptions
                var message = formatter(innserState, innerException) ?? string.Empty;
                return message + " additional stuff in here";
            }

            _logger.Log(logLevel, eventId, state, exception, Formatter);
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return _logger.IsEnabled(logLevel);
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return _logger.BeginScope(state);
        }
    }
}