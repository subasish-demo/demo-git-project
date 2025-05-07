using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATS.CodeLibrary.DataUtilities.StringExtensions
{
    public static class StringHelper
    {
        /// <summary>
        /// Add quotes around string
        /// </summary>
        /// <param name="input">value to wrap with quotes</param>
        /// <returns>For input = value, return = "value" (with quotes)</returns>
        public static string WrapWithQuotes(this string input)
        {
            return "\"" + input + "\"";
        }

        /// <summary>
        /// Add suffix to filename
        /// </summary>
        /// <param name="input">File path</param>
        /// <param name="suffix">value to add as suffix</param>
        /// <returns>Filepath with suffix added before extension</returns>
        public static string FileAddSuffix(this string input, string suffix)
        {
            if (suffix == null)
                throw new ArgumentNullException();

            var path = Path.GetDirectoryName(input);
            var filename = Path.GetFileNameWithoutExtension(input);
            var ext = Path.GetExtension(input);

            return Path.Combine(path, filename + suffix + ext);
        }

        /// <summary>
        /// Replace an existing suffix with a new suffix. Do nothing if input does not have suffix.
        /// </summary>
        /// <param name="input">Filepath with or without directory</param>
        /// <param name="oldSuffix">value to remove</param>
        /// <param name="newSuffix">value to add</param>
        /// <returns>Original path with oldSuffix replaced with newSuffix</returns>
        public static string FileReplaceSuffix(this string input, string oldSuffix, string newSuffix)
        {
            if (oldSuffix == null || newSuffix == null)
                throw new ArgumentNullException();

            var path = Path.GetDirectoryName(input);
            var filename = Path.GetFileNameWithoutExtension(input);
            var ext = Path.GetExtension(input);

            if (!filename.EndsWith(oldSuffix))
                return input;
                        
            return Path.Combine(path, filename.Substring(0, filename.Length - oldSuffix.Length) + newSuffix + ext);
        }

        /// <summary>
        /// Remove suffix from file path
        /// </summary>
        /// <param name="input">Filepath with or without directory</param>
        /// <param name="suffix">value to remove</param>
        /// <returns></returns>
        public static string FileRemoveSuffix(this string input, string suffix)
        {
            if (suffix == null)
                throw new ArgumentNullException();

            return input.FileReplaceSuffix(suffix, "");
        }

        /// <summary>
        /// Remove prefix from file name in path
        /// </summary>
        /// <param name="input">Filepath with or without directory</param>
        /// <param name="prefix">value to remove</param>
        /// <returns></returns>
        public static string FileRemovePrefix(this string input, string prefix)
        {
            return input.FileReplacePrefix(prefix, "");
        }

        /// <summary>
        /// Replace an existing prefix with a new prefix. Do nothing if input does not have prefix.
        /// </summary>
        /// <param name="input">Filepath with or without directory</param>
        /// <param name="oldPrefix">value to remove</param>
        /// <param name="newPrefix">value to add</param>
        /// <returns>Original path with oldPrefix replaced with newPrefix</returns>
        public static string FileReplacePrefix(this string input, string oldPrefix, string newPrefix)
        {
            if (oldPrefix == null || newPrefix == null)
                throw new ArgumentNullException();

            var path = Path.GetDirectoryName(input);
            var filename = Path.GetFileNameWithoutExtension(input);
            var ext = Path.GetExtension(input);

            if (!filename.StartsWith(oldPrefix))
                return input;

            return Path.Combine(path, newPrefix + filename.Substring(oldPrefix.Length) + ext);
        }

        /// <summary>
        /// Remove prefix from file path
        /// </summary>
        /// <param name="input">Filepath with or without directory</param>
        /// <param name="prefix">value to remove</param>
        public static string RemovePrefix(this string input, string prefix)
        {
            return input.FileReplacePrefix(prefix, "");
        }
    }
}
