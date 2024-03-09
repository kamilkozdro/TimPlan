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
        public Task<TaskModel> ShowTaskEditWindow(AccessType accessType = AccessType.View, TaskModel editedTask = null, UserModel selectedUser = null);
        public Task<TeamModel> ShowTeamEditWindow();
        public Task<UserModel> ShowUserEditWindow();
        public Task<TeamRoleModel> ShowTeamRoleEditWindow();
        public Task<bool> ShowDialogYesNo(string text, string title = "");
    }
}
