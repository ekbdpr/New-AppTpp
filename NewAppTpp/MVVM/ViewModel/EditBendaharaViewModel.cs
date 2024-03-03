using HandyControl.Controls;
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
    internal class EditBendaharaViewModel : BindablePropertyBase
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

        public int CapaiKinerja
        {
            get { return _pegawaiModel.CapaiKinerja; }
            set { _pegawaiModel.CapaiKinerja = value; RaisePropertyChanged(nameof(CapaiKinerja)); }
        }

        public double PotonganPercentKehadiran
        {
            get { return _pegawaiModel.PotonganPercentKehadiran; }
            set { _pegawaiModel.PotonganPercentKehadiran = value; RaisePropertyChanged(nameof(PotonganPercentKehadiran)); }
        }

        public ICommand SaveCommand { get; }

        public EditBendaharaViewModel() 
        {
            InitializeSelectedPegawai();

            SaveCommand = new SimpleRelayCommand(new Action(Save));
        }

        private void InitializeSelectedPegawai()
        {
            Nip = BendaharaMiddlewareService.Instance.SelectedNip;
            Nama = BendaharaMiddlewareService.Instance.SelectedNama;
            CapaiKinerja = BendaharaMiddlewareService.Instance.SelectedCapaiKinerja;
            PotonganPercentKehadiran = BendaharaMiddlewareService.Instance.SelectedPercentKehadiran;
        }

        private async void Save()
        {
            try
            {
                await Task.Run(() => DataPegawaiService.UpdatePegawaiKinerjaKehadiran(Nip, Nama, CapaiKinerja, PotonganPercentKehadiran));
                BendaharaMiddlewareService.Instance.InvokeDataSaved();
            }
            catch (Exception ex)
            {
                Growl.Error($"Error during execute: {ex.Message}", "ErrorMsg");
            }
        }
    }
}
