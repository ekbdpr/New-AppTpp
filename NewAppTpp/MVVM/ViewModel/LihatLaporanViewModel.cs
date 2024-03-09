using HandyControl.Tools;
using HandyControl.Tools.Command;
using NewAppTpp.MVVM.Model;
using NewAppTpp.Services;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace NewAppTpp.MVVM.ViewModel
{
    internal class LihatLaporanViewModel : BindablePropertyBase
    {
        private ObservableCollection<DataPegawaiModel> _pegawaiList = [];
        public ObservableCollection<DataPegawaiModel> PegawaiList
        {
            get { return _pegawaiList; }
            set { _pegawaiList = value; RaisePropertyChanged(nameof(PegawaiList)); }
        }

        private string _bulan;
        public string Bulan
        {
            get { return _bulan; }
            set { _bulan = value; RaisePropertyChanged(nameof(Bulan)); }
        }

        private string _tahun;
        public string Tahun
        {
            get { return _tahun; }
            set { _tahun = value; RaisePropertyChanged(nameof(Tahun)); }
        }

        private string _selectedKepalaDinas;
        public string SelectedKepalaDinas
        {
            get { return _selectedKepalaDinas; }
            set
            { _selectedKepalaDinas = value; RaisePropertyChanged(nameof(SelectedKepalaDinas)); }
        }

        private string _selectedKasubag;
        public string SelectedKasubag
        {
            get { return _selectedKasubag; }
            set
            { _selectedKasubag = value; RaisePropertyChanged(nameof(SelectedKasubag)); }
        }

        public ICommand DownloadPdfCommand { get; }

        public LihatLaporanViewModel()
        {
            PegawaiList = new ObservableCollection<DataPegawaiModel>(DataPegawaiService.GetDataPegawai());

            DownloadPdfCommand = new RelayCommand(new Action<object>(DownloadPdf), new Func<object, bool>(CanDownload));
        }

        private bool CanDownload(object arg)
        {
            return !string.IsNullOrEmpty(Tahun) && 
                   !string.IsNullOrEmpty(Bulan) && 
                   !string.IsNullOrEmpty(SelectedKepalaDinas) && 
                   !string.IsNullOrEmpty(SelectedKasubag);
        }

        private void DownloadPdf(object obj)
        {
            string tglGaji = $"{Tahun}-{ConvertBulanToNumber()}-01".Trim();
            DataPegawaiService.ExportToPdf(tglGaji, Bulan, Tahun, SelectedKepalaDinas, SelectedKasubag);
        }

        private string ConvertBulanToNumber()
        {
            return Bulan switch
            {
                "Januari" => "01",
                "Februari" => "02",
                "Maret" => "03",
                "April" => "04",
                "Mei" => "05",
                "Juni" => "06",
                "Juli" => "07",
                "Agustus" => "08",
                "September" => "09",
                "Oktober" => "10",
                "November" => "11",
                "Desember" => "12",
                _ => "0",
            };
        }
    }
}
