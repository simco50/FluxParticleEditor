using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.CommandWpf;
using ParticleEditor.Data.ParticleSystem;

namespace ParticleEditor.ViewModels.ParameterTabs
{
    class BurstTabViewModel
    {
        public ParticleSystem ParticleSystem
        {
            get { return MainViewModel.ParticleSystem; }
        }

        public RelayCommand AddBurstCommand {
            get { return new RelayCommand(AddBurst); }
        }
        private void AddBurst()
        {
            ParticleSystem.Bursts[0] = 0;
        }
    }
}
