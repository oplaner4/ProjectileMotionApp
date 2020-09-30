using System;
using System.IO;

namespace Utilities.Files
{
    public static class FileUtilities
    {
        /// <summary>
        /// Gets file name with extension when extension is not defined.
        /// </summary>
        /// <param name="fileName">The file name with or without extension. It cannot be null or empty.</param>
        /// <param name="supposedExtension">The file supposed extension. Empty - extension is removed. It cannot be null.</param>
        public static string GetFileName (string fileName, string supposedExtension)
        {
            if (fileName == null || fileName.Length == 0)
            {
                throw new ArgumentException(nameof(fileName), "The name of file cannot be null or empty");
            }

            if (supposedExtension == null)
            {
                throw new ArgumentException(nameof(fileName), "The extension cannot be null");
            }

            FileInfo fileInfo = new FileInfo(fileName);
            supposedExtension = supposedExtension.StartsWith(".") ? supposedExtension : "." + supposedExtension;


            return fileInfo.Extension.Length == 0 || fileInfo.Extension != supposedExtension && supposedExtension != "."
                ? fileName + supposedExtension
                : fileInfo.Name.Replace(fileInfo.Extension, "") + supposedExtension;
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