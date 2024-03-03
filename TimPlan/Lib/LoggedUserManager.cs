using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimPlan.Models;

namespace TimPlan.Lib
{
    static class LoggedUserManager
    {
        static private UserModel? _loggedUser;

        static public UserModel? GetUser() { return _loggedUser; }
        static public void Logout() {  _loggedUser = null; }
        static public void Login(UserModel user) { _loggedUser = user; }
    }
}
