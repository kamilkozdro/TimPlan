using Avalonia.Controls;
using Avalonia.ReactiveUI;
using ReactiveUI;
using System;
using TimPlan.ViewModels;

namespace TimPlan.Views
{
    public partial class TaskEditWindow : ReactiveWindow<TaskEditViewModel>
    {
        public TaskEditWindow()
        {
            InitializeComponent();
            this.WhenActivated(d => d(ViewModel!.ReturnResultCommand.Subscribe(Close)));
        }
    }
}
