using HandyControl.Tools;
using HandyControl.Tools.Command;
using NewAppTpp.MVVM.Model;
using NewAppTpp.MVVM.View;
using NewAppTpp.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace NewAppTpp.MVVM.ViewModel
{
    internal class LoginViewModel : BindablePropertyBase
    {
        private readonly UserAccessModel _userAccessModel = new();

        public string Username
        {
            get { return _userAccessModel.Username; }
            set
            {
                if (_userAccessModel.Username != value)
                {
                    _userAccessModel.Username = value;
                    RaisePropertyChanged(nameof(Username));
                }
            }
        }

        public string Password
        {
            get { return _userAccessModel.Password; }

            set
            {
                if (_userAccessModel.Password != value)
                {
                    _userAccessModel.Password = value;
                    RaisePropertyChanged(nameof(Password));
                }
            }
        }

        private string _errorMessage = string.Empty;
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                if (_errorMessage != value)
                {
                    _errorMessage = value;
                    RaisePropertyChanged(nameof(ErrorMessage));
                }
            }
        }

        private Visibility _isLoading = Visibility.Collapsed;
        public Visibility IsLoading
        {
            get { return _isLoading; }
            set { _isLoading = value; RaisePropertyChanged(nameof(IsLoading)); }
        }

        private bool _isWindowActive = true;
        public bool IsWindowActive
        {
            get { return _isWindowActive; }
            set { _isWindowActive = value; RaisePropertyChanged(nameof(IsWindowActive)); }
        }

        public ICommand LoginCommand { get; }
        public ICommand ExitCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new SimpleRelayCommand(new Action(Login));
            ExitCommand = new SimpleRelayCommand(new Action(Exit));
        }

        private void Exit()
        {
            Application.Current.Shutdown();
        }

        private async void Login()
        {
            try
            {
                IsLoading = Visibility.Visible;
                IsWindowActive = false;

                if (await Task.Run(() => IsValidUser()))
                {
                    OpenMainWindow();
                    CloseCurrentWindow();

                    return;
                }

                ErrorMessage = "* Username atau Password Tidak Valid";
            }
            catch
            {
                ErrorMessage = "* Gagal Menghubungkan ke Server. Periksa Koneksi Internet Anda";
            }
            finally
            {
                IsLoading = Visibility.Collapsed;
                IsWindowActive = true;
            }
        }

        private bool IsValidUser() => UserAccessService.Instance.GetUserLoginData(Username, Password);

        private static void OpenMainWindow()
        {
            MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault() ?? new();
            mainWindow ??= new MainWindow();

            mainWindow.Show();
        }

        private void CloseCurrentWindow()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.DataContext == this) window.Close();
            }
        }
    }
}
