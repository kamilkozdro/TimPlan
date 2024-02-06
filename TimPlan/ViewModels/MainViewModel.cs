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

    #region Models
    public ObservableCollection<TaskModel> WorkTasks { get; set; }
    public ObservableCollection<TeamModel> Teams { get; set; }
    public ObservableCollection<UserModel> Users { get; set; }

    private UserModel _LoggedUser;
    public UserModel LoggedUser
    {
        get { return _LoggedUser; }
        set { this.RaiseAndSetIfChanged(ref _LoggedUser, value); }
    }


    private ObservableCollection<TaskTileViewModel> _myTaskTilesVM;
    public ObservableCollection<TaskTileViewModel> MyTaskTilesVM
    {
        get { return _myTaskTilesVM; }
        set { this.RaiseAndSetIfChanged(ref _myTaskTilesVM, value); }
    }


    #endregion

    private string _LoggedUserName;
    public string LoggedUserName
    {
        get { return _LoggedUserName; }
        set { this.RaiseAndSetIfChanged(ref _LoggedUserName, value); }
    }

    public TreeViewNode SelectedNode { get; set; }
    private bool _EditUsersVisibility;
    public bool EditUsersVisibility
    {
        get { return _EditUsersVisibility; }
        set { this.RaiseAndSetIfChanged(ref _EditUsersVisibility, value); }
    }
    private bool _EditTeamsVisibility;
    public bool EditTeamsVisibility
    {
        get { return _EditTeamsVisibility; }
        set { this.RaiseAndSetIfChanged(ref _EditTeamsVisibility, value); }
    }


    #region Commands

    public ReactiveCommand<Unit, Unit> TaskEditCommand { get; }
    public ReactiveCommand<Unit, Unit> TeamEditCommand { get; }
    public ReactiveCommand<Unit, Unit> UserEditCommand { get; }
    public ReactiveCommand<Unit, Unit> TeamRoleEditCommand { get; }

    #endregion

    public MainViewModel()
    {

        this.WhenAnyValue(o => o.LoggedUser)
            .Subscribe(UpdateUserLogged);

        MyTaskTilesVM = new ObservableCollection<TaskTileViewModel>();

        #region Set Commands

        TaskEditCommand = ReactiveCommand.Create(() =>
        {
            var windowService = App.Current?.Services?.GetService<IWindowService>();
            windowService.ShowTaskEditWindow(LoggedUser);
        });

        TeamEditCommand = ReactiveCommand.Create(() =>
        {
            var windowService = App.Current?.Services?.GetService<IWindowService>();
            windowService.ShowTeamEditWindow();
        });

        UserEditCommand = ReactiveCommand.Create(() =>
        {
            var windowService = App.Current?.Services?.GetService<IWindowService>();
            windowService.ShowUserEditWindow();
        });

        TeamRoleEditCommand = ReactiveCommand.Create(() =>
        {
            var windowService = App.Current?.Services?.GetService<IWindowService>();
            windowService.ShowTeamRoleEditWindow();
        });

        

        #endregion

    }



    private void UpdateUserLogged(UserModel user)
    {
        if(user == null) return;

        user.ReadSystemRole();
        Debug.WriteLine(user);

        if(string.IsNullOrEmpty(user.Name))
        {
            LoggedUserName = "Noname";
        }
        else
        {
            LoggedUserName = user.Name;
        }

        if (user.SystemRole == null)
        {
            EditUsersVisibility = false;
        }
        else
        {
            EditUsersVisibility = user.SystemRole.IsAdmin ||
                                user.SystemRole.CanEditUsers;
            EditTeamsVisibility = user.SystemRole.IsAdmin ||
                                user.SystemRole.CanEditTeams;
        }

        List<TaskModel> myTasks = SQLAccess.SelectUserTasks(user.Id).ToList();

        Debug.WriteLine($"TASK COUNT: {myTasks.Count}");

        foreach (TaskModel task in myTasks)
        {
            MyTaskTilesVM.Add(new TaskTileViewModel(task));
        }


    }
}
