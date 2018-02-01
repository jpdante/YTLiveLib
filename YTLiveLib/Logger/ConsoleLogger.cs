using System;
using System.Collections.Generic;
using System.Text;

namespace YTLiveLib.Logger {
    public sealed class ConsoleLogger : ILogger {

        public LogLevel Level { get; set; }

        public string DateFormat => @"M/d/yyyy hh:mm:ss";

        public DateTime RightNow => DateTime.Now;

        public ConsoleLogger() : this(LogLevel.All) { }

        public ConsoleLogger(LogLevel level) {
            Level = level;
        }

        public void Log(LogEvent evt) {
            if (evt.Level > Level) return;
            Console.WriteLine(CreateMessage(evt));
        }

        public void Debug(object obj) {
            Log(new LogEvent { Level = LogLevel.Debug, DateTime = RightNow, Message = obj.ToString() });
        }

        public void Debug(string message) {
            Log(new LogEvent { Level = LogLevel.Debug, DateTime = RightNow, Message = message });
        }

        public void Debug(string message, Exception ex) {
            Log(new LogEvent { Level = LogLevel.Debug, DateTime = RightNow, Message = ExceptionToString(message, ex) });
        }

        public void Info(object obj) {
            Log(new LogEvent { Level = LogLevel.Info, DateTime = RightNow, Message = obj.ToString() });
        }

        public void Info(string message) {
            Log(new LogEvent { Level = LogLevel.Info, DateTime = RightNow, Message = message });
        }

        public void Info(string message, Exception ex) {
            Log(new LogEvent { Level = LogLevel.Info, DateTime = RightNow, Message = ExceptionToString(message, ex) });
        }

        public void Warn(object obj) {
            Log(new LogEvent { Level = LogLevel.Warn, DateTime = RightNow, Message = obj.ToString() });
        }

        public void Warn(string message) {
            Log(new LogEvent { Level = LogLevel.Warn, DateTime = RightNow, Message = message });
        }

        public void Warn(string message, Exception ex) {
            Log(new LogEvent { Level = LogLevel.Warn, DateTime = RightNow, Message = ExceptionToString(message, ex) });
        }

        public void Error(object obj) {
            Log(new LogEvent { Level = LogLevel.Error, DateTime = RightNow, Message = obj.ToString() });
        }

        public void Error(string message) {
            Log(new LogEvent { Level = LogLevel.Error, DateTime = RightNow, Message = message });
        }

        public void Error(string message, Exception ex) {
            Log(new LogEvent { Level = LogLevel.Error, DateTime = RightNow, Message = ExceptionToString(message, ex) });
        }

        public void Fatal(object obj) {
            Log(new LogEvent { Level = LogLevel.Fatal, DateTime = RightNow, Message = obj.ToString() });
        }

        public void Fatal(string message) {
            Log(new LogEvent { Level = LogLevel.Fatal, DateTime = RightNow, Message = message });
        }

        public void Fatal(string message, Exception ex) {
            Log(new LogEvent { Level = LogLevel.Fatal, DateTime = RightNow, Message = ExceptionToString(message, ex) });
        }

        private string CreateMessage(LogEvent evt) {
            return $"[{RightNow.ToString(DateFormat)}] [{evt.Level}] {evt.Message}";
        }

        private static string ExceptionToString(string message, Exception ex) {
            return $"{message}: {ex.Message}\r\n{ex.StackTrace}";
        }
    }
}
