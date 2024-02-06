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
        void ShowTaskEditWindow(UserModel loggedUser);
        void ShowTeamEditWindow();
        void ShowUserEditWindow();
        void ShowTeamRoleEditWindow();
        public Task<bool> ShowDialogYesNo(string text, string title = "");
    }
}
