﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParticleEditor.Data.ParticleSystem;

namespace ParticleEditor.ViewModels.ParameterTabs
{
    class GeneralTabViewModel
    {
        public ParticleSystem ParticleSystem
        {
            get { return MainViewModel.ParticleSystem; }
        }

        public GeneralTabViewModel()
        {

        }
    }
}