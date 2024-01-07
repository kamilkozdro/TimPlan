using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using TimPlan.Lib;
using TimPlan.Models;

namespace TimPlan.ViewModels
{
    public class UserEditViewModel : ViewModelBase
    {

        private ObservableCollection<UserModel> _Users;
        public ObservableCollection<UserModel> Users
        {
            get { return _Users; }
            set { this.RaiseAndSetIfChanged(ref _Users, value); }
        }
        private ObservableCollection<TeamModel> _Teams;
        public ObservableCollection<TeamModel> Teams
        {
            get { return _Teams; }
            set { this.RaiseAndSetIfChanged(ref _Teams, value); }
        }
        private ObservableCollection<TeamRoleModel> _TeamRoles;
        public ObservableCollection<TeamRoleModel> TeamRoles
        {
            get { return _TeamRoles; }
            set { this.RaiseAndSetIfChanged(ref _TeamRoles, value); }
        }
        private ObservableCollection<SystemRoleModel> _SystemRoles;
        public ObservableCollection<SystemRoleModel> SystemRoles
        {
            get { return _SystemRoles; }
            set { this.RaiseAndSetIfChanged(ref _SystemRoles, value); }
        }
        private UserModel _SelectedUser;
        public UserModel SelectedUser
        {
            get { return _SelectedUser; }
            set { this.RaiseAndSetIfChanged(ref _SelectedUser, value); }
        }
        private TeamModel _SelectedTeam;
        public TeamModel SelectedTeam
        {
            get { return _SelectedTeam; }
            set { this.RaiseAndSetIfChanged(ref _SelectedTeam, value); }
        }
        private TeamRoleModel _SelectedTeamRole;
        public TeamRoleModel SelectedTeamRole
        {
            get { return _SelectedTeamRole; }
            set { this.RaiseAndSetIfChanged(ref _SelectedTeamRole, value); }
        }
        private SystemRoleModel _SelectedSystemRole;
        public SystemRoleModel SelectedSystemRole
        {
            get { return _SelectedSystemRole; }
            set { this.RaiseAndSetIfChanged(ref _SelectedSystemRole, value); }
        }


        #region Bound Properties

        private string _Username;

        public string Username
        {
            get { return _Username; }
            set { this.RaiseAndSetIfChanged(ref _Username, value); }
        }

        private string _Login;

        public string Login
        {
            get { return _Login; }
            set { this.RaiseAndSetIfChanged(ref _Login, value); }
        }

        private string _Password;
        public string Password
        {
            get { return _Password; }
            set { this.RaiseAndSetIfChanged(ref _Password, value); }
        }


        #endregion


        #region Commands

        public ReactiveCommand<Unit, Unit> AddUserCommand { get; }
        public ReactiveCommand<Unit, Unit> EditUserCommand { get; }
        public ReactiveCommand<Unit, Unit> DeleteUserCommand { get; }

        #endregion

        public UserEditViewModel()
        {
            this.WhenAnyValue(o => o.SelectedUser)
                .Subscribe(UpdateSelectedUser);

            IObservable<bool> addUserCheck = this.WhenAnyValue(
                x => x.Username,
                x => x.Login,
                x => x.Password,
                x =>x.SelectedSystemRole)
                .Select(_ => CheckAddUser());

            IObservable<bool> editUserCheck = this.WhenAnyValue(
                x => x.Username,
                x => x.Login,
                x => x.Password,
                x => x.SelectedUser,
                x => x.SelectedSystemRole)
                .Select(_ => CheckEditUser());

            IObservable<bool> deleteUserCheck = this.WhenAnyValue(
                x => x.SelectedUser)
                .Select(_ => CheckDeleteUser());

            AddUserCommand = ReactiveCommand.Create(AddUser, addUserCheck);
            EditUserCommand = ReactiveCommand.Create(EditUser, editUserCheck);
            DeleteUserCommand = ReactiveCommand.Create(DeleteUser, deleteUserCheck);

            UpdateUsersList();
            SystemRoles = new ObservableCollection<SystemRoleModel>(SQLAccess.SelectAll<SystemRoleModel>(SystemRoleModel.DbTableName));
            Teams = new ObservableCollection<TeamModel>(SQLAccess.SelectAll<TeamModel>(TeamModel.DbTableName));
            TeamRoles = new ObservableCollection<TeamRoleModel>(SQLAccess.SelectAll<TeamRoleModel>(TeamRoleModel.DbTableName));
        }

        private void UpdateUsersList()
        {
            Users = new ObservableCollection<UserModel>(SQLAccess.SelectAll<UserModel>(UserModel.DbTableName));
        }

        private void UpdateSelectedUser(UserModel user)
        {
            if (user == null ||
                string.IsNullOrEmpty(user.Name) ||
                string.IsNullOrEmpty(user.Login))
            {
                Username = string.Empty;
                Login = string.Empty;
                Password = string.Empty;
                SelectedSystemRole = null;
                SelectedTeam = null;
                SelectedTeamRole = null;
                return;
            }

            Username = user.Name;
            Login = user.Login;
            SelectedSystemRole = SystemRoles.Single(role => role.Id == user.SystemRoleId);
            SelectedTeam = Teams.SingleOrDefault(team => team.Id == user.TeamId);
            SelectedTeamRole = TeamRoles.SingleOrDefault(teamRole => teamRole.Id == user.TeamRoleId);

        }

        private void AddUser()
        {

            UserModel checkUserLogin = SQLAccess.SelectUserByLogin(Login);

            if(checkUserLogin != null)
            {
                Debug.WriteLine($"User with that login already exists");
                return;
            }

            UserModel newUser = new UserModel();
            newUser.Name = Username;
            newUser.Login = Login;
            newUser.Password = Password;
            newUser.SystemRoleId = SelectedSystemRole.Id;
            newUser.TeamId = SelectedTeam?.Id;
            newUser.TeamRoleId = SelectedTeamRole?.Id;

            SQLAccess.InsertUser(newUser);

            SelectedUser = null;
            UpdateUsersList();
        }

        private bool CheckAddUser()
        {
            if (string.IsNullOrEmpty(Username)
                || string.IsNullOrEmpty(Login)
                || string.IsNullOrEmpty(Password)
                || SelectedSystemRole == null)
            {
                return false;
            }

            return true;
        }

        private void EditUser()
        {
            UserModel checkUserLogin = SQLAccess.SelectUserByLogin(Login);

            if (checkUserLogin != null && SelectedUser.Login != Login)
            {
                Debug.WriteLine($"User with that login already exists");
                return;
            }

            UserModel editedUser = SelectedUser;
            editedUser.Name = Username;
            editedUser.Login = Login;
            editedUser.Password = Password;
            editedUser.SystemRoleId = SelectedSystemRole.Id;
            editedUser.TeamId = SelectedTeam?.Id;
            editedUser.TeamRoleId = SelectedTeamRole?.Id;

            SQLAccess.UpdateUser(editedUser);

            SelectedUser = null;
            UpdateUsersList();
        }

        private bool CheckEditUser()
        {
            if (string.IsNullOrEmpty(Username)
                || string.IsNullOrEmpty(Login)
                || string.IsNullOrEmpty(Password)
                || SelectedUser == null
                || SelectedSystemRole == null)
            {
                return false;
            }

            return true;
        }

        private void DeleteUser()
        {
            SQLAccess.DeleteUser(SelectedUser.Id);

            UpdateUsersList();
            SelectedUser = null;
        }

        private bool CheckDeleteUser()
        {
            if (SelectedUser == null)
            {
                return false;
            }

            return true;
        }

    }
}
