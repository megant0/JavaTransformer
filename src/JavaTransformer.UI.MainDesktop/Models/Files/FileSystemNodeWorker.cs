using JavaTransformer.UI.MainDesktop.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JavaTransformer.UI.MainDesktop.Models.Files
{
    public class FileSystemNodeWorker
    {

        public static FileSystemNode FindParent(ObservableCollection<FileSystemNode> nodes, FileSystemNode target)
        {
            foreach (var node in nodes)
            {
                if (node.Children.Contains(target)) return node;

                var parent = FindParent(node.Children, target);
                if (parent != null) return parent;
            }
            return null;
        }
    }
}
