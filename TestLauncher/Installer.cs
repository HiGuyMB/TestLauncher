using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TestLauncher.JsonTemplates;

namespace TestLauncher
{
    class Installer
    {
        ModConfig m_config;
        string m_baseDirectory;
        protected string m_tempDirectory;

        public Installer(ModConfig config, string baseDirectory)
        {
            m_config = config;
            m_baseDirectory = baseDirectory;
            m_tempDirectory = m_baseDirectory + "/.tmp/";
        }

        async public Task<bool> Install()
        {
            Directory.CreateDirectory(m_baseDirectory);
            Directory.CreateDirectory(m_tempDirectory);

            foreach (String package in m_config.packages.value.Keys)
            {
                bool installed = await InstallPackage(package);
                if (!installed)
                {
                    return false;
                }

            }

            Directory.Delete(m_tempDirectory, true);

            return true;
        }

        async Task<bool> InstallPackage(string name)
        {
            Uri address = m_config.packages.value[name];

            using (WebClient client = new WebClient())
            {
                //Progress updates
                client.DownloadProgressChanged += (object sender, DownloadProgressChangedEventArgs e) =>
                {
                    Console.Write(String.Format("\rDownload of {0} progress: {1}% {2} / {3} ", name, (e.BytesReceived * 100 / e.TotalBytesToReceive), e.BytesReceived, e.TotalBytesToReceive));
                };

                //Temp file where we download the zip
                string tempFile = m_tempDirectory + address.Segments.Last();
                await client.DownloadFileTaskAsync(address, tempFile);

                //Then load that temp file
                using (ZipFile zip = ZipFile.Read(tempFile))
                {
                    foreach (ZipEntry entry in zip)
                    {
                        entry.Extract(m_baseDirectory, ExtractExistingFileAction.OverwriteSilently);
                    }
                }
                
                //Clean up
                File.Delete(tempFile);
            }

            return true;
        }
    }
}
