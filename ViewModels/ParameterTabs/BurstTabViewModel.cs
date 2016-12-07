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
    public class Burst
    {
        public float Time = -1;
        public int Amount = -1;
    }

    public class BurstTabViewModel
    {
        public ParticleSystem ParticleSystem { get; set; }

        public RelayCommand<Burst> AddBurstCommand
        { get { return new RelayCommand<Burst>(AddBurst);} }

        private void AddBurst(Burst burst)
        {
            if (burst.Amount == -1 || burst.Time == -1.0f)
            {
                DebugLog.Log("Input format not valid!", "Add Burst", LogSeverity.Warning);
                return;
            }
            if (burst.Time > ParticleSystem.Duration)
            {
                DebugLog.Log($"The time must be smaller than the duration of the particle system.\nThe duration is {ParticleSystem.Duration:0.00}s while the given timestamp is {burst.Time:0.00}s.", "Add Burst", LogSeverity.Warning);
                return;
            }
            ParticleSystem.Bursts.AddUnique(burst.Time, burst.Amount);
        }

        public RelayCommand<float> RemoveBurstCommand
        { get { return new RelayCommand<float>(RemoveBurst); } }

        private void RemoveBurst(float key)
        {
            Random rand = new Random();
            if (ParticleSystem.Bursts.Remove(key) == false)
                DebugLog.Log($"Key with value {key} does not exists!", "Remove Burst", LogSeverity.Warning);
        }
    }
}
