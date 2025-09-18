using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JavaTransformer.UI.MainDesktop.Models.Files
{
    public class PathBuilder
    {
        private string _fileName;
        private string[] _directories;

        public string Path 
        {
            get
            {
                return string.Join("\\", string.Join("\\", _directories), _fileName);
            }
        }

        public PathBuilder(string path)
        {
            _fileName = GetFileNameFromPath(path);
            _directories = GetDirectoriesFromPath(path);
        }

        public PathBuilder(string fileName, string[] directories)
        {
            _fileName = fileName;
            _directories = directories;
        }

        public void Save()
        {
            var path = Path;
            ThrowIsValidatePath(path);

            if(File.Exists(path))
            {
                return;
            }

            BuildDirectories();
            BuildFile();
        }

        public void BuildFile()
        {
            File.WriteAllText(Path, "");
        }

        public void BuildDirectories()
        {
            string currentDirectory = string.Empty;
            foreach (var directory in _directories)
            {
                currentDirectory += directory + "\\";
                Directory.CreateDirectory(currentDirectory);
            }
        }

        private static void ThrowIsValidatePath(string path)
        {
            if (path is null && string.IsNullOrWhiteSpace(path))
                throw new NullReferenceException(nameof(path));
        }

        public static string GetFileNameFromPath(string path)
        {
            ThrowIsValidatePath(path);

            // validate path
            PathValidation.Validate(ref path);

            var _ = path.Split("\\");
            return _.Skip(_.Length - 1).ToList().ElementAt(0);
        }

        public static string[] GetDirectoriesFromPath(string path)
        {
            ThrowIsValidatePath(path);

            // validate path
            PathValidation.Validate(ref path);

            return path.Split("\\").SkipLast(1).ToList().ToArray();
        }

        public static string BuildPathFromCurrentDirectory(string path)
        {
            if (string.IsNullOrEmpty(path))
                return Environment.CurrentDirectory;

            if (global::System.IO.Path.IsPathRooted(path))
                return global::System.IO.Path.GetFullPath(path);

            string validatedPath = PathValidation.Validate(path);
            return global::System.IO.Path.Combine(Environment.CurrentDirectory, validatedPath);
        }

        public static string BuildPathFromCurrentDirectory(params string[] pathParts)
        {
            if (pathParts == null || pathParts.Length == 0)
                return Environment.CurrentDirectory;

            string combinedPath = global::System.IO.Path.Combine(pathParts);
            
            if (global::System.IO.Path.IsPathRooted(combinedPath))
                return global::System.IO.Path.GetFullPath(combinedPath);

            return global::System.IO.Path.Combine(Environment.CurrentDirectory, combinedPath);
        }
    }
}
