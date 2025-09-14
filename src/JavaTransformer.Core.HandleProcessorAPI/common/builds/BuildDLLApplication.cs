using JavaTransformer.Core.HandleProcessorAPI.api;
using JavaTransformer.Core.HandleProcessorAPI.common.builds.Applications;
using JavaTransformer.Core.HandleProcessorAPI.common.Config;
using JavaTransformer.Core.HandleProcessorAPI.common.model;


namespace JavaTransformer.Core.HandleProcessorAPI.common.builds
{
    public class BuildDLLApplication : ApplicationLauncher
    {
        public BuildDLLApplication(BuildDllModel model) : this(model, LaunchConfiguration.Default) { }
        public BuildDLLApplication(BuildDllModel model, LaunchConfiguration configLaunch) : base(model, configLaunch, "DLL") { }
    }
}
