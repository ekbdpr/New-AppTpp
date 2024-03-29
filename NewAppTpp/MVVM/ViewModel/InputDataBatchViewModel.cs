﻿using AppTpp.Exceptions;
using HandyControl.Controls;
using HandyControl.Tools;
using HandyControl.Tools.Command;
using Microsoft.Win32;
using NewAppTpp.MVVM.View;
using NewAppTpp.Services;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NewAppTpp.MVVM.ViewModel
{
    internal class InputDataBatchViewModel : BindablePropertyBase
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

        private string _fileName;
        public string FileName
        {
            get { return _fileName; }
            set { _fileName = value; RaisePropertyChanged(nameof(FileName)); }
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            set { _isLoading = value; RaisePropertyChanged(nameof(IsLoading)); }
        }

        private bool _isSuccess;
        public bool IsSuccess
        {
            get { return _isSuccess; }
            set { _isSuccess = value; RaisePropertyChanged(nameof(IsSuccess)); }
        }

        private bool _isError;
        public bool IsError
        {
            get { return _isError; }
            set { _isError = value; RaisePropertyChanged(nameof(IsError)); }
        }

        private Dialog InputDataBatchDialog { get; set; } = new Dialog();

        private string _filePath;

        private bool _isFileUploaded;

        public ICommand ChooseFileCommand { get; }
        public ICommand ImportFileCommand { get; }

        public InputDataBatchViewModel()
        {
            InitialFileState();
            InitialIconState();

            ChooseFileCommand = new SimpleRelayCommand(new Action(ChooseFile));
            ImportFileCommand = new RelayCommand(new Action<object>(ImportFile), new Func<object, bool>(CanImport));
        }

        private void InitialFileState()
        {
            FileName = "No File Choosen";
            _isFileUploaded = false;
        }

        private void InitialIconState()
        {
            IsLoading = false;
            IsSuccess = false;
            IsError = false;
        }

        private void ChooseFile()
        {
            OpenFileDialog openFileDialog = new()
            {
                Filter = "Excel Files (*.xlsx;*.xls)|*.xlsx;*.xls|All Files (*.*)|*.*"
            };
            ConfirmationPopupMiddlewareService.Instance.OnButtonClick += CloseConfirmationPopup;

            SaveFileToTempFolder(openFileDialog);
        }

        private void SaveFileToTempFolder(OpenFileDialog openFileDialog)
        {
            if (openFileDialog.ShowDialog() == true)
            {
                string selectedFileName = openFileDialog.FileName;
                string tempFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "temp");
                string destinationPath = Path.Combine(tempFolderPath, Path.GetFileName(selectedFileName));

                _filePath = destinationPath;

                CreateTempFolder(tempFolderPath);

                if (IsFileExists(destinationPath))
                {
                    File.Copy(selectedFileName, destinationPath, true);
                    FileName = Path.GetFileName(selectedFileName);

                    _isFileUploaded = true;
                    InitialIconState();
                }
                else
                {
                    return;
                }
            }
        }

        private static void CreateTempFolder(string folderPath)
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
        }

        private bool IsFileExists(string fileName)
        {
            if (File.Exists(fileName))
            {
                InputDataBatchDialog = Dialog.Show(new ConfirmationPopup("Berkas tersebut sudah ada. Apakah Anda ingin menggantinya?"));
                ConfirmationPopupMiddlewareService.NotifyConfirmationPopupUidChanged("DuplicateFileConfirmation");

                if (ConfirmationPopupMiddlewareService.Instance.ConfirmationState == false)
                {
                    return false;
                }
            }

            return true;

        }

        private bool CanImport(object arg)
        {
            return _isFileUploaded && !string.IsNullOrEmpty(Tahun) && !string.IsNullOrEmpty(Bulan);
        }

        private async void ImportFile(object obj)
        {
            try
            {
                InitialIconState();
                IsLoading = true;

                string tglGaji = $"{Tahun}-{ConvertBulanToNumber()}-01".Trim();

                await Task.Run(() => DataPegawaiService.ImportExcelToDatabase(_filePath, tglGaji));

                DeleteFile();
                InitialFileState();

                IsSuccess = true;
            }
            catch (DuplicateDataException ex)
            {
                Growl.Error($"Error during execute: {ex.Message}", "ErrorMsg");

                IsError = true;
            }
            finally
            {
                IsLoading = false;
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

        private void DeleteFile()
        {
            File.Delete(_filePath);
        }

        private void CloseConfirmationPopup()
        {
            InputDataBatchDialog.Close();
            ConfirmationPopupMiddlewareService.Instance.OnButtonClick -= CloseConfirmationPopup;
        }
    }
}
