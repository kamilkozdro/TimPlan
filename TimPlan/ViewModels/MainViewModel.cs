using DynamicData.Binding;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TimPlan.Models;
using System;
using System.Reactive;

namespace TimPlan.ViewModels;

public class MainViewModel : ViewModelBase
{

    #region Models
    public ObservableCollection<WorkTaskModel> WorkTasks { get; set; }
    public ObservableCollection<TeamModel> Teams { get; set; }
    public ObservableCollection<UserModel> Users { get; set; }

    private UserModel _LoggedUser;
    public UserModel LoggedUser
    {
        get { return _LoggedUser; }
        set { this.RaiseAndSetIfChanged(ref _LoggedUser, value); }
    }


    #endregion

    private string _LoggedUserName;
    public string LoggedUserName
    {
        get { return _LoggedUserName; }
        set { this.RaiseAndSetIfChanged(ref _LoggedUserName, value); }
    }


    public ObservableCollection<TreeViewNode> TreeViewNodes { get; set; }
    public TreeViewNode SelectedNode { get; set; }

    #region Commands

    public ReactiveCommand<Unit, Unit> AddUserCommand { get; }
    public ReactiveCommand<Unit, Unit> DeleteUserCommand { get; }
    public ReactiveCommand<Unit, Unit> EditUserCommand { get; }

    #endregion



    public MainViewModel()
    {

        this.WhenAnyValue(o => o.LoggedUser)
            .Subscribe(UpdateUserLogged);


        #region Set Commands

        AddUserCommand = ReactiveCommand.Create(ShowAddUserDialog);
        DeleteUserCommand = ReactiveCommand.Create(ShowDeleteUserDialog);
        EditUserCommand = ReactiveCommand.Create(ShowEditUserDialog);

        #endregion



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

    private void UpdateUserLogged(UserModel user)
    {
        if(user == null) return;

        if(string.IsNullOrEmpty(user.Name))
        {
            LoggedUserName = "Noname";
        }

        LoggedUserName = user.Name;
    }

    private void ShowAddUserDialog()
    {

    }

    private void ShowDeleteUserDialog()
    {

    }

    private void ShowEditUserDialog()
    {

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
