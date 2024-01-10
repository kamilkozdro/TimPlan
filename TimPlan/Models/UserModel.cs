using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using TimPlan.Lib;

namespace TimPlan.Models
{
    public class UserModel : DbRecordBase<UserModel>
    {
        [Column(DbIdCol)]
        public uint Id { get; set; }
        [Column(DbNameCol)]
        public string? Name { get; set; }
        [Column(DbLoginCol)]
        public string? Login { get; set; }
        [Column(DbPasswordCol)]
        public string? Password {  get; set; }
        [Column(DbSystemRoleIdCol)]
        public uint SystemRoleId { get; set; }
        [Column(DbTeamId)]
        public uint? TeamId { get; set; }
        [Column(DbTeamRoleId)]
        public uint? TeamRoleId { get; set; }

        public SystemRoleModel? SystemRole { get; set; } = null;
        public TeamModel? Team { get; set; } = null;
        public TeamRoleModel? TeamRole { get; set; } = null;

        #region DbNames

        public const string DbTableName = "users";
        public const string DbIdCol = "id";
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
            SystemRole = SQLAccess.SelectSingle<SystemRoleModel>(SystemRoleModel.DbTableName, SystemRoleId);
        }

        public void ReadTeam()
        {
            if(TeamId !=  null)
            {
                Team = SQLAccess.SelectSingle<TeamModel>(TeamModel.DbTableName, (uint)TeamId);
            }
        }

        public void ReadTeamRole()
        {
            if (TeamRoleId != null)
            {
                TeamRole = SQLAccess.SelectSingle<TeamRoleModel>(TeamRoleModel.DbTableName, (uint)TeamRoleId);
            }
        }
            

        public override string ToString()
        {
            return $"Id:{Id}, Name:{Name}, Login:{Login}, SystemRoleID:{SystemRoleId}";
        }

    }
}
