using Avalonia.Controls;
using Avalonia.Interactivity;
using TimPlan.ViewModels;

namespace TimPlan.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
    }

    public void ButtonAddUserClick(object sender, RoutedEventArgs e)
    {
        UserEditViewModel userEditWindowVM = new UserEditViewModel();
        UserEditWindow userEditWindow = new UserEditWindow();
        userEditWindow.DataContext = userEditWindowVM;
        userEditWindow.Show();
    }

    public void ButtonAddTaskClick(object sender, RoutedEventArgs e)
    {
        TaskEditViewModel taskEditWindowVM = new TaskEditViewModel();
        TaskEditWindow taskEditWindow = new TaskEditWindow();
        taskEditWindow.DataContext = taskEditWindowVM;
        taskEditWindow.Show();
    }
}
