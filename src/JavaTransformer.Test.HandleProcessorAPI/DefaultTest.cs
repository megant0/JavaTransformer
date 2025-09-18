
using JavaTransformer.Core.HandleProcessorAPI.common.builds;
using JavaTransformer.Core.HandleProcessorAPI.common.Components;
using JavaTransformer.Core.HandleProcessorAPI.common.Components.JavaTransformer.Core.Reflection.Loaders;
using JavaTransformer.Core.HandleProcessorAPI.common.Config;
using JavaTransformer.Core.HandleProcessorAPI.common.Configuration.Components.Options;
using JavaTransformer.Core.HandleProcessorAPI.common.model;

using System.Diagnostics;

namespace JavaTransformer.Test.HandleProcessorAPI
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {

        }

        #region Default
        [Test]
        public void BuildDllApplicationDefaultTest()
        {
            BuildDllModel model = new BuildDllModel();
            model.IO = new InputOutput("test/input/1.jar", "test/output/1");
            model.ClassLoader = ClassLoader.UniqueClassLoader;
            model.MethodReference = new MethodReference("ru/megantcs/MyClient", "onInitialize");

            BuildDLLApplication application = new BuildDLLApplication(model, new LaunchConfiguration()
            {
                ConfigInput = new ConfigInput("test/config/1.txt"),
                AlwaysShowOutput = true,
                DebugMode = true,
            });
            application.Context.SetPathHandler("test/tools/JavaTransformer.Core.HandleProcessor.exe");
            application.Build();

            Console.WriteLine(application.GetLogs());
            Assert.That(File.Exists("test/output/1.dll"), Is.True);
        }

        [Test]
        public void BuildExeApplicationDefaultTest()
        {
            BuildExeModel model = new BuildExeModel();
            model.IO = new InputOutput("test/input/2.dll", "test/output/2");

            BuildEXEApplication application = new BuildEXEApplication(model, new LaunchConfiguration()
            {
                ConfigInput = new ConfigInput("test/config/2.txt"),
                AlwaysShowOutput = true,
                DebugMode = true,
            });
            application.Context.SetPathHandler("test/tools/JavaTransformer.Core.HandleProcessor.exe");
            application.Build();

            Console.WriteLine(application.GetLogs());
            Assert.That(File.Exists("test/output/2.exe"), Is.True);
        }

        [Test]
        public void BuildJvmApplicationDefaultTest()
        {
            BuildJvmModel model = new BuildJvmModel();
            model.IO = new InputOutput("test/input/3");
            model.JdkInput = new JdkInput("test/input/3");

            BuildJVMApplication application = new BuildJVMApplication(model, new LaunchConfiguration()
            {
                ConfigInput = new ConfigInput("test/config/3.txt"),
                AlwaysShowOutput = true,
                DebugMode = true,
            });

            application.Context.SetPathHandler("test/tools/JavaTransformer.Core.HandleProcessor.exe");
            application.Build();

            Console.WriteLine(application.GetLogs());

            Assert.That(File.Exists("test/input/3/bin/java.exe"), Is.True);
            Assert.That(File.Exists("test/input/3/bin/javaw.exe"), Is.True);
        }

        #endregion
        #region Template
        [Test]
        public void BuildDllApplicationTemplateTest()
        {
            BuildDllModel model = new BuildDllModel();
            model.IO = new InputOutput("test/input/1.jar", "test/output/4");
            model.ClassLoader = ClassLoader.UniqueClassLoader;
            model.MethodReference = new MethodReference("ru/megantcs/MyClient", "onInitialize");
            model.Template = new TemplateCompilation("test/template/dll-template");

            BuildDLLApplication application = new BuildDLLApplication(model, new LaunchConfiguration()
            {
                ConfigInput = new ConfigInput("test/config/4.txt"),
                AlwaysShowOutput = true,
                DebugMode = true,
            });
            application.Context.SetPathHandler("test/tools/JavaTransformer.Core.HandleProcessor.exe");
            application.Build();

            Console.WriteLine(application.GetLogs());
            Assert.That(File.Exists("test/output/4.dll"), Is.True);
        }

        [Test]
        public void BuildExeApplicationTemplateTest()
        {
            BuildExeModel model = new BuildExeModel();
            model.IO = new InputOutput("test/input/2.dll", "test/output/5");
            model.Template = new TemplateCompilation("test/template/ldr-template");

            BuildEXEApplication application = new BuildEXEApplication(model, new LaunchConfiguration()
            {
                ConfigInput = new ConfigInput("test/config/5.txt"),
                AlwaysShowOutput = true,
                DebugMode = true,
            });
            application.Context.SetPathHandler("test/tools/JavaTransformer.Core.HandleProcessor.exe");
            application.Build();

            Console.WriteLine(application.GetLogs());
            Assert.That(File.Exists("test/output/5.exe"), Is.True);
        }

        [Test]
        public void BuildJvmApplicationTemplateTest()
        {
            BuildJvmModel model = new BuildJvmModel();
            model.IO = new InputOutput("test/output/6");
            model.JdkInput = new JdkInput("test/output/6");
            model.Template = new TemplateCompilation("test/template/jvm-template");

            BuildJVMApplication application = new BuildJVMApplication(model, new LaunchConfiguration()
            {
                ConfigInput = new ConfigInput("test/config/6.txt"),
                AlwaysShowOutput = true,
                DebugMode = true,
            });

            application.Context.SetPathHandler("test/tools/JavaTransformer.Core.HandleProcessor.exe");
            application.Build();

            Console.WriteLine(application.GetLogs());

            Assert.That(File.Exists("test/output/6/bin/java.exe"), Is.True);
            Assert.That(File.Exists("test/output/6/bin/javaw.exe"), Is.True);
        }

        #endregion
        #region Header

        [Test]
        public void BuildDllApplicationHeaderTest()
        {
            BuildDllModel model = new BuildDllModel();
            model.IO = new InputOutput("test/input/1", "test/output/11");
            model.Template = new TemplateCompilation("test/output/dll1");
            model.MethodReference = new MethodReference("test/test2", "test");
            model.ClassLoader = ClassLoader.UniqueClassLoader;

            LaunchConfiguration launch = new LaunchConfiguration();
            launch.AlwaysShowOutput = true;
            launch.DebugMode = true;

            launch.HeaderCompiler = HeaderCompilerOption.On;
            launch.CrashLog = new CrashLogOption("test/crash/last-crash.txt");

            BuildDLLApplication application = new BuildDLLApplication(model, launch);
            application.Context.SetPathHandler("test/tools/JavaTransformer.Core.HandleProcessor.exe");
            application.Build();
            Console.WriteLine(application.GetLogs());

          
            Assert.That(new FileInfo("test/output/dll1/header.h").Length > 0, "size is null");
        }

        [Test]
        public void BuildExeApplicationHeaderTest()
        {
            BuildExeModel model = new BuildExeModel();
            model.IO = new InputOutput("test/input/1", "test/output/12");
            model.Template = new TemplateCompilation("test/output/dll1");

            LaunchConfiguration launch = new LaunchConfiguration();
            launch.AlwaysShowOutput = true;
            launch.DebugMode = true;

            launch.HeaderCompiler = HeaderCompilerOption.On;
            launch.CrashLog = new CrashLogOption("test/crash/last-crash.txt");

            BuildEXEApplication application = new BuildEXEApplication(model, launch);

            application.Context.SetPathHandler("test/tools/JavaTransformer.Core.HandleProcessor.exe");
            application.Build();
            Console.WriteLine(application.GetLogs());

            Assert.That(new FileInfo("test/output/exe1/header.h").Length > 0, "size is null");
        }
        #endregion
    }
}