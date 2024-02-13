using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using TimPlan.Lib;

namespace TimPlan.Models
{
    public class UserModel : DbModelBase
    {
        [Column(DbIdCol)]
        public override int Id { get; set; }
        [Column(DbNameCol)]
        public string? Name { get; set; }
        [Column(DbLoginCol)]
        public string? Login { get; set; }
        [Column(DbPasswordCol)]
        public string? Password {  get; set; }
        [Column(DbSystemRoleIdCol)]
        public int SystemRoleId { get; set; }
        [Column(DbTeamId)]
        public int? TeamId { get; set; }
        [Column(DbTeamRoleId)]
        public int? TeamRoleId { get; set; }

        public SystemRoleModel SystemRole { get; set; }
        public TeamModel? Team { get; set; } = null;
        public TeamRoleModel? TeamRole { get; set; } = null;

        public override string DbTableName => "users";

        #region DbNames

        public const string DbNameCol = "name";
        public const string DbLoginCol = "login";
        public const string DbPasswordCol = "password";
        public const string DbSystemRoleIdCol = "system_role_id";
        public const string DbTeamId = "team_id";
        public const string DbTeamRoleId = "team_role_id";

        #endregion

        public UserModel()
        {
            
        }

        public void ReadSystemRole()
        {
            SystemRole = SQLAccess.SelectSingle<SystemRoleModel>(SystemRoleId);
        }

        public void ReadTeam()
        {
            if(TeamId !=  null)
            {
                Team = SQLAccess.SelectSingle<TeamModel>((int)TeamId);
            }
        }

        public void ReadTeamRole()
        {
            if (TeamRoleId != null)
            {
                TeamRole = SQLAccess.SelectSingle<TeamRoleModel>((int)TeamRoleId);
            }
        }

    }
}
