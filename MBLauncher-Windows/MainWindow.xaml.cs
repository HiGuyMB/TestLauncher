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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MBLauncherLib;
using MBLauncherLib.JsonTemplates;

namespace MBLauncher_Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        LauncherConfig config;

        public MainWindow()
        {
            InitializeComponent();
            config = new LauncherConfig(new Uri("https://marbleblast.com/files/launcher/config.json"));

            Task.Run(config.DownloadConfig).ContinueWith((Task<bool> task) =>
            {
                OnMainThread(() =>
                {
                    this.WaitingLabel.Text = "Loaded Config";
                });
            });
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.WaitingLabel.Text = "Installing MBP...";
            ModConfig mod = config.mods["platinum"];
            Task.Run(() =>
            {
                return mod.InstallMod("C:\\Users\\***REMOVED***\\Desktop\\MBP");
            }).ContinueWith((Task<bool> task) =>
            {
                OnMainThread(() =>
                {
                    this.WaitingLabel.Text = "Installed";
                });
            });
        }

        private void OnMainThread(Action action)
        {
            App.Current.Dispatcher.Invoke(action);
        }
    }
}
