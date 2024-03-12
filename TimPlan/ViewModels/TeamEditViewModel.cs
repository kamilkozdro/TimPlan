using DynamicData.Binding;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using TimPlan.Lib;
using TimPlan.Models;

namespace TimPlan.ViewModels
{
    public class TeamEditViewModel : ModelSearchEditBase<TeamModel>
    {

        #region Properties

        #endregion

        public TeamEditViewModel()
        {
            
        }

        protected override string AddModelCheck()
        {
            if (string.IsNullOrEmpty(FormModel.Name))
                return "Enter team name";

            TeamModel checkTeamName = SQLAccess.SelectTeamByName(FormModel.Name);
            if (checkTeamName != null)
                return "Team with that name already exists";

            return string.Empty;
        }

        protected override void ClearForm()
        {
            FormModel = new TeamModel();
        }

        protected override string DeleteModelCheck()
        {
            return string.Empty;
        }

        protected override string EditModelCheck()
        {
            if (string.IsNullOrEmpty(FormModel.Name))
                return "Enter team name";

            TeamModel checkTeamName = SQLAccess.SelectTeamByName(FormModel.Name);
            if (checkTeamName != null && checkTeamName.Id != EditedModel.Id)
                return "Team with that name already exists";

            return string.Empty;
        }

        protected override void LoadSources()
        {
            
        }

        protected override Func<TeamModel, bool> SearchModelFilter(string text) => team =>
        {
            return string.IsNullOrEmpty(text) || team.Name.Contains(text);
        };

        protected override IComparer<TeamModel> SearchModelSort()
        {
            return SortExpressionComparer<TeamModel>.Ascending(team => team.Name);
        }

        protected override void SetFormFromModel(TeamModel model)
        {
            
        }
    }
}
