using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JavaTransformer.Core.HandleProcessorAPI.common
{
    public class InputOutput
    {
        public string Input { get; set; } = "";
        public string Output { get; set; } = "";
    
        public InputOutput(string input, string output)
        {
            Input = input;
            Output = output;
        }
    }
}
