using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JavaTransformer.UI.MainDesktop.Models.Services
{
    public interface IConfigurationService
    {
        T GetSection<T>(string sectionName) where T : new();
        void UpdateSection<T>(string sectionName, T data);
        void SaveConfiguration();
        void LoadConfiguration();
        void InitializeDefaultConfiguration();

        event EventHandler<ConfigurationChangedEventArgs> ConfigurationChanged;
    }
}
