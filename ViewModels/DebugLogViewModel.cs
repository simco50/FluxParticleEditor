using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using ParticleEditor.Annotations;
using ParticleEditor.Helpers;

namespace ParticleEditor.ViewModels
{
    class DebugLogViewModel : ViewModelBase
    {
        public ObservableCollection<LogEntry> FilteredLogEntries { get; set; }
        public ObservableCollection<string> LogCategories { get; set; } = new ObservableCollection<string>() {"All"};

        public DebugLogViewModel()
        {
            FilteredLogEntries = new ObservableCollection<LogEntry>(DebugLog.Entries);
            var categories = FilteredLogEntries.Select(a=>a.Source).Distinct().ToArray();
            foreach (string category in categories)
                LogCategories.Add(category);
        }

        public RelayCommand<string> FilterLogCommand { get { return new RelayCommand<string>(FilterLog);} }

        private void FilterLog(string category)
        {
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

        public RelayCommand ResetFilterCommand { get { return new RelayCommand(ResetFilter);} }

        private void ResetFilter()
        {
            SelectedIndex = 0;
            FilteredLogEntries.Clear();
            foreach(var entry in DebugLog.Entries)
                FilteredLogEntries.Add(entry);
        }
    }
}
