using JavaTransformer.UI.MainDesktop.Models.Files;
using JavaTransformer.UI.MainDesktop.Models.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JavaTransformer.UI.MainDesktop.Models.Common.Services
{
    public class PathService : IPathService
    {
        public string AppBinPath => PathBuilder.BuildPathFromCurrentDirectory("bin");
        public string AppDataPath => PathBuilder.BuildPathFromCurrentDirectory(AppBinPath, "Data");

        public string ConfigPath => PathBuilder.BuildPathFromCurrentDirectory(AppBinPath, "Config");

        public string LogsPath => PathBuilder.BuildPathFromCurrentDirectory(AppBinPath, "Logs");

        public PathService()
        {
            EnsureAllDirectories();
        }

        private void EnsureAllDirectories()
        {
            EnsureDirectoryExists(AppDataPath);
            EnsureDirectoryExists(ConfigPath);
            EnsureDirectoryExists(LogsPath);
        }

        public string EnsureDirectoryExists(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path;
        }

        public string GetFullPath(string relativePath)
        {
            return global::System.IO.Path.Combine(AppDataPath, relativePath);
        }
    }
}
