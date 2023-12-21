using ReactiveUI;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TimPlan.Models;

namespace TimPlan.ViewModels;

public class MainViewModel : ViewModelBase
{

    #region Models

    public ObservableCollection<Project> Projects { get; set; }
    public ObservableCollection<WorkTaskModel> WorkTasks { get; set; }
    public ObservableCollection<TeamModel> Teams { get; set; }
    public ObservableCollection<UserModel> Users { get; set; }



    #endregion


    public ObservableCollection<TreeViewNode> TreeViewNodes { get; set; }
    public TreeViewNode SelectedNode { get; set; }

    public MainViewModel()
    {
        TreeViewNodes = new ObservableCollection<TreeViewNode>()
        {
            new TreeViewNode("Node 1", 1,
                new ObservableCollection<TreeViewNode>()
                {
                    new TreeViewNode("Node 1-1", 3)
                }),
            new TreeViewNode("Node 2", 2)
        };

    }

    private void ClearTreeView()
    {
        TreeViewNodes.Clear();
    }

    private void BuildProjectBasedTreeView()
    {




        ClearTreeView();

    }
}
