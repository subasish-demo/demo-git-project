using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATS.CodeLibrary.Windows
{
    public class FileHelper
    {
        // prevents race conditions while creating folders
        private static object _folderLock = new object();

        /// <summary>
        /// Overwrite files in the destination with files in the source that have a more recent last write time
        /// </summary>
        /// <param name="sourceFolder">Full path to source folder</param>
        /// <param name="destinationFolder">Full path to destination folder</param>
        /// <param name="searchPattern">The search string to match against names of files in path. This parameter can contain a combination of valid literal path and wildcard (* and ?) characters (see Remarks for Directory.GetFiles), but doesn't support regular expressions.</param>
        /// <param name="includeSubFolders">Include all subfolders or only the source folder</param>
        public static void CopyDirectoryIfNewer(string sourceFolder, string destinationFolder, string searchPattern, bool includeSubFolders)
        {
            //Prepare arguments
            if (!sourceFolder.EndsWith(Path.DirectorySeparatorChar.ToString())) { sourceFolder += Path.DirectorySeparatorChar; }
            if (!destinationFolder.EndsWith(Path.DirectorySeparatorChar.ToString())) { destinationFolder += Path.DirectorySeparatorChar; }
            var dir = new DirectoryInfo(sourceFolder);
            SearchOption so = (includeSubFolders ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
            var files = Directory.EnumerateFiles(sourceFolder, searchPattern, so);

            // Create missing folders
            var folders = files.Select(x => Path.GetDirectoryName(x)).Distinct();
            foreach (var folder in folders)
            {
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
            }

            // Copy files
            Parallel.ForEach(files, (sourceFilename) =>
            {
                string destFilename = sourceFilename.Replace(sourceFolder, destinationFolder);
                string destDirectory = Path.GetDirectoryName(destFilename);                
                CopyFileIfNewer(sourceFilename, destFilename);
            });
        }

        /// <summary>
        /// Overwrite the destination file if the source file has a more recent last write time. Creates destination folder if it does not exist.
        /// </summary>
        /// <param name="sourceFilepath">Full path to source file</param>
        /// <param name="destinationFilepath">Full path to destination file</param>
        public static bool CopyFileIfNewer(string sourceFilepath, string destinationFilepath)
        {
            FileInfo srcFile = new FileInfo(sourceFilepath);
            FileInfo destFile = new FileInfo(destinationFilepath);
            bool ret = false;

            if (!destFile.Exists || srcFile.LastWriteTime > destFile.LastWriteTime)
            {
                if (!Directory.Exists(destFile.DirectoryName))
                    Directory.CreateDirectory(destFile.DirectoryName);

                File.Copy(srcFile.FullName, destFile.FullName, true);
                ret = true;
            }

            return ret;
        }
    }
}
