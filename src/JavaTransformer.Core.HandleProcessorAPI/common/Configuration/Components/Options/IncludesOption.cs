using JavaTransformer.Core.HandleProcessorAPI.api;
using JavaTransformer.Core.HandleProcessorAPI.common.Config.Components.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JavaTransformer.Core.HandleProcessorAPI.common.Configuration.Components.Options
{
    public class IncludesOption : CommandLineOption
    {
        protected override string OptionName => "includes";

        public IncludesOption(string path) : base(path) { }
        public static readonly IncludesOption Default = new IncludesOption("includes");
    }
}
