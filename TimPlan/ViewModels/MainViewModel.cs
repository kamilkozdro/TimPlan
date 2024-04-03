using Avalonia.Threading;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using TimPlan.Lib;
using TimPlan.Models;
using TimPlan.Services;

namespace TimPlan.ViewModels;

public class MainViewModel : ViewModelBase
{

    #region Properties

    private UserModel? _LoggedUser;
    public UserModel? LoggedUser
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

    private ObservableCollection<TaskModel> _myTasks;
    public ObservableCollection<TaskModel> MyTasks
    {
        get { return _myTasks; }
        set { this.RaiseAndSetIfChanged(ref _myTasks, value); }
    }


    #endregion

    #region Team Overview Tab

    private UserModel? _selectedTeamMember;
    public UserModel? SelectedTeamMember
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

    private ObservableCollection<TaskModel> _selectedTeamMemberTasks;
    public ObservableCollection<TaskModel> SelectedTeamMemberTasks
    {
        get { return _selectedTeamMemberTasks; }
        set { this.RaiseAndSetIfChanged(ref _selectedTeamMemberTasks, value); }
    }

    #endregion

    #region Task Archive Tab

    // TODO: Task Archive Tab

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
        get { return _editTeamRolesVisibility; }
        set { this.RaiseAndSetIfChanged(ref _editTeamRolesVisibility, value); }
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

    private DispatcherTimer _updateMyTaskTimer;

    #endregion

    #region Commands

    public ReactiveCommand<Unit, Unit> TaskEditCommand { get; }
    public ReactiveCommand<UserModel, Unit> TaskAddCommand { get; }
    public ReactiveCommand<Unit, Unit> TeamEditCommand { get; }
    public ReactiveCommand<Unit, Unit> UserEditCommand { get; }
    public ReactiveCommand<Unit, Unit> TeamRoleEditCommand { get; }
    public ReactiveCommand<Unit, Unit> LogoutCommand { get; }
    public ReactiveCommand<Unit, Unit> LoginCommand { get; }

    #endregion

    public MainViewModel()
    {
        _myTasks = new ObservableCollection<TaskModel>();
        _myTaskTilesVM = new ObservableCollection<TaskTileViewModel>();
        _teamMembers = new ObservableCollection<UserModel>();
        _selectedTeamMemberTasks = new ObservableCollection<TaskModel>();
        _selectedTeamMemberTaskTiles = new ObservableCollection<TaskTileViewModel>();

        _updateMyTaskTimer = new DispatcherTimer();
        _updateMyTaskTimer.Interval = TimeSpan.FromSeconds(10);
        _updateMyTaskTimer.Tick += UpdateMyTaskTimer_Tick;

        this.WhenAnyValue(o => o.LoggedUser)
            .Subscribe(UpdateUserLogged);

        this.WhenAnyValue(o => o.SelectedTeamMember)
            .Subscribe(UpdateTeamMemberTasks);

        IObservable<bool> LoginCommandCanExecute = this.WhenAnyValue(
                x => x.LoggedUser,
                (UserModel? user) => user == null);

        IObservable<bool> LogoutCommandCanExecute = this.WhenAnyValue(
                x => x.LoggedUser,
                (UserModel? user) => user != null);

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

        LogoutCommand = ReactiveCommand.Create(Logout, LogoutCommandCanExecute);
        LoginCommand = ReactiveCommand.Create(Login, LoginCommandCanExecute);

        #endregion

    }

    

    private void Logout()
    {
        LoggedUser = null;
        SelectedTeamMember = null;
        _updateMyTaskTimer.Stop();
        LoggedUserManager.Logout();
    }
    private async void Login()
    {
        var windowService = App.Current?.Services?.GetService<IWindowService>();
        UserModel loggedUser = await windowService.ShowLoginWindow();

        LoggedUserManager.Login(loggedUser);
        LoggedUser = loggedUser;
        _updateMyTaskTimer.Start();
    }
    private async void AddTask(UserModel selectedTeamMember)
    {
        var windowService = App.Current?.Services?.GetService<IWindowService>();
        TaskModel returnedTask = await windowService.ShowTaskEditWindow(EditWindowType.Add, null, SelectedTeamMember);
        if (returnedTask != null)
        {
            SelectedTeamMemberTasks.Add(returnedTask);
            SelectedTeamMemberTaskTiles.Add(new TaskTileViewModel(returnedTask));
        }
    }
    private void UpdateUserLogged(UserModel? user)
    {
        if (user != null)
        {
            user.ReadSystemRole();
            user.ReadTeamRole();
        }

        if (user == null || user?.SystemRole == null || user?.TeamRole == null)
        {
            EditUsersVisibility = false;
            EditTaskVisiblity = false;
            EditTeamsVisibility = false;
            EditTeamRolesVisibility = false;
            AddTeamMemberTaskVisibility = false;
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
        UpdateTeamMembers();
    }
    private void UpdateMyTasks()
    {
        MyTasks.Clear();

        if (LoggedUser != null)
        {
            MyTasks = new ObservableCollection<TaskModel>(
            SQLAccess.SelectUserTasks(LoggedUser.Id));
        }

        UpdateMyTaskTiles();
    }
    private void UpdateMyTaskTiles()
    {
        MyTaskTilesVM.Clear();
        foreach (TaskModel task in MyTasks)
        {
            TaskTileViewModel newTaskTileVM = new TaskTileViewModel(task);
            MyTaskTilesVM.Add(newTaskTileVM);
        }
    }
    private void UpdateTeamMemberTasks(UserModel? member)
    {
        SelectedTeamMemberTasks.Clear();

        if (member != null)
        {
            SelectedTeamMemberTasks = new ObservableCollection<TaskModel>(
            SQLAccess.SelectUserTasks(member.Id));
        }

        UpdateSelectedTeamMemberTaskTiles();
    }
    private void UpdateSelectedTeamMemberTaskTiles()
    {
        SelectedTeamMemberTaskTiles.Clear();
        foreach (TaskModel task in SelectedTeamMemberTasks)
        {
            TaskTileViewModel newTaskTileVM = new TaskTileViewModel(task);
            newTaskTileVM.ReadOnlyTask = true;
            SelectedTeamMemberTaskTiles.Add(newTaskTileVM);
        }
    }
    private void UpdateTeamMembers()
    {
        TeamMembers.Clear();

        if (LoggedUser == null || LoggedUser?.TeamId == null)
            return;

        TeamMembers = new ObservableCollection<UserModel>(
            SQLAccess.SelectTeamMembers((int)LoggedUser.TeamId).ToList());
    }

    private void UpdateMyTaskTimer_Tick(object? sender, EventArgs e)
    {
        UpdateMyTasks();
    }
}
