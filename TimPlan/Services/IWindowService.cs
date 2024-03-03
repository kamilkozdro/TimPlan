using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimPlan.Models;
using TimPlan.ViewModels;

namespace TimPlan.Services
{
    public interface IWindowService
    {
        void ShowTaskEditWindow(AccessType accessType = AccessType.View, TaskModel editedTask = null);
        void ShowTeamEditWindow();
        void ShowUserEditWindow();
        void ShowTeamRoleEditWindow();
        public Task<bool> ShowDialogYesNo(string text, string title = "");
    }
}
