using CommunityToolkit.Mvvm.ComponentModel.__Internals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JavaTransformer.UI.MainDesktop.Models.Files
{
    public class PathValidation
    {
        public static string Validate(string path)
        {
            return new PathValidationBuilder(path).ReplaceBackslashesWithSlashes().GetValidate();
        }

        public static void Validate(ref string path)
        {
            path = new PathValidationBuilder(path).ReplaceBackslashesWithSlashes().GetValidate();
        }

        public class PathValidationBuilder
        {
            private string _path;

            public PathValidationBuilder(string path)
            {
                _path = path;
            }

            public PathValidationBuilder ReplaceBackslashesWithSlashes()
            {
                if (_path.Contains("//"))_path = _path.Replace("//", "\\");
                if (_path.Contains("/")) _path = _path.Replace("/", "\\");
                
                return this;
            }

            public string GetValidate()
            {
                return _path;
            }
        }
    }
}
