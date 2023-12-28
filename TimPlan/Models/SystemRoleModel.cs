using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimPlan.Models
{
    public class SystemRoleModel
    {
        [Column(DbIdCol)]
        public int Id { get; set; }
        [Column(DbNameCol)]
        public string Name { get; set; }
        [Column(DbIsAdminCol)]
        public bool IsAdmin { get; set; }
        [Column(DbCanEditUsersCol)]
        public bool CanEditUsers { get; set; }
        [Column(DbCanEditSystemRolesCol)]
        public bool CanEditSystemRoles { get; set; }

        #region DbNames

        public const string DbTableName = "system_roles";
        public const string DbIdCol = "id";
        public const string DbNameCol = "name";
        public const string DbIsAdminCol = "is_admin";
        public const string DbCanEditUsersCol = "can_edit_users";
        public const string DbCanEditSystemRolesCol = "can_edit_system_roles";

        #endregion
        public SystemRoleModel()
        {
            
        }

        public override string ToString()
        {
            return $"Id:{Id}, Name:{Name}, IsAdmin:{IsAdmin}, CanEditUsers:{CanEditUsers}," +
                $" CanEditUsers:{CanEditUsers}, CanEditSystemRoles:{CanEditSystemRoles}"; ;
        }
    }
}
