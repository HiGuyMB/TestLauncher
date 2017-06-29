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
            Task.Run(() =>
            {
                return mod.InstallMod("TODO: Path Manager");
            }).ContinueWith((Task<bool> task) =>
            {
                OnMainThread(() =>
                {
                    this.WaitingLabel.StringValue = "Installed";
                });
            });
        }

        void OnMainThread(Action action)
        {
            InvokeOnMainThread(action);
        }
    }
}
