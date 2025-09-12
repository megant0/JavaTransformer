using JavaTransformer.Core.HandleProcessorAPI.api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JavaTransformer.Core.HandleProcessorAPI.common.model
{
    public class BuildDllConfig : BaseModel
    {
        public ClassLoader ClassLoader { get; set; }
        public MethodReference MethodReference { get; set; }

        public BuildDllConfig(InputOutput iO, ClassLoader classLoader, string @class, string method)
        {
            IO = iO;
            ClassLoader = classLoader;
            MethodReference = new MethodReference(@class, method);
        }

        public BuildDllConfig(InputOutput iO, ClassLoader classLoader, MethodReference methodReference)
        {
            IO = iO;
            ClassLoader = classLoader;
            MethodReference = methodReference;
        }

        public override string SerializeConfig()
        {
            return $@"
meta={MethodReference.Class}[{MethodReference.Method}
classloader={ClassLoader.GetClass()}
input={IO.Input}
output={IO.Output}
compiler={GCC.GetPath()}
dllcomp={Template.GetPath()}
";
        }
    }
}
