using System.ComponentModel.DataAnnotations.Schema;

namespace TimPlan.Models
{
    public class SystemRoleModel : DbModelBase
    {
        [Column(DbIdCol)]
        public override int Id { get; set; }
        [Column(DbNameCol)]
        public string Name { get; set; }
        [Column(DbIsAdminCol)]
        public bool IsAdmin { get; set; }
        [Column(DbCanEditUsersCol)]
        public bool CanEditUsers { get; set; }
        [Column(DbCanEditSystemRolesCol)]
        public bool CanEditSystemRoles { get; set; }
        [Column(DbCanEditTeamsCol)]
        public bool CanEditTeams { get; set; }

        public override string DbTableName => "system_roles";

        public const string DbNameCol = "name";
        public const string DbIsAdminCol = "is_admin";
        public const string DbCanEditUsersCol = "can_edit_users";
        public const string DbCanEditSystemRolesCol = "can_edit_system_roles";
        public const string DbCanEditTeamsCol = "can_edit_teams";


        public SystemRoleModel()
        {
            
        }
    }
}
