using JavaTransformer.Core.HandleProcessorAPI.api;
using JavaTransformer.Core.HandleProcessorAPI.common.Config.Components.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace JavaTransformer.Core.HandleProcessorAPI.common.Configuration.Components.Options
{
    public class CrashLogOption : CommandLineOption
    {
        protected override string OptionName => "crashLog";

        public CrashLogOption(string path) : base(path) { }
        public static readonly CrashLogOption Default = new CrashLogOption("crashlog.txt");
    }
}
