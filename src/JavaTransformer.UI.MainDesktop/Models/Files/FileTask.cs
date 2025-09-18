#pragma warning disable CS8603 

using JavaTransformer.UI.MainDesktop.Models.Exceptions;
using JavaTransformer.UI.MainDesktop.Views.UtilWindow.MessageBoxs;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Tmds.DBus.Protocol;

namespace JavaTransformer.UI.MainDesktop.Models.Files
{
    public static class FileTask
    {
        public static bool RequestCreateFile(string path, string content = "")
        {
            return ExceptionInvoker.TryInvoke<Exception>(() => { File.WriteAllText(path, content); });
        }

        public static string? RequestName(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) return null;

            return IsFile(path) ? Path.GetFileName(path) :
                   IsDirectory(path) ? new DirectoryInfo(path).Name : null;
        }

        public static async Task<bool> RequestRenamePath(string sourcePath, string destinationPath)
        {
            
            if (string.IsNullOrWhiteSpace(sourcePath) || string.IsNullOrWhiteSpace(destinationPath))
                return false;

            if (IsFile(sourcePath))
                return await RequestRenameFile(sourcePath, destinationPath);
            
            if (IsDirectory(sourcePath))
                return await RequestRenameDirectory(sourcePath, destinationPath);
            
            return false;
        }

        public static async Task<bool> RequestRenameFile(string sourcePath, string destinationPath)
        {
            if (!IsFile(sourcePath)) return false;

            return await ExecuteFileOperationAsync(() =>
                ExceptionInvoker.TryInvoke<Exception>(() => File.Move(sourcePath, destinationPath), out _));
        }

        public static async Task<bool> RequestRenameDirectory(string sourcePath, string destinationPath)
        {
            if (!IsDirectory(sourcePath)) return false;

            return await ExecuteFileOperationAsync(() =>
                ExceptionInvoker.TryInvoke<Exception>(() => Directory.Move(sourcePath, destinationPath), out _));
        }

        public static async Task<bool> RequestDeletePath(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) return false;

            return IsFile(path) ? await RequestDeleteFile(path) :
                   IsDirectory(path) ? await RequestDeleteDirectory(path) : false;
        }

        public static async Task<bool> RequestDeleteFile(string path)
        {
            if (!IsFile(path)) return false;

            return await ExecuteFileOperationAsync(() =>
            {
                ExceptionInvoker.TryInvoke<Exception>(() => File.Delete(path), out _);
                return !File.Exists(path);
            });
        }

        public static async Task<bool> RequestDeleteDirectory(string path)
        {
            if (!IsDirectory(path)) return false;

            return await ExecuteFileOperationAsync(() =>
            {
                ExceptionInvoker.TryInvoke<Exception>(() => Directory.Delete(path, true), out _);
                return !Directory.Exists(path);
            });
        }

        public static bool IsDirectory(string path) =>
            !string.IsNullOrWhiteSpace(path) && Directory.Exists(path);

        public static bool IsFile(string path) =>
            !string.IsNullOrWhiteSpace(path) && File.Exists(path);

        public static async Task<string?> ReadAllTextAsync(string path)
        {
            if (string.IsNullOrWhiteSpace(path) || !IsFile(path)) return null;

            return await ExecuteFileOperationAsync(async () =>
            {
                using var reader = new StreamReader(path, Encoding.UTF8, true);
                return await reader.ReadToEndAsync();
            });
        }

        private static async Task<T> ExecuteFileOperationAsync<T>(Func<T> operation)
        {
            try
            {
                return await Task.Run(operation);
            }
            catch
            {
                return default;
            }
        }

        private static async Task<T> ExecuteFileOperationAsync<T>(Func<Task<T>> operation)
        {
            try
            {
                return await operation();
            }
            catch
            {
                return default;
            }
        }
    }
}
#pragma warning restore CS8603 