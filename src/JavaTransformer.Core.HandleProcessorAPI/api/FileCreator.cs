using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JavaTransformer.Core.HandleProcessorAPI.api
{
    public class FileCreator
    {
        public static void Create(string path)
        {
            Create(path, Array.Empty<string>());
        }

        public static void Create(string path, string[] text)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentException("Path cannot be null or empty", nameof(path));
          
            if (File.Exists(path))
            {
                if (text == null || text.Length == 0) File.WriteAllText(path, string.Empty);
                else File.WriteAllLines(path, text);
            }

            char delimiter = DeterminePathDelimiter(path);

            CreateDirectoriesForPath(path, delimiter);

            if (text == null || text.Length == 0) File.WriteAllText(path, string.Empty);
            else File.WriteAllLines(path, text);

        }

        private static char DeterminePathDelimiter(string path)
        {
            bool hasBackslash = path.Contains('\\');
            bool hasForwardSlash = path.Contains('/');

            if (hasBackslash && hasForwardSlash)
                throw new ArgumentException($"Mixed path delimiters in path: {path}");

            if (hasBackslash) return '\\';
            if (hasForwardSlash) return '/';

            return Path.DirectorySeparatorChar;
        }

        private static void CreateDirectoriesForPath(string path, char delimiter)
        {
            string directoryPath = Path.GetDirectoryName(path);

            if (!string.IsNullOrEmpty(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }

        public static void Create(string path, IEnumerable<string> lines = null)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentException("Path cannot be null or empty", nameof(path));

            CreateDirectoriesForPath(path, Path.DirectorySeparatorChar);

            if (lines == null)
            {
                File.WriteAllText(path, string.Empty);
            }
            else
            {
                File.WriteAllLines(path, lines);
            }
        }
    }
}
