using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class JavaNativeLibraries
{
    public static bool HasAnnotationManual(string classFilePath, string annotation)
    {
        try
        {
            byte[] classData = File.ReadAllBytes(classFilePath);

            if (classData.Length < 8 ||
                classData[0] != 0xCA || classData[1] != 0xFE ||
                classData[2] != 0xBA || classData[3] != 0xBE)
            {
                return false;
            }

            return ContainsSequence(classData, Encoding.UTF8.GetBytes(annotation));
        }
        catch
        {
            return false;
        }
    }

    private static bool ContainsSequence(byte[] source, byte[] pattern)
    {
        for (int i = 0; i < source.Length - pattern.Length; i++)
        {
            bool found = true;
            for (int j = 0; j < pattern.Length; j++)
            {
                if (source[i + j] != pattern[j])
                {
                    found = false;
                    break;
                }
            }
            if (found) return true;
        }
        return false;
    }

    public static string GetClassPackage(string classFilePath)
    {
        try
        {
            byte[] classData = File.ReadAllBytes(classFilePath);

            if (classData.Length < 10 ||
                classData[0] != 0xCA || classData[1] != 0xFE ||
                classData[2] != 0xBA || classData[3] != 0xBE)
            {
                return null;
            }

            int pos = 8;

            ushort constantPoolCount = ReadU2(classData, ref pos);
            if (constantPoolCount == 0) return "";

            for (int i = 1; i < constantPoolCount; i++)
            {
                if (pos >= classData.Length) break;

                byte tag = classData[pos++];
                switch (tag)
                {
                    case 7:
                    case 8:
                        pos += 2;
                        break;
                    case 3:
                    case 4:
                    case 9:
                    case 10:
                    case 11:
                    case 12:
                    case 18:
                        pos += 4;
                        break;
                    case 5:
                    case 6:
                        pos += 8;
                        i++;
                        break;
                    case 1:
                        ushort length = ReadU2(classData, ref pos);
                        pos += length;
                        break;
                    case 15:
                        pos += 3;
                        break;
                    case 16:
                        pos += 2;
                        break;
                    default:
                        throw new Exception($"Неизвестный тег в Constant Pool: {tag}");
                }
            }

            if (pos + 6 > classData.Length) return "";

            pos += 2;
            ushort thisClassIndex = ReadU2(classData, ref pos);

            string className = "unknown";
            try
            {
                int namePos = FindUtf8InConstantPool(classData, thisClassIndex);
                if (namePos != -1)
                {
                    className = GetUtf8String(classData, namePos);
                }
            }
            catch { }

            return ExtractPackageName(className);
        }
        catch (Exception)
        {
            return null;
        }
    }

    private static int FindUtf8InConstantPool(byte[] data, int index)
    {
        for (int i = 0; i < data.Length - 2; i++)
        {
            if (data[i] == 0x01)
            {
                ushort length = (ushort)((data[i + 1] << 8) | data[i + 2]);
                if (i + 3 + length <= data.Length)
                {
                    return i + 1;
                }
            }
        }
        return -1;
    }

    private static string GetUtf8String(byte[] data, int pos)
    {
        ushort length = ReadU2(data, ref pos);
        if (pos + length > data.Length) throw new IndexOutOfRangeException();
        return Encoding.UTF8.GetString(data, pos, length);
    }

    private static string ExtractPackageName(string className)
    {
        if (string.IsNullOrEmpty(className)) return "";

        int lastSlash = className.LastIndexOf('/');
        if (lastSlash < 0) return "";

        return className.Substring(0, lastSlash).Replace('/', '.');
    }

    private static ushort ReadU2(byte[] data, ref int pos)
    {
        ushort value = (ushort)((data[pos] << 8) | data[pos + 1]);
        pos += 2;
        return value;
    }

    private static string extract(string start)
    {
        return start.Split('.')[0];
    }

    public static string jvmti_get_signature(string java_class)
    {
        string package = GetClassPackage(java_class).Replace(".", "/");
        return package + "/" + extract(new FileInfo(java_class).Name);
    }
}

