using System;
using System.Collections.Generic;
using System.Text;

namespace YTLiveLib.Logger {
    public interface ILogger {
        LogLevel Level { get; }
        void Log(LogEvent evt);
        void Debug(object obj);
        void Debug(string message);
        void Debug(string message, Exception ex);
        void Info(object obj);
        void Info(string message);
        void Info(string message, Exception ex);
        void Warn(object obj);
        void Warn(string message);
        void Warn(string message, Exception ex);
        void Error(object obj);
        void Error(string message);
        void Error(string message, Exception ex);
        void Fatal(object obj);
        void Fatal(string message);
        void Fatal(string message, Exception ex);
    }

    public struct LogEvent {
        public LogLevel Level { get; set; }
        public DateTime DateTime { get; set; }
        public string Message { get; set; }
        public Exception Exception { get; set; }
    }

    public enum LogLevel {
        Debug = 5,
        Info = 4,
        Warn = 3,
        Error = 2,
        Fatal = 1,
        All = 0
    }
}
