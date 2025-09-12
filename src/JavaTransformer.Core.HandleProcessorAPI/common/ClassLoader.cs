using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JavaTransformer.Core.HandleProcessorAPI.common
{
    public class ClassLoader
    {
        private string _class;

        public ClassLoader(string @class)
        {
            _class = @class;
        }

        public string GetClass()
        {
            return _class;
        }

        /**
         * fabric class loader
         * 
         * класс лоадер будет получен из класса:
         * "net/minecraft/class_310"
         */
        public static ClassLoader KnotClassLoader   = new ClassLoader("\"net/minecraft/class_310\"");

        /**
         * thread class loader.
         * (Render thread) -
         *  
         *  в теории может работать с любым майнкрафтом,
         *  советуется использовать именно его.
         */
        public static ClassLoader UniqueClassLoader = new ClassLoader("\"un\"");
    }
}
