using JavaTransformer.Core.HandleProcessorAPI.api;
using JavaTransformer.Core.HandleProcessorAPI.common.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JavaTransformer.Core.HandleProcessorAPI.common.builds
{
    public class SimpleApplication : BaseApplication
    {
        private string type;

        public SimpleApplication(BaseModel model, string type) : base(model)
        {
            this.type = type;
        }

        public override void Build()
        {
            config.Clear();
            config.LoadSerialize(model);
            config.Save();

            var result = ProcessExecutor.Execute(PathHandler, type);

            if (result.IsError())
            {
                SendLog($"ExitCode: {result.ExitCode}", "");
                SendLog($"Output: {result.Output}", "");

                SendLog($"Compile {type} Failed.", "");
            }
            else
            {
                SendLog($"Compile {type} Success.", "");
            }
        }
    }
}
