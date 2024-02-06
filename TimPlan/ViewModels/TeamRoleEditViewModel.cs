using Avalonia;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Xml.Linq;
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
        private bool _CanEditAllTasks;
        public bool CanEditAllTasks
        {
            get { return _CanEditAllTasks; }
            set { this.RaiseAndSetIfChanged(ref _CanEditAllTasks, value); }
        }
        private bool _CanEditCreatedTasks;
        public bool CanEditCreatedTasks
        {
            get { return _CanEditCreatedTasks; }
            set { this.RaiseAndSetIfChanged(ref _CanEditCreatedTasks, value); }
        }
        private bool _CanAssignTasks;
        public bool CanAssignTasks
        {
            get { return _CanAssignTasks; }
            set { this.RaiseAndSetIfChanged(ref _CanAssignTasks, value); }
        }
        private bool _CanViewForeignTasks;
        public bool CanViewForeignTasks
        {
            get { return _CanViewForeignTasks; }
            set { this.RaiseAndSetIfChanged(ref _CanViewForeignTasks, value); }
        }
        private bool _CanViewAllTasks;
        public bool CanViewAllTasks
        {
            get { return _CanViewAllTasks; }
            set { this.RaiseAndSetIfChanged(ref _CanViewAllTasks, value); }
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
                .Select(_ => CheckDeleteTeamRole());

            CreateTeamRoleCommand = ReactiveCommand.Create(CreateTeamRole, createTeamCheck);
            EditTeamRoleCommand = ReactiveCommand.Create(EditTeamRole, editTeamCheck);
            DeleteTeamRoleCommand = ReactiveCommand.Create(DeleteTeamRole, deleteTeamCheck);

            UpdateTeamRolesList();
        }

        private void UpdateTeamRolesList()
        {
            TeamRoles = new ObservableCollection<TeamRoleModel>(SQLAccess.SelectAll<TeamRoleModel>());
        }
        private void UpdateSelectedTeamRole(TeamRoleModel selectedTeamRole)
        {
            if (selectedTeamRole == null ||
                string.IsNullOrEmpty(selectedTeamRole.Name))
            {
                ResetProperties();
                return;
            }

            RoleName = selectedTeamRole.Name;
            CanEditAllTasks = selectedTeamRole.CanEditAllTasks;
            CanEditCreatedTasks = selectedTeamRole.CanEditCreatedTasks;
            CanAssignTasks = selectedTeamRole.CanAssignTasks;
            CanViewForeignTasks = selectedTeamRole.CanViewForeignTasks;
            CanViewAllTasks = selectedTeamRole.CanViewAllTasks;
        }
        private void CreateTeamRole()
        {
            TeamRoleModel checkTeamRoleName = SQLAccess.SelectTeamRoleByName(RoleName);

            if (checkTeamRoleName != null && SelectedTeamRole.Name != RoleName)
            {
                Debug.WriteLine($"Team Role with that name already exists");
                return;
            }

            TeamRoleModel newTeamRole = new TeamRoleModel();
            SetSelectedPropertiesToTeamRole(ref newTeamRole);

            SQLAccess.InsertSingle<TeamRoleModel>(newTeamRole);

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

            if (checkRoleName != null && SelectedTeamRole.Name != RoleName)
            {
                Debug.WriteLine($"Team with that name already exists");
                return;
            }

            TeamRoleModel editedTeamRole = SelectedTeamRole;
            SetSelectedPropertiesToTeamRole(ref editedTeamRole);

            SQLAccess.UpdateSingle<TeamRoleModel>(editedTeamRole);

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
            SQLAccess.DeleteSingle<TeamRoleModel>(SelectedTeamRole.Id);

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
        private void SetSelectedPropertiesToTeamRole(ref TeamRoleModel teamRole)
        {
            teamRole.Name = RoleName;
            teamRole.CanEditAllTasks = CanEditAllTasks;
            teamRole.CanEditCreatedTasks = CanEditCreatedTasks;
            teamRole.CanAssignTasks = CanAssignTasks;
            teamRole.CanViewForeignTasks = CanViewForeignTasks;
            teamRole.CanViewAllTasks = CanViewAllTasks;
        }
        private void ResetProperties()
        {
            RoleName = string.Empty;
            CanEditAllTasks = false;
            CanEditCreatedTasks = false;
            CanAssignTasks = false;
            CanViewForeignTasks = false;
            CanViewAllTasks = false;
        }

    }
}
