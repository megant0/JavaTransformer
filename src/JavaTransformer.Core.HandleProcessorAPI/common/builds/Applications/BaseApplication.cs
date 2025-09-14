#pragma warning disable CS8625 // Литерал, равный NULL, не может быть преобразован в ссылочный тип, не допускающий значение NULL.

using JavaTransformer.Core.HandleProcessorAPI.common.Config;
using JavaTransformer.Core.HandleProcessorAPI.common.model.@base;
using System.Text;

namespace JavaTransformer.Core.HandleProcessorAPI.common.builds.Applications
{
    public abstract class BaseApplication
    {
        private StringBuilder logBuilder;

        protected ApplicationConfig config;
        protected BaseModel model;

        protected string PathHandler = "tools/JavaTransformer.Core.HandleProcessor.exe";
        protected string PathLibraries = "libraries";
        protected string PathCrashLog = "crashlog.txt";

        public ContextApplication Context;

        public BaseApplication(BaseModel model) 
        {
            this.model = model;
            
            config = new ApplicationConfig(LaunchConfiguration.Default.ConfigInput.Path);
            logBuilder = new StringBuilder();

            Context = new ContextApplication(this);
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
        

        public class ContextApplication
        {
            private BaseApplication instance;
            public ContextApplication(BaseApplication src)
            {
                instance = src;
            }

            public void SetPathHandler(string newPath)
            {
                instance.PathHandler = newPath;
            }

            public void SetPathLibraries(string newPathLibraries)
            {
                instance.PathLibraries = newPathLibraries;
            }

            public void SetPathCrashLog(string newPathCrashLog) 
            {
                instance.PathCrashLog = newPathCrashLog;
            }
        } 
    }
}
#pragma warning restore CS8625 // Литерал, равный NULL, не может быть преобразован в ссылочный тип, не допускающий значение NULL.
