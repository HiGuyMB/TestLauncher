﻿using System;
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
using Microsoft.WindowsAPICodePack.Dialogs;

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

            string installDir = GetInstallDir("platinum");

            Task.Run(() =>
            {
                try
                {
                    Task.WaitAll(mod.InstallMod(installDir));
                }
                catch (Exception ex)
                {
                    this.WaitingLabel.Text = "Oh no";
                }
            }).ContinueWith((Task task) =>
            {
                OnMainThread(() =>
                {
                    this.WaitingLabel.Text = "Installed";
                });
            });
        }

        public string GetInstallDir(string mod)
        {
            var settings = MBLauncher_Windows.Properties.Settings.Default;
            
            if (settings.InstallPath == null)
            {
                settings.InstallPath = new System.Collections.Specialized.StringDictionary();
            }

            //Check preferences
            if (settings.InstallPath.ContainsKey(mod))
            {
                return settings.InstallPath[mod];
            }

            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            dialog.Title = "Choose Install Location";
            dialog.EnsureValidNames = true;
            dialog.Multiselect = false;

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                var dir = dialog.FileName;
                settings.InstallPath[mod] = dir;
                settings.Save();

                return dir;
            }

            //No?
            throw (new Exception("Cancelled by user"));
        }


        private void OnMainThread(Action action)
        {
            App.Current.Dispatcher.Invoke(action);
        }
    }
}
