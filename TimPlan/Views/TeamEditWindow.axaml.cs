using Avalonia.Controls;
using Avalonia.ReactiveUI;
using ReactiveUI;
using System;
using TimPlan.ViewModels;

namespace TimPlan.Views
{
    public partial class TeamEditWindow : ReactiveWindow<TeamEditViewModel>
    {
        public TeamEditWindow()
        {
            InitializeComponent();
            this.WhenActivated(d => d(ViewModel!.ReturnResultCommand.Subscribe(Close)));
        }
    }
}
