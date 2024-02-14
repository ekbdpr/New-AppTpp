using HandyControl.Tools;
using HandyControl.Tools.Command;
using NewAppTpp.MVVM.Model;
using NewAppTpp.MVVM.View;
using NewAppTpp.Services;
using System;
using System.Windows.Input;

namespace NewAppTpp.MVVM.ViewModel
{
    class MainWindowViewModel : BindablePropertyBase
    {
        private object _currentView;
        public object CurrentView
        {
            get { return _currentView; }
            set { _currentView = value; RaisePropertyChanged(nameof(CurrentView)); }
        }

        private readonly UserAccessModel _userAccessModel = new();

        public string Nama
        {
            get { return _userAccessModel.Nama; }
            set { _userAccessModel.Nama = value; RaisePropertyChanged(nameof(Nama)); }
        }

        public string Privilege
        {
            get { return _userAccessModel.Privilege; }
            set { _userAccessModel.Privilege = value; RaisePropertyChanged(nameof(Privilege)); }
        }

        public ICommand HomeCommand { get; set; }
        public ICommand InputDataBatchCommand { get; set; }
        public ICommand KelolaDataCommand { get; set; }
        public ICommand LihatLaporanCommand { get; set; }
        public ICommand BendaharaCommand { get; set; }
        public ICommand UserManagerCommand { get; set; }

        public MainWindowViewModel()
        {
            Nama = string.IsNullOrEmpty(UserAccessService.Instance.CurrentNama) ? string.Empty : UserAccessService.Instance.CurrentNama;
            Privilege = string.IsNullOrEmpty(UserAccessService.Instance.CurrentPrivilege) ? string.Empty : UserAccessService.Instance.CurrentPrivilege; ;

            HomeCommand = new SimpleRelayCommand(new Action(Home));
            InputDataBatchCommand = new SimpleRelayCommand(new Action(InputDataBatch));
            KelolaDataCommand = new SimpleRelayCommand(new Action(KelolaData));
            LihatLaporanCommand = new SimpleRelayCommand(new Action(LihatLaporan));
            BendaharaCommand = new SimpleRelayCommand(new Action(Bendahara));
            UserManagerCommand = new SimpleRelayCommand(new Action(UserManager));

            CurrentView = new Home();
        }

        private void Home() => CurrentView = new Home();
        private void InputDataBatch() => CurrentView = new InputDataBatch();
        private void KelolaData() => CurrentView = new KelolaData();
        private void LihatLaporan() => CurrentView = new LihatLaporan();
        private void Bendahara() => CurrentView = new Bendahara();
        private void UserManager() => CurrentView = new UserManager();


    }
}
