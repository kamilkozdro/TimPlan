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
using System.Diagnostics;

namespace TimPlan.Services
{
    public class WindowService : IWindowService
    {
        private readonly Window _target;

        public WindowService(Window target)
        {
            _target = target;
        }

        public async Task<TaskModel> ShowTaskEditWindow(EditWindowType editWindowType = EditWindowType.View, TaskModel editedTask = null, UserModel selectedUser = null)
        {
            TaskEditViewModel taskEditVM = new TaskEditViewModel();
            taskEditVM.SetEditWindowType(editWindowType);
            taskEditVM.SetEditedModel(editedTask);
            if(selectedUser != null)
            {
                taskEditVM.SelectedTeam = taskEditVM.Teams.FirstOrDefault(team => team.Id == selectedUser.TeamId);
                taskEditVM.SelectedUser = taskEditVM.Users.FirstOrDefault(user => user.Id == selectedUser.Id);
            }
            TaskEditWindow taskEditWindow = new TaskEditWindow();
            taskEditWindow.DataContext = taskEditVM;
            return await taskEditWindow.ShowDialog<TaskModel>(_target);
        }

        public async Task<TeamModel> ShowTeamEditWindow()
        {
            TeamEditViewModel teamEditVM = new TeamEditViewModel();
            TeamEditWindow teamEditWindow = new TeamEditWindow();
            teamEditWindow.DataContext = teamEditVM;
            return await teamEditWindow.ShowDialog<TeamModel>(_target);
        }

        public async Task<TeamRoleModel> ShowTeamRoleEditWindow()
        {
            TeamRoleEditViewModel teamRoleEditVM = new TeamRoleEditViewModel();
            TeamRoleEditWindow teamRoleEditWindow = new TeamRoleEditWindow();
            teamRoleEditWindow.DataContext = teamRoleEditVM;
            return await teamRoleEditWindow.ShowDialog<TeamRoleModel>(_target);
        }

        public async Task<UserModel> ShowUserEditWindow()
        {
            UserEditViewModel userEditWindowVM = new UserEditViewModel();
            UserEditWindow userEditWindow = new UserEditWindow();
            userEditWindow.DataContext = userEditWindowVM;
            return await userEditWindow.ShowDialog<UserModel>(_target);
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
