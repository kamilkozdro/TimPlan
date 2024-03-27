using ReactiveUI;
using System;
using System.Reactive;
using TimPlan.Lib;
using TimPlan.Models;

namespace TimPlan.ViewModels
{
    public class LoginWindowViewModel : ViewModelBase
    {
        private string _username;   
        public string Username
        {
            get { return _username; }
            set { this.RaiseAndSetIfChanged(ref _username, value); }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set { this.RaiseAndSetIfChanged(ref _password, value); }
        }

        private string _errorText;
        public string ErrorText
        {
            get { return _errorText; }
            set { this.RaiseAndSetIfChanged(ref _errorText, value); }
        }

        public ReactiveCommand<UserModel?, UserModel?> ReturnResultCommand { get; }
        public ReactiveCommand<Unit, Unit> LoginCommand { get; }
        public ReactiveCommand<Unit, Unit> CancelCommand { get; }

        public LoginWindowViewModel()
        {
            IObservable<bool> IsButtonLoginEnabled = this.WhenAnyValue(
                x => x.Username,
                x => x.Password,
                (username, password) => !string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password));

            LoginCommand = ReactiveCommand.Create(Login, IsButtonLoginEnabled);
            CancelCommand = ReactiveCommand.Create(CancelLogging);
            ReturnResultCommand = ReactiveCommand.Create<UserModel?, UserModel?>(model =>
            {
                return model;
            });
        }

        private void Login()
        {
            UserModel? loggedUser = SQLAccess.SelectUser(Username, Password);

            if (loggedUser == null)
            {
                ErrorText = "Wrong credentials";
                return;
            }

            ReturnResultCommand.Execute(loggedUser).Subscribe();
        }

        private void CancelLogging()
        {
            ReturnResultCommand.Execute(null).Subscribe();
        }


    }
}
