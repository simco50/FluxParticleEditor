using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Win32;
using Newtonsoft.Json;
using ParticleEditor.Data;

namespace ParticleEditor.Views
{
    public partial class MainView : Window
    {
        private ParticleSystem _particleSystem = new ParticleSystem();

        public MainView()
        {
            InitializeComponent();
        }
    }
}
