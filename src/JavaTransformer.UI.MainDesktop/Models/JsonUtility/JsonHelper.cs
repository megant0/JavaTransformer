using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace JavaTransformer.UI.MainDesktop.Core.ConfigManager
{
    public class JsonHelper
    {
        public JsonSerializerOptions Options = BuilderOption
            .ForWebApi()
            .Build();


        public string Serialize(object obj)
        {
            return JsonSerializer.Serialize(obj, Options);
        }

        public T? Deserialize<T>(string json)
        {
            var _ = JsonSerializer.Deserialize<T>(json);

            return _ == null ? default : _;
        }

        public class BuilderOption
        {
            private JsonSerializerOptions _options;

            public BuilderOption()
            {
                _options = new JsonSerializerOptions();
            }

            public JsonSerializerOptions Build() => _options;

            public BuilderOption WriteIndented(bool value)
            {
                _options.WriteIndented = value;
                return this;
            }

            public BuilderOption PropertyNamingPolicy(JsonNamingPolicy value)
            {
                _options.PropertyNamingPolicy = value;
                return this;
            }

            [Obsolete("Use IgnoreNullValues method instead. This method will be removed in future versions.")]
            public BuilderOption IgnoreNullValues(bool value)
            {
                _options.IgnoreNullValues = value;
                return this;
            }

            public BuilderOption DefaultIgnoreCondition(JsonIgnoreCondition condition)
            {
                _options.DefaultIgnoreCondition = condition;
                return this;
            }

            public BuilderOption Encoder(JavaScriptEncoder encoder)
            {
                _options.Encoder = encoder;
                return this;
            }

            public BuilderOption NumberHandling(JsonNumberHandling handling)
            {
                _options.NumberHandling = handling;
                return this;
            }

            public BuilderOption ReferenceHandler(ReferenceHandler handler)
            {
                _options.ReferenceHandler = handler;
                return this;
            }

            public BuilderOption ReadCommentHandling(JsonCommentHandling handling)
            {
                _options.ReadCommentHandling = handling;
                return this;
            }

            public BuilderOption MaxDepth(int maxDepth)
            {
                _options.MaxDepth = maxDepth;
                return this;
            }

            public BuilderOption AllowTrailingCommas(bool allow)
            {
                _options.AllowTrailingCommas = allow;
                return this;
            }

            public BuilderOption PropertyNameCaseInsensitive(bool caseInsensitive)
            {
                _options.PropertyNameCaseInsensitive = caseInsensitive;
                return this;
            }

            public BuilderOption DictionaryKeyPolicy(JsonNamingPolicy policy)
            {
                _options.DictionaryKeyPolicy = policy;
                return this;
            }

            public BuilderOption WriteIndented()
            {
                _options.WriteIndented = true;
                return this;
            }

            public BuilderOption PropertyNamingPolicyCamelCase()
            {
                _options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                return this;
            }

            public BuilderOption PropertyNamingPolicySnakeCaseUpper()
            {
                _options.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseUpper;
                return this;
            }

            public BuilderOption PropertyNamingPolicySnakeCaseLower()
            {
                _options.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
                return this;
            }

            public BuilderOption IgnoreNullValues()
            {
                _options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                return this;
            }

            public BuilderOption IgnoreReadOnlyProperties()
            {
                _options.IgnoreReadOnlyProperties = true;
                return this;
            }

            public BuilderOption IgnoreReadOnlyProperties(bool ignore)
            {
                _options.IgnoreReadOnlyProperties = ignore;
                return this;
            }

            public BuilderOption IncludeFields(bool include)
            {
                _options.IncludeFields = include;
                return this;
            }

            public BuilderOption IncludeFields()
            {
                _options.IncludeFields = true;
                return this;
            }

            public BuilderOption UnsafeRelaxedJsonEscaping()
            {
                _options.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
                return this;
            }

            public static BuilderOption Default() => new BuilderOption();

            public static BuilderOption PrettyPrint() =>
                new BuilderOption()
                    .WriteIndented(true)
                    .PropertyNamingPolicyCamelCase();

            public static BuilderOption Minimal() =>
                new BuilderOption()
                    .WriteIndented(false)
                    .IgnoreNullValues();

            public static BuilderOption ForWebApi() =>
                new BuilderOption()
                    .PropertyNamingPolicyCamelCase()
                    .IgnoreNullValues()
                    .PropertyNameCaseInsensitive(true);

            public static BuilderOption ForConfiguration() =>
                new BuilderOption()
                    .WriteIndented(true)
                    .PropertyNameCaseInsensitive(true)
                    .AllowTrailingCommas(true)
                    .ReadCommentHandling(JsonCommentHandling.Skip);
        }
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
}
