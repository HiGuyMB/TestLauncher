using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestLauncher.JsonTemplates
{
    [JsonObject(MemberSerialization.OptIn)]
    class LauncherConfig
    {
        [JsonProperty("launchermd5"), JsonConverter(typeof(PlatformSpecificField<String>))]
        public String launchermd5;

        [JsonProperty("selfupdate"), JsonConverter(typeof(PlatformSpecificField<String>))]
        public String selfupdate;

        [JsonProperty("launcher"), JsonConverter(typeof(PlatformSpecificField<String>))]
        public String launcher;

        [JsonProperty("customlist"), JsonConverter(typeof(PlatformSpecificField<String>))]
        public String customlist;

        [JsonProperty("defaultmods")]
        public IDictionary<String, String> defaultmods;
    }
}
