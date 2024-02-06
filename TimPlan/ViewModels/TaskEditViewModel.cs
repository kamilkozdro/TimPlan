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
    public class TaskEditViewModel : ModelEditViewModel<TaskModel>
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
        private ObservableCollection<TaskModel> _tasks;
        public ObservableCollection<TaskModel> Tasks
        {
            get { return _tasks; }
            set { this.RaiseAndSetIfChanged(ref _tasks, value); }
        }
        private ObservableCollection<TaskModel> _parentTasks;
        public ObservableCollection<TaskModel> ParentTasks
        {
            get { return _parentTasks; }
            set { this.RaiseAndSetIfChanged(ref _parentTasks, value); }
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

        private ReadOnlyCollection<UserModel> _loadedUsers;
        private ReadOnlyCollection<TeamModel> _loadedTeams;

        public TaskEditViewModel(UserModel loggedUser)
        {
            _LoggedUser = loggedUser;

            this.WhenAnyValue(o => o.SelectedTeam)
                .Subscribe(UpdateUsers);
            
            this.WhenAnyValue(o => o.Private)
                .Subscribe(o =>
                {
                    if(Private)
                    {
                        SelectedTeam = null;
                        SelectedUser = _LoggedUser;
                    }
                });

            _loadedTeams = new ReadOnlyCollection<TeamModel>(
                SQLAccess.SelectAll<TeamModel>());

            _loadedUsers = new ReadOnlyCollection<UserModel>(
                SQLAccess.SelectAll<UserModel>());
            
            UpdateTeams();
            UpdateParentTasks();

        }

        private void UpdateUsers(TeamModel selectedTeam)
        {
            if (selectedTeam == null)
                return;

            Users = new ObservableCollection<UserModel>(
                _loadedUsers.Where(user => user.TeamId == selectedTeam?.Id)
                            .ToList());
        }

        private void UpdateTeams()
        {
            Teams = new ObservableCollection<TeamModel>(
                _loadedTeams);
        }

        private void UpdateParentTasks()
        {
            ParentTasks = new ObservableCollection<TaskModel>(
                _loadedItems);
        }

        protected override void SetupCommandsCanExecute()
        {
            addItemCheck = this.WhenAnyValue
                (x => x.Name,
                x => x.SelectedUser)
                .Select(_ => AddItemCheck());

            editItemCheck = this.WhenAnyValue
                (x => x.Name,
                x => x.SelectedUser,
                x => x.SelectedItem)
                .Select(_ => EditItemCheck());

            deleteItemCheck = this.WhenAnyValue
                (x => x.SelectedItem)
                .Select(_ => DeleteItemCheck());
        }

        protected override TaskModel GetNewItemFromForm()
        {

            TaskModel newTask;

            if (SelectedItem != null)
                newTask = SelectedItem;
            else
                newTask = new TaskModel();

            newTask.Name = Name;
            newTask.DateCreated = DateTime.Now.Date;
            newTask.DateStart = StartDate?.DateTime.Date;
            newTask.DateEnd = EndDate.DateTime.Date;
            newTask.Private = Private;
            newTask.ParentTaskID = SelectedParentTask?.Id;
            newTask.TeamId = SelectedTeam?.Id;
            newTask.UserId = SelectedUser.Id;
            newTask.Description = Description;

            return newTask;
        }

        protected override void OnItemSelection(TaskModel selectedItem)
        {
            if (selectedItem == null)
            {
                ClearForm();
                return;
            }

            Name = SelectedItem.Name;
            StartDate = SelectedItem?.DateStart;
            EndDate = SelectedItem.DateEnd;
            Private = SelectedItem.Private;
            SelectedParentTask = _loadedItems.Where(task => task.Id == selectedItem.ParentTaskID)
                                             .SingleOrDefault();
            SelectedTeam = Teams.Where(team => team.Id == SelectedItem.TeamId)
                                .SingleOrDefault();
            SelectedUser = Users.Where(user => user.Id == SelectedItem.UserId)
                                .SingleOrDefault();
            Description = SelectedItem.Description;
        }

        protected override bool AddItemCheck()
        {
            return !string.IsNullOrEmpty(Name) &&
                    SelectedUser != null;
        }

        protected override bool EditItemCheck()
        {
            return !string.IsNullOrEmpty(Name) &&
                    SelectedUser != null &&
                    SelectedItem != null;
        }

        protected override bool DeleteItemCheck()
        {
            return SelectedItem != null;
        }

        protected override void ClearForm()
        {
            Name = string.Empty;
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
            Private = false;
            SelectedParentTask = null;
            SelectedTeam = null;
            SelectedUser = null;
            Description = string.Empty;
        }

        protected override void FilterItemList(string filterText)
        {
            if (string.IsNullOrEmpty(filterText))
            {
                Tasks = new ObservableCollection<TaskModel>(_loadedItems
                    .OrderByDescending(Task => Task.Name));
            }
            else
            {
                Tasks = new ObservableCollection<TaskModel>(_loadedItems
                                .Where(o => o.Name.Contains(filterText, StringComparison.OrdinalIgnoreCase))
                                .OrderByDescending(Task => Task.Name));
            }
        }
    }
}
