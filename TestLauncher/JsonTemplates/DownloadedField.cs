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

        public void Download()
        {
            Console.WriteLine(String.Format("Downloading file {0}", m_address.ToString()));
            WebClient client = new WebClient();
            byte[] data = client.DownloadData(m_address);

            Console.WriteLine(String.Format("Downloaded file {0}", m_address.ToString()));

            try
            {
                string json = Encoding.UTF8.GetString(data);
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
