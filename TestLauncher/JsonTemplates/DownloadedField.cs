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
        protected Uri m_address;
        public bool ready;
        public T value;

        public DownloadedField(Uri address)
        {
            m_address = address;
            ready = false;
        }

        async public void Start()
        {
            WebClient client = new WebClient();
            client.DownloadDataCompleted += DataCompleted;
            await client.DownloadDataTaskAsync(m_address);
        }

        private void DataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            if (e.Error != null || e.Cancelled)
            {
                //TODO: Error handling
                return;
            }

            try
            {
                string json = Encoding.UTF8.GetString(e.Result);
                value = JsonConvert.DeserializeObject<T>(json);

                ready = true;
            }
            catch (Exception ex)
            {
                //TODO: Error handling
            }
        }

    }
}
