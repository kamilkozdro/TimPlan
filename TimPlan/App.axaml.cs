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
            

            
            MainWindow mainWindow = new MainWindow();
            MainViewModel mainViewModel = new MainViewModel();
            mainWindow.DataContext = mainViewModel;
            desktop.MainWindow = mainWindow;

            ServiceCollection services = new ServiceCollection();
            services.AddSingleton<IWindowService>(x => new WindowService(desktop.MainWindow));
            Services = services.BuildServiceProvider();

            mainWindow.Show();
            

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
