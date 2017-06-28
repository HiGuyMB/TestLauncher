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

            LoadConfig(address);

            Console.Read();
        }

        static void LoadConfig(Uri address)
        {
            WebClient client = new WebClient();
            byte[] data = client.DownloadData(address);

            string json = Encoding.UTF8.GetString(data);

            try
            {
                LauncherConfig config = JsonConvert.DeserializeObject<LauncherConfig>(json);
                Console.WriteLine("Downloading config...");
                Task.WaitAll(config.Download());
                Console.WriteLine("Got config");
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }
    }
}
