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


        public TaskEditViewModel(UserModel loggedUser)
        {
            _LoggedUser = loggedUser;

            this.WhenAnyValue(o => o.SelectedTeam)
                .Subscribe(UpdateUsers);
            
            this.WhenAnyValue(o => o.FormModel.Private)
                .Subscribe(o =>
                {
                    if(FormModel.Private)
                    {
                        SelectedTeam = null;
                        SelectedUser = _LoggedUser;
                    }
                });

        }

        private void UpdateUsers(TeamModel model)
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
            SelectedTeam = Teams.SingleOrDefault(team => team.Id == model.TeamId);
            SelectedParentTask = ParentTasks.SingleOrDefault(parentTask => parentTask.Id == model.ParentTaskID);
            SelectedUser = _loadedUsers.SingleOrDefault(user => user.Id == model.UserId);
        }

        protected override string AddModelCheck()
        {
            if (string.IsNullOrEmpty(FormModel.Name))
                return "Enter task name";
            if (FormModel.DateEnd == null)
                return "Set task end date";
            if (SelectedUser == null)
                return "Set user for task";

            return string.Empty;
        }

        protected override string EditModelCheck()
        {
            if (string.IsNullOrEmpty(FormModel.Name))
                return "Enter task name";
            if (FormModel.DateEnd == null)
                return "Set task end date";
            if (SelectedUser == null)
                return "Set user for task";

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
