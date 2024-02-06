using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimPlan.Models;
using TimPlan.ViewModels;
using TimPlan.Views;
using MsBox.Avalonia;
using Avalonia.Controls;

namespace TimPlan.Services
{
    public class WindowService : IWindowService
    {
        private readonly Window _target;

        public WindowService(Window target)
        {
            _target = target;
        }

        public void ShowTaskEditWindow(UserModel loggedUser)
        {
            TaskEditViewModel taskEditVM = new TaskEditViewModel(loggedUser);
            TaskEditWindow taskEditWindow = new TaskEditWindow();
            taskEditWindow.DataContext = taskEditVM;
            taskEditWindow.Show();
        }

        public void ShowTeamEditWindow()
        {
            TeamEditViewModel teamEditVM = new TeamEditViewModel();
            TeamEditWindow teamEditWindow = new TeamEditWindow();
            teamEditWindow.DataContext = teamEditVM;
            teamEditWindow.Show();
        }

        public void ShowTeamRoleEditWindow()
        {
            TeamRoleEditViewModel teamRoleEditVM = new TeamRoleEditViewModel();
            TeamRoleEditWindow teamRoleEditWindow = new TeamRoleEditWindow();
            teamRoleEditWindow.DataContext = teamRoleEditVM;
            teamRoleEditWindow.Show();
        }

        public void ShowUserEditWindow()
        {
            UserEditViewModel userEditWindowVM = new UserEditViewModel();
            UserEditWindow userEditWindow = new UserEditWindow();
            userEditWindow.DataContext = userEditWindowVM;
            userEditWindow.Show();
        }

        public async Task<bool> ShowDialogYesNo(string text, string title = "")
        {
            var msgBox = MessageBoxManager
                .GetMessageBoxStandard(title, text, MsBox.Avalonia.Enums.ButtonEnum.YesNo);
            var result = await msgBox.ShowWindowDialogAsync(_target);

            if (result == MsBox.Avalonia.Enums.ButtonResult.Yes)
                return true;
            else
                return false;
        }
    }
}
