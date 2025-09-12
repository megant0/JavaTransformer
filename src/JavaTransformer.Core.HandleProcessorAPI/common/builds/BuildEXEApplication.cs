using JavaTransformer.Core.HandleProcessorAPI.common.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JavaTransformer.Core.HandleProcessorAPI.common.builds
{
    public class BuildEXEApplication : SimpleApplication
    {
        public BuildEXEApplication(BuildExeConfig model) : base(model, "EXE")
        {

        }

        public override void Build()
        {
            base.Build();
        }
    }
}
