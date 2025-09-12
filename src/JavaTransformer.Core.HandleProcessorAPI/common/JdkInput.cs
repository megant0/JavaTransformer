using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JavaTransformer.Core.HandleProcessorAPI.common
{
    public class JdkInput
    {
        /**
         * Должен принимать путь до jdk.
         * в jdk обязательно должны быть файлы:
         * 
         * + javaw.exe
         * + java.exe
         */
        private string _path;

        public JdkInput(string path) 
        {
            _path = path;
            validate(path);
        }

        void validate(string path)
        {
            if (!Directory.Exists(path))
                throw new DirectoryNotFoundException(path);

            string bin = path + "\\bin";
            if (!Directory.Exists(bin))
            {
                throw new DirectoryNotFoundException(bin);
            }

            string java = bin + "\\java.exe";
            string javaw = bin + "\\javaw.exe";

            if(!File.Exists(java))  throw new FileNotFoundException(javaw);
            if(!File.Exists(javaw)) throw new FileNotFoundException(javaw);
        }

        public string GetPath()
        {
            return _path;
        }

    }
}
