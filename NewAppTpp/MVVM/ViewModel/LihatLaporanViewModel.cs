using HandyControl.Tools;
using HandyControl.Tools.Command;
using NewAppTpp.Services;
using System;
using System.Windows.Input;

namespace NewAppTpp.MVVM.ViewModel
{
    internal class LihatLaporanViewModel : BindablePropertyBase
    {
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

        public ICommand DownloadPdfCommand { get; }

        public LihatLaporanViewModel() 
        {
            DownloadPdfCommand = new RelayCommand(new Action<object>(DownloadPdf), new Func<object, bool>(CanDownload));
        }

        private bool CanDownload(object arg)
        {
            return !string.IsNullOrEmpty(Tahun) && !string.IsNullOrEmpty(Bulan);
        }

        private void DownloadPdf(object obj)
        {
            string tglGaji = $"{Tahun}-{ConvertBulanToNumber()}-01".Trim();
            DataPegawaiService.ExportToPdf(tglGaji, Bulan, Tahun);
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
