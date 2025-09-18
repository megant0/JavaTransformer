using JavaTransformer.UI.MainDesktop.Models.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JavaTransformer.UI.MainDesktop.Models.Common.Services
{
    public class LoggerService : ILoggerService
    {
        private readonly StringBuilder _stringBuilder;
        private readonly IPathService _pathService;
        private readonly Timer _autoSaveTimer;

        private string _currentLogName;

        const double AUTO_SAVE_TIME = 30;

        public LoggerService(IPathService pathService)
        {
            _stringBuilder = new StringBuilder();
            _pathService = pathService;

            _autoSaveTimer = new Timer(_ => Save(), null, TimeSpan.FromSeconds(AUTO_SAVE_TIME), TimeSpan.FromSeconds(AUTO_SAVE_TIME));
            _currentLogName = $"log_{DateTime.Now:yyyyMMdd_HHmmss}";
        }

        private async void SendMessage(string level, string message)
        {
            lock (_stringBuilder)
            {
                _stringBuilder.AppendLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level}] {message}");
            }

            await SaveAsync();
        }

        public void Error(string message) => SendMessage("ERROR", message);
        public void Error(string message, Exception ex) => SendMessage("ERROR", $"{message}: {ex}");
        public void Fatal(string message) => SendMessage("FATAL", message);
        public void Info(string message) => SendMessage("INFO", message);
        public void Warn(string message) => SendMessage("WARN", message);

        public void Save()
        {
            try
            {
                string logContent;
                lock (_stringBuilder)
                {
                    logContent = _stringBuilder.ToString();
                    _stringBuilder.Clear();
                }

                if (!string.IsNullOrEmpty(logContent))
                {
                    string logPath = Path.Combine(_pathService.LogsPath, $"{_currentLogName}.txt");
                    File.AppendAllText(logPath, logContent);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to save log: {ex.Message}");
            }
        }

        public async Task SaveAsync()
        {
            await Task.Run(Save);
        }

        public void Flush() => Save();

        public void Dispose()
        {
            _autoSaveTimer?.Dispose();
            Save(); 
        }
    }
}
