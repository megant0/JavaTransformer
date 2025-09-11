using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace JavaTransformer.Core.ModelViews
{
    public class MainModelView : INotifyPropertyChanging
    {
        public event PropertyChangingEventHandler? PropertyChanging;

        #region Properties

        private string _machineName;

        public string MachineName 
        {
            get => _machineName; 
            set 
            {
                _machineName = value;
                OnPropertyChanged(nameof(_machineName));
            } 
        }

        private string _machineVersion;

        public string MachineVersion
        {
            get => _machineVersion;
            set
            {
                _machineVersion = value;
                OnPropertyChanged(nameof(_machineVersion));
            }
        }

        #endregion

        #region Protected Methods
      
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) 
        {
            PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
        }

        #endregion

        #region Constructors

        public MainModelView(string name, string version)
        {
            _machineName = name;
            _machineVersion = version;
        }

        #endregion
    }
}
