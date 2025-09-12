#pragma warning disable CS8625 // Литерал, равный NULL, не может быть преобразован в ссылочный тип, не допускающий значение NULL.

using JavaTransformer.Core.HandleProcessorAPI.common.model;
using System.Text;

namespace JavaTransformer.Core.HandleProcessorAPI.common.builds
{
    public abstract class BaseApplication
    {
        private StringBuilder logBuilder;

        protected ApplicationConfig config;
        protected BaseModel model;

        public const string PathHandler = "tools/JavaTransformer.Core.HandleProcessor.exe";

        public BaseApplication(BaseModel model) 
        {
            this.model = model;
            
            config = new ApplicationConfig();
            logBuilder = new StringBuilder();
        }

        public abstract void Build();

       protected void SendLog(string message, string format = null)
       {
            if(format == null)
            {
                format = $"{DateTime.Now.ToString()} :";
            }
            logBuilder.AppendLine(format + message);
        }
    
        public string GetLogs()
        {
            return logBuilder.ToString();
        }
        

    }
}
#pragma warning restore CS8625 // Литерал, равный NULL, не может быть преобразован в ссылочный тип, не допускающий значение NULL.
