using AppTpp.Services;
using HandyControl.Tools;
using HandyControl.Tools.Command;
using NewAppTpp.MVVM.Model;
using NewAppTpp.Services;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace NewAppTpp.MVVM.ViewModel
{
    class EditUserAccessPopupViewModel : BindablePropertyBase
    {
        private readonly UserAccessModel _userAccessModel = new();

        public string Nip
        {
            get { return _userAccessModel.Nip; }
            set { _userAccessModel.Nip = value; RaisePropertyChanged(nameof(Nip)); }
        }

        public string Nama
        {
            get { return _userAccessModel.Nama; }
            set { _userAccessModel.Nama = value; RaisePropertyChanged(nameof(Nama)); }
        }

        public string Jabatan
        {
            get { return _userAccessModel.Jabatan; }
            set { _userAccessModel.Jabatan = value; RaisePropertyChanged(nameof(Jabatan)); }
        }

        public string Username
        {
            get { return _userAccessModel.Username; }
            set { _userAccessModel.Username = value; RaisePropertyChanged(nameof(Username)); }
        }

        public string Privilege
        {
            get { return _userAccessModel.Privilege; }
            set { _userAccessModel.Privilege = value; RaisePropertyChanged(nameof(Privilege)); }
        }

        public ICommand SaveCommand { get; }

        public EditUserAccessPopupViewModel()
        {
            InitializeSelectedUser();

            SaveCommand = new RelayCommand(new Action<object>(Save), new Func<object, bool>(CanSave));
        }

        private void InitializeSelectedUser()
        {
            Nip = UserAccessMiddlewareService.Instance.SelectedNip;
            Nama = UserAccessMiddlewareService.Instance.SelectedNama;
            Jabatan = UserAccessMiddlewareService.Instance.SelectedJabatan;
            Username = UserAccessMiddlewareService.Instance.SelectedUsername;
            Privilege = UserAccessMiddlewareService.Instance.SelectedPrivilege;
        }

        private bool CanSave(object arg)
        {
            return !string.IsNullOrEmpty(Nip) &&
                   !string.IsNullOrEmpty(Nama) &&
                   !string.IsNullOrEmpty(Jabatan) &&
                   !string.IsNullOrEmpty(Username) &&
                   !string.IsNullOrEmpty(Privilege);
        }

        private async void Save(object obj)
        {
            try
            {
                await Task.Run(() => UserAccessService.UpdateUser(Nip, Nama, Jabatan, Username, Privilege));
                UserAccessMiddlewareService.Instance.InvokeDataSaved();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during execute: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
