using Avalonia;
using DynamicData.Binding;
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
    public class TeamRoleEditViewModel : ModelSearchEditBase<TeamRoleModel>
    {

        #region Properties

        #endregion

        #region Commands

        #endregion


        public TeamRoleEditViewModel()
        {

        }

        protected override Func<TeamRoleModel, bool> SearchModelFilter(string text) => teamRole =>
        {
            return string.IsNullOrEmpty(text) || teamRole.Name.Contains(text);
        };

        protected override IComparer<TeamRoleModel> SearchModelSort()
        {
            return SortExpressionComparer<TeamRoleModel>.Ascending(teamRole => teamRole.Name);
        }

        protected override void LoadSources()
        {

        }

        protected override void SetFormFromModel(TeamRoleModel model)
        {
            
        }

        protected override string AddModelCheck()
        {
            if (string.IsNullOrEmpty(FormModel.Name))
                return "Enter team role name";

            TeamRoleModel checkTeamRoleName = SQLAccess.SelectTeamRoleByName(FormModel.Name);
            if (checkTeamRoleName != null)
                return "Team role with that name already exists";

            return string.Empty;
        }

        protected override string EditModelCheck()
        {
            if(string.IsNullOrEmpty(FormModel.Name))
                return "Enter team role name";

            TeamRoleModel checkTeamRoleName = SQLAccess.SelectTeamRoleByName(FormModel.Name);
            if (checkTeamRoleName != null && checkTeamRoleName.Id != EditedModel.Id)
                return "Team role with that name already exists";

            return string.Empty;
        }

        protected override string DeleteModelCheck()
        {
            throw new NotImplementedException();
        }

        protected override void ClearForm()
        {

        }
    }
}
