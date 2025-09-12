using JavaTransformer.Core.HandleProcessorAPI.api;
using JavaTransformer.Core.HandleProcessorAPI.common.model;


namespace JavaTransformer.Core.HandleProcessorAPI.common.builds
{
    public class BuildDLLApplication : SimpleApplication
    {
        public BuildDLLApplication(BuildDllConfig model) : base(model, "DLL") { }

        public override void Build()
        {
            base.Build();
        }
    }
}
