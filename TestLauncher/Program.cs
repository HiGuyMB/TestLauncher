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

            WebClient client = new WebClient();
            client.DownloadDataCompleted += ConfigCompleted;
            client.DownloadDataAsync(address);

            Console.Read();
        }

        private static void ConfigCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            string json = Encoding.UTF8.GetString(e.Result);

            try
            {
                LauncherConfig config = JsonConvert.DeserializeObject<LauncherConfig>(json);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }
    }
}
