using Ionic.Crc;
using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using MBLauncherLib.JsonTemplates;

namespace MBLauncherLib
{
    public class Installer
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

        /// <summary>
        /// Install the mod. Main work function
        /// </summary>
        /// <returns>True if install succeeded</returns>
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

        /// <summary>
        /// Download and install files from a specific mod package.
        /// </summary>
        /// <param name="package">Package whose files to download and install</param>
        /// <returns>True if install succeeded</returns>
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
                        string fullFile = m_baseDirectory + "/" + entry.FileName;
                        if (!entry.IsDirectory && File.Exists(fullFile))
                        {
                            //Check if we need to extract this
                            if (entry.Crc == GetCRC32(fullFile))
                            {
                                continue;
                            }
                        }
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

        /// <summary>
        /// Find if any file in a package is not present or has changed.
        /// </summary>
        /// <param name="package">Package whose files are checked</param>
        /// <returns>True if there has been a change</returns>
        bool FindUnmatchedFile(ModPackage package)
        {
            foreach (ModPackage.PackageEntry file in package.GetFiles())
            {
                string fullPath = m_baseDirectory + file.path;
                //New file
                if (!File.Exists(fullPath))
                {
                    return true;
                }
                //Changed file
                if (GetMD5(fullPath) != file.md5)
                {
                    return true;
                }
            }
            //Nothing new
            return false;
        }

        /// <summary>
        /// Get the MD5 hash of a file (hex string)
        /// </summary>
        /// <param name="path">Path of the file</param>
        /// <returns>Hex string of the hash</returns>
        string GetMD5(string path)
        {
            //https://stackoverflow.com/a/827694
            StringBuilder builder = new StringBuilder();
            MD5 hasher = MD5.Create();
            using (FileStream stream = File.OpenRead(path))
            {
                foreach (Byte b in hasher.ComputeHash(stream))
                    builder.Append(b.ToString("x2").ToLower());
            }

            return builder.ToString();
        }

        /// <summary>
        /// Get the CRC32 hash of a file (hex string)
        /// </summary>
        /// <param name="path">Path of the file</param>
        /// <returns>Hash</returns>
        int GetCRC32(string path)
        {
            CRC32 hasher = new CRC32();
            using (FileStream stream = File.OpenRead(path))
            {
                return hasher.GetCrc32(stream);
            }
        }
    }
}
