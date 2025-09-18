using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace JavaTransformer.UI.MainDesktop.Core.ConfigManager
{
    public class JsonConfigApplication : JsonHelper
    {
        public string Path { get; }
        private JsonObject _jsonObject;
        private bool _isModified;

        public JsonConfigApplication(string path)
        {
            Path = path;
            Load();
        }

        private void Load()
        {
            if (File.Exists(Path))
            {
                try
                {
                    var jsonText = File.ReadAllText(Path);
                    _jsonObject = JsonSerializer.Deserialize<JsonObject>(jsonText, Options) ?? new JsonObject();
                    _isModified = false;
                }
                catch (JsonException)
                {
                    _jsonObject = new JsonObject();
                    _isModified = true;
                }
            }
            else
            {
                _jsonObject = new JsonObject();
                _isModified = true;
            }
        }

        public void Set(string name, object value)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be null or empty", nameof(name));

#pragma warning disable CS8600 
            JsonNode jsonValue = value switch
            {
                null => JsonValue.Create((string)null),
                string s => JsonValue.Create(s),
                int i => JsonValue.Create(i),
                long l => JsonValue.Create(l),
                double d => JsonValue.Create(d),
                float f => JsonValue.Create(f),
                decimal dec => JsonValue.Create(dec),
                bool b => JsonValue.Create(b),
                DateTime dt => JsonValue.Create(dt.ToString("O")), // ISO 8601 format
                DateTimeOffset dto => JsonValue.Create(dto.ToString("O")),
                Guid guid => JsonValue.Create(guid.ToString()),
                _ => JsonSerializer.SerializeToNode(value, Options) ?? JsonValue.Create(value.ToString())
            };
#pragma warning restore CS8600 

            _jsonObject[name] = jsonValue;
            _isModified = true;
        }

        public T Get<T>(string name, T defaultValue = default)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be null or empty", nameof(name));

            if (_jsonObject.TryGetPropertyValue(name, out var value) && value != null)
            {
                try
                {
#pragma warning disable CS8603 
                    return value.Deserialize<T>(Options);
#pragma warning restore CS8603 
                }
                catch (JsonException)
                {
                    return defaultValue;
                }
            }

            return defaultValue;
        }

        public bool GetBool(string name, bool defaultValue = false)
        {
            return Get(name, defaultValue);
        }

        public string GetString(string name, string defaultValue = "")
        {
            return Get(name, defaultValue);
        }

        public int GetInt(string name, int defaultValue = 0)
        {
            return Get(name, defaultValue);
        }

        public double GetDouble(string name, double defaultValue = 0.0)
        {
            return Get(name, defaultValue);
        }

        public T[] GetArray<T>(string name, T[] defaultValue = null)
        {
            return Get(name, defaultValue ?? Array.Empty<T>());
        }

        public List<T> GetList<T>(string name, List<T> defaultValue = null)
        {
            return Get(name, defaultValue ?? new List<T>());
        }

        public Dictionary<string, T> GetDictionary<T>(string name, Dictionary<string, T> defaultValue = null)
        {
            return Get(name, defaultValue ?? new Dictionary<string, T>());
        }

        public bool ContainsKey(string name)
        {
            return _jsonObject.ContainsKey(name);
        }

        public void Remove(string name)
        {
            if (_jsonObject.Remove(name))
            {
                _isModified = true;
            }
        }

        public void Clear()
        {
            if (_jsonObject.Count > 0)
            {
                _jsonObject.Clear();
                _isModified = true;
            }
        }

        public IEnumerable<string> GetKeys()
        {
            foreach (var property in _jsonObject)
            {
                yield return property.Key;
            }
        }

        public string GetJsonText()
        {
            return JsonSerializer.Serialize(_jsonObject, new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        }

        public void Save()
        {
            if (_isModified)
            {
                var jsonText = GetJsonText();
                FileCreator.Create(Path, new[] { jsonText });
                _isModified = false;
            }
        }

        public void Reload()
        {
            Load();
        }

        public void SaveIfModified()
        {
            if (_isModified)
            {
                Save();
            }
        }

        public JsonConfigApplication GetSection(string sectionName)
        {
            if (_jsonObject.TryGetPropertyValue(sectionName, out var section) && section is JsonObject sectionObject)
            {
                return new JsonConfigApplication(Path)
                {
                    _jsonObject = sectionObject,
                    _isModified = false
                };
            }

            var newSection = new JsonObject();
            _jsonObject[sectionName] = newSection;
            _isModified = true;
            return new JsonConfigApplication(Path)
            {
                _jsonObject = newSection,
                _isModified = true
            };
        }

        public void MergeWith(JsonObject otherObject)
        {
            foreach (var property in otherObject)
            {
                var serialized = JsonSerializer.SerializeToNode(property.Value, Options);
                _jsonObject[property.Key] = serialized;
            }
            _isModified = true;
        }

        public JsonObject ToJsonObject()
        {
            return JsonSerializer.Deserialize<JsonObject>(
                JsonSerializer.Serialize(_jsonObject, Options), Options) ?? new JsonObject();
        }

        private JsonNode DeepCloneJsonNode(JsonNode node)
        {
            if (node == null) return null;

            var serialized = JsonSerializer.SerializeToNode(node, Options);
            return serialized;
        }
    }
}
