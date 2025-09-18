using JavaTransformer.Core.HandleProcessorAPI.api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JavaTransformer.Core.HandleProcessorAPI.common.Configuration.Components.Options
{
    public class HeaderCompilerOption : ISerializeConfig
    {
        private bool value;

        public HeaderCompilerOption(bool value)
        {
            this.value = value;
        }

        public string SerializeConfig()
            => value ? "--header" : "";

        public readonly static HeaderCompilerOption Default = new HeaderCompilerOption(false);

        public readonly static HeaderCompilerOption On = new HeaderCompilerOption(true);
        public readonly static HeaderCompilerOption Off = new HeaderCompilerOption(false);
    }
}
