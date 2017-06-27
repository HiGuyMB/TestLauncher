using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TestLauncher.JsonTemplates
{
    class LauncherConfig
    {
        [JsonConverter(typeof(PlatformSpecificConverter<String>))]
        public String launchermd5;

        [JsonConverter(typeof(PlatformSpecificConverter<String>))]
        public String selfupdate;

        [JsonConverter(typeof(PlatformSpecificConverter<Uri>))]
        public Uri launcher;

        [JsonConverter(typeof(PlatformSpecificConverter<Uri>))]
        public Uri customlist;

        public IDictionary<Uri, String> defaultmods;

        public IDictionary<String, ModConfig> mods;
        
        LauncherConfig()
        {
            mods = new Dictionary<String, ModConfig>();
        }

        void DownloadMod(Uri address)
        {
            Console.WriteLine(String.Format("Downloading mod from {0}", address.ToString()));
            WebClient client = new WebClient();
            byte[] data = client.DownloadData(address);

            string json = Encoding.UTF8.GetString(data);
            try
            {
                ModConfig config = JsonConvert.DeserializeObject<ModConfig>(json);
                mods.Add(config.name, config);

                Console.WriteLine(String.Format("Got mod: {0}", config.name));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public Task Download()
        {
            List<Task> tasks = new List<Task>();
            foreach (Uri address in defaultmods.Keys)
            {
                tasks.Add(Task.Factory.StartNew(() => DownloadMod(address)));
            }
            return Task.WhenAll(tasks.ToArray());
        }
    }
}
