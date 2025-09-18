using JavaTransformer.UI.MainDesktop.Models.Common.DataTransferObject.Settings;
using JavaTransformer.UI.MainDesktop.Models.Common.Services;
using JavaTransformer.UI.MainDesktop.Models.Services;
using JavaTransformer.UI.MainDesktop.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JavaTransformer.UI.MainDesktop.Models
{
    public static class ServiceLocator
    {
        public static IServiceProvider Services { get; private set; }

        public static void Initialize()
        {
            var services = new ServiceCollection();

            services.AddSingleton<IConfigurationService, ConfigurationService>();
            services.AddSingleton<IPathService, PathService>();
            services.AddSingleton<ILoggerService, LoggerService>();

            services.AddTransient(provider => provider.GetService<IConfigurationService>().GetSection<AppSettings>("AppSettings"));
            services.AddTransient(provider => provider.GetService<IConfigurationService>().GetSection<EditorSettings>("EditorSettings"));
            services.AddTransient<MainWindowViewModel>();

            services.AddTransient<ProjectPage>();
            services.AddTransient<Func<string, ProjectPage>>(provider => path => provider.GetRequiredService<ProjectPage>());
            
            Services = services.BuildServiceProvider();

            InitializeConfiguration();
        }
        private static void InitializeConfiguration()
        {
            var configService = Services.GetService<IConfigurationService>();
            configService.LoadConfiguration();
        }

        public static T? GetService<T>() where T : class 
        {
            return Services.GetService<T>();
        }
    }
}
