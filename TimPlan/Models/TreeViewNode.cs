using System.Collections.ObjectModel;

namespace TimPlan.Models
{
    public class TreeViewNode
    {
        public ObservableCollection<TreeViewNode>? SubNodes { get; }
        public string Title { get; }
        public int Id { get; }

        public TreeViewNode(string title, int id)
        {
            Title = title;
            Id = id;
        }

        public TreeViewNode(string title, int id, ObservableCollection<TreeViewNode> subNodes)
        {
            Title = title;
            Id = id;
            SubNodes = subNodes;
        }
    }
}
