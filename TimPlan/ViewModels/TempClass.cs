using DynamicData.Binding;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using TimPlan.Lib;
using TimPlan.Models;

namespace TimPlan.ViewModels
{
    public class TampClass : ViewModelBase
    {
        #region Bound Properties

        private string _Name;
        public string Name
        {
            get { return _Name; }
            set { this.RaiseAndSetIfChanged(ref _Name, value); }
        }

        private DateTimeOffset? _StartDate;
        public DateTimeOffset? StartDate
        {
            get { return _StartDate; }
            set { this.RaiseAndSetIfChanged(ref _StartDate, value); }
        }

        private DateTimeOffset _EndDate;
        public DateTimeOffset EndDate
        {
            get { return _EndDate; }
            set { this.RaiseAndSetIfChanged(ref _EndDate, value); }
        }

        private bool _Private;
        public bool Private
        {
            get { return _Private; }
            set { this.RaiseAndSetIfChanged(ref _Private, value); }
        }

        private string _Description;
        public string Description
        {
            get { return _Description; }
            set { this.RaiseAndSetIfChanged(ref _Description, value); }
        }

        #endregion

        private ObservableCollection<TeamModel> _Teams;
        public ObservableCollection<TeamModel> Teams
        {
            get { return _Teams; }
            set { this.RaiseAndSetIfChanged(ref _Teams, value); }
        }

        private ObservableCollection<UserModel> _Users;
        public ObservableCollection<UserModel> Users
        {
            get { return _Users; }
            set { this.RaiseAndSetIfChanged(ref _Users, value); }
        }

        private ObservableCollection<TaskModel> _Tasks;
        public ObservableCollection<TaskModel> Tasks
        {
            get { return _Tasks; }
            set { this.RaiseAndSetIfChanged(ref _Tasks, value); }
        }

        private TeamModel _SelectedTeam;
        public TeamModel SelectedTeam
        {
            get { return _SelectedTeam; }
            set { this.RaiseAndSetIfChanged(ref _SelectedTeam, value); }
        }

        private UserModel _SelectedUser;
        public UserModel SelectedUser
        {
            get { return _SelectedUser; }
            set { this.RaiseAndSetIfChanged(ref _SelectedUser, value); }
        }

        private TaskModel _SelectedParentTask;
        public TaskModel SelectedParentTask
        {
            get { return _SelectedParentTask; }
            set { this.RaiseAndSetIfChanged(ref _SelectedParentTask, value); }
        }

        private UserModel _LoggedUser;

        #region Commands

        public ReactiveCommand<Unit, Unit> CreateTaskCommand { get; }

        #endregion

        public TampClass(UserModel loggedUser)
        {
            _LoggedUser = loggedUser;

            this.WhenAnyValue(o => o.SelectedTeam)
                .Subscribe(UpdateUsers);

            this.WhenAnyValue(o => o.Private)
                .Subscribe(o =>
                {
                    if (Private)
                    {
                        SelectedTeam = null;
                        SelectedUser = _LoggedUser;
                    }
                });

            IObservable<bool> createTaskCheck = this.WhenAnyValue
                (x => x.Name,
                x => x.SelectedUser)
                .Select(_ => CheckCreateTask());

            CreateTaskCommand = ReactiveCommand.Create(CreateTask, createTaskCheck);

            UpdateTeams();
            UpdateUsers(null);

            ResetProperties();

        }

        private void UpdateTeams()
        {
            Teams = new ObservableCollection<TeamModel>(SQLAccess.SelectAll<TeamModel>());
        }

        private void UpdateUsers(TeamModel selectedTeam)
        {
            Users = new ObservableCollection<UserModel>(SQLAccess.SelectAll<UserModel>());
        }

        private void UpdateTasks()
        {
            Tasks = new ObservableCollection<TaskModel>(SQLAccess.SelectAllTasksWithoutForeignPrivate(_LoggedUser.Id));
        }

        private void CreateTask()
        {
            TaskModel newTask = new TaskModel();
            newTask.Name = Name;
            newTask.DateCreated = DateTime.Now.Date;
            newTask.DateStart = StartDate?.DateTime.Date;
            newTask.DateEnd = EndDate.DateTime.Date;
            newTask.Private = Private;
            newTask.ParentTaskID = SelectedParentTask?.Id;
            newTask.TeamId = SelectedTeam?.Id;
            newTask.UserId = SelectedUser.Id;
            newTask.Description = Description;

            newTask.CreatorUserId = _LoggedUser.Id;

            if (SQLAccess.InsertSingle<TaskModel>(newTask))
            {
                ResetProperties();
            }
            else
            {
                Debug.WriteLine("Creating task failed. Could not insert task into database.");
            }

        }

        private bool CheckCreateTask()
        {
            if (string.IsNullOrEmpty(Name) ||
                SelectedUser == null)
            {
                return false;
            }

            return true;
        }

        private void ResetProperties()
        {
            Name = string.Empty;
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
            Private = false;
            SelectedParentTask = null;
            SelectedTeam = null;
            SelectedUser = _LoggedUser;
            Description = string.Empty;

            UpdateTasks();
        }
    }
}
