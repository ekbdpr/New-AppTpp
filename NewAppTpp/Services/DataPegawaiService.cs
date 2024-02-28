using MySqlConnector;
using OfficeOpenXml;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System;
using System.Configuration;
using AppTpp.Exceptions;
using NewAppTpp.MVVM.Model;
using HandyControl.Data;
using OfficeOpenXml.Style;
using Microsoft.Win32;
using HandyControl.Controls;

namespace NewAppTpp.Services
{
    internal class DataPegawaiService
    {
        private static DataPegawaiService _instance = new();
        public static DataPegawaiService Instance
        {
            get
            {
                _instance ??= new DataPegawaiService();

                return _instance;
            }
        }

        private static MySqlConnection OpenConnection()
        {
            string connectionString = GetConnectionString();
            var connection = new MySqlConnection(connectionString);

            connection.Open();
            return connection;
        }

        private static string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString;
        }

        public static void ImportExcelToDatabase(string filePath, string tahun, string bulan)
        {
            using var package = new ExcelPackage(new FileInfo(filePath!));
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var worksheet = package.Workbook.Worksheets[0];
            int startRow = 2;

            using var connection = OpenConnection();

            try
            {
                for (int row = startRow; row <= worksheet.Dimension.Rows; row++)
                {
                    string tglGaji = $"{tahun}-{bulan}-01".Trim();
                    string nip = worksheet.Cells[row, 1].Value?.ToString()?.Trim() ?? string.Empty;
                    string nama = worksheet.Cells[row, 2].Value?.ToString()?.Trim() ?? string.Empty;
                    string kdSatker = worksheet.Cells[row, 3].Value?.ToString()?.Trim() ?? string.Empty;
                    string norek = worksheet.Cells[row, 4].Value?.ToString()?.Trim() ?? string.Empty;
                    string kdPangkat = worksheet.Cells[row, 5].Value?.ToString()?.Trim() ?? string.Empty;
                    string piwp = worksheet.Cells[row, 6].Value?.ToString()?.Trim() ?? string.Empty;
                    string nmSkpd = worksheet.Cells[row, 7].Value?.ToString()?.Trim() ?? string.Empty;
                    string paguTppBk = worksheet.Cells[row, 8].Value?.ToString()?.Trim() ?? string.Empty;
                    string paguTppKk = worksheet.Cells[row, 9].Value?.ToString()?.Trim() ?? string.Empty;

                    string query = $"INSERT INTO data_pegawai (Tgl_Gaji, Nip, Nama, Kd_Satker, Norek, Kd_Pangkat, Piwp, Nm_Skpd, Pagu_TppBk, Pagu_TppKk) " +
                                    "VALUES (@Tgl_Gaji, @Nip, @Nama, @Kd_Satker, @Norek, @Kd_Pangkat, @Piwp, @Nm_Skpd, @Pagu_TppBk, @Pagu_TppKk);";

                    if (IsNipExist(nip, tglGaji))
                    {
                        throw new DuplicateDataException("Terdapat NIP yang terduplikasi / telah ada di database pada bulan yang sama !");
                    }

                    using MySqlCommand command = new(query, connection);

                    command.Parameters.AddWithValue("@Tgl_Gaji", tglGaji);
                    command.Parameters.AddWithValue("@Nip", nip);
                    command.Parameters.AddWithValue("@Nama", nama);
                    command.Parameters.AddWithValue("@Kd_Satker", kdSatker);
                    command.Parameters.AddWithValue("@Norek", norek);
                    command.Parameters.AddWithValue("@Kd_Pangkat", kdPangkat);
                    command.Parameters.AddWithValue("@Piwp", piwp);
                    command.Parameters.AddWithValue("@Nm_Skpd", nmSkpd);
                    command.Parameters.AddWithValue("@Pagu_TppBk", paguTppBk);
                    command.Parameters.AddWithValue("@Pagu_TppKk", paguTppKk);

                    command.ExecuteNonQuery();
                }

                Growl.Success($"Berhasil Import File Excel ke Database!", "SuccessMsg");
            }
            catch (MySqlException ex)
            {
                Growl.Error($"Error during execute: {ex.Message}", "ErrorMsg");
            }
        }

        private static bool IsNipExist(string nip, string tglGaji)
        {
            using var connection = OpenConnection();

            try
            {
                string query = "SELECT COUNT(*) FROM data_pegawai WHERE Nip = @Nip AND Tgl_Gaji = @Tgl_Gaji";

                using var command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@Nip", nip);
                command.Parameters.AddWithValue("@Tgl_Gaji", tglGaji);

                int count = Convert.ToInt32(command.ExecuteScalar());

                return count > 0;
            }
            catch (MySqlException ex)
            {
                Growl.Error($"Error during execute: {ex.Message}", "ErrorMsg");

                return false;
            }
        }

        public static List<DataPegawaiModel> GetAllDataPegawai(string tahun, string bulan)
        {
            CalculateKinerja();
            using var connection = OpenConnection();

            try
            {
                string query = $"SELECT * FROM data_pegawai WHERE Tgl_Gaji = @Tgl_Gaji ORDER BY Nama ASC";
                string tglGaji = $"{tahun}-{bulan}-01".Trim();

                using MySqlCommand command = new(query, connection);

                command.Parameters.AddWithValue("@Tgl_Gaji", tglGaji);

                List<DataPegawaiModel> pegawaiList = [];

                using MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {

                    DataPegawaiModel pegawaiModel = new()
                    {
                        TglGaji = reader["Tgl_Gaji"].ToString(),
                        Nip = reader["Nip"].ToString(),
                        Nama = reader["Nama"].ToString(),
                        KdSatker = reader["Kd_Satker"].ToString(),
                        Norek = reader["Norek"].ToString(),
                        KdPangkat = reader["Kd_Pangkat"].ToString(),
                        Piwp = Convert.ToInt32(reader["Piwp"]),
                        NmSkpd = reader["Nm_Skpd"].ToString(),
                        PaguTppBk = Convert.ToInt32(reader["Pagu_TppBk"]),
                        PaguTppKk = Convert.ToInt32(reader["Pagu_TppKk"]),
                        CapaiKinerja = Convert.ToInt32(reader["Cap_Kinerja"]),
                        PercentKehadiran = Convert.ToInt32(reader["Kehadiran"]),
                        KinerjaMaks = Convert.ToInt32(reader["Kinerja_Maks"]),
                        NilaiKinerja = Convert.ToInt32(reader["Nilai_Kinerja"]),
                        KehadiranMaks = Convert.ToInt32(reader["Kehadiran_Maks"]),
                        RpKehadiran = Convert.ToInt32(reader["Potongan_Kehadiran"]),
                        NilaiKehadiran = Convert.ToInt32(reader["Nilai_Kehadiran"])
                    };

                    pegawaiList.Add(pegawaiModel);
                }

                return pegawaiList;
            }
            catch (Exception ex)
            {
                Growl.Error($"Error during execute: {ex.Message}", "ErrorMsg");
                return [];
            }
        }

        public static void UpdatePegawai(string nip, string nama, string kdSatker, string norek, string kdPangkat, int piwp, string nmSkpd, int paguTppBk, int paguTppKk)
        {
            using var connection = OpenConnection();

            try
            {
                string query = "UPDATE data_pegawai SET Nama = @Nama, Kd_Satker = @Kd_Satker, Norek = @Norek, Kd_Pangkat = @Kd_Pangkat, Piwp = @Piwp, Nm_Skpd = @Nm_Skpd, Pagu_TppBk = @Pagu_TppBk, Pagu_TppKk = @Pagu_TppKk " +
                               "WHERE Nip = @Nip;";

                using var command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@Nip", nip);
                command.Parameters.AddWithValue("@Nama", nama);
                command.Parameters.AddWithValue("@Kd_Satker", kdSatker);
                command.Parameters.AddWithValue("@Norek", norek);
                command.Parameters.AddWithValue("@Kd_Pangkat", kdPangkat);
                command.Parameters.AddWithValue("@Piwp", piwp);
                command.Parameters.AddWithValue("@Nm_Skpd", nmSkpd);
                command.Parameters.AddWithValue("@Pagu_TppBk", paguTppBk);
                command.Parameters.AddWithValue("@Pagu_TppKk", paguTppKk);

                command.ExecuteNonQuery();

                Growl.Success($"{nip} - {nama} Berhasil Diubah!", "SuccessMsg");
            }
            catch (Exception ex)
            {
                Growl.Error($"Error during execute: {ex.Message}", "ErrorMsg");
            }
        }

        public static void DeletePegawai(string nip, string nama)
        {
            using var connection = OpenConnection();

            try
            {
                string query = "DELETE FROM data_pegawai WHERE Nip = @Nip";

                using var command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@Nip", nip);

                command.ExecuteNonQuery();

                Growl.Success($"{nip} - {nama} Berhasil Dihapus!", "SuccessMsg");
            }
            catch (Exception ex)
            {
                Growl.Error($"Error during execute: {ex.Message}", "ErrorMsg");
            }
        }

        public static void UpdatePegawaiKinerjaKehadiran(string nip, string nama, int capaiKinerja, double percentKehadiran)
        {
            using var connection = OpenConnection();

            try
            {
                string query = "UPDATE data_pegawai SET Cap_Kinerja = @Cap_Kinerja, Kehadiran = @Kehadiran WHERE Nip = @Nip;";

                using var command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@Cap_Kinerja", capaiKinerja);
                command.Parameters.AddWithValue("@Kehadiran", percentKehadiran);
                command.Parameters.AddWithValue("@Nip", nip);

                command.ExecuteNonQuery();

                Growl.Success($"Data {nip} - {nama} Berhasil Diubah!", "SuccessMsg");
            }
            catch (Exception ex)
            {
                Growl.Error($"Error during execute: {ex.Message}", "ErrorMsg");
            }
        }

        public static void CalculateKinerja()
        {
            using var connection = OpenConnection();

            try
            {
                string query = "UPDATE data_pegawai SET Kinerja_Maks = (Pagu_TppBk + Pagu_TppKk) * (60/100)," + " " +
                               "Nilai_Kinerja = (Cap_Kinerja / 100) * Kinerja_Maks," + " " +
                               "Kehadiran_Maks = (Pagu_TppBk + Pagu_TppKk) * (40/100)," + " " +
                               "Potongan_Kehadiran = (Kehadiran / 100) * Kehadiran_Maks," + " " +
                               "Nilai_Kehadiran = Kehadiran_Maks - Potongan_Kehadiran";

                using var command = new MySqlCommand(query, connection);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Growl.Error($"Error during execute: {ex.Message}", "ErrorMsg");
            }
        }

        public static void ExportToPdf()
        {
            try
            {
                using var excel = new ExcelPackage();

                var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
                workSheet.TabColor = System.Drawing.Color.Black;
                workSheet.PrinterSettings.PaperSize = ePaperSize.A4;
                workSheet.PrinterSettings.Orientation = eOrientation.Landscape;
                workSheet.PrinterSettings.FitToPage = true;
                workSheet.PrinterSettings.FitToWidth = 1;
                workSheet.Cells.Style.Font.Name = "Times New Roman";
                workSheet.Cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                //title style

                //baris pertama
                workSheet.Row(1).Style.Font.Size = 12;
                workSheet.Cells["A1:W1"].Merge = true;

                workSheet.Cells["A1"].Value = "LAPORAN PEMBAYARAN TAMBAHAN PENGHASILAN PEGAWAI";
                //end baris pertama

                //baris dua
                workSheet.Row(2).Style.Font.Size = 12;
                workSheet.Cells["A2:W2"].Merge = true;

                workSheet.Cells["A2"].Value = "DINAS KEPENDUDUKAN DAN PENCATATAN SIPIL";
                //end baris dua

                //baris tiga
                workSheet.Row(3).Style.Font.Size = 12;
                workSheet.Cells["A3:W3"].Merge = true;

                workSheet.Cells["A3"].Value = "FEBRUARI 2024";
                //end baris tiga

                //baris lima
                workSheet.Row(5).Style.Font.Size = 8;
                workSheet.Row(5).Style.Font.Name = "Times New Roman";
                workSheet.Row(5).Style.Font.Bold = true;
                workSheet.Row(5).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Row(5).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                workSheet.Row(5).Style.WrapText = true;
                workSheet.Cells["A5:X7"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                workSheet.Cells["A5:X7"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                workSheet.Cells["A5:X7"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                workSheet.Cells["A5:X7"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                workSheet.Cells["A5:A7"].Merge = true;
                workSheet.Cells["B5:B7"].Merge = true;
                workSheet.Cells["C5:C7"].Merge = true;
                workSheet.Cells["D5:D7"].Merge = true;
                workSheet.Cells["E5:E7"].Merge = true;
                workSheet.Cells["F5:F7"].Merge = true;
                workSheet.Cells["G5:G7"].Merge = true;
                workSheet.Cells["H5:H7"].Merge = true;
                workSheet.Cells["I5:I7"].Merge = true;
                workSheet.Cells["J5:P5"].Merge = true;
                workSheet.Cells["Q5:Q7"].Merge = true;
                workSheet.Cells["R5:R7"].Merge = true;
                workSheet.Cells["S5:S7"].Merge = true;
                workSheet.Cells["T5:T7"].Merge = true;
                workSheet.Cells["U5:U7"].Merge = true;
                workSheet.Cells["V5:V7"].Merge = true;
                workSheet.Cells["W5:W7"].Merge = true;
                workSheet.Cells["X5:X7"].Merge = true;

                workSheet.Cells["A5"].Value = "NO";
                workSheet.Cells["B5"].Value = "NAMA";
                workSheet.Cells["C5"].Value = "NIP";
                workSheet.Cells["D5"].Value = "GOL";
                workSheet.Cells["E5"].Value = "JABATAN";
                workSheet.Cells["F5"].Value = "JENIS JABATAN SESUAI PERPUB TPP";
                workSheet.Cells["G5"].Value = "KELAS JABATAN";
                workSheet.Cells["H5"].Value = "PAGU TPP = Beban Kerja + Kondisi Kerja";
                workSheet.Cells["I5"].Value = "PIWP";
                workSheet.Cells["J5"].Value = "BESARAN TPP";
                workSheet.Cells["Q5"].Value = "BPJS 1%";
                workSheet.Cells["R5"].Value = "TPP BRUTO";
                workSheet.Cells["S5"].Value = "PPH PSL 21";
                workSheet.Cells["T5"].Value = "TPP NETTO";
                workSheet.Cells["U5"].Value = "NILAI BRUTO SPM";
                workSheet.Cells["V5"].Value = "NO.REK";
                workSheet.Cells["W5"].Value = "IURAN 4% (DIBAYAR OLEH PEMDA)";
                workSheet.Cells["X5"].Value = "KETERANGAN";
                //end baris lima

                //baris enam
                workSheet.Row(6).Style.Font.Size = 8;
                workSheet.Row(6).Style.Font.Name = "Times New Roman";
                workSheet.Row(6).Style.Font.Bold = true;
                workSheet.Row(6).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Row(6).Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                workSheet.Cells["J6:L6"].Merge = true;
                workSheet.Cells["M6:P6"].Merge = true;

                workSheet.Cells["J6"].Value = "KINERJA 60%";
                workSheet.Cells["M6"].Value = "KINERJA 40%";
                //end baris enam

                //baris tujuh
                workSheet.Row(7).Height = 60;
                workSheet.Row(7).Style.Font.Size = 8;
                workSheet.Row(7).Style.Font.Name = "Times New Roman";
                workSheet.Row(7).Style.Font.Bold = true;
                workSheet.Row(7).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Row(7).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                workSheet.Row(7).Style.WrapText = true;

                workSheet.Cells["J7"].Value = "KINERJA MAKS (Rp)";
                workSheet.Cells["K7"].Value = "CAPAIAN KINERJA (%)";
                workSheet.Cells["L7"].Value = "NILAI KINERJA (Rp)";
                workSheet.Cells["M7"].Value = "KEHADIRAN MAKS (Rp)";
                workSheet.Cells["N7"].Value = "POTONGAN KEHADIRAN (%)";
                workSheet.Cells["O7"].Value = "POTONGAN KEHADIRAN (Rp)";
                workSheet.Cells["P7"].Value = "NILAI KEHADIRAN (Rp)";
                //end baris enam

                var saveFileDialog = new SaveFileDialog
                {
                    Filter = "PDF Files (*.pdf)|*.pdf|All Files (*.*)|*.*",
                    DefaultExt = "xslx",
                    FileName = $"{workSheet.Cells["A1"].Value}" + " " + $"{workSheet.Cells["A2"].Value}" + " " + $"{workSheet.Cells["A3"].Value}"
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    var excelStream = new MemoryStream(excel.GetAsByteArray());

                    Spire.Xls.Workbook workbook = new();
                    workbook.LoadFromStream(excelStream);

                    workbook.SaveToFile(saveFileDialog.FileName, Spire.Xls.FileFormat.PDF);

                    Growl.Success("File Berhasi Disimpan!", "SuccessMsg");
                }
            }
            catch (Exception ex)
            {
                Growl.Error($"Error during execute: {ex.Message}", "ErrorMsg");
            }
        }
    }
}
