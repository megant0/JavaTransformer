using JavaTransformer.Core.HandleProcessorAPI.api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace JavaTransformer.Core.HandleProcessorAPI.common.model
{
    public class BuildJvmConfig : BaseModel
    {
        /**
         * в конструкторе проходит валидация, которая кидает исключение
         * смотрите: JavaTransformer.Core.HandleProcessorAPI.common.JdkInput
         */
        public JdkInput JdkInput { get; set; }

        /**
         * при использование BuildJvmConfig:
         * 
         * в InputOutput
         * указывать "Ouput" - не нужно.
         * он статический.
         * 
         * но если он будет = "", будет ошибка,
         * можно указать = "-".
         */
        public BuildJvmConfig(InputOutput iO, JdkInput jdk) 
        {
            IO = iO;
            JdkInput = jdk;
        }

        public override string SerializeConfig()
        {
            return $@"
input={IO.Input}
output={IO.Output}
jdkInput={JdkInput.GetPath()}
compiler={GCC.GetPath()}
javacomp={Template.GetPath()}
";
        }
    }
}
