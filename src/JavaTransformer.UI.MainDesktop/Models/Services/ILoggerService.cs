using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JavaTransformer.UI.MainDesktop.Models.Services
{
    public interface ILoggerService
    {
        void Error(string message);
        void Error(string message, Exception exception);
        void Fatal(string message);
        void Info(string message);
        void Warn(string message);
        Task SaveAsync();
        void Flush(); 
    }
}
