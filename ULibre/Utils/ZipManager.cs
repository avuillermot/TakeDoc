using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using Ionic.Zip;
using System.IO;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;

namespace ULibre.Utils
{
    /// <summary>
    /// Permet la gestion des fichiers compressés au format zip
    /// </summary>
    public class ZipManager : Utility.Base.ClassBase
    {
        public void createZip(string source, string zipFile)
        {
            ICSharpCode.SharpZipLib.Zip.FastZip z = new ICSharpCode.SharpZipLib.Zip.FastZip();
            z.CreateEmptyDirectories = true;
            z.RestoreAttributesOnExtract = true;
            z.RestoreDateTimeOnExtract = true;
            z.CreateZip(zipFile,source, true, "");
        }

        /// <summary>
        /// Décompresse entierement un fichier LibreOffice
        /// </summary>
        /// <param name="zipFile"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        public void extractZip(string zipFile, string destination)
        {
            ICSharpCode.SharpZipLib.Zip.FastZip z = new ICSharpCode.SharpZipLib.Zip.FastZip();
            z.CreateEmptyDirectories = true;
            z.RestoreAttributesOnExtract = true;
            z.RestoreDateTimeOnExtract = true;
            z.ExtractZip(zipFile, destination, string.Empty);
        }

        /// <summary>
        /// Unzip a file and return content
        /// </summary>
        /// <param name="file"></param>
        /// <param name="zipEntry"></param>

        public string extractEntry(string zipFile, string zipEntry)
        {
            string retour = string.Empty;
            byte[] data = null;
            FileStream fs = null;
            ZipFile zf = null;
            try
            {

                byte[] buffer = new byte[4096];
                fs = File.OpenRead(zipFile);
                zf = new ZipFile(fs);
                ZipEntry entry = zf.GetEntry(zipEntry);

                if (entry != null)
                {
                    Stream s = zf.GetInputStream(entry);
                    data = new byte[entry.Size];
                    s.Read(data, 0, data.Length);
                }

                retour = System.Text.Encoding.UTF8.GetString(data);
            }
            catch (Exception ex)
            {
                this.setError(ex);
            }
            finally
            {
                if (zf != null) zf.Close();
                if (fs != null) {
                    fs.Close();
                    fs.Dispose();
                }
            }
            return retour.Trim();
        }

        public bool contains(FileInfo zipFile, string zipEntry)
        {
            bool retour = true;
            ZipFile zf = null;
            try
            {
                zf = new ZipFile(File.OpenRead(zipFile.FullName));
                ZipEntry entry = zf.GetEntry(zipEntry);
                if (entry == null) retour = false;
            }
            catch (Exception ex)
            {
                this.setError(ex);
            }
            finally
            {
                if (zf != null) zf.Close();
            }
            return retour;
        }

        public void addZipEntry(string zipFullName, string entryName, MemoryStream ms)
        {
            FileStream sin = null;
            ZipFile zipFile = null;
            CustomStaticDataSource sds;
            try
            {
                sin = new FileStream(zipFullName, FileMode.Open);
                zipFile = new ZipFile(sin);
                zipFile.BeginUpdate();

                sds = new CustomStaticDataSource();
                sds.SetStream(ms);

                zipFile.Add(sds, entryName);
                zipFile.CommitUpdate();
            }
            catch (Exception ex)
            {
                Utility.Logger.myLogger.Error(ex);
            }
            finally
            {
                if (ms != null)
                {
                    ms.Close();
                    ms.Dispose();
                }
                if (zipFile != null) zipFile.Close();
                if (sin != null)
                {
                    sin.Close();
                    sin.Dispose();
                }
            }
        }

        private class CustomStaticDataSource : IStaticDataSource
        {
            private Stream _stream;
            // Implement method from IStaticDataSource
            public Stream GetSource()
            {
                return _stream;
            }

            // Call this to provide the memorystream
            public void SetStream(Stream inputStream)
            {
                _stream = inputStream;
                _stream.Position = 0;
            }
        }
    }
}
