using JavaTransformer.Core.HandleProcessorAPI.api;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JavaTransformer.Core.HandleProcessorAPI.common
{
    public class ApplicationConfig : IAppConfig
    {
        public string path { get; }
        public event EventHandler? ConfigChange;

        private List<string> lines;

        public ApplicationConfig() 
        {
            path = "config/generate_code.txt";
            lines = new List<string>();
        }

        public void AddLine(string line)
        {
            lines.Add(line);
        }

        public bool HasLine(string line)
        {
            foreach (var word in GetLines())
            {
                if (word.Contains(line))
                {
                    return true;
                }
            }

            return false;
        }

        public bool HasLine(string line, out int index)
        {
            var _ = 0;
            foreach (var word in GetLines())
            {
                _++;
                if(word.Contains(line))
                {
                    index = _;
                    return true;
                }
            }

            index = -1;
            return false;
        }

        public void RemoveString(string line)
        {
            lines.Remove(line);
        }

        public void RemoveString(int index)
        {
            lines.RemoveAt(index);
        }

        public void Save()
        {
            if(!Directory.Exists("config"))
                Directory.CreateDirectory("config");

            ConfigChange?.Invoke(this, new EventArgs());
            File.WriteAllLines(path, GetLines());
        }

        public string[] GetLines()
        {
            return lines.ToArray();
        }

        public void Delete()
        {
            if (!File.Exists(path))
                throw new FileNotFoundException(path);

            ConfigChange?.Invoke(this, new EventArgs());
            File.Delete(path);
        }

        public void Clear()
        {
            if (!File.Exists(path)) Save();

            File.WriteAllText(path, string.Empty);  
        }
    }
}
