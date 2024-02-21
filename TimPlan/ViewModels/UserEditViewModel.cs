using DynamicData.Binding;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using TimPlan.Lib;
using TimPlan.Models;

namespace TimPlan.ViewModels
{
    public class UserEditViewModel : ModelSearchEditBase<UserModel>
    {
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

        #region Commands


        #endregion

        public UserEditViewModel()
        {
            SystemRoles = new ObservableCollection<SystemRoleModel>(SQLAccess.SelectAll<SystemRoleModel>());
            Teams = new ObservableCollection<TeamModel>(SQLAccess.SelectAll<TeamModel>());
            TeamRoles = new ObservableCollection<TeamRoleModel>(SQLAccess.SelectAll<TeamRoleModel>());
        }

        protected override Func<UserModel, bool> SearchModelFilter(string text) => user =>
        {
            return string.IsNullOrEmpty(text) || user.Name.Contains(text);
        };

        protected override IComparer<UserModel> SearchModelSort()
        {
            return SortExpressionComparer<UserModel>.Ascending(user => user.Name);
        }

        protected override void LoadSources()
        {
            SystemRoles = new ObservableCollection<SystemRoleModel>(SQLAccess.SelectAll<SystemRoleModel>());
            Teams = new ObservableCollection<TeamModel>(SQLAccess.SelectAll<TeamModel>());
            TeamRoles = new ObservableCollection<TeamRoleModel>(SQLAccess.SelectAll<TeamRoleModel>());
        }

        protected override void SetFormFromModel(UserModel model)
        {
            SelectedSystemRole = SystemRoles.Single(role => role.Id == model.SystemRoleId);
            SelectedTeam = Teams.SingleOrDefault(team => team.Id == model.TeamId);
            SelectedTeamRole = TeamRoles.SingleOrDefault(teamRole => teamRole.Id == model.TeamRoleId);
        }

        protected override string AddModelCheck()
        {
            if (string.IsNullOrEmpty(FormModel.Name))
                return "Enter user name";
            if (string.IsNullOrEmpty(FormModel.Login))
                return "Enter user login";
            if (string.IsNullOrEmpty(FormModel.Password))
                return "Enter user password";

            if (SelectedSystemRole == null)
                return "Select user system role";

            UserModel checkUserLogin = SQLAccess.SelectUserByLogin(FormModel.Login);
            if (checkUserLogin != null)
                return "User with that login already exists";

            return string.Empty;
        }

        protected override string EditModelCheck()
        {
            if (string.IsNullOrEmpty(FormModel.Name))
                return "Enter user name";
            if (string.IsNullOrEmpty(FormModel.Login))
                return "Enter user login";
            if (string.IsNullOrEmpty(FormModel.Password))
                return "Enter user password";

            if (SelectedSystemRole == null)
                return "Select user system role";

            UserModel checkUserLogin = SQLAccess.SelectUserByLogin(FormModel.Login);
            if (checkUserLogin != null && checkUserLogin.Id != EditedModel.Id)
                return "User with that login already exists";

            return string.Empty;
        }

        protected override string DeleteModelCheck()
        {
            throw new NotImplementedException();
        }

        protected override void ClearForm()
        {
            SelectedTeam = null;
            SelectedTeamRole = null;
            SelectedSystemRole = null;
        }

        protected override UserModel GetModelFromForm()
        {
            FormModel.TeamId = SelectedTeam?.Id;
            FormModel.TeamRoleId = SelectedTeamRole?.Id;

            return base.GetModelFromForm();
        }
    }
}
