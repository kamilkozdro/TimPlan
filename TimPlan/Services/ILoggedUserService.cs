using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimPlan.Models;

namespace TimPlan.Services
{
    public interface ILoggedUserService
    {
        public UserModel? GetLoggedUser();
        public SystemRoleModel? GetLoggedUserSystemRole();
        public TeamRoleModel? GetLoggedUserTeamRole();

    }
}
