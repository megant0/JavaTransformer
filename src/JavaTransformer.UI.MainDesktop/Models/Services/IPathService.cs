using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JavaTransformer.UI.MainDesktop.Models.Services
{
    public interface IPathService
    {
        string AppBinPath { get; }
        string AppDataPath { get; }
        string ConfigPath { get; }
        string LogsPath { get; }

        string GetFullPath(string relativePath);
        string EnsureDirectoryExists(string path);
    }
}
