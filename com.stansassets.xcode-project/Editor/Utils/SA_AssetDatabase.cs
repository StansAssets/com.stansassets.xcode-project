using System.IO;
using System.Collections.Generic;
using UnityEngine;
using SA.Foundation.Utility;
#if UNITY_EDITOR
using UnityEditor;
using System.Reflection;

#endif

namespace SA.Foundation.UtilitiesEditor
{
    /// <summary>
    /// An Interface for accessing assets and performing operations on assets.
    /// </summary>
    public static class SA_AssetDatabase
    {
        //--------------------------------------
        // Public Methods
        //--------------------------------------

        /// <summary>
        /// Creates a new asset at path.
        /// </summary>
        /// <param name="asset"> Object to use in creating the asset. </param>
        /// <param name="path"> Filesystem project folder relative path.</param>
        public static void CreateAsset(Object asset, string path)
        {
            path = FixRelativePath(path);
            SA_AssetDatabaseProxy.Create(asset, path);
        }

        /// <summary>
        /// Creates a new folder at path.
        /// </summary>
        /// <param name="path"> Filesystem project folder relative path.</param>
        public static void CreateFolder(string path)
        {
            if (!path.EndsWith(SA_PathUtil.FOLDER_SEPARATOR, System.StringComparison.CurrentCulture)) path = path + SA_PathUtil.FOLDER_SEPARATOR;

            ValidateFoldersPath(path);
        }

        /// <summary>
        ///  Returns the path name relative to the Assets/ folder where the asset is stored.
        ///  All paths are relative to the project folder, for example: "Plugins/MyTextures/hello.png".
        /// </summary>
        /// <param name="assetObject"> A reference to the asset. </param>
        public static string GetAssetPath(Object assetObject)
        {
            return FixRelativePath(SA_AssetDatabaseProxy.GetAssetPath(assetObject), false);
        }

        /// <summary>
        ///  Returns the absolute path name relative of a given asset
        ///  for example: "/Users/user/Project/Assets/MyTextures/hello.png".
        /// </summary>
        /// <param name="assetObject"> A reference to the asset. </param>
        public static string GetAbsoluteAssetPath(Object assetObject)
        {
            var relativePath = GetAssetPath(assetObject);
            return SA_PathUtil.ConvertRelativeToAbsolutePath(relativePath);
        }

        /// <summary>
        /// Returns the extension of the specified path string.
        /// </summary>
        /// <param name="filePath">Filesystem project folder relative file path.</param>
        /// <returns></returns>
        public static string GetExtension(string filePath)
        {
            return SA_PathUtil.GetExtension(filePath);
        }

        /// <summary>
        /// Returns true if given asset is located inside the provided folder path
        /// </summary>
        public static bool IsAssetInsideFolder(Object assetObject, string folderPath)
        {
            var assetPath = GetAssetPath(assetObject);
            var assetFolder = SA_PathUtil.GetDirectoryPath(assetPath) + SA_PathUtil.FOLDER_SEPARATOR;

            return folderPath.Equals(assetFolder);
        }

        /// <summary>
        /// Returns the file name and extension of the specified path string.
        /// </summary>
        /// <param name="filePath">Filesystem project folder relative file path.</param>
        /// <returns></returns>
        public static string GetFileName(string filePath)
        {
            return SA_PathUtil.GetFileName(filePath);
        }

        /// <summary>
        /// Returns the asset name of the specified path string.
        /// The file name without extention
        /// </summary>
        /// <param name="filePath">Filesystem project folder relative file path.</param>
        /// <returns></returns>
        public static string GetAssetNameWithoutExtension(string filePath)
        {
            return Path.GetFileNameWithoutExtension(filePath);
        }

        /// <summary>
        /// Duplicates the asset at path and stores it at newPath.
        /// Returns true if the asset has been successfully duplicated, false if it doesn't exit or couldn't be duplicated.
        /// All paths are relative to the project folder, for example: "Assets/Plugins/IOS/hello.png".
        /// </summary>
        /// <param name="path"> Filesystem project source relative path. </param>
        /// <param name="newPath"> Filesystem project destination relative path.</param>
        public static bool CopyAsset(string path, string newPath)
        {
            path = FixRelativePath(path);
            newPath = FixRelativePath(newPath);
            return SA_AssetDatabaseProxy.CopyAsset(path, newPath);
        }

        /// <summary>
        /// Move an asset file (or folder) from one folder to another.
        /// Returns an empty string if the asset has been successfully moved, otherwise an error message.
        /// All paths are relative to the project folder, for example: "Assets/Plugins/IOS/hello.png".
        /// </summary>
        /// <param name="oldPath"> Filesystem project source relative path. </param>
        /// <param name="newPath"> Filesystem project destination relative path.</param>
        public static string MoveAsset(string oldPath, string newPath)
        {
            oldPath = FixRelativePath(oldPath);
            newPath = FixRelativePath(newPath);
            return SA_AssetDatabaseProxy.Move(oldPath, newPath);
        }

        /// <summary>
        /// Moves the asset at path to the trash.
        /// Returns true if the asset has been successfully removed, false if it doesn't exit or couldn't be moved to the trash.
        /// All paths are relative to the project folder, for example: "Assets/Plugins/IOS/hello.png".
        /// </summary>
        /// <param name="path"> Filesystem project relative path. </param>
        public static bool DeleteAsset(string path)
        {
            path = FixRelativePath(path);
            return SA_AssetDatabaseProxy.Delete(path);
        }
        

        /// <summary>
        /// Returns the first asset object of type type at given path assetPath.
        ///
        /// Some asset files may contain multiple objects. (such as a Maya file which may contain multiple Meshes and GameObjects).
        /// All paths are relative to the project project folder, for example: "Assets/MyTextures/hello.png".
        ///
        /// The <see cref="assetPath"/> parameter is not case sensitive.
        /// ALL asset names and paths in Unity use forward slashes, even on Windows.
        /// This returns only an asset object that is visible in the Project view.If the asset is not found LoadAssetAtPath returns Null.
        /// </summary>
        /// <returns>The asset at path.</returns>
        /// <param name="assetPath">Path of the asset to load.</param>
        /// <typeparam name="T"> Data type of the asset.</typeparam>
        public static T LoadAssetAtPath<T>(string assetPath) where T : Object
        {
            assetPath = FixRelativePath(assetPath);
            return SA_AssetDatabaseProxy.LoadAssetAtPath<T>(assetPath);
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
        /// Determines whether the given path refers to an existing file on disk
        /// </summary>
        /// <param name="path"> Filesystem project folder relative path.</param>
        public static bool IsFileExists(string path)
        {
            return SA_PathUtil.IsFileExists(path);
        }

        /// <summary>
        /// Returns the names of subdirectories (including their paths) in the specified directory.
        /// </summary>
        /// <returns>The directories.</returns>
        /// <param name="folderPath">The relative path to the directory to search. This string is not case-sensitive..</param>
        public static string[] GetDirectories(string folderPath)
        {
            folderPath = FixRelativePath(folderPath);
            return SA_FilesUtil.GetDirectories(folderPath);
        }

        /// <summary>
        /// Given a path to a folder, returns true if it exists, false otherwise.
        /// </summary>
        /// <param name="path"> Filesystem project folder relative path.</param>
        public static bool IsValidFolder(string path)
        {
            return IsDirectoryExists(path);
        }
        

        /// <summary>
        /// Search the asset database inside the given folder
        /// </summary>
        /// <param name="pathToDirectory">Filesystem project folder relative path.</param>
        /// <param name="extentions">files extentions to match</param>
        /// <returns></returns>
        public static List<string> FindAssetsWithExtentions(string pathToDirectory, params string[] extentions)
        {
            //we just looking for files inside this folder, we do not want to create it if does not exists
            pathToDirectory = FixRelativePath(pathToDirectory, false);

            //Nothing to search for
            if (!IsValidFolder(pathToDirectory)) return new List<string>();

            return SA_AssetDatabaseProxy.FindAssets(pathToDirectory, string.Empty, extentions);
        }
        
        /// <summary>
        /// Import asset at path.
        ///  All paths are relative to the project folder, for example: "Assets/MyTextures/hello.png"
        /// </summary>
        /// <param name="path"> Filesystem project folder relative path.</param>
        public static void ImportAsset(string path)
        {
            path = FixRelativePath(path);
            SA_AssetDatabaseProxy.ImportAsset(path);
        }
        

        //--------------------------------------
        // Private Methods
        //--------------------------------------

        static string FixRelativePath(string path, bool validateFoldersPath = true)
        {
            path = SA_PathUtil.FixRelativePath(path);
            if (validateFoldersPath) ValidateFoldersPath(path);

            return path;
        }

        public static void ValidateFoldersPath(string path)
        {
            var parentDir = string.Empty;
            foreach (var dir in SA_PathUtil.GetDirectoriesOutOfPath(path))
            {
                if (!IsDirectoryExists(dir))
                {
                    var dirName = SA_PathUtil.GetPathDirectoryName(dir);
                    SA_AssetDatabaseProxy.CreateFolder(parentDir, dirName);
                }

                parentDir = dir;
            }
        }

        //--------------------------------------
        // Private Classes
        //--------------------------------------

        class SA_AssetDatabaseProxy
        {
            public static void Create(Object asset, string path)
            {
#if UNITY_EDITOR
                AssetDatabase.CreateAsset(asset, path);
#endif
            }

            public static List<string> FindAssets(string path, string filter, params string[] extentions)
            {
                var assets = new List<string>();
#if UNITY_EDITOR
                var guids = AssetDatabase.FindAssets(filter, new string[] { path });

                foreach (var guid in guids)
                {
                    var assetPath = AssetDatabase.GUIDToAssetPath(guid);
                    if (extentions.Length != 0)
                    {
                        foreach (var extention in extentions)
                            if (extention.Equals(Path.GetExtension(assetPath)))
                            {
                                assets.Add(assetPath);
                                break;
                            }
                    }
                    else
                    {
                        assets.Add(assetPath);
                    }
                }
#endif

                return assets;
            }

            public static bool CopyAsset(string path, string newPath)
            {
#if UNITY_EDITOR
                return AssetDatabase.CopyAsset(path, newPath);
#else
            return false;
#endif
            }

            public static string Move(string oldPath, string newPath)
            {
#if UNITY_EDITOR
                return AssetDatabase.MoveAsset(oldPath, newPath);
#else
            return "";
#endif
            }

            public static bool Delete(string path)
            {
#if UNITY_EDITOR
                return AssetDatabase.MoveAssetToTrash(path);
#else
                return false;
#endif
            }

            public static void CreateFolder(string parentFolder, string newFolderName)
            {
#if UNITY_EDITOR
                AssetDatabase.CreateFolder(parentFolder, newFolderName);
#endif
            }

            public static void ImportAsset(string path)
            {
#if UNITY_EDITOR
                AssetDatabase.ImportAsset(path);
#endif
            }

            public static string GetAssetPath(Object assetObject)
            {
#if UNITY_EDITOR
                return AssetDatabase.GetAssetPath(assetObject);
#else
                return string.Empty;
#endif
            }

            public static T LoadAssetAtPath<T>(string assetPath) where T : Object
            {
#if UNITY_EDITOR
                return AssetDatabase.LoadAssetAtPath<T>(assetPath);
#else
                return null;
#endif
            }
        }
    }
}
