using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
