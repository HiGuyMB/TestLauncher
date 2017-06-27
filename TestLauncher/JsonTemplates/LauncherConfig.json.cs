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
        [JsonConverter(typeof(PlatformSpecificField<String>))]
        public String launchermd5;

        [JsonConverter(typeof(PlatformSpecificField<String>))]
        public String selfupdate;

        [JsonConverter(typeof(PlatformSpecificField<String>))]
        public String launcher;

        [JsonConverter(typeof(PlatformSpecificField<String>))]
        public String customlist;

        public IDictionary<String, String> defaultmods;
    }
}
