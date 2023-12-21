using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimPlan.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Login { get; set; }
        public string? Password { get; set; }
        public string? UserName { get; set; }
        public int TeamId { get; set; }
        public int TeamRoleID { get; set; }

        public TeamRoleModel? Role { get; set; }

        public UserModel()
        {
            
        }

    }
}
