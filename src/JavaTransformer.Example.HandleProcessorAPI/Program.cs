using JavaTransformer.Core.HandleProcessorAPI.api;
using JavaTransformer.Core.HandleProcessorAPI.common;
using JavaTransformer.Core.HandleProcessorAPI.common.builds;
using JavaTransformer.Core.HandleProcessorAPI.common.model;
using ru.megantcs.core.common;

namespace JavaTransformer.Example.HandleProcessorAPI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            BuildJVMApplication application = new BuildJVMApplication(
new BuildJvmConfig(new InputOutput("output.dll", "-"), new JdkInput("jdk-23")));

            application.Build();

            Console.WriteLine(application.GetLogs());
        }
    }
}
