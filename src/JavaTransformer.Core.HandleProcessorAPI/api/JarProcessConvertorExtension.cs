using JavaTransformer.Core.HandleProcessorAPI.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JavaTransformer.Core.HandleProcessorAPI.api
{
    public static class JarProcessConvertorExtension
    {
        public static MethodReference MethodReference(this JarInfo jarInfo)
        {
            return new MethodReference(jarInfo.MethodLoad, jarInfo.ClassLoad);
        }
    }
}
