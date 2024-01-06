using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimPlan.Interfaces;
using TimPlan.Lib;

namespace TimPlan.Models
{
    public class UserModel : IDbRecord
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

        public SystemRoleModel SystemRole { get; set; }

        #region DbNames

        public const string DbTableName = "users";
        public const string DbIdCol = "id";
        public const string DbNameCol = "name";
        public const string DbLoginCol = "login";
        public const string DbPasswordCol = "password";
        public const string DbSystemRoleIdCol = "system_role_id";

        #endregion

        public UserModel()
        {
            
        }

        public void ReadSystemRole()
        {
            SystemRole = SQLAccess.SelectSystemRole(SystemRoleId);
        }

        public override string ToString()
        {
            return $"Id:{Id}, Name:{Name}, Login:{Login}, SystemRoleID:{SystemRoleId}";
        }

    }
}
