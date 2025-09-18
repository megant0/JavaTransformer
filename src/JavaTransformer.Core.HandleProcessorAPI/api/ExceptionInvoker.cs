using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JavaTransformer.Core.HandleProcessorAPI.api
{
    public class ExceptionInvoker
    {
        public static bool Invoke<RuntimeError>(Action action, out RuntimeError? exception) 
            where RuntimeError : Exception 
        {
            try
            {
                action?.Invoke();

                exception = null;
                return true;
            }
            catch(RuntimeError ex)
            {
                exception = ex;
                return false;
            }
        }

        public static bool Invoke<RuntimeError>(Action action, Action<RuntimeError?> exception)
            where RuntimeError : Exception
        {
            bool _ = Invoke<RuntimeError>(action, out RuntimeError? error);
            if(_ == false) exception?.Invoke(error);
            
            return _;
        }

        public static bool Invoke<RuntimeException>(Action action) where RuntimeException : Exception
        {
            return Invoke<RuntimeException>(action, (e) => { });
        }

        public static void Throw(Exception? exception)
        {
            if (exception != null) throw exception;
            else throw new Exception();
        }
    }
}
