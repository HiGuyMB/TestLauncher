using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TestLauncher.JsonTemplates
{
    class DownloadedConverter<T> : JsonConverter
    {
        T obj;
    
        public override bool CanConvert(Type objectType)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            String address = serializer.Deserialize<String>(reader);
            Uri uri = new Uri(address);

            return new DownloadedField<T>(uri);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
