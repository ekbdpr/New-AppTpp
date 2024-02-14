using HandyControl.Controls;
using HandyControl.Tools;
using HandyControl.Tools.Command;
using NewAppTpp.MVVM.Model;
using NewAppTpp.MVVM.View;
using NewAppTpp.Services;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace NewAppTpp.MVVM.ViewModel
{
    class UserManagerViewModel : BindablePropertyBase
    {
        private ObservableCollection<UserAccessModel> _userAccessModel = new();
        public ObservableCollection<UserAccessModel> UserAccessModel
        {
            get { return _userAccessModel; }
            set
            {
                _userAccessModel = value;
                RaisePropertyChanged(nameof(UserAccessModel));
            }
        }

        public ICommand AddUserCommand { get; }

        public UserManagerViewModel()
        {
            UserAccessModel = new ObservableCollection<UserAccessModel>(UserAccessService.GetAllUsers());

            AddUserCommand = new SimpleRelayCommand(new Action(AddUser));
        }

        private void AddUser()
        {
            Dialog.Show(new AddUserPopup());
        }
    }
}
