using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimPlan.ViewModels
{
    public class TaskEditViewModel : ViewModelBase
    {
        #region Bound Properties

        private string _Name;
        public string Name
        {
            get { return _Name; }
            set { this.RaiseAndSetIfChanged(ref _Name, value); }
        }

        private DateOnly _StartDate;
        public DateOnly StartDate
        {
            get { return _StartDate; }
            set { this.RaiseAndSetIfChanged(ref _StartDate, value); }
        }

        private DateOnly _EndDate;
        public DateOnly EndDate
        {
            get { return _EndDate; }
            set { this.RaiseAndSetIfChanged(ref _EndDate, value); }
        }

        private bool _Private;
        public bool Private
        {
            get { return _Private; }
            set { this.RaiseAndSetIfChanged(ref _Private, value); }
        }

        private string _Description;
        public string Description
        {
            get { return _Description; }
            set { this.RaiseAndSetIfChanged(ref _Description, value); }
        }



        #endregion

        #region Commands



        #endregion

        public TaskEditViewModel()
        {
            
        }

    }
}
