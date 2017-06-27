using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestLauncher.JsonTemplates
{
    class ModConfig
    {
        public String name;
        public String gamename;
        public String shortname;
        public String image;
        [JsonConverter(typeof(PlatformSpecificField<String>))]
        public String prunelist;
        [JsonConverter(typeof(PlatformSpecificField<String>))]
        public String packages;
        [JsonConverter(typeof(PlatformSpecificField<String>))]
        public String listing;
        public String conversions;
        public String migrations;
        public String searches;
        public String prefsfile;
        public String lineending;
        public String opensub;
        public String rootname;
        public String title;
        public String news;
        public String changelog;
        public String doconsolepost;
        public String consolepost;
        public String consoleposttitle;
        public String consolepostmessage;
        public String consolepostattachmentname;
        public String consolepostattachmentfile;
        public String offlinetitle;
        public String offlinemessage;
        public String docopyprefs;
        public String copyprefsask;
        public String copyposttitle;
        public String copyprefsmessage;
        public IDictionary<String, String> copydata;
        [JsonConverter(typeof(PlatformSpecificField<String>))]
        public String launchpath;
    }
}
