using JavaTransformer.Core.HandleProcessorAPI.common.builds.Applications;
using JavaTransformer.Core.HandleProcessorAPI.common.Config;
using JavaTransformer.Core.HandleProcessorAPI.common.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JavaTransformer.Core.HandleProcessorAPI.common.builds
{
    public class BuildEXEApplication : ApplicationLauncher
    {

        public BuildEXEApplication(BuildExeModel model) : this(model, LaunchConfiguration.Default) { }
        public BuildEXEApplication(BuildExeModel model, LaunchConfiguration cfg) : base(model, cfg, "EXE") { }
    }
}
