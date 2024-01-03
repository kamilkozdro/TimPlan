using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using Avalonia.ReactiveUI;
using ReactiveUI;
using System.Threading.Tasks;
using TimPlan.Models;
using TimPlan.ViewModels;

namespace TimPlan.Views;

public partial class MainView : ReactiveUserControl<MainViewModel>
{
    public MainView()
    {
        InitializeComponent();

        this.WhenActivated(action =>
            action(ViewModel!.ShowTaskEditWindow.RegisterHandler(DoShowTaskEditWindow)));
    }

    private async Task DoShowTaskEditWindow(InteractionContext<TaskEditViewModel, bool?> interaction)
    {
        TaskEditWindow taskEditWindow = new TaskEditWindow();
        taskEditWindow.DataContext = interaction.Input;

        if (Avalonia.Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            bool? result = await taskEditWindow.ShowDialog<bool?>(desktop.MainWindow);
            interaction.SetOutput(result);
        }
    }

    public void ButtonAddUserClick(object sender, RoutedEventArgs e)
    {
        UserEditViewModel userEditWindowVM = new UserEditViewModel();
        UserEditWindow userEditWindow = new UserEditWindow();
        userEditWindow.DataContext = userEditWindowVM;
        userEditWindow.Show();
    }

    public void ButtonAddTeamClick(object sender, RoutedEventArgs e)
    {
        TeamEditViewModel teamEditWindowVM = new TeamEditViewModel();
        TeamEditWindow teamEditWindow = new TeamEditWindow();
        teamEditWindow.DataContext = teamEditWindowVM;
        teamEditWindow.Show();
    }
    /*
    public void ButtonAddTaskClick(object sender, RoutedEventArgs e)
    {


        TaskEditViewModel taskEditWindowVM = new TaskEditViewModel();
        TaskEditWindow taskEditWindow = new TaskEditWindow();
        taskEditWindow.DataContext = taskEditWindowVM;
        taskEditWindow.Show();
    }
    */
}
