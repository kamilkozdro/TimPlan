using Avalonia.Controls;
using Avalonia.ReactiveUI;
using ReactiveUI;
using System;
using TimPlan.ViewModels;

namespace TimPlan.Views
{
    public partial class UserEditWindow : ReactiveWindow<UserEditViewModel>
    {
        public UserEditWindow()
        {
            InitializeComponent();
            this.WhenActivated(d => d(ViewModel!.ReturnResultCommand.Subscribe(Close)));
        }
    }
}
