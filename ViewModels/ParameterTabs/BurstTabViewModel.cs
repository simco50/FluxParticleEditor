using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GalaSoft.MvvmLight.CommandWpf;
using ParticleEditor.Annotations;
using ParticleEditor.Data.ParticleSystem;
using ParticleEditor.Debugging;
using ParticleEditor.Helpers;
using ParticleEditor.Views;

namespace ParticleEditor.ViewModels.ParameterTabs
{
    public class BurstTabViewModel
    {
        public MainViewModel MainViewModel
        {
            get
            {
                return Application.Current.MainWindow.DataContext as MainViewModel;
            }
        }

        public BurstTabViewModel()
        {

        }

        public RelayCommand<float> AddBurstCommand
        { get { return new RelayCommand<float>(AddBurst);} }

        private void AddBurst(float key)
        {
            Random rand = new Random();
            if(MainViewModel.ParticleSystem.Bursts.Add(key, rand.Next()) == false)
                DebugLog.Log($"Key with value {key} already exists!", "Add burst", LogSeverity.Warning);
        }
    }
}
