using JavaTransformer.Core.HandleProcessorAPI.api;
using JavaTransformer.Core.HandleProcessorAPI.common.model.@base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace JavaTransformer.Core.HandleProcessorAPI.common.builds.Applications
{
    public class SimpleApplication : BaseApplication
    {
        protected string applicationType;

        public SimpleApplication(BaseModel model, string type) : base(model)
        {
            this.applicationType = type;
        }

        protected virtual void ConfigChange()
        {
            config.Clear();
            config.LoadSerialize(model);
            config.Save();
        }

        protected virtual ProcessExecutor.ProcessExecutionResult ExecuteHandler(string path, string args)
        {
            return ProcessExecutor.Execute(path, args);
        }

        protected virtual void HandlerLogs(ProcessExecutor.ProcessExecutionResult result) 
        {
            if (result.IsError())
            {
                SendLog($"ExitCode: {result.ExitCode}", "");
                SendLog($"Output: {result.Output}", "");

                SendLog($"Compile {applicationType} Failed.", "");
            }
            else
            {
                SendLog($"Compile {applicationType} Success.", "");
            }
        }

        public override void Build()
        {
            ConfigChange();
            HandlerLogs(ExecuteHandler(PathHandler, applicationType));
        }
    }
}
