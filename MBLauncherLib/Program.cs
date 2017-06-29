using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MBLauncherLib.JsonTemplates;

namespace MBLauncherLib
{
    class Program
    {
        static void Main(string[] args)
        {
            Uri address = new Uri("https://marbleblast.com/files/launcher/config.json");

            LauncherConfig config = new LauncherConfig(address);
            Task.WaitAll(config.DownloadConfig());

            ModConfig mbp = config.mods["platinum"];

            Task.WaitAll(mbp.InstallMod(""));
            Console.WriteLine("After Install");

            Console.Read();
        }
    }
}
