
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using GalaSoft.MvvmLight.CommandWpf;
using MahApps.Metro;
using ParticleEditor.Annotations;
using ParticleEditor.Data.ParticleSystem;
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
            ParticleSystem = ParticleFormatter.MakeNewSystem();

            FileLogger fileLogger = new FileLogger("ParticleEditor.log");
            DebugLog.LogEvent += fileLogger.LogInfo;
            ApplicationLogger appLogger = new ApplicationLogger();
            DebugLog.LogEvent += appLogger.LogInfo;
            DebugLog.Log("Initialized", "Application");
        }

        private  ParticleSystem _particleSystem;
        public ParticleSystem ParticleSystem
        {
            get { return _particleSystem; }
            set
            {
                _particleSystem = value;
                SpriteImage = ImageLoader.ToImageSource(_particleSystem.ImagePath);
                OnPropertyChanged("ParticleSystem");
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

        private SolidColorBrush _backgroundColor = new SolidColorBrush(Color.FromRgb(50, 50, 50));
        public SolidColorBrush BackgroundColor
        {
            get { return _backgroundColor; }
            set
            {
                _backgroundColor = value;
                OnPropertyChanged("BackgroundColor");
            }
        }

        public RelayCommand OpenFileCommand{
            get { return new RelayCommand(OpenFile); }
        }
        private void OpenFile()
        {
            CheckForUnsavedChanges();
            ParticleSystem system = ParticleFormatter.Open();
            if (system != null) ParticleSystem = system;
        }

        public RelayCommand SaveFileCommand{
            get { return new RelayCommand(SaveFile); }
        }
        private void SaveFile()
        {
            if(ParticleFormatter.Save(ParticleSystem))
                HasUnsavedChanges = false;
        }

        public RelayCommand SaveAsFileCommand
        {
            get { return new RelayCommand(SaveAsFile); }
        }
        private void SaveAsFile()
        {
            if (ParticleFormatter.SaveAs(ParticleSystem))
                HasUnsavedChanges = false;
        }

        private async void CheckForUnsavedChanges()
        {
            if (HasUnsavedChanges)
            {
                MessageBoxResult result =
                    MessageBox.Show(
                        "Your current particle system has unsaved changes, would you like to save it first?",
                        "Unsaved changes!", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                    SaveFile();
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
            SpriteImage = ImageLoader.ToImageSource(_particleSystem.ImagePath);
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
            HelpWindowView helpWindow = new HelpWindowView();
            helpWindow.ShowInTaskbar = false;
            helpWindow.Owner = Application.Current.MainWindow;
            helpWindow.ShowDialog();
        }

        public RelayCommand OpenColorPickerCommand
        {
            get { return new RelayCommand(OpenColorPicker);}
        }

        private void OpenColorPicker()
        {
            ColorDialog dialog = new ColorDialog();
            dialog.SolidColorOnly = true;
            dialog.AnyColor = true;
            dialog.FullOpen = true;
            dialog.Color = System.Drawing.Color.FromArgb(BackgroundColor.Color.R, BackgroundColor.Color.G, BackgroundColor.Color.B);
            DialogResult result = dialog.ShowDialog();
            if (result != DialogResult.OK)
                return;
            BackgroundColor = new SolidColorBrush(Color.FromRgb(dialog.Color.R, dialog.Color.G, dialog.Color.B));
        }

        public RelayCommand NewParticleCommand { get { return new RelayCommand(NewParticle);} }
        private void NewParticle()
        {
            CheckForUnsavedChanges();
            ParticleSystem = ParticleFormatter.MakeNewSystem();
            DebugLog.Log("Reset particle system", "Application");
        }

        public RelayCommand OpenDebugLogView { get { return new RelayCommand(OpenDebugLog);} }

        private void OpenDebugLog()
        {
            DebugLogView logWindow = new DebugLogView();
            logWindow.ShowInTaskbar = false;
            logWindow.Owner = Application.Current.MainWindow;
            logWindow.ShowDialog();
        }

        public RelayCommand<string> ChangeThemeCommand
        {
            get { return new RelayCommand<string>(ChangeTheme); }
        }

        private void ChangeTheme(string theme)
        {
            string[] parameters = theme.Split('/');
            Accent accent = ThemeManager.GetAccent(parameters[1]);
            AppTheme appTheme = ThemeManager.GetAppTheme(parameters[0]);
            ThemeManager.ChangeAppStyle(Application.Current, accent, appTheme);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
