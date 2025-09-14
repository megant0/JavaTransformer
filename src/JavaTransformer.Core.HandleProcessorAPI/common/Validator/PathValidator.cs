using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JavaTransformer.Core.HandleProcessorAPI.common.Validator
{
    public abstract class PathValidator
    {
        protected string _path;

        protected PathValidator(string path)
        {
            _path = path;
            Validate();
        }

        protected abstract void Validate();

        protected void ValidateDirectoryExists(string path, string message = null)
        {
            if (!Directory.Exists(path))
                throw new DirectoryNotFoundException(message ?? path);
        }

        protected void ValidateFileExists(string path, string message = null)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException(message ?? path);
        }

        protected void ValidateNotNullOrEmpty(string value, string paramName)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException(paramName, "Value cannot be null or empty");
        }

        public string GetPath() => _path;
    }
}
