using System.Windows.Controls;
using ParticleEditor.ViewModels;

namespace ParticleEditor.Views
{
    /// <summary>
    /// Interaction logic for TimelineView.xaml
    /// </summary>
    public partial class TimelineView : UserControl
    {
        public TimelineView()
        {
            InitializeComponent();
            Root.DataContext = new TimelineViewModel(TimelineGrid);
        }
    }
}
