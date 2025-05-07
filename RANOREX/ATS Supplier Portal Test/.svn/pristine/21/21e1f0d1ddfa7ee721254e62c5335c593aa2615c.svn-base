using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ATS.CodeLibrary.PLMWorx.WebServices
{
    public class FileServices
    {
        private File file;

        /// <summary>
        /// Provides methods for getting and modifying file version records in PLMWorx
        /// </summary>
        /// <param name="fileName">Filename including extension</param>
        /// <param name="fileVersion">File version with or without leading zeros</param>
        public FileServices(string fileName, string fileVersion)
        {
            file = new File()
            {
                fileName = fileName,
                fileVersion = fileVersion
            };
        }

        private class File
        {
            internal string fileName;
            internal string fileVersion;
        }

        /// <summary>
        /// Check if file version is locked
        /// </summary>
        /// <returns>Lock id if locked or 0 if not locked</returns>
        public int IsFileLocked()
        {
            var val = ServerInfo.SubmitRequest(HttpMethod.Get, $"/atsplmworxapi/is_file_locked?file_name={file.fileName}&file_version={file.fileVersion}");
            var def = new[] { new { is_file_locked = 0 } };
            var temp = Newtonsoft.Json.JsonConvert.DeserializeAnonymousType(val, def);
            return temp.First().is_file_locked;
        }

        /// <summary>
        /// Attempt to add lock to file
        /// </summary>
        /// <param name="checkoutPath">Path to checkout to</param>
        /// <returns>Lock id if lock was successful, 0 if lock failed</returns>
        public int AddLock(string checkoutPath)
        {
            string URL = $"/atsplmworxapi/lock_file?file_name={file.fileName.ToUpper()}&file_version={file.fileVersion}&checkout_path={checkoutPath}";
            var val = ServerInfo.SubmitRequestAlt(URL);
            Console.WriteLine(DateTime.Now.ToString() + "\t" + val + "\t" + URL);

            if (val == "0")
            {
                var ret = IsFileLocked();
                Console.WriteLine("Rechecked" + "\t" + ret + "\t" + URL);

                if (ret == 0)
                {
                    System.Threading.Thread.Sleep(10000);
                    ret = IsFileLocked();
                    Console.WriteLine("Rechecked again" + "\t" + ret + "\t" + URL);
                }

                return ret;
            }

            var def = new[] { new { lock_id = 26141141 } };
            var valOut = Newtonsoft.Json.JsonConvert.DeserializeAnonymousType(val, def);
            return valOut.First().lock_id;
        }

        /// <summary>
        /// Remove lock from file
        /// </summary>
        /// <param name="lockID">Lock ID to remove</param>
        /// <returns>True if successful</returns>
        public static bool RemoveLock(int lockID)
        {
            string val = ServerInfo.SubmitRequest(HttpMethod.Post, $"/atsplmworxapi/remove_file_lock?lock_id={lockID}");
            return val == "1";
        }

        /// <summary>
        /// Remove lock from file
        /// </summary>
        /// <param name="lockID">Lock ID to remove</param>
        /// <returns>True if successful</returns>
        public static bool RemoveLock(decimal lockID)
        {
            return RemoveLock(Convert.ToInt32(lockID));
        }

        /// <summary>
        /// Change CAD software version of file
        /// </summary>
        /// <param name="Version">New CAD software version</param>
        /// <returns>True if succesful</returns>
        public bool UpdateFileVersion(string Version)
        {
            var val = ServerInfo.SubmitRequest(HttpMethod.Post, $"/atsplmworxapi/update_file_version?file_name={file.fileName}&file_version={file.fileVersion}&cad_software_version={Version}");
            return val == "1";
        }

        /// <summary>
        /// Type returned by GetPDF methods
        /// </summary>
        public class FileVersion
        {
            /// <summary>
            /// Version of file
            /// </summary>
            public string file_version { get; set; }

            /// <summary>
            /// Relative vault path to file
            /// </summary>
            public string file_path { get; set; }

            /// <summary>
            /// Database ID for file version
            /// </summary>
            public int PB_FILE_VERSION_ID { get; set; }
        }

        /// <summary>
        /// Get the first PDF associated with the file version
        /// </summary>
        /// <returns>FileVersion (version number, relative vault path, and database ID</returns>
        public FileVersion GetFirstPDF()
        {
            try
            {
                var val = ServerInfo.SubmitRequest(HttpMethod.Get, $"/atsplmworxapi/get_related_drawing_pdf?file_name={file.fileName}&file_version={file.fileVersion}");
                //val = ServerInfo.SubmitRequest(HttpMethod.Get, $"/atsplmworxapi/get_related_drawing_pdf?file_name=31216430.SLDDRW&file_version=001");
                if (val == "")
                    return null;


                return Newtonsoft.Json.JsonConvert.DeserializeObject<List<FileVersion>>(val).First();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Gets all PDFs associated with the file version
        /// </summary>
        /// <returns>Collection of FileVersion (version number, relative vault path, and database ID</returns>
        public IEnumerable<FileVersion> GetPDFs()
        {
            var val = ServerInfo.SubmitRequest(HttpMethod.Get, $"/atsplmworxapi/get_related_drawing_pdf?file_name={file.fileName}&file_version={file.fileVersion}");
            //val = ServerInfo.SubmitRequest(HttpMethod.Get, $"/atsplmworxapi/get_related_drawing_pdf?file_name=31216430.SLDDRW&file_version=001");
            if (val == "")
                return null;

            try
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<List<FileVersion>>(val);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
