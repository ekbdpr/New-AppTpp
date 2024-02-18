using AppTpp.Services;
using HandyControl.Controls;
using HandyControl.Tools;
using HandyControl.Tools.Command;
using HandyControl.Tools.Extension;
using NewAppTpp.MVVM.Model;
using NewAppTpp.MVVM.View;
using NewAppTpp.Services;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace NewAppTpp.MVVM.ViewModel
{
    class UserManagerViewModel : BindablePropertyBase
    {
        private ObservableCollection<UserAccessModel> _userAccessModelCollection = new();
        public ObservableCollection<UserAccessModel> UserAccessModelCollection
        {
            get { return _userAccessModelCollection; }
            set
            {
                _userAccessModelCollection = value;
                RaisePropertyChanged(nameof(UserAccessModelCollection));
            }
        }

        private UserAccessModel _selectedUser;
        public UserAccessModel SelectedUser
        {
            get { return _selectedUser; }
            set
            {
                _selectedUser = value;

                if (value != null)
                {
                    UserAccessMiddlewareService.Instance.SelectedNip = value.Nip;
                    UserAccessMiddlewareService.Instance.SelectedNama = value.Nama;
                    UserAccessMiddlewareService.Instance.SelectedJabatan = value.Jabatan;
                    UserAccessMiddlewareService.Instance.SelectedUsername = value.Username;
                    UserAccessMiddlewareService.Instance.SelectedPrivilege = value.Privilege;
                }

                RaisePropertyChanged(nameof(SelectedUser));
            }
        }

        private Dialog UserAccessDialog { get; set; } = new Dialog();

        public ICommand AddUserCommand { get; }
        public ICommand EditUserCommand { get; }
        public ICommand DeleteUserCommand { get; }

        public UserManagerViewModel()
        {
            InitializeUserAccessData();

            AddUserCommand = new SimpleRelayCommand(new Action(OpenAddUserPopup));
            EditUserCommand = new SimpleRelayCommand(new Action(OpenEditUserPopup));
            DeleteUserCommand = new SimpleRelayCommand(new Action(OpenConfirmationPopup));

            UserAccessMiddlewareService.Instance.OnDataSaved += SaveChangedData;
            UserAccessMiddlewareService.Instance.OnDeleteData += DeleteUser;
        }

        private void InitializeUserAccessData()
        {
            UserAccessModelCollection = new ObservableCollection<UserAccessModel>(UserAccessService.GetAllUsers());
        }

        private void SaveChangedData()
        {
            InitializeUserAccessData();
            UserAccessDialog.Close();
        }

        private void DeleteUser()
        {
            UserAccessService.DeleteUser(SelectedUser.Username);
            InitializeUserAccessData();
            UserAccessDialog.Close();
        }

        private void OpenAddUserPopup() => UserAccessDialog = Dialog.Show(new AddUserAccessPopup());

        private void OpenEditUserPopup() => UserAccessDialog = Dialog.Show(new EditUserAccessPopup());

        private void OpenConfirmationPopup()
        {
            UserAccessDialog = Dialog.Show(new ConfirmationPopup($"Anda yakin akan menghapus {_selectedUser.Nip} - {_selectedUser.Nama} dari sistem ?"));
            ConfirmationPopupMiddlewareService.NotifyConfirmationPopupUidChanged("DeleteConfirmation");
        }
    }
}
