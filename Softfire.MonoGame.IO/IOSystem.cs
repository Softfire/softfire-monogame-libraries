using System.Collections.Generic;
using System.IO;

namespace Softfire.MonoGame.IO
{
    public static class IOSystem
    {
        /// <summary>
        /// File Name.
        /// </summary>
        private static string FileName { get; set; }

        /// <summary>
        /// File Path.
        /// Leave off trailing slash.
        /// </summary>
        private static string FilePath { get; set; }

        /// <summary>
        /// File System.
        /// </summary>
        private static FileSystems FileSystem { get; set; }

        /// <summary>
        /// File Systems.
        /// </summary>
        public enum FileSystems
        {
            Windows,
            Linux,
            Mac
        }

        /// <summary>
        /// Load Text File.
        /// Reads the requested file and splits text on lines with a ':' character.
        /// Key: Value
        /// </summary>
        /// <param name="fileSystem">The file system in use.</param>
        /// <param name="filePath">The file's path. Leave off trailing slash. Intaken as a <see cref="string"/>.</param>
        /// <param name="fileName">The file's name. Intaken as a <see cref="string"/>.</param>
        /// <returns>Returns a Dictionary{string, string}. Key/Value pairs.</returns>
        public static Dictionary<string, string> LoadTextFile(FileSystems fileSystem, string filePath, string fileName)
        {
            FileSystem = fileSystem;
            FilePath = filePath;
            FileName = fileName;

            var result = new Dictionary<string, string>();

            switch (FileSystem)
            {
                case FileSystems.Windows:
                    foreach (var line in File.ReadLines($@"{filePath}\{fileName}"))
                    {
                        if (!string.IsNullOrWhiteSpace(line))
                        {
                            var keyValueArray = line.Split(':');
                            result.Add(keyValueArray[0], keyValueArray[1]);
                        }
                    }

                    break;
                case FileSystems.Linux:

                    break;
                case FileSystems.Mac:

                    break;
            }

            return result;
        }
    }
}