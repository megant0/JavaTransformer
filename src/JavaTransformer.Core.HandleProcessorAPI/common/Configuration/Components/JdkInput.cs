using JavaTransformer.Core.HandleProcessorAPI.common.Validator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JavaTransformer.Core.HandleProcessorAPI.common.Components
{
    public class JdkInput : PathValidator
    {
        public JdkInput(string path) : base(path) { }

        protected override void Validate()
        {
            ValidateDirectoryExists(_path, "JDK directory not found");

            string bin = Path.Combine(_path, "bin");
            ValidateDirectoryExists(bin, "JDK bin directory not found");

            string java = Path.Combine(bin, "java.exe");
            string javaw = Path.Combine(bin, "javaw.exe");


            ValidateFileExists(java, "java.exe not found in JDK bin directory");
            ValidateFileExists(javaw, "javaw.exe not found in JDK bin directory");
        }
    }
}
