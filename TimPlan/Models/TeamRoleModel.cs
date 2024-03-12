using System.ComponentModel.DataAnnotations.Schema;

namespace TimPlan.Models
{
    public class TeamRoleModel : DbModelBase
    {
        [Column(DbIdCol)]
        public override int Id { get; set; }

        [Column(DbNameCol)]
        public string Name { get; set; }

        [Column(DbCanViewForeignTeamTaskCol)]
        public bool CanViewForeignTeamTask { get; set; }

        [Column(DbCanAddForeignTeamTaskcol)]
        public bool CanAddForeignTeamTask { get; set; }

        [Column(DbCanEditForeignTeamTaskCol)]
        public bool CanEditForeignTeamTask { get; set; }

        [Column(DbCanViewTeamMemberTaskCol)]
        public bool CanViewTeamMemberTask { get; set; }

        [Column(DbCanAddTeamMemberTaskCol)]
        public bool CanAddTeamMemberTask { get; set; }

        [Column(DbCanEditTeamMemberTaskCol)]
        public bool CanEditTeamMemberTask { get; set; }


        public override string DbTableName => "team_roles";


        #region DbNames

        public const string DbNameCol = "name";
        public const string DbCanViewForeignTeamTaskCol = "view_foreign_team_task";
        public const string DbCanAddForeignTeamTaskcol = "add_foreign_team_task";
        public const string DbCanEditForeignTeamTaskCol = "edit_foreign_team_task";
        public const string DbCanViewTeamMemberTaskCol = "view_team_member_task";
        public const string DbCanAddTeamMemberTaskCol = "add_team_member_task";
        public const string DbCanEditTeamMemberTaskCol = "edit_team_member_task";

        #endregion

        public TeamRoleModel()
        {
            
        }
    }
}
