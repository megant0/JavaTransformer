using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JavaTransformer.Core.HandleProcessorAPI.common.model
{
    public class BuildExeConfig : BaseModel
    {
        
        public BuildExeConfig(InputOutput io) 
        {
            IO = io;
        }

        public override string SerializeConfig()
        {
            return $@"
input={IO.Input}
output={IO.Output}
compiler={GCC.GetPath()}
execomp={Template.GetPath()}
";
        }
    }
}
