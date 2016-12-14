using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
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

    public interface ILogger
    {
        void LogInfo(string what, string source, LogSeverity severity);
    }

    public class FileLogger : ILogger
    {
        private string _filePath;

        public FileLogger(string filePath)
        {
            _filePath = (ApplicationHelper.IsDesignMode ? ApplicationHelper.DataPath : "") + filePath;
            FileStream logFile = File.Create(_filePath);
            logFile.Close();
        }
        public void LogInfo(string what, string source, LogSeverity severity)
        {
            using (StreamWriter sw = File.AppendText(_filePath))
            {
                sw.WriteLine($"[{DateTime.Now.ToShortTimeString()}] {source} > {what}");
            }
        }
    }

    public class ApplicationLogger : ILogger
    {
        public void LogInfo(string what, string source, LogSeverity severity)
        {
            switch (severity)
            {
                case LogSeverity.Info:
                    break;
                case LogSeverity.Warning:
                    MetroWindow window = Application.Current.MainWindow as MetroWindow;
                    MetroDialogSettings settings = new MetroDialogSettings();
                    settings.AnimateHide = false;
                    settings.AnimateShow = false;
                    window.ShowMessageAsync(source, what, MessageDialogStyle.Affirmative, settings);
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
