using System;
using Core.Logging;

namespace ShapezSteamModPackageManager
{
    public class PrefixedLogger : ILogger
    {
        public PrefixedLogger(ILogger logger, string prefix)
        {
            Debug = logger.Debug == null ? null : new PrefixedLogChannel(logger.Debug, prefix);
            Info = logger.Info == null ? null : new PrefixedLogChannel(logger.Info, prefix);
            Warning = logger.Warning == null ? null : new PrefixedLogChannel(logger.Warning, prefix);
            Error = logger.Error == null ? null : new PrefixedLogChannel(logger.Error, prefix);
            Exception = logger.Exception == null ? null : new PrefixedLogChannel(logger.Exception, prefix);
        }

        public ILogChannel Debug { get; }
        public ILogChannel Info { get; }
        public ILogChannel Warning { get; }
        public ILogChannel Error { get; }
        public ILogChannel Exception { get; }

        private class PrefixedLogChannel : ILogChannel
        {
            private readonly ILogChannel BaseLogChannel;
            private readonly string Prefix;

            public PrefixedLogChannel(ILogChannel baseLogChannel, string prefix)
            {
                BaseLogChannel = baseLogChannel;
                Prefix = prefix;
            }

            public void Log(string message)
            {
                BaseLogChannel.Log($"{Prefix} {message}");
            }

            public void LogFormat(string format, params object[] args)
            {
                BaseLogChannel.LogFormat($"{Prefix} {format}", args);
            }

            public void LogException(Exception exception)
            {
                BaseLogChannel.LogException(new PrefixedException(Prefix, exception));
            }
        }
    }

    internal class PrefixedException : Exception
    {
        public PrefixedException(string prefix, Exception innerException) : base(
            $"Exception occurred with the {prefix} prefix", innerException)
        {
        }
    }
}