using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using TimPlan.Lib;
using TimPlan.Models;

namespace TimPlan.ViewModels
{
    public class TeamEditViewModel : ModelEditViewModel<TeamModel>
    {

        #region Properties

        private string _TeamName;
        public string TeamName
        {
            get { return _TeamName; }
            set { this.RaiseAndSetIfChanged(ref _TeamName, value); }
        }

        #endregion

        public TeamEditViewModel()
        {
            
        }

        protected override void OnItemSelection(TeamModel selectedItem)
        {
            if (selectedItem == null)
            {
                ClearForm();
                return;
            }

            TeamName = selectedItem.Name;
        }
        protected override bool AddItemCheck()
        {
            return !string.IsNullOrEmpty(TeamName);
        }

        protected override void AddItem()
        {
            TeamModel checkTeamName = SQLAccess.SelectTeamByName(TeamName);

            if (checkTeamName != null)
            {
                Debug.WriteLine($"Team with that name already exists");
                return;
            }

            base.AddItem();
        }

        protected override void ClearForm()
        {
            TeamName = string.Empty;
        }

        protected override bool DeleteItemCheck()
        {
            return SelectedItem != null;
        }

        protected override void EditItem()
        {
            TeamModel checkTeamName = SQLAccess.SelectTeamByName(TeamName);

            if (checkTeamName != null)
            {
                Debug.WriteLine($"Team with that name already exists");
                return;
            }

            base.EditItem();
        }

        protected override bool EditItemCheck()
        {
            return !string.IsNullOrEmpty(TeamName) &&
                    SelectedItem != null;
        }

        protected override void FilterItemList(string filterText)
        {
            if (string.IsNullOrEmpty(filterText))
            {
                Items = new ObservableCollection<TeamModel>(_loadedItems
                    .OrderByDescending(Item => Item.Name));
            }
            else
            {
                Items = new ObservableCollection<TeamModel>(_loadedItems
                                .Where(o => o.Name.Contains(filterText, StringComparison.OrdinalIgnoreCase))
                                .OrderByDescending(Item => Item.Name));
            }
        }

        protected override TeamModel GetNewItemFromForm()
        {
            TeamModel newTeam = new TeamModel();

            if (SelectedItem != null)
                newTeam = SelectedItem;
            newTeam.Name = TeamName;

            return newTeam;
        }

        protected override void SetupCommandsCanExecute()
        {
            addItemCheck = this.WhenAnyValue(
                x => x.TeamName)
                .Select(_ => AddItemCheck());

            editItemCheck = this.WhenAnyValue(
                x => x.TeamName,
                x => x.SelectedItem)
                .Select(_ => EditItemCheck());

            deleteItemCheck = this.WhenAnyValue(
                x => x.SelectedItem)
                .Select(_ => DeleteItemCheck());
        }
    }
}
