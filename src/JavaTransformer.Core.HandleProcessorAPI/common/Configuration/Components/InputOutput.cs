using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JavaTransformer.Core.HandleProcessorAPI.common.Components
{
    public class InputOutput
    {
        public string Input { get; }
        public string Output { get; }

        public InputOutput(string input) : this(input, "-") { }

        public InputOutput(string input, string output)
        {
            Input = input ?? throw new ArgumentNullException(nameof(input));
            Output = output ?? throw new ArgumentNullException(nameof(output));
        }

        public override string ToString() =>
            Output == "-" ? Input : $"{Input} -> {Output}";
    }
}
