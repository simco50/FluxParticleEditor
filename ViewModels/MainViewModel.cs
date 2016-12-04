using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using GalaSoft.MvvmLight.CommandWpf;
using Newtonsoft.Json;
using ParticleEditor.Annotations;
using ParticleEditor.Data.ParticleSystem;
using ParticleEditor.Debugging;
using ParticleEditor.Helpers;
using ParticleEditor.Views;
using Application = System.Windows.Application;
using MessageBox = System.Windows.MessageBox;

namespace ParticleEditor.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            ParticleSystem = new ParticleSystem();

            DebugLog.LogEvent += AppLogger.LogInfo;
            FileLogger fileLogger = new FileLogger("ParticleEditor.log");
            DebugLog.LogEvent += fileLogger.LogInfo;
            DebugLog.Log("Initialized", "Application");
        }
        public ApplicationLogger AppLogger { get; set; } = new ApplicationLogger();

        private  ParticleSystem _particleSystem;
        public ParticleSystem ParticleSystem
        {
            get { return _particleSystem; }
            set
            {
                _particleSystem = value;
                SpriteImage = DataProvider.ToImageSource(_particleSystem.ImagePath);
            }
        }

        private ImageSource _spriteImage;
        public ImageSource SpriteImage {
            get { return _spriteImage; }
            set
            {
                _spriteImage = value;
                OnPropertyChanged("SpriteImage");
            }
        }

        private bool _hasUnsavedChanged = false;
        public bool HasUnsavedChanges {
            get { return _hasUnsavedChanged; }
            set { _hasUnsavedChanged = value; }
        }

        public RelayCommand ImportParticleCommand{
            get { return new RelayCommand(ImportParticle); }
        }
        private void ImportParticle()
        {
            CheckForUnsavedChanges();

            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.DefaultExt = ".json";
            dialog.Filter = "Json (.json) | *.json";
            bool? success = dialog.ShowDialog();
            if (success == false)
                return;
            try
            {
                DebugLog.Log($"Import from {dialog.FileName}...", "Json import");
                string data = File.ReadAllText(dialog.FileName);
                ParticleSystem = JsonConvert.DeserializeObject<ParticleSystem>(data);
                DebugLog.Log("Import successful");
            }
            catch (Exception exception)
            {
                DebugLog.Log(exception.Message, "Json import failed!", LogSeverity.Warning);
            }
        }

        public RelayCommand ExportParticleCommand{
            get { return new RelayCommand(ExportParticle); }
        }
        private void ExportParticle()
        {
            Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.DefaultExt = ".json";
            dialog.Filter = "Json (.json) | *.json";
            bool? success = dialog.ShowDialog();
            if (success == false)
                return;
            try
            {
                DebugLog.Log($"Export to '{dialog.FileName}'...", "Json export");
                string data = JsonConvert.SerializeObject(ParticleSystem, Formatting.Indented);
                File.WriteAllText(dialog.FileName, data);
                DebugLog.Log("Export successful", "Json export");
            }
            catch (Exception exception)
            {
                DebugLog.Log(exception.Message, "Json export failed!", LogSeverity.Warning);
            }
            HasUnsavedChanges = false;
        }

        private void CheckForUnsavedChanges()
        {
            if (HasUnsavedChanges)
            {
                MessageBoxResult result =
                    MessageBox.Show(
                        "Your current particle system has unsaved changes, would you like to save it first?",
                        "Unsaved changes!", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                    ExportParticle();
            }
        }

        public RelayCommand LoadImageCommand {
            get { return new RelayCommand(LoadImage);}
        }
        private void LoadImage()
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.tif;...";
            bool? success = dialog.ShowDialog();
            if (success == false)
                return;
            ParticleSystem.ImagePath = dialog.FileName;
            SpriteImage = DataProvider.ToImageSource(_particleSystem.ImagePath);
        }

        public RelayCommand ShutdownCommand {
            get { return new RelayCommand(Shutdown);}
        }
        private void Shutdown()
        {
            CheckForUnsavedChanges();
            DebugLog.Log("Shutdown", "Application");
            Application.Current.Shutdown();
        }

        public RelayCommand ShowHelpWindowCommand {
            get { return new RelayCommand(ShowHelpWindow);}
        }
        private void ShowHelpWindow()
        {
            if (Application.Current.MainWindow.OwnedWindows.Count > 0)
            {
                Application.Current.MainWindow.OwnedWindows[0]?.Focus();
            }
            else
            {   
                HelpWindowView helpWindow = new HelpWindowView();
                helpWindow.ShowInTaskbar = false;
                helpWindow.Owner = Application.Current.MainWindow;
                helpWindow.ShowDialog();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
