﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        [JsonConverter(typeof(DownloadedPlatformSpecificConverter<JObject>))]
        public DownloadedField<JObject> prunelist;
        [JsonConverter(typeof(DownloadedPlatformSpecificConverter<IDictionary<String, String>>))]
        public DownloadedField<IDictionary<String, String>> packages;
        [JsonConverter(typeof(DownloadedPlatformSpecificConverter<JObject>))]
        public DownloadedField<JObject> listing;
        [JsonConverter(typeof(DownloadedConverter<IDictionary<String, String>>))]
        public DownloadedField<IDictionary<String, String>> conversions;
        [JsonConverter(typeof(DownloadedConverter<IDictionary<String, String>>))]
        public DownloadedField<IDictionary<String, String>> migrations;
        [JsonConverter(typeof(DownloadedConverter<IDictionary<String, String>>))]
        public DownloadedField<IDictionary<String, String>> searches;
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
