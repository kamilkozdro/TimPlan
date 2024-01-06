using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimPlan.Models;
using TimPlan.ViewModels;
using TimPlan.Views;

namespace TimPlan.Services
{
    public class WindowService : IWindowService
    {
        public void OpenTaskEditWindow(UserModel loggedUser)
        {
            TaskEditViewModel taskEditVM = new TaskEditViewModel(loggedUser);
            TaskEditWindow taskEditWindow = new TaskEditWindow();
            taskEditWindow.DataContext = taskEditVM;
            taskEditWindow.Show();
        }

        public void OpenTeamEditWindow()
        {
            TeamEditViewModel teamEditVM = new TeamEditViewModel();
            TeamEditWindow teamEditWindow = new TeamEditWindow();
            teamEditWindow.DataContext = teamEditVM;
            teamEditWindow.Show();
        }

        public void OpenTeamRoleEditWindow()
        {
            TeamRoleEditViewModel teamRoleEditVM = new TeamRoleEditViewModel();
            TeamRoleEditWindow teamRoleEditWindow = new TeamRoleEditWindow();
            teamRoleEditWindow.DataContext = teamRoleEditVM;
            teamRoleEditWindow.Show();
        }

        public void OpenUserEditWindow()
        {
            UserEditViewModel userEditWindowVM = new UserEditViewModel();
            UserEditWindow userEditWindow = new UserEditWindow();
            userEditWindow.DataContext = userEditWindowVM;
            userEditWindow.Show();
        }
    }
}
