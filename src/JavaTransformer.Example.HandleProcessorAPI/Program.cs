using JavaTransformer.Core.HandleProcessorAPI.api;
using JavaTransformer.Core.HandleProcessorAPI.common;
using JavaTransformer.Core.HandleProcessorAPI.common.builds;
using JavaTransformer.Core.HandleProcessorAPI.common.builds.Applications;
using JavaTransformer.Core.HandleProcessorAPI.common.Components;
using JavaTransformer.Core.HandleProcessorAPI.common.Components.JavaTransformer.Core.Reflection.Loaders;
using JavaTransformer.Core.HandleProcessorAPI.common.Config;
using JavaTransformer.Core.HandleProcessorAPI.common.Config.Components;
using JavaTransformer.Core.HandleProcessorAPI.common.Configuration.Components.Options;
using JavaTransformer.Core.HandleProcessorAPI.common.model;
using ru.megantcs.core.common;

namespace JavaTransformer.Example.HandleProcessorAPI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            BuildDllModel model = new BuildDllModel();
            model.IO = new InputOutput("test/input/1.jar", "test/output/1");
            model.ClassLoader = ClassLoader.UniqueClassLoader;
            model.MethodReference = new MethodReference("ru/megantcs/MyClient", "onInitialize");

            BuildDLLApplication application = new BuildDLLApplication(model, new LaunchConfiguration()
            {
                ConfigInput = new ConfigInput("test/config/1.txt"),
                DebugMode = true,
                AlwaysShowOutput = true,
                Libraries = new LibrariesOption("ahaha")
            });
            application.Context.SetPathHandler("test/tools/JavaTransformer.Core.HandleProcessor.exe");
            application.Build();

            Console.WriteLine(application.GetLogs());
        }
    }
}
