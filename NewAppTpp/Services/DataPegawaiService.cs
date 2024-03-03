using MySqlConnector;
using OfficeOpenXml;
using System.Collections.Generic;
using System.IO;
using System;
using System.Configuration;
using AppTpp.Exceptions;
using NewAppTpp.MVVM.Model;
using OfficeOpenXml.Style;
using Microsoft.Win32;
using HandyControl.Controls;
using System.Linq;

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

        public static void ImportExcelToDatabase(string filePath, string tglGaji)
        {
            using var package = new ExcelPackage(new FileInfo(filePath!));
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var worksheet = package.Workbook.Worksheets[0];
            int startRow = 2;

            using var connectionData = OpenConnection();
            using var connetionBk = OpenConnection();
            using var ConnectionKk = OpenConnection();

            try
            {
                for (int row = startRow; row <= worksheet.Dimension.Rows; row++)
                {
                    string nip = worksheet.Cells[row, 1].Value?.ToString()?.Trim() ?? string.Empty;
                    string nama = worksheet.Cells[row, 2].Value?.ToString()?.Trim() ?? string.Empty;
                    string kdSatker = worksheet.Cells[row, 3].Value?.ToString()?.Trim() ?? string.Empty;
                    string norek = worksheet.Cells[row, 4].Value?.ToString()?.Trim() ?? string.Empty;
                    string kdPangkat = worksheet.Cells[row, 5].Value?.ToString()?.Trim() ?? string.Empty;
                    string piwp = worksheet.Cells[row, 6].Value?.ToString()?.Trim() ?? string.Empty;
                    string nmSkpd = worksheet.Cells[row, 7].Value?.ToString()?.Trim() ?? string.Empty;
                    string paguTppBk = worksheet.Cells[row, 8].Value?.ToString()?.Trim() ?? string.Empty;
                    string paguTppKk = worksheet.Cells[row, 9].Value?.ToString()?.Trim() ?? string.Empty;

                    string queryDataPegawai = $"INSERT INTO data_pegawai (Nip, Nama, Kd_Satker, Norek, Kd_Pangkat, Nm_Skpd) " +
                                    "VALUES (@Nip, @Nama, @Kd_Satker, @Norek, @Kd_Pangkat, @Nm_Skpd);";

                    string queryTppBebanKerja = $"INSERT INTO tpp_beban_kerja (Nip, Tgl_Gaji, Tpp_Bk, Piwp) " +
                                    "VALUES (@Nip, @Tgl_Gaji, @Tpp_Bk, @Piwp);";

                    string queryTppKondisiKerja = $"INSERT INTO tpp_kondisi_kerja (Nip, Tgl_Gaji, Tpp_Kk) " +
                                    "VALUES (@Nip, @Tgl_Gaji, @Tpp_Kk);";

                    if (IsNipExist(nip, tglGaji))
                    {
                        throw new DuplicateDataException("Terdapat NIP yang terduplikasi / telah ada di database pada bulan yang sama !");
                    }

                    using MySqlCommand commandData = new(queryDataPegawai, connectionData);
                    using MySqlCommand commandBk = new(queryTppBebanKerja, connetionBk);
                    using MySqlCommand commandKk = new(queryTppKondisiKerja, ConnectionKk);

                    commandData.Parameters.AddWithValue("@Nip", nip);
                    commandData.Parameters.AddWithValue("@Nama", nama);
                    commandData.Parameters.AddWithValue("@Kd_Satker", kdSatker);
                    commandData.Parameters.AddWithValue("@Norek", norek);
                    commandData.Parameters.AddWithValue("@Kd_Pangkat", kdPangkat);
                    commandData.Parameters.AddWithValue("@Nm_Skpd", nmSkpd);

                    commandBk.Parameters.AddWithValue("@Nip", nip);
                    commandBk.Parameters.AddWithValue("@Tgl_Gaji", tglGaji);
                    commandBk.Parameters.AddWithValue("@Tpp_Bk", paguTppBk);
                    commandBk.Parameters.AddWithValue("@Piwp", piwp);

                    commandKk.Parameters.AddWithValue("@Nip", nip);
                    commandKk.Parameters.AddWithValue("@Tgl_Gaji", tglGaji);
                    commandKk.Parameters.AddWithValue("@Tpp_Kk", paguTppKk);

                    commandData.ExecuteNonQuery();
                    commandBk.ExecuteNonQuery();
                    commandKk.ExecuteNonQuery();
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
                string query = "SELECT COUNT(*) FROM tpp_beban_kerja WHERE Nip = @Nip AND Tgl_Gaji = @Tgl_Gaji";

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

        public static List<DataPegawaiModel> GetAllDataPegawai(string tglGaji)
        {
            CalculateKinerja();
            using var connectionData = OpenConnection();
            using var connectionBk = OpenConnection();
            using var ConnectionKk = OpenConnection();

            try
            {
                List<DataPegawaiModel> pegawaiList = [];

                string queryDataPegawai = "SELECT Nip, Nama, Kd_Satker, Norek, Kd_Pangkat, Nm_Skpd FROM data_pegawai ORDER BY Nama ASC";
                string queryTppBebanKerja = "SELECT Nip, Tgl_Gaji, Tpp_Bk, Piwp, Cap_Kinerja, Potongan_Percent_Kehadiran, Tpp_Netto FROM tpp_beban_kerja WHERE Tgl_Gaji = @Tgl_Gaji";
                string queryTppKondisiKerja = "SELECT Nip, Tgl_Gaji, Tpp_Kk, Cap_Kinerja, Potongan_Percent_Kehadiran, Tpp_Netto FROM tpp_kondisi_kerja WHERE Tgl_Gaji = @Tgl_Gaji";

                using MySqlCommand commandData = new(queryDataPegawai, connectionData);
                using MySqlCommand commandBk = new(queryTppBebanKerja, connectionBk);
                using MySqlCommand commandKk = new(queryTppKondisiKerja, ConnectionKk);

                commandBk.Parameters.AddWithValue("@Tgl_Gaji", tglGaji);
                commandKk.Parameters.AddWithValue("@Tgl_Gaji", tglGaji);

                using MySqlDataReader readerData = commandData.ExecuteReader();
                while (readerData.Read())
                {

                    DataPegawaiModel pegawaiModel = new()
                    {
                        Nip = readerData["Nip"].ToString(),
                        Nama = readerData["Nama"].ToString(),
                        KdSatker = readerData["Kd_Satker"].ToString(),
                        Norek = readerData["Norek"].ToString(),
                        KdPangkat = readerData["Kd_Pangkat"].ToString(),
                        NmSkpd = readerData["Nm_Skpd"].ToString(),
                    };

                    pegawaiList.Add(pegawaiModel);
                }

                using MySqlDataReader readerBk = commandBk.ExecuteReader();
                while (readerBk.Read())
                {
                    string nip = readerBk["Nip"].ToString();
                    DataPegawaiModel existingPegawai = pegawaiList.FirstOrDefault(p => p.Nip == nip);

                    if (existingPegawai != null)
                    {
                        existingPegawai.TglGaji = readerBk["Tgl_Gaji"].ToString();
                        existingPegawai.PaguTppBk = Convert.ToInt32(readerBk["Tpp_Bk"]);
                        existingPegawai.Piwp = Convert.ToInt32(readerBk["Piwp"]);
                        existingPegawai.CapaiKinerja = Convert.ToInt32(readerBk["Cap_Kinerja"]);
                        existingPegawai.PotonganPercentKehadiran = Convert.ToInt32(readerBk["Potongan_Percent_Kehadiran"]);
                        existingPegawai.Tpp_Netto += Convert.ToInt32(readerBk["Tpp_Netto"]);
                    }
                }

                using MySqlDataReader readerKk = commandKk.ExecuteReader();
                while (readerKk.Read())
                {
                    string nip = readerKk["Nip"].ToString();
                    DataPegawaiModel existingPegawai = pegawaiList.FirstOrDefault(p => p.Nip == nip);

                    if (existingPegawai != null)
                    {
                        existingPegawai.TglGaji = readerKk["Tgl_Gaji"].ToString();
                        existingPegawai.PaguTppKk = Convert.ToInt32(readerKk["Tpp_Kk"]);
                        existingPegawai.CapaiKinerja = Convert.ToInt32(readerKk["Cap_Kinerja"]);
                        existingPegawai.PotonganPercentKehadiran = Convert.ToInt32(readerKk["Potongan_Percent_Kehadiran"]);
                        existingPegawai.Tpp_Netto += Convert.ToInt32(readerKk["Tpp_Netto"]);
                    }
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
            using var connectionData = OpenConnection();
            using var connectionBk = OpenConnection();
            using var connectionKk = OpenConnection();

            try
            {
                string queryDataPegawai = "UPDATE data_pegawai SET Nama = @Nama, Kd_Satker = @Kd_Satker, Norek = @Norek, Kd_Pangkat = @Kd_Pangkat, Nm_Skpd = @Nm_Skpd " +
                               "WHERE Nip = @Nip;";

                string queryTbbBebanKerja = "UPDATE tpp_beban_kerja SET Tpp_Bk = @Tpp_Bk, Piwp = @Piwp WHERE Nip = @Nip";

                string queryTppKondisiKerja = "UPDATE tpp_kondisi_kerja SET Tpp_Kk = @Tpp_Kk WHERE Nip = @Nip";

                using var commandData = new MySqlCommand(queryDataPegawai, connectionData);
                using var commandBk = new MySqlCommand(queryTbbBebanKerja, connectionBk);
                using var commandKk = new MySqlCommand(queryTppKondisiKerja, connectionKk);

                commandData.Parameters.AddWithValue("@Nip", nip);
                commandData.Parameters.AddWithValue("@Nama", nama);
                commandData.Parameters.AddWithValue("@Kd_Satker", kdSatker);
                commandData.Parameters.AddWithValue("@Norek", norek);
                commandData.Parameters.AddWithValue("@Kd_Pangkat", kdPangkat);
                commandData.Parameters.AddWithValue("@Nm_Skpd", nmSkpd);

                commandBk.Parameters.AddWithValue("@Nip", nip);
                commandBk.Parameters.AddWithValue("@Piwp", piwp);
                commandBk.Parameters.AddWithValue("@Tpp_Bk", paguTppBk);

                commandKk.Parameters.AddWithValue("@Nip", nip);
                commandKk.Parameters.AddWithValue("@Tpp_Kk", paguTppKk);

                commandData.ExecuteNonQuery();
                commandBk.ExecuteNonQuery();
                commandKk.ExecuteNonQuery();

                Growl.Success($"{nip} - {nama} Berhasil Diubah!", "SuccessMsg");
            }
            catch (Exception ex)
            {
                Growl.Error($"Error during execute: {ex.Message}", "ErrorMsg");
            }
        }

        public static void DeletePegawai(string nip, string nama)
        {
            using var connectionData = OpenConnection();
            using var connectionBk = OpenConnection();
            using var connectionKk = OpenConnection();

            try
            {
                string queryDataPegawai = "DELETE FROM data_pegawai WHERE Nip = @Nip";
                string queryTppBebanKerja = "DELETE FROM tpp_beban_kerja WHERE Nip = @Nip";
                string queryTppKondisiKerja = "DELETE FROM tpp_kondisi_kerja WHERE Nip = @Nip";

                using var commandData = new MySqlCommand(queryDataPegawai, connectionData);
                using var commandBk = new MySqlCommand(queryTppBebanKerja, connectionBk);
                using var commandKk = new MySqlCommand(queryTppKondisiKerja, connectionKk);

                commandData.Parameters.AddWithValue("@Nip", nip);
                commandBk.Parameters.AddWithValue("@Nip", nip);
                commandKk.Parameters.AddWithValue("@Nip", nip);

                commandBk.ExecuteNonQuery();
                commandKk.ExecuteNonQuery();
                commandData.ExecuteNonQuery();

                Growl.Success($"{nip} - {nama} Berhasil Dihapus!", "SuccessMsg");
            }
            catch (Exception ex)
            {
                Growl.Error($"Error during execute: {ex.Message}", "ErrorMsg");
            }
        }

        public static void UpdatePegawaiKinerjaKehadiran(string nip, string nama, int capaiKinerja, double percentKehadiran)
        {
            using var connectionBk = OpenConnection();
            using var connectionKk = OpenConnection();

            try
            {
                string queryTppBebanKerja = "UPDATE tpp_beban_kerja SET Cap_Kinerja = @Cap_Kinerja, Potongan_Percent_Kehadiran = @Potongan_Percent_Kehadiran WHERE Nip = @Nip;";
                string queryTppKondisiKerja = "UPDATE tpp_kondisi_kerja SET Cap_Kinerja = @Cap_Kinerja, Potongan_Percent_Kehadiran = @Potongan_Percent_Kehadiran WHERE Nip = @Nip;";

                using var commandBk = new MySqlCommand(queryTppBebanKerja, connectionBk);
                using var commandKk = new MySqlCommand(queryTppKondisiKerja, connectionKk);

                commandBk.Parameters.AddWithValue("@Cap_Kinerja", capaiKinerja);
                commandBk.Parameters.AddWithValue("@Potongan_Percent_Kehadiran", percentKehadiran);
                commandBk.Parameters.AddWithValue("@Nip", nip);

                commandKk.Parameters.AddWithValue("@Cap_Kinerja", capaiKinerja);
                commandKk.Parameters.AddWithValue("@Potongan_Percent_Kehadiran", percentKehadiran);
                commandKk.Parameters.AddWithValue("@Nip", nip);

                commandBk.ExecuteNonQuery();
                commandKk.ExecuteNonQuery();

                Growl.Success($"Data {nip} - {nama} Berhasil Diubah!", "SuccessMsg");
            }
            catch (Exception ex)
            {
                Growl.Error($"Error during execute: {ex.Message}", "ErrorMsg");
            }
        }

        public static void CalculateKinerja()
        {
            using var connectionBk = OpenConnection();
            using var connectionKk = OpenConnection();

            try
            {
                string queryTppBebanKerja = "UPDATE tpp_beban_kerja AS tbk " +
                               "INNER JOIN data_pegawai AS dp ON tbk.Nip = dp.Nip " +
                               "SET " +
                               "tbk.Tpp_Bk = CASE WHEN tbk.Potongan_Percent_Kehadiran >= 15 THEN 0 ELSE tbk.Tpp_Bk END," +
                               "tbk.Kinerja_Maks = CASE WHEN tbk.Potongan_Percent_Kehadiran >= 15 THEN 0 ELSE tbk.Tpp_Bk * 0.6 END," +
                               "tbk.Nilai_Kinerja = (tbk.Cap_Kinerja / 100) * tbk.Kinerja_Maks," +
                               "tbk.Kehadiran_Maks = CASE WHEN tbk.Potongan_Percent_Kehadiran >= 15 THEN 0 ELSE tbk.Tpp_Bk * 0.4 END," +
                               "tbk.Potongan_Kehadiran = (tbk.Potongan_Percent_Kehadiran / 100) * tbk.Kehadiran_Maks," +
                               "tbk.Nilai_Kehadiran = CASE WHEN tbk.Potongan_Percent_Kehadiran >= 15 THEN 0 ELSE tbk.Kehadiran_Maks - tbk.Potongan_Kehadiran END," +
                               "tbk.Bpjs = CASE WHEN (tbk.Bpjs + tbk.Piwp) > 120000 THEN tbk.Tpp_Bk * 0.01 - (tbk.Bpjs + tbk.Piwp - 120000) ELSE tbk.Tpp_Bk * 0.01 END," +
                               "tbk.Tpp_Bruto = tbk.Nilai_Kinerja + tbk.Nilai_Kehadiran - tbk.Bpjs," +
                               "tbk.Pph = CASE WHEN dp.Kd_Pangkat LIKE '%4%' THEN tbk.Tpp_Bruto * 0.15 WHEN dp.Kd_Pangkat LIKE '%3%' THEN tbk.Tpp_Bruto * 0.05 ELSE 0 END," +
                               "tbk.Tpp_Netto = CASE WHEN tbk.Potongan_Percent_Kehadiran >= 15 THEN 0 ELSE tbk.Tpp_Bruto - tbk.Pph END," +
                               "tbk.Iuran = tbk.Tpp_Bk * 0.04," +
                               "tbk.Nilai_Bruto_Spm = tbk.Nilai_Kinerja + tbk.Nilai_Kehadiran + tbk.Iuran";

                string queryTppKondisiKerja = "UPDATE tpp_kondisi_kerja AS tkk " +
                               "INNER JOIN data_pegawai AS dp ON tkk.Nip = dp.Nip " +
                               "SET " +
                               "tkk.Tpp_Kk = CASE WHEN tkk.Potongan_Percent_Kehadiran >= 15 THEN 0 ELSE tkk.Tpp_Kk END," +
                               "tkk.Kinerja_Maks = CASE WHEN tkk.Potongan_Percent_Kehadiran >= 15 THEN 0 ELSE tkk.Tpp_Kk * 0.6 END," +
                               "tkk.Nilai_Kinerja = (tkk.Cap_Kinerja / 100) * tkk.Kinerja_Maks," +
                               "tkk.Kehadiran_Maks = CASE WHEN tkk.Potongan_Percent_Kehadiran >= 15 THEN 0 ELSE tkk.Tpp_Kk * 0.4 END," +
                               "tkk.Potongan_Kehadiran = (tkk.Potongan_Percent_Kehadiran / 100) * tkk.Kehadiran_Maks," +
                               "tkk.Nilai_Kehadiran = CASE WHEN tkk.Potongan_Percent_Kehadiran >= 15 THEN 0 ELSE tkk.Kehadiran_Maks - tkk.Potongan_Kehadiran END," +
                               "tkk.Bpjs = CASE WHEN (tkk.Bpjs + tkk.Piwp) > 120000 THEN tkk.Tpp_Kk * 0.01 - (tkk.Bpjs + tkk.Piwp - 120000) ELSE tkk.Tpp_Kk * 0.01 END," +
                               "tkk.Tpp_Bruto = tkk.Nilai_Kinerja + tkk.Nilai_Kehadiran - tkk.Bpjs," +
                               "tkk.Pph = CASE WHEN dp.Kd_Pangkat LIKE '%4%' THEN tkk.Tpp_Bruto * 0.15 WHEN dp.Kd_Pangkat LIKE '%3%' THEN tkk.Tpp_Bruto * 0.05 ELSE 0 END," +
                               "tkk.Tpp_Netto = CASE WHEN tkk.Potongan_Percent_Kehadiran >= 15 THEN 0 ELSE tkk.Tpp_Bruto - tkk.Pph END," +
                               "tkk.Iuran = tkk.Tpp_Kk * 0.04," +
                               "tkk.Nilai_Bruto_Spm = tkk.Nilai_Kinerja + tkk.Nilai_Kehadiran + tkk.Iuran";

                using var commandBk = new MySqlCommand(queryTppBebanKerja, connectionBk);
                using var commandKk = new MySqlCommand(queryTppKondisiKerja, connectionKk);

                commandBk.ExecuteNonQuery();
                commandKk.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Growl.Error($"Error during execute: {ex.Message}", "ErrorMsg");
            }
        }

        public static void ExportToPdf(string tglGaji, string bulan, string tahun)
        {
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
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

                workSheet.Column(1).Width = 5;
                workSheet.Column(2).Width = 20;
                workSheet.Column(3).Width = 15;
                //title style

                //baris pertama
                workSheet.Row(1).Style.Font.Size = 12;
                workSheet.Cells["A1:X1"].Merge = true;

                workSheet.Cells["A1"].Value = "LAPORAN PEMBAYARAN TAMBAHAN PENGHASILAN PEGAWAI";
                //end baris pertama

                //baris dua
                workSheet.Row(2).Style.Font.Size = 12;
                workSheet.Cells["A2:X2"].Merge = true;

                workSheet.Cells["A2"].Value = "DINAS KEPENDUDUKAN DAN PENCATATAN SIPIL";
                //end baris dua

                //baris tiga
                workSheet.Row(3).Style.Font.Size = 12;
                workSheet.Cells["A3:X3"].Merge = true;

                workSheet.Cells["A3"].Value = $"{bulan.ToUpper()} {tahun}";
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

                List<DataExportModel> list = GetExportableData(tglGaji);

                for (int i = 0; i < list.Count; i++)
                {
                    int headerCol = i + 7;

                    //baris isi tabel
                    workSheet.Cells[$"A{i + headerCol + 1}:X{i + headerCol + 2}"].Style.Font.Name = "Times New Roman";
                    workSheet.Cells[$"A{i + headerCol + 1}:X{i + headerCol + 2}"].Style.Font.Size = 7;
                    workSheet.Cells[$"A{i + headerCol + 1}:X{i + headerCol + 2}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Cells[$"A{i + headerCol + 1}:X{i + headerCol + 2}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    workSheet.Cells[$"A{i + headerCol + 1}:X{i + headerCol + 2}"].Style.WrapText = true;

                    workSheet.Cells[$"A{i + headerCol + 1}:X{i + headerCol + 2}"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells[$"A{i + headerCol + 1}:X{i + headerCol + 2}"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells[$"A{i + headerCol + 1}:X{i + headerCol + 2}"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells[$"A{i + headerCol + 1}:X{i + headerCol + 2}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                    workSheet.Cells[$"B{i + headerCol + 1}:C{i + headerCol + 2}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    workSheet.Cells[$"H{i + headerCol + 1}:J{i + headerCol + 2}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    workSheet.Cells[$"L{i + headerCol + 1}:M{i + headerCol + 2}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    workSheet.Cells[$"O{i + headerCol + 1}:U{i + headerCol + 2}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    workSheet.Cells[$"W{i + headerCol + 1}:X{i + headerCol + 2}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    workSheet.Cells[$"H{i + headerCol + 1}:X{i + headerCol + 2}"].Style.Numberformat.Format = "#,##0";
                    
                    workSheet.Cells[$"A{i + headerCol + 1}:A{i + headerCol + 2}"].Merge = true;
                    workSheet.Cells[$"B{i + headerCol + 1}:B{i + headerCol + 2}"].Merge = true;
                    workSheet.Cells[$"C{i + headerCol + 1}:C{i + headerCol + 2}"].Merge = true;
                    workSheet.Cells[$"D{i + headerCol + 1}:D{i + headerCol + 2}"].Merge = true;
                    workSheet.Cells[$"E{i + headerCol + 1}:E{i + headerCol + 2}"].Merge = true;
                    workSheet.Cells[$"F{i + headerCol + 1}:F{i + headerCol + 2}"].Merge = true;
                    workSheet.Cells[$"G{i + headerCol + 1}:G{i + headerCol + 2}"].Merge = true;
                    workSheet.Cells[$"V{i + headerCol + 1}:V{i + headerCol + 2}"].Merge = true;

                    workSheet.Cells[$"A{i + headerCol + 1}"].Value = i + 1;
                    workSheet.Cells[$"B{i + headerCol + 1}"].Value = list[i].Nama;
                    workSheet.Cells[$"C{i + headerCol + 1}"].Value = list[i].Nip;
                    workSheet.Cells[$"D{i + headerCol + 1}"].Value = list[i].KdPangkat;

                    workSheet.Cells[$"H{i + headerCol + 1}"].Value = list[i].PaguTppBk;
                    workSheet.Cells[$"H{i + headerCol + 2}"].Value = list[i].PaguTppKk;

                    workSheet.Cells[$"I{i + headerCol + 1}"].Value = list[i].PiwpBk;
                    workSheet.Cells[$"I{i + headerCol + 2}"].Value = list[i].PiwpKk;

                    workSheet.Cells[$"J{i + headerCol + 1}"].Value = list[i].KinerjaMaksBk;
                    workSheet.Cells[$"J{i + headerCol + 2}"].Value = list[i].KinerjaMaksKk;

                    workSheet.Cells[$"K{i + headerCol + 1}"].Value = list[i].CapaiKinerjaBk;
                    workSheet.Cells[$"K{i + headerCol + 2}"].Value = list[i].CapaiKinerjaKk;

                    workSheet.Cells[$"L{i + headerCol + 1}"].Value = list[i].NilaiKinerjaBk;
                    workSheet.Cells[$"L{i + headerCol + 2}"].Value = list[i].NilaiKinerjaKk;

                    workSheet.Cells[$"M{i + headerCol + 1}"].Value = list[i].KehadiranMaksBk;
                    workSheet.Cells[$"M{i + headerCol + 2}"].Value = list[i].KehadiranMaksKk;

                    workSheet.Cells[$"N{i + headerCol + 1}"].Value = list[i].PotonganPercentKehadiranBk;
                    workSheet.Cells[$"N{i + headerCol + 2}"].Value = list[i].PotonganPercentKehadiranKk;

                    workSheet.Cells[$"O{i + headerCol + 1}"].Value = list[i].RpKehadiranBk;
                    workSheet.Cells[$"O{i + headerCol + 2}"].Value = list[i].RpKehadiranKk;

                    workSheet.Cells[$"P{i + headerCol + 1}"].Value = list[i].NilaiKehadiranBk;
                    workSheet.Cells[$"P{i + headerCol + 2}"].Value = list[i].NilaiKehadiranKk;

                    workSheet.Cells[$"Q{i + headerCol + 1}"].Value = list[i].BpjsBk;
                    workSheet.Cells[$"Q{i + headerCol + 2}"].Value = list[i].BpjsKk;

                    workSheet.Cells[$"R{i + headerCol + 1}"].Value = list[i].Tpp_BrutoBk;
                    workSheet.Cells[$"R{i + headerCol + 2}"].Value = list[i].Tpp_BrutoKk;

                    workSheet.Cells[$"S{i + headerCol + 1}"].Value = list[i].PphBk;
                    workSheet.Cells[$"S{i + headerCol + 2}"].Value = list[i].PphKk;

                    workSheet.Cells[$"T{i + headerCol + 1}"].Value = list[i].Tpp_NettoBk;
                    workSheet.Cells[$"T{i + headerCol + 2}"].Value = list[i].Tpp_NettoKk;

                    workSheet.Cells[$"U{i + headerCol + 1}"].Value = list[i].Nilai_Bruto_SpmBk;
                    workSheet.Cells[$"U{i + headerCol + 2}"].Value = list[i].Nilai_Bruto_SpmKk;

                    workSheet.Cells[$"V{i + headerCol + 1}"].Value = list[i].Norek;

                    workSheet.Cells[$"W{i + headerCol + 1}"].Value = list[i].IuranBk;
                    workSheet.Cells[$"W{i + headerCol + 2}"].Value = list[i].IuranKk;
                    //end baris isi tabel
                }

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

        private static List<DataExportModel> GetExportableData(string tglGaji)
        {
            CalculateKinerja();
            using var connectionData = OpenConnection();
            using var connectionBk = OpenConnection();
            using var ConnectionKk = OpenConnection();

            try
            {
                List<DataExportModel> pegawaiList = [];

                string queryDataPegawai = "SELECT Nip, Nama, Norek, Kd_Pangkat FROM data_pegawai ORDER BY Nama ASC";
                string queryTppBebanKerja = "SELECT * FROM tpp_beban_kerja WHERE Tgl_Gaji = @Tgl_Gaji";
                string queryTppKondisiKerja = "SELECT * FROM tpp_kondisi_kerja WHERE Tgl_Gaji = @Tgl_Gaji";

                using MySqlCommand commandData = new(queryDataPegawai, connectionData);
                using MySqlCommand commandBk = new(queryTppBebanKerja, connectionBk);
                using MySqlCommand commandKk = new(queryTppKondisiKerja, ConnectionKk);

                commandBk.Parameters.AddWithValue("@Tgl_Gaji", tglGaji);
                commandKk.Parameters.AddWithValue("@Tgl_Gaji", tglGaji);

                using MySqlDataReader readerData = commandData.ExecuteReader();
                while (readerData.Read())
                {

                    DataExportModel pegawaiModel = new()
                    {
                        Nip = readerData["Nip"].ToString(),
                        Nama = readerData["Nama"].ToString(),
                        Norek = readerData["Norek"].ToString(),
                        KdPangkat = readerData["Kd_Pangkat"].ToString(),
                    };

                    pegawaiList.Add(pegawaiModel);
                }

                using MySqlDataReader readerBk = commandBk.ExecuteReader();
                while (readerBk.Read())
                {
                    string nip = readerBk["Nip"].ToString();
                    DataExportModel existingPegawai = pegawaiList.FirstOrDefault(p => p.Nip == nip);

                    if (existingPegawai != null)
                    {
                        existingPegawai.TglGaji = readerBk["Tgl_Gaji"].ToString();
                        existingPegawai.PaguTppBk = Convert.ToInt32(readerBk["Tpp_Bk"]);
                        existingPegawai.PiwpBk = Convert.ToInt32(readerBk["Piwp"]);
                        existingPegawai.KinerjaMaksBk = Convert.ToInt32(readerBk["Kinerja_Maks"]);
                        existingPegawai.CapaiKinerjaBk = Convert.ToInt32(readerBk["Cap_Kinerja"]);
                        existingPegawai.NilaiKinerjaBk = Convert.ToInt32(readerBk["Nilai_Kinerja"]);
                        existingPegawai.KehadiranMaksBk = Convert.ToInt32(readerBk["Kehadiran_Maks"]);
                        existingPegawai.PotonganPercentKehadiranBk = Convert.ToInt32(readerBk["Potongan_Percent_Kehadiran"]);
                        existingPegawai.RpKehadiranBk = Convert.ToInt32(readerBk["Potongan_Kehadiran"]);
                        existingPegawai.NilaiKehadiranBk = Convert.ToInt32(readerBk["Nilai_Kehadiran"]);
                        existingPegawai.BpjsBk = Convert.ToInt32(readerBk["Bpjs"]);
                        existingPegawai.Tpp_BrutoBk = Convert.ToInt32(readerBk["Tpp_Bruto"]);
                        existingPegawai.PphBk = Convert.ToInt32(readerBk["Pph"]);
                        existingPegawai.Tpp_NettoBk = Convert.ToInt32(readerBk["Tpp_Netto"]);
                        existingPegawai.Nilai_Bruto_SpmBk = Convert.ToInt32(readerBk["Nilai_Bruto_Spm"]);
                        existingPegawai.IuranBk = Convert.ToInt32(readerBk["Iuran"]);
                    }
                }

                using MySqlDataReader readerKk = commandKk.ExecuteReader();
                while (readerKk.Read())
                {
                    string nip = readerKk["Nip"].ToString();
                    DataExportModel existingPegawai = pegawaiList.FirstOrDefault(p => p.Nip == nip);

                    if (existingPegawai != null)
                    {
                        existingPegawai.TglGaji = readerKk["Tgl_Gaji"].ToString();
                        existingPegawai.PaguTppKk = Convert.ToInt32(readerKk["Tpp_Kk"]);
                        existingPegawai.PiwpKk = Convert.ToInt32(readerKk["Piwp"]);
                        existingPegawai.KinerjaMaksKk = Convert.ToInt32(readerKk["Kinerja_Maks"]);
                        existingPegawai.CapaiKinerjaKk = Convert.ToInt32(readerKk["Cap_Kinerja"]);
                        existingPegawai.NilaiKinerjaKk = Convert.ToInt32(readerKk["Nilai_Kinerja"]);
                        existingPegawai.KehadiranMaksKk = Convert.ToInt32(readerKk["Kehadiran_Maks"]);
                        existingPegawai.PotonganPercentKehadiranKk = Convert.ToInt32(readerKk["Potongan_Percent_Kehadiran"]);
                        existingPegawai.RpKehadiranKk = Convert.ToInt32(readerKk["Potongan_Kehadiran"]);
                        existingPegawai.NilaiKehadiranKk = Convert.ToInt32(readerKk["Nilai_Kehadiran"]);
                        existingPegawai.BpjsKk = Convert.ToInt32(readerKk["Bpjs"]);
                        existingPegawai.Tpp_BrutoKk = Convert.ToInt32(readerKk["Tpp_Bruto"]);
                        existingPegawai.PphKk = Convert.ToInt32(readerKk["Pph"]);
                        existingPegawai.Tpp_NettoKk = Convert.ToInt32(readerKk["Tpp_Netto"]);
                        existingPegawai.Nilai_Bruto_SpmKk = Convert.ToInt32(readerKk["Nilai_Bruto_Spm"]);
                        existingPegawai.IuranKk = Convert.ToInt32(readerKk["Iuran"]);
                    }
                }

                return pegawaiList;
            }
            catch (Exception ex)
            {
                Growl.Error($"Error during execute: {ex.Message}", "ErrorMsg");
                return [];
            }
        }
    }
}
