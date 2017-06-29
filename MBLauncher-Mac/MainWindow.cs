using System;
using System.Threading.Tasks;

using Foundation;
using AppKit;
using MBLauncherLib.JsonTemplates;

namespace MBLauncherMac
{
    public partial class MainWindow : NSWindow
    {
		LauncherConfig config;
		
        public MainWindow(IntPtr handle) : base(handle)
        {
        }

        [Export("initWithCoder:")]
        public MainWindow(NSCoder coder) : base(coder)
        {
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
			config = new LauncherConfig(new Uri("https://marbleblast.com/files/launcher/config.json"));

			Task.Run(config.DownloadConfig).ContinueWith((Task<bool> task) =>
			{
				OnMainThread(() =>
				{
                    this.WaitingLabel.StringValue = "Loaded Config";
				});
			});

		}
        
        partial void Install(Foundation.NSObject sender)
        {
            this.WaitingLabel.StringValue = "Installing MBP...";
            ModConfig mod = config.mods["platinum"];

            string installDir = GetInstallDir("platinum");

            Task.Run(() =>
            {
				try
				{
					mod.InstallMod(installDir);
				}
				catch (Exception ex)
				{
                    this.WaitingLabel.StringValue = "Oh no";
				}
            }).ContinueWith((Task task) =>
            {
                OnMainThread(() =>
                {
                    this.WaitingLabel.StringValue = "Installed";
                });
            });
        }

        public string GetInstallDir(string mod)
        {
            //Check preferences
			string modKey = "installpath-" + mod;
            if (NSUserDefaults.StandardUserDefaults.URLForKey(modKey) != null)
            {
                return NSUserDefaults.StandardUserDefaults.URLForKey(modKey).Path;
            }
            //Get install directory
            NSOpenPanel openPanel = NSOpenPanel.OpenPanel;
            openPanel.AllowsMultipleSelection = false;
            openPanel.CanChooseDirectories = true;
            openPanel.AllowedFileTypes = new string[]{"app"};
            openPanel.CanChooseFiles = true;
            if (openPanel.RunModal() == (int)NSModalResponse.OK)
            {
                NSUrl dir = openPanel.Urls[openPanel.Urls.Length - 1];
                NSUserDefaults.StandardUserDefaults.SetURL(dir, new NSString(modKey));

                return dir.Path;
            }

            throw(new Exception("Cancelled Directory Search"));
        }

        void OnMainThread(Action action)
        {
            InvokeOnMainThread(action);
        }
    }
}
