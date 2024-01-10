using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TimPlan.Lib;
using TimPlan.Models;

namespace TimPlan.ViewModels
{
    public class LoginWindowViewModel : ViewModelBase
    {
        private string? _username;   
        public string? Username
        {
            get { return _username; }
            set { this.RaiseAndSetIfChanged(ref _username, value); }
        }

        private string? _password;
        public string? Password
        {
            get { return _password; }
            set { this.RaiseAndSetIfChanged(ref _password, value); }
        }

        public Action<UserModel> LoginSuccessful;

        public ReactiveCommand<Unit, Unit> LoginCommand { get; }

        public LoginWindowViewModel()
        {
            IObservable<bool> IsButtonLoginEnabled = this.WhenAnyValue(
                x => x.Username,
                x => x.Password,
                (username, password) => !string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password));

            LoginCommand = ReactiveCommand.Create(ButtonLoginClick, IsButtonLoginEnabled);

        }

        private void ButtonLoginClick()
        {
            UserModel loggedUser = SQLAccess.SelectUser(Username, Password);

            if (loggedUser == null)
            {
                Debug.WriteLine("Wrong credentials");
                return;
            }
            else
            {
                OnSuccesfullLogin(loggedUser);
            }
        }

        public void OnSuccesfullLogin(UserModel user)
        {
            LoginSuccessful?.Invoke(user);
        }


    }
}
