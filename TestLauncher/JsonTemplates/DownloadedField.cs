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

        /// <summary>
        /// Download the file this field is assigned to
        /// </summary>
        /// <returns>If the download was successful</returns>
        async public Task<bool> Download()
        {
            WebClient client = new WebClient();
            byte[] data = await client.DownloadDataTaskAsync(m_address);

            try
            {
                string json = Encoding.UTF8.GetString(data);
                value = JsonConvert.DeserializeObject<T>(json);

                ready = true;
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}
