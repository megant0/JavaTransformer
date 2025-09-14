using JavaTransformer.Core.HandleProcessorAPI.api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JavaTransformer.Core.HandleProcessorAPI.common.Components
{
    public class MethodReference : IReflectionItem
    {
        public string Class { get; }
        public string Method { get; }

        public MethodReference(string @class, string method)
        {
            Class = @class ?? throw new ArgumentNullException(nameof(@class));
            Method = method ?? throw new ArgumentNullException(nameof(method));
            Validate();
        }

        private void Validate()
        {
            if (string.IsNullOrWhiteSpace(Class))
                throw new ArgumentException("Class name cannot be empty", nameof(Class));

            if (string.IsNullOrWhiteSpace(Method))
                throw new ArgumentException("Method name cannot be empty", nameof(Method));
        }

        public string GetIdentifier() => $"{Class}::{Method}";

        public bool IsValid() => !string.IsNullOrWhiteSpace(Class) && !string.IsNullOrWhiteSpace(Method);

        public override string ToString() => $"{Class}.{Method}";

        public static MethodReference Create(string @class, string method) =>
            new MethodReference(@class, method);
    }
}
