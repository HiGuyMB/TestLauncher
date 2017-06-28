using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TestLauncher.JsonTemplates;

namespace TestLauncher
{
    class Program
    {
        static void Main(string[] args)
        {
            Uri address = new Uri("https://marbleblast.com/files/launcher/config.json");

            LauncherConfig config = LoadConfig(address);
            ModConfig mbp = config.mods["platinum"];


            Console.Read();
        }

        static LauncherConfig LoadConfig(Uri address)
        {
            WebClient client = new WebClient();
            byte[] data = client.DownloadData(address);

            string json = Encoding.UTF8.GetString(data);

            try
            {
                LauncherConfig config = JsonConvert.DeserializeObject<LauncherConfig>(json);
                Task.WaitAll(config.DownloadConfig());
                Console.WriteLine("Got config");

                return config;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return null;
            }
        }
    }
}
