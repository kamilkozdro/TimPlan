using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using TimPlan.Models;

namespace TimPlan.ViewModels
{
    public class TaskTileViewModel : ViewModelBase
    {

        #region Properties

        private int _daysLeft = 0;

        public int DaysLeft
        {
            get { return _daysLeft; }
            set { this.RaiseAndSetIfChanged(ref _daysLeft, value); }
        }

        private string _taskName = string.Empty;

        public string TaskName
        {
            get { return _taskName; }
            set { this.RaiseAndSetIfChanged(ref _taskName, value); }
        }

        private string _description;

        public string Description
        {
            get { return _description; }
            set { this.RaiseAndSetIfChanged(ref _description, value); }
        }

        private TaskModel _task;

        #endregion


        #region Commands

        public ReactiveCommand<Unit, Unit> AcceptTaskCommand { get;}
        public ReactiveCommand<Unit, Unit> CompleteTaskCommand { get; }

        #endregion

        public TaskTileViewModel()
        {
            
        }
        public TaskTileViewModel(TaskModel task)
        {
            _task = task;
            TaskName = _task.Name;
            Description = _task.Description;
            TimeSpan daysLeft = _task.DateEnd - DateTime.Now;
            DaysLeft = (int)daysLeft.TotalDays;

            AcceptTaskCommand = ReactiveCommand.Create(AcceptTask);
            CompleteTaskCommand = ReactiveCommand.Create(CompleteTask);

        }
        private void AcceptTask()
        {

        }
        private void CompleteTask()
        {

        }
    }
}
