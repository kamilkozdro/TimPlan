﻿using ReactiveUI;
using System.Collections.ObjectModel;
using TimPlan.Models;
using System;
using System.Diagnostics;
using System.Reactive;
using System.Windows.Input;
using System.Reactive.Linq;
using TimPlan.Services;

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


    #endregion

    private string _LoggedUserName;
    public string LoggedUserName
    {
        get { return _LoggedUserName; }
        set { this.RaiseAndSetIfChanged(ref _LoggedUserName, value); }
    }


    public ObservableCollection<TreeViewNode> TreeViewNodes { get; set; }
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

    #endregion

    private IWindowService _WindowService = new WindowService();

    public MainViewModel()
    {
        

        this.WhenAnyValue(o => o.LoggedUser)
            .Subscribe(UpdateUserLogged);

        #region Set Commands

        TaskEditCommand = ReactiveCommand.Create(() =>
        {
            _WindowService.OpenTaskEditWindow(LoggedUser);
        });

        TeamEditCommand = ReactiveCommand.Create(() =>
        {
            _WindowService.OpenTeamEditWindow();
        });

        UserEditCommand = ReactiveCommand.Create(() =>
        {
            _WindowService.OpenUserEditWindow();
        });




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
