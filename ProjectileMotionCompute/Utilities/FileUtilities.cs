using System;
using System.IO;

namespace Utilities.Files
{
    public class FileUtilities
    {
        /// <summary>
        /// Gets file name with extension when extension is not defined.
        /// </summary>
        /// <param name="fileName">The name of file with or without extension. It cannot be null or empty.</param>
        /// <param name="extension">The extension to fill to file name. It cannot be null or empty.</param>
        public static string GetFileNameWithExtension(string fileName, string extension)
        {
            if (fileName == null || fileName.Length == 0) throw new ArgumentException(nameof(fileName), "The name of file cannot be null or empty");
            if (extension == null || extension.Length == 0) throw new ArgumentException(nameof(fileName), "The extension cannot be null or empty");
            extension = extension.StartsWith(".") ? extension : "." + extension;

            FileInfo fileInfo = new FileInfo(fileName);
            if (fileInfo.Extension.Length == 0 || fileInfo.Extension != extension)
                return fileName + extension;

            return fileInfo.Name.Replace(fileInfo.Extension, "") + extension;
        }

        /// <summary>
        /// Gets folder path with slash on its end.
        /// </summary>
        /// <param name="path">The path. It cannot be null.</param>
        public static string EndPathWithSlash(string path)
        {
            if (path == null) throw new ArgumentException(nameof(path), "The path cannot be null");
            if (path.Length == 0) return "\\";
            FileInfo fileInfo = new FileInfo(path);
            return (fileInfo.Extension.Length > 0 ?  fileInfo.DirectoryName : fileInfo.FullName) + "\\";
            
        }
    }
}