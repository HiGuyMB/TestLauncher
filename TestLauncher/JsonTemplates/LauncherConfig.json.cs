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

        Uri m_address;
        
        public LauncherConfig(Uri address)
        {
            mods = new Dictionary<String, ModConfig>();
            m_address = address;
        }

        async public Task<bool> DownloadConfig()
        {
			WebClient client = new WebClient();
			byte[] data = await client.DownloadDataTaskAsync(m_address);

			string json = Encoding.UTF8.GetString(data);
            return await DownloadMods();
		}

        /// <summary>
        /// Download the mod at a given address and add it to the mods list for this config.
        /// </summary>
        /// <param name="address">Address of the mod's config.json file</param>
        /// <returns>If the download was successful</returns>
        async Task<bool> DownloadMod(Uri address)
        {
            WebClient client = new WebClient();
            byte[] data = await client.DownloadDataTaskAsync(address);

            string json = Encoding.UTF8.GetString(data);
            try
            {
                ModConfig config = JsonConvert.DeserializeObject<ModConfig>(json);
                mods.Add(config.name, config);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Download the config files for the mods this launcher config contains. Does not
        /// call DownloadConfig() on the mods that are created.
        /// </summary>
        /// <returns>If the download was successful</returns>
        async Task<bool> DownloadMods()
        {
            List<Task<bool>> tasks = new List<Task<bool>>();
            foreach (Uri address in defaultmods.Keys)
            {
                tasks.Add(DownloadMod(address));
            }

            //We're successful when all mods are downloaded successfully
            bool[] results = await Task.WhenAll<bool>(tasks.ToArray());
            return !results.Contains(false);
        }
    }
}
