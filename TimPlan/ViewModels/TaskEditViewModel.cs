using DynamicData.Binding;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using TimPlan.Lib;
using TimPlan.Models;

namespace TimPlan.ViewModels
{
    public class TaskEditViewModel : ViewModelBase
    {
        #region Bound Properties

        private string _Name;
        public string Name
        {
            get { return _Name; }
            set { this.RaiseAndSetIfChanged(ref _Name, value); }
        }

        private DateTime _StartDate;
        public DateTime StartDate
        {
            get { return _StartDate; }
            set { this.RaiseAndSetIfChanged(ref _StartDate, value); }
        }

        private DateTime _EndDate;
        public DateTime EndDate
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

        public TaskEditViewModel(UserModel loggedUser)
        {
            _LoggedUser = loggedUser;

            this.WhenAnyValue(o => o.SelectedTeam)
                .Subscribe(UpdateUsers);

            IObservable<bool> createTaskCheck = this.WhenAnyValue
                (x => x.Name,
                x => x.SelectedUser)
                .Select(_ => CheckCreateTask());

            CreateTaskCommand = ReactiveCommand.Create(CreateTask, createTaskCheck);

            UpdateTeams();
            UpdateUsers(null);
            UpdateTasks();
        }

        private void UpdateTeams()
        {
            Teams = new ObservableCollection<TeamModel>(SQLAccess.SelectAllTeams());
        }

        private void UpdateUsers(TeamModel selectedTeam)
        {
            Users = new ObservableCollection<UserModel>(SQLAccess.SelectAllUsers());
        }

        private void UpdateTasks()
        {
            Tasks = new ObservableCollection<TaskModel>(SQLAccess.SelectAllTask());
        }

        private void CreateTask()
        {
            TaskModel newTask = new TaskModel();
            newTask.Name = Name;
            newTask.DateStart = StartDate;
            newTask.DateEnd = EndDate;
            newTask.Private = Private;
            if(SelectedParentTask != null)
            {
                newTask.ParentTaskID = SelectedParentTask.Id;
            }
            else
            {
                newTask.ParentTaskID = null;
            }
            if(SelectedTeam != null)
            {
                newTask.TeamId = SelectedTeam.Id;
            }
            else
            {
                newTask.TeamId = null;
            }
            newTask.UserId = SelectedUser.Id;
            newTask.Description = Description;
            
            newTask.CreatorUserId = _LoggedUser.Id;


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

    }
}
