using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JavaTransformer.Core.HandleProcessorAPI.common
{
    public class TemplateCompilation
    {
        /**
         * Если указать - "default",
         * то он будет использовать стандартный
         * встроенный темплейт для компиляции.
         * 
         * если вы используете свой "template" для компиляции
         * то вам нужно будет создать в нем "header.h"
         * куда будет записан hex файла, после чего компиляция.
         */
        private string pathToDirTemplate;

        public TemplateCompilation(string pathToDir)
        {
            pathToDirTemplate = pathToDir;
        }

        public string GetPath()
        {
            return pathToDirTemplate;
        }

        public static TemplateCompilation Default = new TemplateCompilation("default");
    }
}
