using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using ParticleEditor.Helpers;

namespace ParticleEditor.ViewModels
{
    class DebugLogViewModel : ViewModelBase
    {
        public ObservableCollection<LogEntry> FilteredLogEntries { get; set; }
        public ObservableCollection<string> LogCategories { get; set; } = new ObservableCollection<string>() {"All"};
        private string _category = "All";

        public DebugLogViewModel()
        {
            FilteredLogEntries = new ObservableCollection<LogEntry>(DebugLog.Entries);
            var categories = FilteredLogEntries.Select(a=>a.Source).Distinct().ToArray();
            foreach (string category in categories)
                LogCategories.Add(category);
        }

        public RelayCommand<string> FilterLogCommand => new RelayCommand<string>(FilterLog);

        private void FilterLog(string category)
        {
            _category = category;
            if (category == "All")
            {
                ResetFilter();
                return;
            }

            FilteredLogEntries.Clear();
            var result = DebugLog.Entries.Where(entry => entry.Source == category).ToArray();
            foreach(var r in result)
                FilteredLogEntries.Add(r);
        }

        private int _selectedIndex;

        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                _selectedIndex = value;
                RaisePropertyChanged("SelectedIndex");
            }
        }

        public RelayCommand ResetFilterCommand => new RelayCommand(ResetFilter);

        private void ResetFilter()
        {
            SelectedIndex = 0;
            FilteredLogEntries.Clear();
            foreach(var entry in DebugLog.Entries)
                FilteredLogEntries.Add(entry);
        }

        public RelayCommand SaveDebugLogCommand => new RelayCommand(SaveDebugLog);

        private void SaveDebugLog()
        {
            Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.DefaultExt = ".log";
            dialog.Filter = "Log (.log) | *.log";
            bool? success = dialog.ShowDialog();
            if (success == false)
                return;
            try
            {
                using (StreamWriter writer = new StreamWriter(File.Create(dialog.FileName)))
                {
                    writer.WriteLine("Particle Editor - Simon Coenen");
                    writer.WriteLine($"Application log. Category: {_category}");
                    writer.WriteLine($"Log created on {DateTime.Now}\n");
                    foreach (LogEntry entry in FilteredLogEntries)
                    {
                        writer.WriteLine($"[{entry.Timestamp.ToShortTimeString()}] {entry.Source} > {entry.What}");
                    }
                }
                DebugLog.Log($"Saved log to '{dialog.FileName}'...", "Log save");
            }
            catch (Exception exception)
            {
                DebugLog.Log(exception.Message, "Log save", LogSeverity.Warning);
            }
        }
    }
}
