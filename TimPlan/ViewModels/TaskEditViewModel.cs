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
    public class TaskEditViewModel : ModelEditBase<TaskModel>
    {

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

        private ObservableCollection<TaskModel> _parentTasks;
        public ObservableCollection<TaskModel> ParentTasks
        {
            get { return _parentTasks; }
            set { this.RaiseAndSetIfChanged(ref _parentTasks, value); }
        }

        private TeamModel? _SelectedTeam;
        public TeamModel? SelectedTeam
        {
            get { return _SelectedTeam; }
            set { this.RaiseAndSetIfChanged(ref _SelectedTeam, value); }
        }

        private UserModel? _SelectedUser;
        public UserModel? SelectedUser
        {
            get { return _SelectedUser; }
            set { this.RaiseAndSetIfChanged(ref _SelectedUser, value); }
        }

        private TaskModel? _SelectedParentTask;
        public TaskModel? SelectedParentTask
        {
            get { return _SelectedParentTask; }
            set { this.RaiseAndSetIfChanged(ref _SelectedParentTask, value); }
        }

        private DateTime _dateStart;
        public DateTime DateStart
        {
            get { return _dateStart; }
            set { this.RaiseAndSetIfChanged(ref _dateStart, value); }
        }

        private DateTime _dateEnd;
        public DateTime DateEnd
        {
            get { return _dateEnd; }
            set { this.RaiseAndSetIfChanged(ref _dateEnd, value); }
        }

        private DateTime _dateCreated;
        public DateTime DateCreated
        {
            get { return _dateCreated; }
            set { this.RaiseAndSetIfChanged(ref _dateCreated, value); }
        }


        private bool _canSelectTeam;
        public bool CanSelectTeam
        {
            get { return _canSelectTeam; }
            set { this.RaiseAndSetIfChanged(ref _canSelectTeam, value); }
        }

        private bool _canSelectUser;
        public bool CanSelectUser
        {
            get { return _canSelectUser; }
            set { this.RaiseAndSetIfChanged(ref _canSelectUser, value); }
        }


        private UserModel _loggedUser;

        private ReadOnlyCollection<UserModel> _loadedUsers;


        public TaskEditViewModel()
        {
            _loggedUser = LoggedUserManager.GetUser();

            FormModel.CreatorUserId = _loggedUser.Id;
            DateStart = DateTime.Now;
            DateEnd = DateTime.Now;
            DateCreated = DateTime.Now;

            SetupPrivileges(_loggedUser);

            this.WhenAnyValue(o => o.SelectedTeam)
                .Subscribe(UpdateUsers);

        }

        private void SetupPrivileges(UserModel loggedUser)
        {
            CanSelectTeam = loggedUser.SystemRole.IsAdmin ||
                            loggedUser.TeamRole.CanAddForeignTeamTask;
            CanSelectUser = loggedUser.SystemRole.IsAdmin ||
                            loggedUser.TeamRole.CanAddForeignTeamTask ||
                            loggedUser.TeamRole.CanAddTeamMemberTask;
        }

        private void UpdateUsers(TeamModel? model)
        {
            if (model == null)
                Users = null;
            else
                Users = new ObservableCollection<UserModel>(
                    _loadedUsers.Where(user => user?.TeamId == model.Id));
        }

        protected override void LoadSources()
        {
            Teams = new ObservableCollection<TeamModel>(
                SQLAccess.SelectAll<TeamModel>().ToList());

            _loadedUsers = new ReadOnlyCollection<UserModel>(
                SQLAccess.SelectAll<UserModel>().ToList());

            ParentTasks = new ObservableCollection<TaskModel>(
                SQLAccess.SelectAll<TaskModel>().ToList());
        }

        protected override void SetFormFromModel(TaskModel model)
        {
            if (model == null)
                ClearForm();
            
            FormModel.DateStart = model.DateStart;
            FormModel.DateEnd = model.DateEnd;
            FormModel.DateCreated = model.DateCreated;

            SelectedTeam = Teams.SingleOrDefault(team => team.Id == model.TeamId);
            SelectedParentTask = ParentTasks.SingleOrDefault(parentTask => parentTask.Id == model.ParentTaskID);
            SelectedUser = _loadedUsers.SingleOrDefault(user => user.Id == model.UserId);
        }

        protected override TaskModel GetModelFromForm()
        {

            FormModel.DateStart = DateStart;
            FormModel.DateEnd = DateEnd;

            FormModel.DateCreated = DateTime.Now.Date;

            FormModel.UserId = SelectedUser.Id;
            FormModel.ParentTaskID = SelectedParentTask?.Id;
            FormModel.TeamId = SelectedTeam?.Id;

            return base.GetModelFromForm();
        }

        protected override string AddModelCheck()
        {
            if (string.IsNullOrEmpty(FormModel.Name))
                return "Enter task name";
            if (SelectedUser == null)
                return "Set user for task";
            if( DateEnd < DateStart)
                return "Start date must be set earlier than end date";

            return string.Empty;
        }

        protected override string EditModelCheck()
        {
            if (string.IsNullOrEmpty(FormModel.Name))
                return "Enter task name";
            if (SelectedUser == null)
                return "Set user for task";
            if (DateEnd < DateStart)
                return "Start date must be set earlier than end date";

            return string.Empty;
        }

        protected override string DeleteModelCheck()
        {
            throw new NotImplementedException();
        }

        protected override void ClearForm()
        {
            SelectedUser = null;
            SelectedTeam = null;
            SelectedParentTask = null;
        }
    }
}
