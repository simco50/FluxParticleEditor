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
using MahApps.Metro;
using MahApps.Metro.Controls;
using Microsoft.Win32;
using Newtonsoft.Json;
using ParticleEditor.Data;
using Application = System.Windows.Application;

namespace ParticleEditor.Views
{
    public partial class MainView : MetroWindow
    {
        public MainView()
        {
            InitializeComponent();
        }
    }
}
