using System;
using GalaSoft.MvvmLight.CommandWpf;
using ParticleEditor.Data.ParticleSystem;
using ParticleEditor.Helpers;

namespace ParticleEditor.ViewModels.ParameterTabs
{
    public class Burst
    {
        public float Time = -1;
        public int Amount = -1;
    }

    public class BurstTabViewModel
    {
        public int SelectedBurst { get; set; } = 0;

        public ParticleSystem ParticleSystem { get; set; }

        public RelayCommand<Burst> AddBurstCommand => new RelayCommand<Burst>(AddBurst);
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
            ParticleSystem.Bursts[burst.Time] = burst.Amount;
        }

        public RelayCommand<float> RemoveBurstCommand => new RelayCommand<float>(RemoveBurst);
        private void RemoveBurst(float key)
        {
            Random rand = new Random();
            if (ParticleSystem.Bursts.Remove(key) == false)
                DebugLog.Log($"Key with value {key} does not exists!", "Remove Burst", LogSeverity.Warning);
        }
    }
}
