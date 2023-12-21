using ReactiveUI;

namespace TimPlan.Models
{
    public class CheckableComboBoxItem : ReactiveObject
    {

        private string? _name;
        public string? Name
        {
            get { return _name; }
            set
            {
                this.RaiseAndSetIfChanged(ref _name, value);
            }
        }
        private bool _isChecked;
        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                this.RaiseAndSetIfChanged(ref _isChecked, value);
            }
        }
    }
}
