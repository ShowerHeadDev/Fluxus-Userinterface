using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using Microsoft.Win32;
using ICSharpCode.AvalonEdit.Document;

namespace FluxusRewrite
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string ver = "1.15a";
        public MainWindow()
        {
            InitializeComponent();
            Directory.CreateDirectory(@"./bin/");
            using (WebClient wc = new WebClient())
            {
                wc.DownloadFile(new Uri("https://fluxteam.xyz/external-files/lua.xshd"), @"./bin/lua.xshd");
                wc.Dispose();
            }
            if (!Directory.Exists(Environment.CurrentDirectory + "/workspace/"))
            {
                Directory.CreateDirectory(Environment.CurrentDirectory + "/workspace");
            }
            if(File.Exists(Environment.CurrentDirectory + "/FluxusTeamAPI.dll"))
            {
                File.Delete(Environment.CurrentDirectory + "/FluxusTeamAPI.dll");
            }
            updateCheck();
            if(!isWin10())
            {
                System.Windows.MessageBox.Show("We have detected you're not using Windows 10, UI icons may not appear!");
            }
            textEditor.Options.EnableHyperlinks = false;
            textEditor.Options.ShowSpaces = false;
            textEditor.Options.ShowTabs = false;
            textEditor.TextArea.TextView.ElementGenerators.Add(new TruncateLongLines());

            System.Windows.Threading.DispatcherTimer themeThing = new System.Windows.Threading.DispatcherTimer();
            themeThing.Tick += _themeThing;
            themeThing.Interval = new TimeSpan(0, 0, 1);
            themeThing.Start();
        }

        private void _themeThing(object sender, EventArgs e)
        {
            shadowEffect.Color = Color.FromRgb(Properties.Settings.Default.shadowColor.R, Properties.Settings.Default.shadowColor.G, Properties.Settings.Default.shadowColor.B);
            headerBar.Background = new SolidColorBrush(Color.FromRgb(Properties.Settings.Default.headerColor.R, Properties.Settings.Default.headerColor.G, Properties.Settings.Default.headerColor.B));
        }

        static bool isWin10()
        {
            var reg = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion");
            string productName = (string)reg.GetValue("ProductName");
            return productName.StartsWith("Windows 10");
        }

        private async void updateCheck()
        {
            using (WebClient wc = new WebClient())
            {
                if (ver.Equals(wc.DownloadString("https://pastebin.com/raw/zXyJReEe"), StringComparison.InvariantCultureIgnoreCase))
                {
                    Uri dll = new Uri(wc.DownloadString("https://pastebin.com/raw/cJpn0qQt"));
                    wc.DownloadFile(dll, Environment.CurrentDirectory + "/FluxusTeamAPI.dll");
                }
                else
                {
                    System.Windows.MessageBox.Show("Update Needed!");
                    await Task.Delay(1500);
                    System.Diagnostics.Process.Start("https://wearedevs.net/d/Fluxus");
                    System.Windows.Application.Current.Shutdown();
                }
            }
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            //headerBarCntnt.Visibility = Visibility.Visible;
            bottomBar.Visibility = Visibility.Visible;
            textEditor.Visibility = Visibility.Visible;
            Animate(
                headerBar,
                "Height",
                headerBar.Height,
                31,
                0.1,
                new Duration(new TimeSpan(TimeSpan.TicksPerSecond)));
        }
        //24
        private void Window_Deactivated(object sender, EventArgs e)
        {
            bottomBar.Visibility = Visibility.Hidden;
            textEditor.Visibility = Visibility.Hidden;
            Animate(
                headerBar,
                "Height",
                headerBar.Height,
                7,
                0.1,
                new Duration(new TimeSpan(TimeSpan.TicksPerSecond)));
            //headerBarCntnt.Visibility = Visibility.Hidden;
        }

        private void headerBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.ChangedButton == MouseButton.Left)
            {
                //if (headerBar.Height == 31)
                //{
                    DragMove();
                //}
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void textEditor_Initialized(object sender, EventArgs e)
        {
            
        }

        private void Animate(DependencyObject item, string property, double from, double to, double? by, Duration duration)
        {
            var storyboard = new Storyboard();
            var animation = new DoubleAnimation();
            animation.From = from;
            animation.To = to;
            animation.Duration = duration;
            animation.EasingFunction = new SineEase() { EasingMode = EasingMode.EaseInOut };
            animation.By = by;
            Storyboard.SetTarget(animation, item);
            Storyboard.SetTargetProperty(animation, new PropertyPath(property));
            storyboard.Children.Add(animation);
            storyboard.Begin();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (File.Exists(@"./bin/lua.xshd"))
            {
                Stream xshd_stream = File.OpenRead(@"./bin/lua.xshd");
                XmlTextReader reader = new XmlTextReader(xshd_stream);
                textEditor.SyntaxHighlighting = HighlightingLoader.Load(reader, HighlightingManager.Instance);
            }
            else
            {
                using (WebClient wc = new WebClient())
                {
                    wc.DownloadFile(new Uri("https://fluxteam.xyz/external-files/lua.xshd"), @"./bin/lua.xshd");
                    wc.Dispose();
                }
                Stream xshd_stream = File.OpenRead(@"./bin/lua.xshd");
                XmlTextReader reader = new XmlTextReader(xshd_stream);
                textEditor.SyntaxHighlighting = HighlightingLoader.Load(reader, HighlightingManager.Instance);
            }
            headerBar.Opacity = 0;
            bottomBar.Opacity = 0;
            textEditor.Opacity = 0;
            Animate(
                circle1,
                "Width",
                0,
                this.Width + 300,
                0.1,
                new Duration(new TimeSpan(TimeSpan.TicksPerSecond)));
            Animate(
                circle1,
                "Height",
                0,
                this.Width + 300,
                0.1,
                new Duration(new TimeSpan(TimeSpan.TicksPerSecond)));
            await Task.Delay(1000);
            Animate(
                headerBar,
                "Opacity",
                0,
                1,
                0.1,
                new Duration(new TimeSpan(TimeSpan.TicksPerSecond)));
            Animate(
                bottomBar,
                "Opacity",
                0,
                1,
                0.1,
                new Duration(new TimeSpan(TimeSpan.TicksPerSecond)));
            Animate(
                textEditor,
                "Opacity",
                0,
                1,
                0.1,
                new Duration(new TimeSpan(TimeSpan.TicksPerSecond)));
        }

        private void injectButton_Click(object sender, RoutedEventArgs e)
        {
            Functions.Inject();
        }

        private void execButton_Click(object sender, RoutedEventArgs e)
        {
            NamedPipes.LuaPipe(textEditor.Text);
        }

        private void clearButton_Click(object sender, RoutedEventArgs e)
        {
            textEditor.Text = "";
        }

        private void openButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog fs = new System.Windows.Forms.OpenFileDialog();
            fs.Title = "Fluxus | Open Script";
            fs.Filter = "Lua Scripts (*.lua)|*.lua|Text Files (*.txt)|*.txt";
            if(fs.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textEditor.Text = File.ReadAllText(fs.FileName);
            }
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.SaveFileDialog fs = new System.Windows.Forms.SaveFileDialog();
            fs.Title = "Fluxus | Save Script";
            fs.Filter = "Lua Scripts (*.lua)|*.lua|Text Files (*.txt)|*.txt";
            if (fs.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                File.WriteAllText(fs.FileName, textEditor.Text);
            }
        }

        private void scriptHubButton_Click(object sender, RoutedEventArgs e)
        {
            NamedPipes.LuaPipe("loadstring(game:HttpGet(\"https://fluxteam.xyz/scripts/Fluxus/scripthub.txt\", true))()");
        }

        private void settingButton_Click(object sender, RoutedEventArgs e)
        {
            if (System.Windows.Application.Current.Windows.Cast<Window>().Any(x => x.GetType() == typeof(Settings))) return;
            Settings settings = new Settings(this);
            settings.Show();
            settings.Left = this.Left + this.Width - 30;
            settings.Top = this.Top;
        }

        private void imGuiButton_Click(object sender, RoutedEventArgs e)
        {
            NamedPipes.LuaPipe("imgui()");
        }

        private void wrdButton_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://wearedevs.net");
            System.Diagnostics.Process.Start("https://youtube.com/iZestyYT?sub_confirmation=1");
        }

        private void button_Copy_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void gameHubButton_Click(object sender, RoutedEventArgs e)
        {
            /*if (System.Windows.Application.Current.Windows.Cast<Window>().Any(x => x.GetType() == typeof(GameHub))) return;
            GameHub gh = new GameHub(this);
            gh.Show();
            gh.Left = this.Left;
            gh.Top = this.Top + this.Height - 30;*/
            System.Windows.Forms.MessageBox.Show("Under Development\nSomething big planned here ;)");
        }
    }
}
