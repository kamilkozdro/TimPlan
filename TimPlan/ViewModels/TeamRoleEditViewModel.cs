using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using TimPlan.Lib;
using TimPlan.Models;

namespace TimPlan.ViewModels
{
    public class TeamRoleEditViewModel : ViewModelBase
    {

        #region Properties

        private string _RoleName;
        public string RoleName
        {
            get { return _RoleName; }
            set { this.RaiseAndSetIfChanged(ref _RoleName, value); }
        }

        private ObservableCollection<TeamRoleModel> _TeamRoles;
        public ObservableCollection<TeamRoleModel> TeamRoles
        {
            get { return _TeamRoles; }
            set { this.RaiseAndSetIfChanged(ref _TeamRoles, value); }
        }

        private TeamRoleModel _SelectedTeamRole;
        public TeamRoleModel SelectedTeamRole
        {
            get { return _SelectedTeamRole; }
            set { this.RaiseAndSetIfChanged(ref _SelectedTeamRole, value); }
        }



        #endregion

        #region Commands

        public ReactiveCommand<Unit, Unit> CreateTeamRoleCommand { get; }
        public ReactiveCommand<Unit, Unit> EditTeamRoleCommand { get; }
        public ReactiveCommand<Unit, Unit> DeleteTeamRoleCommand { get; }

        #endregion


        public TeamRoleEditViewModel()
        {
            this.WhenAnyValue(o => o.SelectedTeamRole)
                .Subscribe(UpdateSelectedTeamRole);

            IObservable<bool> createTeamCheck = this.WhenAnyValue(
                x => x.RoleName)
                .Select(_ => CheckCreateTeamRole());

            IObservable<bool> editTeamCheck = this.WhenAnyValue(
                x => x.RoleName,
                x => x.SelectedTeamRole)
                .Select(_ => CheckEditTeamRole());

            IObservable<bool> deleteTeamCheck = this.WhenAnyValue(
                x => x.SelectedTeamRole)
                .Select(_ => CheckEditTeamRole());

            CreateTeamRoleCommand = ReactiveCommand.Create(CreateTeamRole, createTeamCheck);
            EditTeamRoleCommand = ReactiveCommand.Create(EditTeamRole, editTeamCheck);
            DeleteTeamRoleCommand = ReactiveCommand.Create(DeleteTeamRole, deleteTeamCheck);

            UpdateTeamRolesList();
        }

        private void UpdateTeamRolesList()
        {
            TeamRoles = new ObservableCollection<TeamRoleModel>(SQLAccess.SelectAll<TeamRoleModel>(TeamRoleModel.DbTableName));
        }

        private void UpdateSelectedTeamRole(TeamRoleModel selectedTeamRole)
        {
            if (selectedTeamRole == null ||
                string.IsNullOrEmpty(selectedTeamRole.Name))
            {
                RoleName = string.Empty;
                return;
            }

            RoleName = selectedTeamRole.Name;
        }
        private void CreateTeamRole()
        {
            TeamRoleModel checkTeamRoleName = SQLAccess.SelectTeamRoleByName(RoleName);

            if (checkTeamRoleName != null)
            {
                Debug.WriteLine($"Team Role with that name already exists");
                return;
            }

            TeamRoleModel newTeamRole = new TeamRoleModel();
            newTeamRole.Name = RoleName;

            SQLAccess.InsertTeamRole(newTeamRole);

            SelectedTeamRole = null;
            UpdateTeamRolesList();
        }

        private bool CheckCreateTeamRole()
        {
            if (string.IsNullOrEmpty(RoleName))
            {
                return false;
            }

            return true;
        }

        private void EditTeamRole()
        {
            TeamRoleModel checkRoleName = SQLAccess.SelectTeamRoleByName(RoleName);

            if (checkRoleName != null)
            {
                Debug.WriteLine($"Team with that name already exists");
                return;
            }

            TeamRoleModel editedTeamRole = SelectedTeamRole;
            editedTeamRole.Name = RoleName;

            SQLAccess.UpdateTeamRole(editedTeamRole);

            SelectedTeamRole = null;
            UpdateTeamRolesList();
        }

        private bool CheckEditTeamRole()
        {
            if (SelectedTeamRole == null ||
                string.IsNullOrEmpty(RoleName))
            {
                return false;
            }

            return true;
        }

        private void DeleteTeamRole()
        {
            SQLAccess.DeleteTeamRole(SelectedTeamRole.Id);

            UpdateTeamRolesList();
            SelectedTeamRole = null;
        }

        private bool CheckDeleteTeamRole()
        {
            if (SelectedTeamRole == null)
            {
                return false;
            }

            return true;
        }



    }
}
