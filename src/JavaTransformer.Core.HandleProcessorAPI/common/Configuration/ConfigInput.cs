using JavaTransformer.Core.HandleProcessorAPI.api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JavaTransformer.Core.HandleProcessorAPI.common.Config
{
    public class ConfigInput
    {
        private string config_path;

        public string Path 
        {
            get
            {
                return config_path;
            } 
        }

        public ConfigInput(string config_path)
        {
            this.config_path = config_path;
        }

        public readonly static ConfigInput Default = new ConfigInput("config/generate_code.txt");
    }
}
