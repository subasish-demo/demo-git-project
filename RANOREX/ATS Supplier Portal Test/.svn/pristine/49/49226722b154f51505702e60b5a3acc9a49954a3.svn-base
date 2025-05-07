using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATS.CodeLibrary.DataUtilities.Hierarchy
{
    /// <summary>
    /// Used by HierarchyHelper to make a flat collection into a hierarchical collection
    /// </summary>
    /// <typeparam name="T">Original Type</typeparam>
    public class TreeItem<T>
    {
        public T Item { get; set; }
        public IEnumerable<TreeItem<T>> Children { get; set; }

        public TreeItem()
        {
            Children = new List<TreeItem<T>>();
        }

        public override string ToString()
        {
            return Item.ToString();
        }
    }

    public class DisplayTreeItem<T>
    {
        public T Item { get; set; }
        public ObservableCollection<DisplayTreeItem<T>> Children { get; set; }

        public DisplayTreeItem()
        {
            Children = new ObservableCollection<DisplayTreeItem<T>>();
        }
        
        public override string ToString()
        {
            return Item.ToString();
        }
    }
}
