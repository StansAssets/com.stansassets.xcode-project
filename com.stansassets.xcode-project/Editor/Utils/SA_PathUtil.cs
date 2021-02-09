using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace SA.Foundation.Utility
{
    public static class SA_PathUtil
    {
        public const string FOLDER_SEPARATOR = "/";

        /// <summary>
        /// Use this method to make sure given path is a correct 
        /// project folder relative path.
        /// Methods will check path end and begin, and will try to fix is 
        /// if any issue is found
        /// </summary>
        /// <param name="path"> Filesystem project folder relative path.</param> 
        public static string FixRelativePath(string path)
        {
            if (path.StartsWith(FOLDER_SEPARATOR, System.StringComparison.CurrentCulture)) path = path.Remove(0, 1);

            if (path.EndsWith(FOLDER_SEPARATOR, System.StringComparison.CurrentCulture)) path = path.Substring(0, path.Length - 1);

            return path;
        }

        /// <summary>
        /// Convert's an application file path to absolute system path.
        /// For Editor application relative path is the root of an Asset folder.
        /// </summary>
        /// <param name="relativePath"> Filesystem project folder relative path.</param> 
        public static string ConvertRelativeToAbsolutePath(string relativePath)
        {
            relativePath = FixRelativePath(relativePath);

            string dataPath;
            if (Application.isEditor)
            {
                dataPath = Application.dataPath;

                //Removing the "Assets" folder from path
                dataPath = dataPath.Substring(0, dataPath.Length - 7);
            }
            else
            {
                dataPath = Application.persistentDataPath;
            }

            return dataPath + FOLDER_SEPARATOR + relativePath;
        }

        /// <summary>
        /// Determines whether the given path refers to an existing directory on disk
        /// </summary>
        /// <param name="path"> Filesystem project folder relative path.</param> 
        public static bool IsDirectoryExists(string path)
        {
            return Directory.Exists(ConvertRelativeToAbsolutePath(path));
        }

        /// <summary>
        /// Determines whether the given path refers to an existing directory on disk
        /// </summary>
        /// <param name="path"> Filesystem project folder relative path.</param> 
        public static bool IsFileExists(string path)
        {
            return File.Exists(ConvertRelativeToAbsolutePath(path));
        }

        /// <summary>
        /// Returns the extension of the specified path string.
        /// </summary>
        /// <param name="filePath">Filesystem project folder relative file path.</param>
        /// <returns></returns>
        public static string GetExtension(string filePath)
        {
            return Path.GetExtension(ConvertRelativeToAbsolutePath(filePath));
        }

        /// <summary>
        /// Returns the file name and extension of the specified path string.
        /// </summary>
        /// <param name="filePath">Filesystem project folder relative file path.</param>
        /// <returns></returns>
        public static string GetFileName(string filePath)
        {
            return Path.GetFileName(ConvertRelativeToAbsolutePath(filePath));
        }

        /// <summary>
        /// Returns the file name without extension of the specified path string.
        /// </summary>
        /// <param name="filePath">Filesystem project folder relative file path.</param>
        /// <returns></returns>
        public static string GetFileNameWithoutExtension(string filePath)
        {
            return Path.GetFileNameWithoutExtension(ConvertRelativeToAbsolutePath(filePath));
        }

        /// <summary>
        /// Returns Directory information for path, or null if path denotes a root directory or is null. 
        /// Returns Empty if path does not contain directory information.
        /// </summary>
        /// <param name="path">The path of a file or directory.</param>
        public static string GetDirectoryPath(string path)
        {
            return Path.GetDirectoryName(path).Replace("\\", "/");
        }

        public static List<string> GetDirectoriesOutOfPath(string path)
        {
            var directories = new List<string>();
            var parentFolder = string.Empty;
            var separatorIndex = path.IndexOf(FOLDER_SEPARATOR, System.StringComparison.CurrentCulture);
            while (separatorIndex != -1)
            {
                var offset = separatorIndex + 1;
                var testedFolder = string.Concat(parentFolder, path.Substring(0, offset));
                directories.Add(testedFolder.Substring(0, testedFolder.Length - 1));

                path = path.Substring(offset, path.Length - offset);
                separatorIndex = path.IndexOf(FOLDER_SEPARATOR, System.StringComparison.CurrentCulture);
                parentFolder = testedFolder;
            }

            return directories;
        }

        /// <summary>
        /// Methods return's name of the last directory in a path
        /// for example: from /x/y/z/ -> z will be result 
        /// </summary>
        public static string GetPathDirectoryName(string folderPath)
        {
            folderPath = FixRelativePath(folderPath);

            var separatorIndex = folderPath.LastIndexOf(FOLDER_SEPARATOR, System.StringComparison.CurrentCulture);

            if (separatorIndex == -1)
            {
                return folderPath;
            }
            else
            {
                var offset = separatorIndex + 1;
                return folderPath.Substring(offset, folderPath.Length - offset);
            }
        }
    }
}
