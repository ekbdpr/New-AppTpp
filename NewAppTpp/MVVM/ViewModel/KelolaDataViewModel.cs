﻿using AppTpp.Services;
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
    internal class KelolaDataViewModel : BindablePropertyBase
    {
        private ObservableCollection<DataPegawaiModel> _pegawaiModelCollection = new();
        public ObservableCollection<DataPegawaiModel> PegawaiModelCollection
        {
            get { return _pegawaiModelCollection; }
            set { _pegawaiModelCollection = value; RaisePropertyChanged(nameof(PegawaiModelCollection)); }
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

        private DataPegawaiModel _selectedPegawai;
        public DataPegawaiModel SelectedPegawai
        {
            get { return _selectedPegawai; }
            set
            {
                if (value != null)
                {
                    _selectedPegawai = value;

                    RaisePropertyChanged(nameof(SelectedPegawai));
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
                    InitializeDataPegawaiList();
                    RaisePropertyChanged(nameof(PageIndex));
                }
            }
        }

        private Dialog DataPegawaiDialog { get; set; } = new Dialog();

        public ICommand SubmitSearchCommand { get; }
        public ICommand SearchPegawaiCommand { get; }
        public ICommand EditPegawaiCommand { get; }
        public ICommand DeletePegawaiCommand { get; }

        public KelolaDataViewModel()
        {
            SubmitSearchCommand = new SimpleRelayCommand(new Action(InitializeDataPegawaiList));
            SearchPegawaiCommand = new SimpleRelayCommand(new Action(SearchPegawai));

            EditPegawaiCommand = new SimpleRelayCommand(new Action(EditPegawai));
            DeletePegawaiCommand = new SimpleRelayCommand(new Action(OpenConfirmationPopup));

            KelolaDataMiddlewareService.Instance.OnDataSaved += InitializeDataPegawaiList;
            KelolaDataMiddlewareService.Instance.OnDeleteData += DeletePegawai;
        }

        private void InitializeDataPegawaiList()
        {
            try
            {
                string bulanAsString = ConvertBulanToNumber();

                var pegawaiData = DataPegawaiService.GetAllDataPegawai(Tahun, bulanAsString);
                var filteredPegawaiData = string.IsNullOrEmpty(SearchText) ? pegawaiData : pegawaiData.Where(data => data.Nama.Contains(SearchText, StringComparison.OrdinalIgnoreCase) || data.Nip.Contains(SearchText, StringComparison.OrdinalIgnoreCase));

                MaxPageCount = Convert.ToInt32(Math.Ceiling((double)filteredPegawaiData.Count() / 10.0));
                var startIndex = (PageIndex - 1) * 10;

                PegawaiModelCollection = new ObservableCollection<DataPegawaiModel>(filteredPegawaiData.Skip(startIndex).Take(10));
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
            finally
            {
                DataPegawaiDialog.Close();
            }
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

        private void SearchPegawai()
        {
            InitializeDataPegawaiList();
        }

        private void EditPegawai()
        {
            throw new NotImplementedException();
        }

        private void DeletePegawai()
        {
            DataPegawaiService.DeletePegawai(_selectedPegawai.Nip);
            InitializeDataPegawaiList();
            DataPegawaiDialog.Close();
        }

        private void OpenConfirmationPopup()
        {
            DataPegawaiDialog = Dialog.Show(new ConfirmationPopup($"Anda yakin akan menghapus {_selectedPegawai.Nip} - {_selectedPegawai.Nama} dari data pegawai ?"));
            ConfirmationPopupMiddlewareService.NotifyConfirmationPopupUidChanged("DeletePegawaiConfirmation");
        }
    }
}