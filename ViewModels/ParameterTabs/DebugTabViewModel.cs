using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ParticleEditor.ViewModels.ParameterTabs
{
    class DebugTabViewModel
    {
        public MainViewModel MainViewModel
        {
            get
            {
                return Application.Current.MainWindow.DataContext as MainViewModel;
            }
        }
    }
}
