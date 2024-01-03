using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimPlan.Models;

namespace TimPlan.Services
{
    public interface IWindowService
    {
        void OpenTaskEditWindow(UserModel loggedUser);
        void OpenTeamEditWindow();
        void OpenUserEditWindow();
    }
}
