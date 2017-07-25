using MBLauncherLib.JsonTemplates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBLauncher_Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.Error.WriteLine("Usage: MBLauncher-Cli <mod> <install dir>");
                Environment.Exit(1);
            }

            LauncherConfig config = new LauncherConfig(new Uri("https://marbleblast.com/files/launcher/config.json"));
            Task.WaitAll(config.DownloadConfig());

            ModConfig mod = null;
            if (config.mods.ContainsKey(args[0]))
            {
                mod = config.mods[args[0]];
            }
            else if (Uri.IsWellFormedUriString(args[0], UriKind.RelativeOrAbsolute))
            {
                Task<ModConfig> task = config.DownloadMod(new Uri(args[0]));
                Task.WaitAll(task);
                mod = task.Result;
            }

            if (mod != null)
            {
                Task.WaitAll(mod.InstallMod(args[1]));
            }
        }
    }
}
