using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JavaTransformer.Core.HandleProcessorAPI.common
{
    public class MethodReference
    {
        public string Class { get; set; }
        public string Method { get; set; }

        public MethodReference(string @class, string method)
        {
            Class = @class;
            Method = method;
        }
    }
}
