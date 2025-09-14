using JavaTransformer.Core.HandleProcessorAPI.api;
using JavaTransformer.Core.HandleProcessorAPI.common.Config;
using JavaTransformer.Core.HandleProcessorAPI.common.model.@base;


namespace JavaTransformer.Core.HandleProcessorAPI.common.builds.Applications
{
    public class ApplicationLauncher : SimpleApplication
    {
        private readonly LaunchConfiguration _launchConfig;

        public ApplicationLauncher(BaseModel model, LaunchConfiguration launchConfig, string applicationType)
            : base(model, applicationType)
        {
            _launchConfig = launchConfig ?? throw new ArgumentNullException(nameof(launchConfig));
            config = new ApplicationConfig(_launchConfig.ConfigInput.Path);
        }

        protected override ProcessExecutor.ProcessExecutionResult ExecuteHandler(string path, string arguments)
        {
            return base.ExecuteHandler(path, arguments);
        }

        protected override void HandlerLogs(ProcessExecutor.ProcessExecutionResult result)
        {
            bool hasErrors = result.IsError();

            string statusMessage = hasErrors
                ? $"Build {applicationType} failed with errors"
                : $"Build {applicationType} completed successfully";

            SendLog(statusMessage, "");

            if (hasErrors || _launchConfig.AlwaysShowOutput)
            {
                SendLog($"Exit code: {result.ExitCode}", "");
                SendLog($"Program output: {result.Output}", "");
            }
        }

        protected override void ConfigChange()
        {
            base.ConfigChange();
        }

        protected virtual string GetDebugFlag(bool debugEnabled) => debugEnabled ? "--debug" : "";

        protected virtual string BuildCommandLineArguments()
        {
            return $"{applicationType} config={_launchConfig.ConfigInput.Path} " +
                   $"{GetDebugFlag(_launchConfig.DebugMode)} " +
                   $"{_launchConfig.CrashLog.SerializeConfig()} " +
                   $"{_launchConfig.Libraries.SerializeConfig()} " +
                   $"{_launchConfig.Includes.SerializeConfig()}";
        }

        public override void Build()
        {
            ConfigChange();
            string arguments = BuildCommandLineArguments();
            var executionResult = ExecuteHandler(PathHandler, arguments);
            HandlerLogs(executionResult);
        }

        public virtual LaunchConfiguration GetLaunchConfiguration() => _launchConfig;
    }
}
