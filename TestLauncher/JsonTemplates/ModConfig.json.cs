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
        [JsonConverter(typeof(PlatformSpecificConverter<String>))]
        public String prunelist;
        [JsonConverter(typeof(PlatformSpecificConverter<String>))]
        public String packages;
        [JsonConverter(typeof(PlatformSpecificConverter<String>))]
        public String listing;
        public String conversions;
        [JsonConverter(typeof(DownloadedConverter<IDictionary<String, String>>))]
        public DownloadedField<IDictionary<String, String>> migrations;
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
        [JsonConverter(typeof(PlatformSpecificConverter<String>))]
        public String launchpath;
    }
}
