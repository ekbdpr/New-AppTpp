using HandyControl.Tools;
using HandyControl.Tools.Command;
using NewAppTpp.Services;
using System;
using System.Windows.Input;

namespace NewAppTpp.MVVM.ViewModel
{
    internal class LihatLaporanViewModel : BindablePropertyBase
    {
        public ICommand DownloadExcelCommand { get; }

        public LihatLaporanViewModel() 
        {
            DownloadExcelCommand = new SimpleRelayCommand(new Action(DownloadExcel));
        }

        private void DownloadExcel()
        {
            DataPegawaiService.ExportToPdf();
        }
    }
}
