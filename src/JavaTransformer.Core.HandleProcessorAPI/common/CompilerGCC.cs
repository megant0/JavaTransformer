using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JavaTransformer.Core.HandleProcessorAPI.common
{
    public class CompilerGCC
    {
        /**
         * Если в переменной среде "Path"
         * есть путь до папки с компилятором g++,
         * то можно просто использовать "g++"
         * 
         * и это будет работать.
         * 
         * Если же его нет, то нужно указать путь
         * до файла "../g++.exe"
         */
        private string path;

        public CompilerGCC(string path)
        {
            this.path = path;
        }

        public string GetPath()
        {
            return path;
        }

        public static CompilerGCC GCC = new CompilerGCC("g++");
    }
}
