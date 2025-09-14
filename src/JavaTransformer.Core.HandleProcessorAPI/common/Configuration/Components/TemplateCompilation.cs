using JavaTransformer.Core.HandleProcessorAPI.common.Validator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JavaTransformer.Core.HandleProcessorAPI.common.Components
{
    public class TemplateCompilation : PathValidator
    {
        public TemplateCompilation(string pathToDir) : base(pathToDir) { }

        protected override void Validate()
        {
            if (_path == "default") return;

            ValidateNotNullOrEmpty(_path, nameof(_path));
            ValidateDirectoryExists(_path, "Template directory not found");

            string header_h = Path.Combine(_path, "header.h");
            ValidateFileExists(header_h, "header.h not found in template directory");
        }

        public static TemplateCompilation Default = new TemplateCompilation("default");
    }
}
