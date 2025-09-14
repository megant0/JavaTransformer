using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace JavaTransformer.Core.HandleProcessorAPI.common.Exceptions
{
    public class FileNotFoundException : IOException
    {
        public FileNotFoundException()
        {
        }

        public FileNotFoundException(string? message) : base(message == null? message : message.Replace("\\", "/"))
        {
        }

        public FileNotFoundException(string? message, System.Exception? innerException) : base(message == null ? message : message.Replace("\\", "/"), innerException)
        {
        }

        public FileNotFoundException(string? message, int hresult) : base(message == null ? message : message.Replace("\\", "/"), hresult)
        {

        }
    }
}
