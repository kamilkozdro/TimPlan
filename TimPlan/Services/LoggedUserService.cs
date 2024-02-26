using TimPlan.Models;

namespace TimPlan.Services
{
    public class LoggedUserService : ILoggedUserService
    {
        private readonly UserModel _loggedUser = null;

        public LoggedUserService(UserModel loggedUser)
        {
            _loggedUser = loggedUser;
        }

        public UserModel? GetLoggedUser()
        {
            return _loggedUser;
        }

        public SystemRoleModel? GetLoggedUserSystemRole()
        {
            return _loggedUser?.SystemRole;
        }

        public TeamRoleModel? GetLoggedUserTeamRole()
        {
            return _loggedUser?.TeamRole;
        }
    }
}
