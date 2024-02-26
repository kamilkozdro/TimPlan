using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using TimPlan.Lib;
using TimPlan.Models;
using TimPlan.Services;
using TimPlan.ViewModels;
using TimPlan.Views;

namespace TimPlan;

public partial class App : Application
{
    public new static App? Current => Application.Current as App;

    /// <summary>
    /// Gets the <see cref="IServiceProvider"/> instance to resolve application services.
    /// </summary>
    public IServiceProvider? Services { get; private set; }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
        SQLAccess.MapClassAttributes();
    }

    public override async void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            
            LoginWindowViewModel loginWindowVM = new LoginWindowViewModel();
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.DataContext = loginWindowVM;
            desktop.MainWindow = loginWindow;
            desktop.MainWindow.SizeToContent = SizeToContent.WidthAndHeight;

            // Use TaskCompletionSource to await for the login to complete
            var loginCompletionSource = new TaskCompletionSource<UserModel>();

            // Set up the event handler for successful login
            loginWindowVM.LoginSuccessful += user =>
            {
                // Set the result of the TaskCompletionSource
                loginCompletionSource.SetResult(user);
            };

            // Wait for the login to complete
            UserModel usernameResult = await loginCompletionSource.Task;

            MainViewModel mainViewModel = new MainViewModel();
            MainWindow mainWindow = new MainWindow();
            mainWindow.DataContext = mainViewModel;

            desktop.MainWindow = mainWindow;

            mainWindow.Show();
            loginWindow.Close();

            mainViewModel.LoggedUser = usernameResult;

            ServiceCollection services = new ServiceCollection();
            services.AddSingleton<IWindowService>(x => new WindowService(desktop.MainWindow));
            services.AddSingleton<ILoggedUserService>(x => new LoggedUserService(usernameResult));
            Services = services.BuildServiceProvider();

        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new MainView
            {
                DataContext = new MainViewModel()
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}
