using JavaTransformer.Core.HandleProcessorAPI.common.Configuration.Components.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JavaTransformer.Core.HandleProcessorAPI.common.Config
{
    public class LaunchConfiguration
    {
        public bool DebugMode { get; set; } = false;
        public bool AlwaysShowOutput { get; set; } = false;

        public LibrariesOption Libraries { get; set; } = LibrariesOption.Default;
        public CrashLogOption CrashLog { get; set; } = CrashLogOption.Default;
        public IncludesOption Includes { get; set; } = IncludesOption.Default;
        public ConfigInput ConfigInput { get; set; } = ConfigInput.Default;

        public LaunchConfiguration() { }

        public static LaunchConfiguration Default = new LaunchConfiguration();
    }
}
