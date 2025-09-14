using JavaTransformer.Core.HandleProcessorAPI.api;
using JavaTransformer.Core.HandleProcessorAPI.common.Config.Components.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JavaTransformer.Core.HandleProcessorAPI.common.Configuration.Components.Options
{
    public class LibrariesOption : CommandLineOption
    {
        protected override string OptionName => "libraries";

        public LibrariesOption(string path) : base(path) { }
        public static LibrariesOption Default = new LibrariesOption("libraries");
    }
}
