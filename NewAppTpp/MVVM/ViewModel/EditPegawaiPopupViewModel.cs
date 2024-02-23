using HandyControl.Data;
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
    internal class EditPegawaiPopupViewModel : BindablePropertyBase
    {
        private readonly DataPegawaiModel _pegawaiModel = new();

        public string Nip
        {
            get { return _pegawaiModel.Nip; }
            set { _pegawaiModel.Nip = value; RaisePropertyChanged(nameof(Nip)); }
        }

        public string Nama
        {
            get { return _pegawaiModel.Nama; }
            set { _pegawaiModel.Nama = value; RaisePropertyChanged(nameof(Nama)); }
        }

        public string KdSatker
        {
            get { return _pegawaiModel.KdSatker; }
            set { _pegawaiModel.KdSatker = value; RaisePropertyChanged(nameof(KdSatker)); }
        }

        public string Norek
        {
            get { return _pegawaiModel.Norek; }
            set { _pegawaiModel.Norek = value; RaisePropertyChanged(nameof(Norek)); }
        }

        public string KdPangkat
        {
            get { return _pegawaiModel.KdPangkat; }
            set { _pegawaiModel.KdPangkat = value; RaisePropertyChanged(nameof(KdPangkat)); }
        }

        public int Piwp
        {
            get { return _pegawaiModel.Piwp; }
            set { _pegawaiModel.Piwp = value; RaisePropertyChanged(nameof(Piwp)); }
        }

        public string NmSkpd
        {
            get { return _pegawaiModel.NmSkpd; }
            set { _pegawaiModel.NmSkpd = value; RaisePropertyChanged(nameof(NmSkpd)); }
        }

        public int PaguTppBk
        {
            get { return _pegawaiModel.PaguTppBk; }
            set { _pegawaiModel.PaguTppBk = value; RaisePropertyChanged(nameof(PaguTppBk)); }
        }

        public int PaguTppKk
        {
            get { return _pegawaiModel.PaguTppKk; }
            set { _pegawaiModel.PaguTppKk = value; RaisePropertyChanged(nameof(PaguTppKk)); }
        }

        public ICommand SaveCommand { get; }

        public EditPegawaiPopupViewModel()
        {
            InitializeSelectedPegawai();

            SaveCommand = new RelayCommand(new Action<object>(Save), new Func<object, bool>(CanSave));
        }

        private void InitializeSelectedPegawai()
        {
            Nip = KelolaDataMiddlewareService.Instance.SelectedNip;
            Nama = KelolaDataMiddlewareService.Instance.SelectedNama;
            KdSatker = KelolaDataMiddlewareService.Instance.SelectedKdSatker;
            Norek = KelolaDataMiddlewareService.Instance.SelectedNorek;
            KdPangkat = KelolaDataMiddlewareService.Instance.SelectedKdPangkat;
            Piwp = KelolaDataMiddlewareService.Instance.SelectedPiwp;
            NmSkpd = KelolaDataMiddlewareService.Instance.SelectedNmSkpd;
            PaguTppBk = KelolaDataMiddlewareService.Instance.SelectedPaguTppBk;
            PaguTppKk = KelolaDataMiddlewareService.Instance.SelectedPaguTppKk;
        }

        private bool CanSave(object arg)
        {
            return !string.IsNullOrWhiteSpace(Nip) &&
                   !string.IsNullOrWhiteSpace(Nama) &&
                   !string.IsNullOrWhiteSpace(KdSatker) &&
                   !string.IsNullOrWhiteSpace(Norek) &&
                   !string.IsNullOrWhiteSpace(KdPangkat) &&
                   !string.IsNullOrWhiteSpace(NmSkpd);
        }

        private async void Save(object obj)
        {
            try
            {
                await Task.Run(() => DataPegawaiService.UpdatePegawai(Nip, Nama, KdSatker, Norek, KdPangkat, Piwp, NmSkpd, PaguTppBk, PaguTppKk));
                KelolaDataMiddlewareService.Instance.InvokeDataSaved();
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
    }
}
