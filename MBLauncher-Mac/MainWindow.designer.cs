// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace MBLauncherMac
{
    [Register ("MainWindow")]
    partial class MainWindow
    {
        [Outlet]
        AppKit.NSTextField WaitingLabel { get; set; }

        [Action ("Install:")]
        partial void Install (Foundation.NSObject sender);
        
        void ReleaseDesignerOutlets ()
        {
            if (WaitingLabel != null) {
                WaitingLabel.Dispose ();
                WaitingLabel = null;
            }
        }
    }
}
