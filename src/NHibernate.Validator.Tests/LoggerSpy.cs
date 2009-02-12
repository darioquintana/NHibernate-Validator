using System;
using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Repository.Hierarchy;

namespace NHibernate.Validator.Tests
{
	public class LoggerSpy : IDisposable
	{
		private readonly Logger logger;
		private readonly Level prevLogLevel;
		private readonly MemoryAppender appender;

		public MemoryAppender Appender
		{
			get { return appender; }
		}

		public LoggerSpy(System.Type loggerType, Level level)
			: this(LogManager.GetLogger(loggerType), level)
		{
		}

		public LoggerSpy(string loggerName, Level level)
			: this(LogManager.GetLogger(loggerName), level)
		{
		}

		public LoggerSpy(ILog log, Level level)
		{
			logger = log.Logger as Logger;
			if (logger == null)
				throw new Exception("Unable to get the logger");

			prevLogLevel = logger.Level;
			logger.Level = level;

			// Add a new MemoryAppender to the logger.
			appender = new MemoryAppender();
			logger.AddAppender(appender);
		}

		public void Dispose()
		{
			// Restore the previous log level and remove the MemoryAppender
			logger.Level = prevLogLevel;
			logger.RemoveAppender(appender);
		}

		public int GetOccurenceContaining(string message)
		{
			int result = 0;
			foreach (LoggingEvent loggingEvent in Appender.GetEvents())
			{
				if (loggingEvent.RenderedMessage.Contains(message))
					result++;
			}
			return result;
		}

		public int GetOccurencesOfMessage(Predicate<string> predicate)
		{
			int result = 0;
			foreach (LoggingEvent loggingEvent in Appender.GetEvents())
			{
				if (predicate(loggingEvent.RenderedMessage))
					result++;
			}
			return result;
		}
	}
}