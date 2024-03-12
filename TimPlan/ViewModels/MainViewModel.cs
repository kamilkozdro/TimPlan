using ReactiveUI;
using System.Collections.ObjectModel;
using TimPlan.Models;
using System;
using System.Diagnostics;
using System.Reactive;
using System.Windows.Input;
using System.Reactive.Linq;
using TimPlan.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using TimPlan.Lib;
using System.Linq;

namespace TimPlan.ViewModels;

public class MainViewModel : ViewModelBase
{

    #region Properties
    public ObservableCollection<TaskModel> WorkTasks { get; set; }
    public ObservableCollection<TeamModel> Teams { get; set; }
    public ObservableCollection<UserModel> Users { get; set; }

    private UserModel _LoggedUser;
    public UserModel LoggedUser
    {
        get { return _LoggedUser; }
        set { this.RaiseAndSetIfChanged(ref _LoggedUser, value); }
    }

    #region My Tasks Tab

    private ObservableCollection<TaskTileViewModel> _myTaskTilesVM;
    public ObservableCollection<TaskTileViewModel> MyTaskTilesVM
    {
        get { return _myTaskTilesVM; }
        set { this.RaiseAndSetIfChanged(ref _myTaskTilesVM, value); }
    }

    #endregion

    #region Team Overview Tab

    private UserModel _selectedTeamMember;
    public UserModel SelectedTeamMember
    {
        get { return _selectedTeamMember; }
        set { this.RaiseAndSetIfChanged(ref _selectedTeamMember, value); }
    }
    private ObservableCollection<UserModel> _teamMembers;
    public ObservableCollection<UserModel> TeamMembers
    {
        get { return _teamMembers; }
        set { this.RaiseAndSetIfChanged(ref _teamMembers, value); }
    }
    private ObservableCollection<TaskTileViewModel> _selectedTeamMemberTaskTiles;
    public ObservableCollection<TaskTileViewModel> SelectedTeamMemberTaskTiles
    {
        get { return _selectedTeamMemberTaskTiles; }
        set { this.RaiseAndSetIfChanged(ref _selectedTeamMemberTaskTiles, value); }
    }

    #endregion

    #region Project Owerview Tab



    #endregion
    private bool _editTaskVisiblity;
    public bool EditTaskVisiblity
    {
        get { return _editTaskVisiblity; }
        set { this.RaiseAndSetIfChanged(ref _editTaskVisiblity, value); }
    }

    private bool _editUsersVisibility;
    public bool EditUsersVisibility
    {
        get { return _editUsersVisibility; }
        set { this.RaiseAndSetIfChanged(ref _editUsersVisibility, value); }
    }

    private bool _editTeamRolesVisibility;
    public bool EditTeamRolesVisibility
    {
        get { return _editTeamsVisibility; }
        set { this.RaiseAndSetIfChanged(ref _editTeamsVisibility, value); }
    }

    private bool _editTeamsVisibility;
    public bool EditTeamsVisibility
    {
        get { return _editTeamsVisibility; }
        set { this.RaiseAndSetIfChanged(ref _editTeamsVisibility, value); }
    }

    private bool _addTeamMemberTaskVisibility;
    public bool AddTeamMemberTaskVisibility
    {
        get { return _addTeamMemberTaskVisibility; }
        set { this.RaiseAndSetIfChanged(ref _addTeamMemberTaskVisibility, value); }
    }
    #endregion

    #region Commands

    public ReactiveCommand<Unit, Unit> TaskEditCommand { get; }
    public ReactiveCommand<UserModel, Unit> TaskAddCommand { get; }
    public ReactiveCommand<Unit, Unit> TeamEditCommand { get; }
    public ReactiveCommand<Unit, Unit> UserEditCommand { get; }
    public ReactiveCommand<Unit, Unit> TeamRoleEditCommand { get; }

    #endregion

    public MainViewModel()
    {
        MyTaskTilesVM = new ObservableCollection<TaskTileViewModel>();
        SelectedTeamMemberTaskTiles = new ObservableCollection<TaskTileViewModel>();
        TeamMembers = new ObservableCollection<UserModel>();

        LoggedUser = LoggedUserManager.GetUser();

        this.WhenAnyValue(o => o.LoggedUser)
            .Subscribe(UpdateUserLogged);

        this.WhenAnyValue(o => o.SelectedTeamMember)
            .Subscribe(UpdateTeamMemberTasks);

        //PopulateMyTasks();
        //PopulateTeamMembers();

        #region Set Commands

        TaskEditCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            var windowService = App.Current?.Services?.GetService<IWindowService>();
            TaskModel returnedTask = await windowService.ShowTaskEditWindow(accessType: EditWindowType.Add);
        });

        TaskAddCommand = ReactiveCommand.Create<UserModel>(AddTask);

        TeamEditCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            var windowService = App.Current?.Services?.GetService<IWindowService>();
            TeamModel returnedTeam = await windowService.ShowTeamEditWindow();
        });

        UserEditCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            var windowService = App.Current?.Services?.GetService<IWindowService>();
            UserModel returnedUser = await windowService.ShowUserEditWindow();
        });

        TeamRoleEditCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            var windowService = App.Current?.Services?.GetService<IWindowService>();
            TeamRoleModel returnedTeamRole = await windowService.ShowTeamRoleEditWindow();
        });

        

        #endregion

    }

    private async void AddTask(UserModel selectedTeamMember)
    {
        if (selectedTeamMember == null)
        {
            var windowService = App.Current?.Services?.GetService<IWindowService>();
            TaskModel returnedTask = await windowService.ShowTaskEditWindow(EditWindowType.Add, null, LoggedUser);
        }
        else
        {
            var windowService = App.Current?.Services?.GetService<IWindowService>();
            TaskModel returnedTask = await windowService.ShowTaskEditWindow(EditWindowType.Add, null, SelectedTeamMember);
        }
    }
    private void PopulateMyTasks()
    {
        List<TaskModel> myTasks = new List<TaskModel>
        {
            new TaskModel()
            {
                Name = "Task 1",
                DateEnd = DateTime.Now
            },
            new TaskModel()
            {
                Name = "Task 2",
                DateEnd = DateTime.Now
            },
            new TaskModel()
            {
                Name = "Task 3",
                DateEnd = DateTime.Now
            }
        };

        foreach (TaskModel task in myTasks)
        {
            MyTaskTilesVM.Add(new TaskTileViewModel(task));
        }
    }
    private void PopulateTeamMembers()
    {
        TeamMembers = new ObservableCollection<UserModel> 
        { 
            new UserModel() { Name = "User 1" },
            new UserModel() { Name = "User 2" },
            new UserModel() { Name = "User 3" },
            new UserModel() { Name = "User 4" },
        };
    }
    private void UpdateUserLogged(UserModel user)
    {
        if(user == null) return;

        user.ReadSystemRole();
        user.ReadTeamRole();

        if (user.SystemRole == null)
        {
            EditUsersVisibility = false;
        }
        else
        {
            EditTaskVisiblity = user.SystemRole.IsAdmin;
            EditUsersVisibility = user.SystemRole.IsAdmin ||
                                  user.SystemRole.CanEditUsers;
            EditTeamsVisibility = user.SystemRole.IsAdmin ||
                                  user.SystemRole.CanEditTeams;
            EditTeamRolesVisibility = user.SystemRole.IsAdmin ||
                                      user.SystemRole.CanEditTeamRoles;
            AddTeamMemberTaskVisibility = user.SystemRole.IsAdmin ||
                                          user.TeamRole.CanAddTeamMemberTask;
        }

        UpdateMyTasks();
        UpdateTeamMembers((int)user.TeamId);

    }    
    private void UpdateMyTasks()
    {
        List<TaskModel> myTasks = SQLAccess.SelectUserTasks(LoggedUser.Id).ToList();

        foreach (TaskModel task in myTasks)
        {
            TaskTileViewModel newTaskTileVM = new TaskTileViewModel(task);
            MyTaskTilesVM.Add(newTaskTileVM);
        }
    }
    private void UpdateTeamMemberTasks(UserModel member)
    {
        if (member == null) return;

        SelectedTeamMemberTaskTiles.Clear();
        List<TaskModel> teamMemberTasks = SQLAccess.SelectUserTasks(member.Id).ToList();
        foreach (TaskModel task in teamMemberTasks)
        {
            TaskTileViewModel newTaskTileVM = new TaskTileViewModel(task);
            newTaskTileVM.ReadOnlyTask = true;
            SelectedTeamMemberTaskTiles.Add(newTaskTileVM);
        }
    }
    private void UpdateTeamMembers(int teamId)
    {
        TeamMembers = new ObservableCollection<UserModel>(
            SQLAccess.SelectTeamMembers(teamId).ToList());
    }
}
