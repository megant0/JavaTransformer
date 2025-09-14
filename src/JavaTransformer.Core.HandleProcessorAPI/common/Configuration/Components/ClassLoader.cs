using JavaTransformer.Core.HandleProcessorAPI.api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JavaTransformer.Core.HandleProcessorAPI.common.Components
{
    namespace JavaTransformer.Core.Reflection.Loaders
    {
        public class ClassLoader : IReflectionItem
        {
            private readonly string _className;

            public ClassLoader(string className)
            {
                _className = className ?? throw new ArgumentNullException(nameof(className));
                Validate();
            }

            private void Validate()
            {
                if (string.IsNullOrWhiteSpace(_className))
                    throw new ArgumentException("Class name cannot be empty or whitespace", nameof(_className));

                if (_className == "\"un\"")
                    return; 

                if (!_className.StartsWith("\"") || !_className.EndsWith("\""))
                    throw new ArgumentException("Class name must be quoted", nameof(_className));
            }

            public string GetClass() => _className;

            public string GetIdentifier() => _className;

            public bool IsValid() => !string.IsNullOrWhiteSpace(_className);

            public override string ToString() => _className;

            public static ClassLoader CreateKnotLoader() =>
                new ClassLoader("\"net/minecraft/class_310\"");

            public static ClassLoader CreateUniqueLoader() =>
                new ClassLoader("\"un\"");

            public static readonly ClassLoader KnotClassLoader = CreateKnotLoader();
            public static readonly ClassLoader UniqueClassLoader = CreateUniqueLoader();
        }
    }
}
