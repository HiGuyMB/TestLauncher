using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
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

            //See which packages we need to install
            ModPackage[] installList = GetInstallList();

            Directory.CreateDirectory(m_tempDirectory);

            foreach (ModPackage package in installList)
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

        async Task<bool> InstallPackage(ModPackage package)
        {
            Uri address = package.Address;

            using (WebClient client = new WebClient())
            {
                //Progress updates
                client.DownloadProgressChanged += (object sender, DownloadProgressChangedEventArgs e) =>
                {
                    Console.Write(String.Format("\rDownload of {0} progress: {1}% {2} / {3} ", package.Name, (e.BytesReceived * 100 / e.TotalBytesToReceive), e.BytesReceived, e.TotalBytesToReceive));
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

        /// <summary>
        /// Get the list of packages that we should download and install
        /// </summary>
        /// <returns>A string list of packages</returns>
        ModPackage[] GetInstallList()
        {
            List<ModPackage> packagesToInstall = new List<ModPackage>();

            //Check current packages
            foreach (ModPackage package in m_config.modPackages.Values)
            {
                if (FindUnmatchedFile(package))
                {
                    packagesToInstall.Add(package);
                }
            }

            return packagesToInstall.ToArray();
        }

        bool FindUnmatchedFile(ModPackage package)
        {
            foreach (ModPackage.PackageEntry file in package.GetFiles())
            {
                string fullPath = m_baseDirectory + file.path;
                if (!File.Exists(fullPath))
                {
                    return true;
                }
                //Check MD5
                if (GetMD5(fullPath) != file.md5)
                {
                    return true;
                }
            }
            return false;
        }

        string GetMD5(string path)
        {
            StringBuilder builder = new StringBuilder();
            MD5 hasher = MD5.Create();
            using (FileStream stream = File.OpenRead(path))
            {
                foreach (Byte b in hasher.ComputeHash(stream))
                    builder.Append(b.ToString("x2").ToLower());
            }

            return builder.ToString();
        }
    }
}
