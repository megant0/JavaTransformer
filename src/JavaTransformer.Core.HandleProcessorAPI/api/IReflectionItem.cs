using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JavaTransformer.Core.HandleProcessorAPI.api
{
    public interface IReflectionItem
    {
        string GetIdentifier();
        bool IsValid();
    }
}
