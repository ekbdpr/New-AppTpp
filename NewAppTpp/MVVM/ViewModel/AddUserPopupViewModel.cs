﻿using AppTpp.Services;
using HandyControl.Tools;
using HandyControl.Tools.Command;
using NewAppTpp.MVVM.Model;
using NewAppTpp.Services;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;

namespace NewAppTpp.MVVM.ViewModel
{
    class AddUserPopupViewModel : BindablePropertyBase
    {
        private readonly UserAccessModel _userAccessModel;

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

        public string Password
        {
            get { return _userAccessModel.Password; }
            set { _userAccessModel.Password = value; RaisePropertyChanged(nameof(Password)); }
        }

        public string Privilege
        {
            get { return _userAccessModel.Privilege; }
            set { _userAccessModel.Privilege = value; RaisePropertyChanged(nameof(Privilege)); }
        }

        public ICommand SaveCommand { get; }

        public AddUserPopupViewModel()
        {
            SaveCommand = new RelayCommand(new Action<object>(Save), new Func<object, bool>(CanSave));
        }

        private bool CanSave(object arg)
        {
            return !string.IsNullOrEmpty(Nip) &&
                   !string.IsNullOrEmpty(Nama) &&
                   !string.IsNullOrEmpty(Jabatan) &&
                   !string.IsNullOrEmpty(Username) &&
                   !string.IsNullOrEmpty(Password) &&
                   !string.IsNullOrEmpty(Privilege);
        }

        private async void Save(object obj)
        {
            try
            {
                await Task.Run(() => UserAccessService.InsertNewUser(Nip, Nama, Jabatan, Username, Password, Privilege));
                DataMiddlewareService.Instance.InvokeDataSaved();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during execute: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                CloseCurrentWindow();
            }
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
