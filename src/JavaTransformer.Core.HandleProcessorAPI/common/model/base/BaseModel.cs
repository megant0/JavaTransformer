using JavaTransformer.Core.HandleProcessorAPI.api;
using JavaTransformer.Core.HandleProcessorAPI.common.Components;

namespace JavaTransformer.Core.HandleProcessorAPI.common.model.@base
{
    public abstract class BaseModel : ISerializeConfig
    {
        public InputOutput IO { get; set; }
        public CompilerGCC GCC { get; set; } = CompilerGCC.GCC;
        public TemplateCompilation Template { get; set; } = TemplateCompilation.Default;

        public abstract string SerializeConfig(); 
    }
}
