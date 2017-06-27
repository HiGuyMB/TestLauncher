using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TestLauncher.JsonTemplates
{
    class DownloadedField<T>
    {
        public bool ready;
        public T value;

        public DownloadedField(Uri address)
        {
            WebClient client = new WebClient();
            client.DownloadDataCompleted += DataCompleted;
            client.DownloadDataAsync(address);

            ready = false;
        }

        private void DataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            try
            {
                string json = Encoding.UTF8.GetString(e.Result);
                value = JsonConvert.DeserializeObject<T>(json);

                ready = true;
            }
            catch (Exception ex)
            {

            }
        }

    }
}
