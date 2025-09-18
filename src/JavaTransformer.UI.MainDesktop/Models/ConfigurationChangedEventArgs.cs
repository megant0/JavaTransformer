using System;

namespace JavaTransformer.UI.MainDesktop.Models
{
    public class ConfigurationChangedEventArgs : EventArgs
    {
        public string SectionName { get; }
        public object NewValue { get; }

        public ConfigurationChangedEventArgs(string sectionName, object newValue)
        {
            SectionName = sectionName;
            NewValue = newValue;
        }
    }
}
