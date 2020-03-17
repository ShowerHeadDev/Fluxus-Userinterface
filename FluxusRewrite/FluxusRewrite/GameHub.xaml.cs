using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FluxusRewrite
{
    /// <summary>
    /// Interaction logic for GameHub.xaml
    /// </summary>
    public partial class GameHub : Window
    {
        private readonly Window window;
        public GameHub(Window cW)
        {
            InitializeComponent();
            window = cW;
            System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();
            timer.Tick += idkThing;
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();
        }
        private void idkThing(object sender, EventArgs e)
        {
            shadowEffect.Color = Color.FromRgb(Properties.Settings.Default.shadowColor.R, Properties.Settings.Default.shadowColor.G, Properties.Settings.Default.shadowColor.B);
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            window.LocationChanged += Window_LocChanged;
        }
        private void Window_LocChanged(object sender, EventArgs e)
        {
            Left = window.Left;
            Top = window.Top + window.Height - 30;
        }


    }
}
