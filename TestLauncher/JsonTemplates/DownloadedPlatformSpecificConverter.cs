using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TestLauncher.JsonTemplates
{
    class DownloadedPlatformSpecificConverter<T> : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            try
            {
                IDictionary<String, String> dict = serializer.Deserialize<IDictionary<String, String>>(reader);
                
                String address = dict["windows"];
                Uri uri = new Uri(address);

                return new DownloadedField<T>(uri);
            }
            catch (Exception e)
            {
                String address = serializer.Deserialize<String>(reader);
                Uri uri = new Uri(address);

                return new DownloadedField<T>(uri);
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
