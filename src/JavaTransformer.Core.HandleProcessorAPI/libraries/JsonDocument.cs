using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class JsonDocument
{
    private protected readonly string json_file;

    public JsonDocument(string json_file)
    {
        this.json_file = json_file;
    }

    public object SearchProperty(string key)
    {
        return SearchProperty(this.json_file, key);
    }

    public static object SearchProperty(string path, string key)
    {
        string content;
        if (!File.Exists(path))
        {
            content = path;
        }
        else
            content = File.ReadAllText(path);

        string[] keys = key.Split('.');

        int currentPos = 0;
        object currentValue = FindJsonValue(content, ref currentPos);

        foreach (var k in keys)
        {
            if (currentValue is Dictionary<string, object> dict)
            {
                if (!dict.TryGetValue(k, out currentValue))
                    return null;
            }
            else
            {
                return null;
            }
        }

        if (currentValue is List<object> list && list.Count > 0)
            return list[0];

        return currentValue;
    }

    private static object FindJsonValue(string json, ref int pos)
    {
        SkipWhitespace(json, ref pos);

        switch (json[pos])
        {
            case '{':
                return ParseObject(json, ref pos);
            case '[':
                return ParseArray(json, ref pos);
            case '"':
                return ParseString(json, ref pos);
            case 't':
                return ParseBoolean(json, ref pos);
            case 'f':
                return ParseBoolean(json, ref pos);
            case 'n':
                ParseNull(json, ref pos);
                return null;
            default:
                if (char.IsDigit(json[pos]) || json[pos] == '-')
                    return ParseNumber(json, ref pos);
                throw new FormatException($"Unexpected character at position {pos}");
        }
    }

    private static Dictionary<string, object> ParseObject(string json, ref int pos)
    {
        var obj = new Dictionary<string, object>();
        pos++;

        while (true)
        {
            SkipWhitespace(json, ref pos);
            if (json[pos] == '}')
            {
                pos++;
                break;
            }

            string key = ParseString(json, ref pos);
            SkipWhitespace(json, ref pos);

            if (json[pos] != ':')
                throw new FormatException($"Expected ':' at position {pos}");
            pos++;

            object value = FindJsonValue(json, ref pos);
            obj[key] = value;

            SkipWhitespace(json, ref pos);
            if (json[pos] == ',')
            {
                pos++;
                continue;
            }
            else if (json[pos] == '}')
            {
                pos++;
                break;
            }
            else
            {
                throw new FormatException($"Expected ',' or '}}' at position {pos}");
            }
        }

        return obj;
    }

    private static List<object> ParseArray(string json, ref int pos)
    {
        var list = new List<object>();
        pos++;

        while (true)
        {
            SkipWhitespace(json, ref pos);
            if (json[pos] == ']')
            {
                pos++;
                break;
            }

            object value = FindJsonValue(json, ref pos);
            list.Add(value);

            SkipWhitespace(json, ref pos);
            if (json[pos] == ',')
            {
                pos++;
                continue;
            }
            else if (json[pos] == ']')
            {
                pos++;
                break;
            }
            else
            {
                throw new FormatException($"Expected ',' or ']' at position {pos}");
            }
        }

        return list;
    }

    private static string ParseString(string json, ref int pos)
    {
        pos++;
        int start = pos;

        while (pos < json.Length && json[pos] != '"')
        {
            if (json[pos] == '\\') pos++;
            pos++;
        }

        if (pos >= json.Length)
            throw new FormatException("Unterminated string");

        string result = json.Substring(start, pos - start);
        pos++;
        return result;
    }

    private static double ParseNumber(string json, ref int pos)
    {
        int start = pos;

        while (pos < json.Length && (char.IsDigit(json[pos]) || json[pos] == '.' || json[pos] == '-' || json[pos] == 'e' || json[pos] == 'E'))
            pos++;

        string numStr = json.Substring(start, pos - start);
        if (double.TryParse(numStr, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out double result))
            return result;

        throw new FormatException($"Invalid number format at position {start}");
    }

    private static bool ParseBoolean(string json, ref int pos)
    {
        if (json.Substring(pos, 4) == "true")
        {
            pos += 4;
            return true;
        }
        else if (json.Substring(pos, 5) == "false")
        {
            pos += 5;
            return false;
        }

        throw new FormatException($"Invalid boolean value at position {pos}");
    }

    private static void ParseNull(string json, ref int pos)
    {
        if (json.Substring(pos, 4) != "null")
            throw new FormatException($"Expected 'null' at position {pos}");
        pos += 4;
    }

    private static void SkipWhitespace(string json, ref int pos)
    {
        while (pos < json.Length && char.IsWhiteSpace(json[pos]))
            pos++;
    }
}

