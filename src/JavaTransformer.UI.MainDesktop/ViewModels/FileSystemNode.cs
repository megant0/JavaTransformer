using CommunityToolkit.Mvvm.ComponentModel;
using JavaTransformer.UI.MainDesktop.Models.Files;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JavaTransformer.UI.MainDesktop.ViewModels
{
    public partial class FileSystemNode : ViewModelProgram
    {
        [ObservableProperty]
        private string _name;

        [ObservableProperty]
        private string _path;

        [ObservableProperty]
        private bool _isExpanded;

        [ObservableProperty]
        private bool _isSelected;

        [ObservableProperty]
        private ObservableCollection<FileSystemNode> _children;


        public bool IsDirectory => FileTask.IsDirectory(Path);
        public bool IsFile => FileTask.IsFile(Path);

        public FileSystemNode(string path)
        {
            Path = path;
            Name = System.IO.Path.GetFileName(path);
            Children = new ObservableCollection<FileSystemNode>();
        }
    }
}
