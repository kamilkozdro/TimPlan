using System.ComponentModel.DataAnnotations.Schema;

namespace TimPlan.Models
{
    public class TeamRoleModel : DbModelBase
    {
        [Column(DbIdCol)]
        public override int Id { get; set; }

        [Column(DbNameCol)]
        public string Name { get; set; }

        [Column(DbCanViewForeignTeamTask)]
        public bool CanViewForeignTeamTask { get; set; }

        [Column(DbCanAddForeignTeamTask)]
        public bool CanAddForeignTeamTask { get; set; }

        [Column(DbCanEditForeignTeamTask)]
        public bool CanEditForeignTeamTask { get; set; }

        [Column(DbCanViewTeamMemberTask)]
        public bool CanViewTeamMemberTask { get; set; }

        [Column(DbCanAddTeamMemberTask)]
        public bool CanAddTeamMemberTask { get; set; }

        [Column(DbCanEditTeamMemberTask)]
        public bool CanEditTeamMemberTask { get; set; }


        public override string DbTableName => "team_roles";


        #region DbNames

        public const string DbNameCol = "name";
        public const string DbCanViewForeignTeamTask = "view_foreign_team_task";
        public const string DbCanAddForeignTeamTask = "add_foreign_team_task";
        public const string DbCanEditForeignTeamTask = "edit_foreign_team_task";
        public const string DbCanViewTeamMemberTask = "view_team_member_task";
        public const string DbCanAddTeamMemberTask = "add_team_member_task";
        public const string DbCanEditTeamMemberTask = "edit_team_member_task";

        #endregion

        public TeamRoleModel()
        {
            
        }
    }
}
