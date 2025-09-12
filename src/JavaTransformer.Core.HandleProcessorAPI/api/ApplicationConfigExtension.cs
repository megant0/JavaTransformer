using JavaTransformer.Core.HandleProcessorAPI.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JavaTransformer.Core.HandleProcessorAPI.api
{
    public static class ApplicationConfigExtension
    {
        public static void LoadSerialize(this ApplicationConfig cfg, ISerializeConfig serialize)
        {
            var text = serialize.SerializeConfig();
            if (text == null) throw new NullReferenceException("Text is null");

            cfg.AddLine(text);
        }
    }
}
