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

    public static class DebugLog
    {
        public static ObservableCollection<string> Entries { get; set; } = new ObservableCollection<string>();

        public delegate void LogDelegate(string what, string source, LogSeverity severity);
        public static event LogDelegate LogEvent;

        public static void Log(string what, string source = "", LogSeverity severity = LogSeverity.Info)
        {
            LogEvent?.Invoke(what, source, severity);
            Entries.Add($"[{DateTime.Now.ToShortTimeString()}] {source} > {what}");
        }
    }

    public abstract class Logger
    {
        abstract public void LogInfo(string what, string source, LogSeverity severity);
    }

    public class FileLogger : Logger
    {
        private StreamWriter _fileStream;

        public FileLogger(string filePath)
        {
            filePath += ApplicationHelper.IsDesignMode ? ApplicationHelper.DataPath : "";
            _fileStream = new StreamWriter(filePath);
            _fileStream.AutoFlush = true;
        }
        public override void LogInfo(string what, string source, LogSeverity severity)
        {
            _fileStream.WriteLine($"[{DateTime.Now.ToShortTimeString()}] {source} > {what}");
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
