using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using TimPlan.Lib;
using TimPlan.Models;
using static TimPlan.Models.TaskModel;

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
        private TaskModel.TaskState _taskState;
        public TaskModel.TaskState TaskState
        {
            get { return _taskState; }
            set { this.RaiseAndSetIfChanged(ref _taskState, value); }
        }

        private TaskModel _task;
        public TaskModel Task
        {
            get { return _task; }
            set { this.RaiseAndSetIfChanged(ref _task, value); }
        }

        private bool _readOnlyTask = false;
        public bool ReadOnlyTask
        {
            get { return _readOnlyTask; }
            set { this.RaiseAndSetIfChanged(ref _readOnlyTask, value); }
        }


        #endregion


        #region Commands

        public ReactiveCommand<Unit, Unit> SuspendTaskCommand { get; }
        public ReactiveCommand<Unit, Unit> AcceptTaskCommand { get;}
        public ReactiveCommand<Unit, Unit> CompleteTaskCommand { get; }

        #endregion

        public TaskTileViewModel()
        {
            
        }
        public TaskTileViewModel(TaskModel task)
        {
            Task = task;
            TaskState = Task.State;
            TimeSpan daysLeft = Task.DateEnd - DateTime.Now;
            DaysLeft = (int)daysLeft.TotalDays;

            IObservable<bool> suspendTaskCanExecute = this.WhenAnyValue(
                o => o.TaskState,
                taskState => taskState == TaskModel.TaskState.Accepted);

            IObservable<bool> acceptTaskCanExecute = this.WhenAnyValue(
                o => o.TaskState,
                taskState => taskState == TaskModel.TaskState.Suspended);

            IObservable<bool> completeTaskCanExecute = this.WhenAnyValue(
                o => o.TaskState,
                taskState => taskState == TaskModel.TaskState.Accepted);

            SuspendTaskCommand = ReactiveCommand.Create(SuspendTask, suspendTaskCanExecute);
            AcceptTaskCommand = ReactiveCommand.Create(AcceptTask, acceptTaskCanExecute);
            CompleteTaskCommand = ReactiveCommand.Create(CompleteTask, completeTaskCanExecute);


        }
        private void SuspendTask()
        {
            if (!SQLAccess.UpdateTaskState(Task.Id, TaskModel.TaskState.Suspended))
            {
                return;
            }

            Task.State = TaskModel.TaskState.Suspended;
            TaskState = TaskModel.TaskState.Suspended;
        }
        private void AcceptTask()
        {
            if (!SQLAccess.UpdateTaskState(Task.Id, TaskModel.TaskState.Accepted))
            {
                return;
            }

            Task.State = TaskModel.TaskState.Accepted;
            TaskState = TaskModel.TaskState.Accepted;
        }
        private void CompleteTask()
        {
            if (!SQLAccess.UpdateTaskState(Task.Id, TaskModel.TaskState.Completed))
            {
                return;
            }

            Task.State = TaskModel.TaskState.Completed;
            TaskState = TaskModel.TaskState.Completed;
        }
    }
}
