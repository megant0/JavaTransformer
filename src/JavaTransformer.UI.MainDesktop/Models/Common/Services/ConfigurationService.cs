#pragma warning disable CS8603 
#pragma warning disable CS8601 

using JavaTransformer.UI.MainDesktop.Models.Common.DataTransferObject.Settings;
using JavaTransformer.UI.MainDesktop.Models.Files;
using JavaTransformer.UI.MainDesktop.Models.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace JavaTransformer.UI.MainDesktop.Models.Common.Services
{
    public class ConfigurationService : IConfigurationService
    {
        public event EventHandler<ConfigurationChangedEventArgs> ConfigurationChanged;

        private Dictionary<string, object> _configuration;
        private string _pathConfiguration;
        private readonly JsonSerializerOptions _jsonOptions;

        public ConfigurationService(string pathConfiguration)
        {
            _pathConfiguration = pathConfiguration;
            _configuration = new();

            _jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNameCaseInsensitive = true
            };
        }

        public ConfigurationService() : this("bin\\config\\main-config.json")
        {

        }

        public T GetSection<T>(string sectionName) where T : new()
        {
            if (_configuration.TryGetValue(sectionName, out var section))
            {
                if (section is JsonElement jsonElement)
                {
                    return jsonElement.Deserialize<T>(_jsonOptions);
                }
                return (T)section;
            }
            return new T();
        }

        public bool Exist()
        {
            return File.Exists(_pathConfiguration);
        }

        public void LoadConfiguration()
        {
            if (Exist())
            {
                var json = File.ReadAllText(_pathConfiguration);
                _configuration = JsonSerializer.Deserialize<Dictionary<string, object>>(json, _jsonOptions)
                               ?? new Dictionary<string, object>();
            }
            else
            {
                _configuration = new Dictionary<string, object>();
                 InitializeDefaultConfiguration();
            }
        }

        public void SaveConfiguration()
        {
            PathBuilder builder = new PathBuilder(_pathConfiguration);
            builder.Save();

            var json = JsonSerializer.Serialize(_configuration, _jsonOptions);
            File.WriteAllTextAsync(builder.Path, json);
        }

        public void UpdateSection<T>(string sectionName, T data)
        {
            _configuration[sectionName] = data;
            ConfigurationChanged?.Invoke(this, new ConfigurationChangedEventArgs(sectionName, data));
        }

        public void InitializeDefaultConfiguration()
        {
            _configuration["AppSettings"] = new AppSettings();
            _configuration["EditorSettings"] = new EditorSettings();
            SaveConfiguration();
        }
    }
}
#pragma warning restore CS8603
#pragma warning restore CS8601