using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ParticleEditor.Helpers;

namespace ParticleEditor.Helpers
{
    public enum LogSeverity
    {
        Info,
        Warning,
        Error,
    }

    public struct LogEntry
    {
        public LogEntry(string what, string source)
        {
            What = what;
            Source = source;
            Timestamp = DateTime.Now;
        }
        public string Source { get; set; }
        public string What { get; set; }
        public DateTime Timestamp { get; set; }
    }

    public static class DebugLog
    {
        public static ObservableCollection<LogEntry> Entries { get; set; } = new ObservableCollection<LogEntry>();

        public delegate void LogDelegate(string what, string source, LogSeverity severity);
        public static event LogDelegate LogEvent;

        public static void Log(string what, string source = "", LogSeverity severity = LogSeverity.Info)
        {
            LogEvent?.Invoke(what, source, severity);
            Entries.Add(new LogEntry(what, source));
        }
    }

    public abstract class Logger
    {
        abstract public void LogInfo(string what, string source, LogSeverity severity);
    }

    public class FileLogger : Logger
    {
        private string _filePath;

        public FileLogger(string filePath)
        {
            _filePath = (ApplicationHelper.IsDesignMode ? ApplicationHelper.DataPath : "") + filePath;
            FileStream logFile = File.Create(_filePath);
            logFile.Close();
        }
        public override void LogInfo(string what, string source, LogSeverity severity)
        {
            FileStream logFile = File.OpenWrite(_filePath);
            using (StreamWriter sw = new StreamWriter(logFile))
            {
                sw.WriteLine($"[{DateTime.Now.ToShortTimeString()}] {source} > {what}");
            }
        }
    }

    public class ApplicationLogger : Logger
    {
        public override void LogInfo(string what, string source, LogSeverity severity)
        {
            switch (severity)
            {
                case LogSeverity.Info:
                    break;
                case LogSeverity.Warning:
                    MessageBox.Show(what, source, MessageBoxButton.OK, MessageBoxImage.Warning);
                    break;
                case LogSeverity.Error:
                    MessageBox.Show(what, source, MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(severity), severity, null);
            }
        }
    }
}
