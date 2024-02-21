using AppTpp.Services;
using HandyControl.Controls;
using HandyControl.Data;
using HandyControl.Tools;
using HandyControl.Tools.Command;
using NewAppTpp.MVVM.Model;
using NewAppTpp.MVVM.View;
using NewAppTpp.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
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
                if (value != null)
                {
                    _selectedUser = value;

                    UserAccessMiddlewareService.Instance.SelectedNip = value.Nip;
                    UserAccessMiddlewareService.Instance.SelectedNama = value.Nama;
                    UserAccessMiddlewareService.Instance.SelectedJabatan = value.Jabatan;
                    UserAccessMiddlewareService.Instance.SelectedUsername = value.Username;
                    UserAccessMiddlewareService.Instance.SelectedPrivilege = value.Privilege;

                    RaisePropertyChanged(nameof(SelectedUser));
                }
            }
        }

        private string _searchText;
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                RaisePropertyChanged(nameof(SearchText));
            }
        }

        private int _maxPageCount;
        public int MaxPageCount
        {
            get { return _maxPageCount; }
            set { _maxPageCount = value; RaisePropertyChanged(nameof(MaxPageCount)); }
        }

        private int _pageIndex;
        public int PageIndex
        {
            get { return _pageIndex; }
            set
            {
                if (value != _pageIndex)
                {
                    _pageIndex = value;
                    InitializeUserAccessData();
                    RaisePropertyChanged(nameof(PageIndex));
                }
            }
        }

        private Dialog UserAccessDialog { get; set; } = new Dialog();

        public ICommand SearchUserCommand { get; }
        public ICommand AddUserCommand { get; }
        public ICommand EditUserCommand { get; }
        public ICommand DeleteUserCommand { get; }

        public UserManagerViewModel()
        {
            InitializeUserAccessData();

            SearchUserCommand = new SimpleRelayCommand(new Action(SearchUser));
            AddUserCommand = new SimpleRelayCommand(new Action(OpenAddUserPopup));
            EditUserCommand = new SimpleRelayCommand(new Action(OpenEditUserPopup));
            DeleteUserCommand = new SimpleRelayCommand(new Action(OpenConfirmationPopup));

            UserAccessMiddlewareService.Instance.OnDataSaved += SaveChangedData;
            UserAccessMiddlewareService.Instance.OnDeleteData += DeleteUser;
        }

        private void InitializeUserAccessData()
        {
            try
            {
                var userData = UserAccessService.GetAllUsers();
                var filteredUserData = string.IsNullOrEmpty(SearchText) ? userData : userData.Where(data => data.Nama.Contains(SearchText, StringComparison.OrdinalIgnoreCase) || data.Nip.Contains(SearchText, StringComparison.OrdinalIgnoreCase));

                MaxPageCount = Convert.ToInt32(Math.Ceiling((double)filteredUserData.Count() / 10.0));
                var startIndex = (PageIndex - 1) * 10;

                UserAccessModelCollection = new ObservableCollection<UserAccessModel>(filteredUserData.Skip(startIndex).Take(10));
            }
            catch (Exception ex)
            {
                HandyControl.Controls.MessageBox.Show(new MessageBoxInfo
                {
                    Message = $"Error during execute: {ex.Message}",
                    Caption = "Error",
                    Button = MessageBoxButton.OK,
                    IconBrushKey = ResourceToken.AccentBrush,
                    IconKey = ResourceToken.ErrorGeometry,
                    StyleKey = "MessageBoxCustom"
                });
            }
        }

        private void SaveChangedData()
        {
            InitializeUserAccessData();
            UserAccessDialog.Close();
        }

        private void SearchUser()
        {
            InitializeUserAccessData();
        }

        private void OpenAddUserPopup() => UserAccessDialog = Dialog.Show(new AddUserAccessPopup());

        private void OpenEditUserPopup() => UserAccessDialog = Dialog.Show(new EditUserAccessPopup());

        private void DeleteUser()
        {
            UserAccessService.DeleteUser(SelectedUser.Username);
            InitializeUserAccessData();
            UserAccessDialog.Close();
        }

        private void OpenConfirmationPopup()
        {
            UserAccessDialog = Dialog.Show(new ConfirmationPopup($"Anda yakin akan menghapus {_selectedUser.Nip} - {_selectedUser.Nama} dari sistem ?"));
            ConfirmationPopupMiddlewareService.NotifyConfirmationPopupUidChanged("DeleteUserConfirmation");
        }
    }
}