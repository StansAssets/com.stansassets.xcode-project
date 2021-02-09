////////////////////////////////////////////////////////////////////////////////
//  
// @module Assets Common Lib
// @author Osipov Stanislav (Stan's Assets) 
// @support support@stansassets.com
// @website https://stansassets.com
//
////////////////////////////////////////////////////////////////////////////////

using System.IO;
using System.Collections.Generic;
using UnityEngine;

namespace SA.Foundation.Utility
{
    /// <summary>
    /// API for manipulation with files
    /// Can be used with Unity ediotr and with runtime as well.
    /// </summary>
    public static class SA_FilesUtil
    {
        /// <summary>
        /// Creates or overrides file in a specified path
        /// </summary>
        /// <param name="path"> Filesystem project folder relative path.</param>
        public static void CreateFile(string path)
        {
            path = FixRelativePath(path);
            File.Create(SA_PathUtil.ConvertRelativeToAbsolutePath(path));
        }

        /// <summary>
        /// Delete the specified file
        /// </summary>
        /// <param name="path"> Filesystem project folder relative path.</param>
        public static void DeleteFile(string path)
        {
            if (SA_PathUtil.IsFileExists(path))
            {
                path = SA_PathUtil.ConvertRelativeToAbsolutePath(path);
                File.Delete(path);
            }
        }

        /// <summary>
        /// Determines whether the given path refers to an existing directory on disk
        /// </summary>
        /// <param name="path"> Filesystem project folder relative path.</param> 
        public static bool IsDirectoryExists(string path)
        {
            return SA_PathUtil.IsDirectoryExists(path);
        }

        /// <summary>
        /// Determines whether the given path refers to an existing directory on disk
        /// </summary>
        /// <param name="path"> Filesystem project folder relative path.</param> 
        public static bool IsFileExists(string path)
        {
            return SA_PathUtil.IsFileExists(path);
        }

        /// <summary>
        /// Writes a string to the specific file
        /// </summary>
        /// <param name="path"> Filesystem project folder relative file path.</param>
        /// <param name="contents"> the string content to write </param>
        public static void Write(string path, string contents)
        {
            path = FixRelativePath(path);
            var absolutePath = SA_PathUtil.ConvertRelativeToAbsolutePath(path);
            TextWriter tw = new StreamWriter(absolutePath, false);
            tw.Write(contents);
            tw.Close();
        }

        /// <summary>
        /// Creates a new file, writes one or more strings to the file, and then closes the file.
        /// </summary>
        /// <param name="path"> Filesystem project folder relative file path.</param>
        /// <param name="contents"> the string content to write </param>
        public static void WriteAllLines(string path, string[] contents)
        {
            path = FixRelativePath(path);
            var absolutePath = SA_PathUtil.ConvertRelativeToAbsolutePath(path);
            File.WriteAllLines(absolutePath, contents);
        }

        /// <summary>
        /// Open's a text file, reads all the lines of the file, and the closes the file
        /// </summary>
        /// <param name="path"> Filesystem project folder relative file path.</param>
        public static string Read(string path)
        {
#if !UNITY_WEBPLAYER
            if (SA_PathUtil.IsFileExists(path))
            {
                path = SA_PathUtil.ConvertRelativeToAbsolutePath(path);
                var content = File.ReadAllText(path);

                return content;
            }
            else
            {
                return string.Empty;
            }
#endif
        }

        /// <summary>
        /// Opens a text file, reads all lines of the file into a string array, and then closes the file.
        /// </summary>
        /// <param name="path"> Filesystem project folder relative file path.</param>
        public static string[] ReadAllLines(string path)
        {
#if !UNITY_WEBPLAYER
            if (SA_PathUtil.IsFileExists(path))
            {
                path = SA_PathUtil.ConvertRelativeToAbsolutePath(path);
                return File.ReadAllLines(path);
            }
            else
            {
                return new string[] { };
            }
#endif
        }

        /// <summary>
        /// Copies and exsiting file to a new file. If file already exists upder the specified 
        /// destination, file will be overrided.
        /// </summary>
        /// <param name="srcPath"> Filesystem project folder relative source file path.</param>
        /// <param name="destPath"> Filesystem project folder relative destination file path.</param>
        public static void CopyFile(string srcPath, string destPath)
        {
            if (SA_PathUtil.IsFileExists(srcPath) && !srcPath.Equals(destPath))
            {
                srcPath = FixRelativePath(srcPath, false);
                destPath = FixRelativePath(destPath);

                var absoluteSrcPath = SA_PathUtil.ConvertRelativeToAbsolutePath(srcPath);
                var absoluteDestPath = SA_PathUtil.ConvertRelativeToAbsolutePath(destPath);

                File.Copy(absoluteSrcPath, absoluteDestPath, true);
            }
        }

        /// <summary>
        /// Copies and exsiting directory to a new location.
        /// </summary>
        /// <param name="srcPath"> Filesystem project folder relative source file path.</param>
        /// <param name="destPath"> Filesystem project folder relative destination file path.</param>
        public static void CopyDirectory(string srcPath, string destPath)
        {
#if !UNITY_WEBPLAYER

            srcPath = FixRelativePath(srcPath);
            destPath = FixRelativePath(destPath);

            srcPath = SA_PathUtil.ConvertRelativeToAbsolutePath(srcPath);
            destPath = SA_PathUtil.ConvertRelativeToAbsolutePath(destPath);

            //Now Create all of the directories
            foreach (var dirPath in Directory.GetDirectories(srcPath, "*", SearchOption.AllDirectories)) Directory.CreateDirectory(dirPath.Replace(srcPath, destPath));

            //Copy all the files & Replaces any files with the same name
            foreach (var newPath in Directory.GetFiles(srcPath, "*.*", SearchOption.AllDirectories)) File.Copy(newPath, newPath.Replace(srcPath, destPath), true);

#endif
        }

        /// <summary>
        /// Creates or overrides directory in a specified path
        /// </summary>
        /// <param name="path"> Filesystem project folder relative path.</param>
        public static void CreateDirectory(string path)
        {
            path = FixRelativePath(path);
            Directory.CreateDirectory(SA_PathUtil.ConvertRelativeToAbsolutePath(path));
        }

        /// <summary>
        /// Delete the specified directory and subdirectories and files in the directory
        /// </summary>
        /// <param name="folderPath"> Filesystem project directory relative file path.</param>
        public static void DeleteDirectory(string folderPath)
        {
#if !UNITY_WEBPLAYER
            folderPath = SA_PathUtil.ConvertRelativeToAbsolutePath(folderPath);
            Directory.Delete(folderPath, true);
#endif
        }

        /// <summary>
        /// Returns the names of subdirectories (including their paths) in the specified directory.
        /// </summary>
        /// <returns>The directories.</returns>
        /// <param name="folderPath">The relative path to the directory to search. This string is not case-sensitive.</param>
        public static string[] GetDirectories(string folderPath)
        {
            folderPath = SA_PathUtil.ConvertRelativeToAbsolutePath(folderPath);
            return Directory.GetDirectories(folderPath);
        }

        //--------------------------------------
        // Private Section
        //--------------------------------------

        static string FixRelativePath(string path, bool createFolders = true)
        {
            path = SA_PathUtil.FixRelativePath(path);
            if (createFolders) CreatePathFolders(path);

            return path;
        }

        static void CreatePathFolders(string path)
        {
            foreach (var dir in SA_PathUtil.GetDirectoriesOutOfPath(path))
                if (!SA_PathUtil.IsDirectoryExists(dir))
                {
                    var dirAbsolutePath = SA_PathUtil.ConvertRelativeToAbsolutePath(dir);
                    Directory.CreateDirectory(dirAbsolutePath);
                }
        }
    }
}
