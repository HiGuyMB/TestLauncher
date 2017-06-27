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

        [JsonConverter(typeof(PlatformSpecificConverter<String>))]
        public String launcher;

        [JsonConverter(typeof(PlatformSpecificConverter<String>))]
        public String customlist;

        public IDictionary<String, String> defaultmods;
    }
}
