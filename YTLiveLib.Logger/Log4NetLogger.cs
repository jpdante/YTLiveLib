using System;
using System.Collections.Generic;
using System.Text;

namespace YTLiveLib.Logger {
    public sealed class Log4NetLogger : ILogger {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(YTClient));

        public LogLevel Level { get; set; }

        public Log4NetLogger() {
            Level = LogLevel.None;
        }

        public void Log(LogEvent evt) {
            if(evt.Level == LogLevel.Debug) {
                log.Debug(evt.Message, evt.Exception);
            } else if(evt.Level == LogLevel.Error) {
                log.Error(evt.Message, evt.Exception);
            } else if(evt.Level == LogLevel.Info) {
                log.Info(evt.Message, evt.Exception);
            } else if(evt.Level == LogLevel.Fatal) {
                log.Fatal(evt.Message, evt.Exception);
            } else if(evt.Level == LogLevel.Warn) {
                log.Warn(evt.Message, evt.Exception);
            }
        }

        public void Debug(object obj) {
            Log(new LogEvent { Level = LogLevel.Debug, Message = obj.ToString() });
        }

        public void Debug(string message) {
            Log(new LogEvent { Level = LogLevel.Debug, Message = message });
        }

        public void Debug(string message, Exception ex) {
            Log(new LogEvent { Level = LogLevel.Debug, Message = ExceptionToString(message, ex) });
        }

        public void Info(object obj) {
            Log(new LogEvent { Level = LogLevel.Info, Message = obj.ToString() });
        }

        public void Info(string message) {
            Log(new LogEvent { Level = LogLevel.Info, Message = message });
        }

        public void Info(string message, Exception ex) {
            Log(new LogEvent { Level = LogLevel.Info, Message = ExceptionToString(message, ex) });
        }

        public void Warn(object obj) {
            Log(new LogEvent { Level = LogLevel.Warn, Message = obj.ToString() });
        }

        public void Warn(string message) {
            Log(new LogEvent { Level = LogLevel.Warn, Message = message });
        }

        public void Warn(string message, Exception ex) {
            Log(new LogEvent { Level = LogLevel.Warn, Message = ExceptionToString(message, ex) });
        }

        public void Error(object obj) {
            Log(new LogEvent { Level = LogLevel.Error, Message = obj.ToString() });
        }

        public void Error(string message) {
            Log(new LogEvent { Level = LogLevel.Error, Message = message });
        }

        public void Error(string message, Exception ex) {
            Log(new LogEvent { Level = LogLevel.Error, Message = ExceptionToString(message, ex) });
        }

        public void Fatal(object obj) {
            Log(new LogEvent { Level = LogLevel.Fatal, Message = obj.ToString() });
        }

        public void Fatal(string message) {
            Log(new LogEvent { Level = LogLevel.Fatal, Message = message });
        }

        public void Fatal(string message, Exception ex) {
            Log(new LogEvent { Level = LogLevel.Fatal, Message = ExceptionToString(message, ex) });
        }

        private static string ExceptionToString(string message, Exception ex) {
            return $"{message}: {ex.Message}\r\n{ex.StackTrace}";
        }
    }
}
