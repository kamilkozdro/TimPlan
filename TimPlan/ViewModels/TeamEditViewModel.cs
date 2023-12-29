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
    public class TeamEditViewModel : ViewModelBase
    {

        #region Bound Properties

        private string _TeamName;
        public string TeamName
        {
            get { return _TeamName; }
            set { this.RaiseAndSetIfChanged(ref _TeamName, value); }
        }

        private ObservableCollection<TeamModel> _Teams;
        public ObservableCollection<TeamModel> Teams
        {
            get { return _Teams; }
            set { this.RaiseAndSetIfChanged(ref _Teams, value); }
        }

        private TeamModel _SelectedTeam;
        public TeamModel SelectedTeam
        {
            get { return _SelectedTeam; }
            set { this.RaiseAndSetIfChanged(ref _SelectedTeam, value); }
        }



        #endregion

        #region Commands

        public ReactiveCommand<Unit, Unit> CreateTeamCommand { get; }
        public ReactiveCommand<Unit, Unit> EditTeamCommand { get; }
        public ReactiveCommand<Unit, Unit> DeleteTeamCommand { get; }

        #endregion


        public TeamEditViewModel()
        {
            this.WhenAnyValue(o => o.SelectedTeam)
                .Subscribe(UpdateSelectedTeam);

            IObservable<bool> createTeamCheck = this.WhenAnyValue(
                x => x.TeamName)
                .Select(_ => CheckCreateTeam());

            IObservable<bool> editTeamCheck = this.WhenAnyValue(
                x => x.TeamName,
                x => x.SelectedTeam)
                .Select(_ => CheckEditTeam());

            IObservable<bool> deleteTeamCheck = this.WhenAnyValue(
                x => x.SelectedTeam)
                .Select(_ => CheckEditTeam());

            CreateTeamCommand = ReactiveCommand.Create(CreateTeam, createTeamCheck);
            EditTeamCommand = ReactiveCommand.Create(EditTeam, editTeamCheck);
            DeleteTeamCommand = ReactiveCommand.Create(DeleteTeam, deleteTeamCheck);

            UpdateTeamsList();
        }

        private void UpdateTeamsList()
        {
            Teams = new ObservableCollection<TeamModel>(SQLAccess.SelectAllTeams());
        }

        private void UpdateSelectedTeam(TeamModel team)
        {
            if(team == null ||
                string.IsNullOrEmpty(team.Name))
            {
                TeamName = string.Empty;
                return;
            }

            TeamName = team.Name;
        }

        private void CreateTeam()
        {
            TeamModel checkTeamName = SQLAccess.SelectTeamByName(TeamName);

            if (checkTeamName != null)
            {
                Debug.WriteLine($"Team with that name already exists");
                return;
            }

            TeamModel newTeam = new TeamModel();
            newTeam.Name = TeamName;

            SQLAccess.InsertTeam(newTeam);

            SelectedTeam = null;
            UpdateTeamsList();
        }

        private bool CheckCreateTeam()
        {
            if (string.IsNullOrEmpty(TeamName))
            {
                return false;
            }

            return true;
        }

        private void EditTeam()
        {
            TeamModel checkTeamName = SQLAccess.SelectTeamByName(TeamName);

            if (checkTeamName != null)
            {
                Debug.WriteLine($"Team with that name already exists");
                return;
            }

            TeamModel editedTeam = SelectedTeam;
            editedTeam.Name = TeamName;

            SQLAccess.UpdateTeam(editedTeam);

            SelectedTeam = null;
            UpdateTeamsList();
        }

        private bool CheckEditTeam()
        {
            if (SelectedTeam == null ||
                string.IsNullOrEmpty(TeamName))
            {
                return false;
            }

            return true;
        }

        private void DeleteTeam()
        {
            SQLAccess.DeleteTeam(SelectedTeam.Id);

            UpdateTeamsList();
            SelectedTeam = null;
        }

        private bool CheckDeleteTeam()
        {
            if (SelectedTeam == null)
            {
                return false;
            }

            return true;
        }

    }
}
