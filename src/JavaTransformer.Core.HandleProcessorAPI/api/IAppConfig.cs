using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JavaTransformer.Core.HandleProcessorAPI.api
{
    public interface IAppConfig
    {
        string path { get; }
        event EventHandler ConfigChange;

        string[] GetLines();
        bool HasLine(string line);
        bool HasLine(string line, out int index);
        void RemoveString(string line);
        void RemoveString(int index);
        void AddLine(string line);


        void Save();
        void Delete();
        void Clear();
    }
}
