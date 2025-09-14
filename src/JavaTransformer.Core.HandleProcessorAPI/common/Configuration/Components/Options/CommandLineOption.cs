using JavaTransformer.Core.HandleProcessorAPI.api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JavaTransformer.Core.HandleProcessorAPI.common.Config.Components.Commands
{
    public abstract class CommandLineOption : ISerializeConfig
    {
        public string Path { get; set; }
        protected abstract string OptionName { get; }

        protected CommandLineOption(string path)
        {
            Path = path ?? throw new ArgumentNullException(nameof(path));
        }

        public virtual string SerializeConfig() => $"--{OptionName}={Path}";
    }
}
