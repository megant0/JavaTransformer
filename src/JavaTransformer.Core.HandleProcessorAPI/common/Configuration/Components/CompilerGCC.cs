using JavaTransformer.Core.HandleProcessorAPI.common.Validator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JavaTransformer.Core.HandleProcessorAPI.common.Components
{
    public class CompilerGCC : PathValidator
    {
        public CompilerGCC(string path) : base(path) { }

        protected override void Validate()
        {
            if (_path != "g++")
            {
                ValidateFileExists(_path, "GCC compiler not found at specified path");
            }
        }

        public static CompilerGCC GCC = new CompilerGCC("g++");
    }
}
