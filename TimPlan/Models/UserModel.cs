using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimPlan.Models
{
    public class UserModel
    {
        public uint Id { get; set; }
        public string? Name { get; set; }
        public string? Login { get; set; }
        public string? Password {  get; set; }

        #region DbNames

        public const string DbName = "users";
        public const string DbIdCol = "id";
        public const string DbNameCol = "name";
        public const string DbLoginCol = "login";
        public const string DbPasswordCol = "password";

        #endregion

        public UserModel()
        {
            
        }

        public override string ToString()
        {
            return $"Id:{Id}, Name:{Name}, Login:{Login}";
        }

    }
}
