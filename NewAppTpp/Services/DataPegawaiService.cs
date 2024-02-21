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
                    string piwp1 = worksheet.Cells[row, 6].Value?.ToString()?.Trim() ?? string.Empty;
                    string nmSkpd = worksheet.Cells[row, 7].Value?.ToString()?.Trim() ?? string.Empty;
                    string paguTppBk = worksheet.Cells[row, 8].Value?.ToString()?.Trim() ?? string.Empty;
                    string paguTppKk = worksheet.Cells[row, 9].Value?.ToString()?.Trim() ?? string.Empty;

                    string query = $"INSERT INTO data_pegawai (Tgl_Gaji, Nip, Nama, Kd_Satker, Norek, Kd_Pangkat, Piwp1, Nm_Skpd, Pagu_TppBk, Pagu_TppKk) " +
                                    "VALUES (@Tgl_Gaji, @Nip, @Nama, @Kd_Satker, @Norek, @Kd_Pangkat, @Piwp1, @Nm_Skpd, @Pagu_TppBk, @Pagu_TppKk);";

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
                    command.Parameters.AddWithValue("@Piwp1", piwp1);
                    command.Parameters.AddWithValue("@Nm_Skpd", nmSkpd);
                    command.Parameters.AddWithValue("@Pagu_TppBk", paguTppBk);
                    command.Parameters.AddWithValue("@Pagu_TppKk", paguTppKk);

                    command.ExecuteNonQuery();
                }
            }
            catch (MySqlException ex)
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
                return;
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
                HandyControl.Controls.MessageBox.Show(new MessageBoxInfo
                {
                    Message = $"Error during execute: {ex.Message}",
                    Caption = "Error",
                    Button = MessageBoxButton.OK,
                    IconBrushKey = ResourceToken.AccentBrush,
                    IconKey = ResourceToken.ErrorGeometry,
                    StyleKey = "MessageBoxCustom"
                });
                return false;
            }
        }

        public static List<DataPegawaiModel> GetAllDataPegawai(string tahun, string bulan)
        {
            using var connection = OpenConnection();

            try
            {
                string query = $"SELECT Tgl_Gaji, Nip, Nama, Kd_Satker, Norek, Kd_Pangkat, Piwp1, Nm_Skpd, Pagu_TppBk, Pagu_TppKk FROM data_pegawai WHERE Tgl_Gaji = @Tgl_Gaji ORDER BY Nama ASC";
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
                        Piwp = reader["Piwp1"].ToString(),
                        NmSkpd = reader["Nm_Skpd"].ToString(),
                        PaguTppBk = Convert.ToInt32(reader["Pagu_TppBk"]),
                        PaguTppKk = Convert.ToInt32(reader["Pagu_TppKk"]),
                    };

                    pegawaiList.Add(pegawaiModel);
                }

                return pegawaiList;
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
                return [];
            }
        }

        public static void UpdatePegawai(string nip, string nama, string kdSatker, string norek, string kdPangkat, string piwp1, string nmSkpd, int paguTppBk, int paguTppKk)
        {
            using var connection = OpenConnection();

            try
            {
                string query = "UPDATE data_pegawai SET Nama = @Nama, Kd_Satker = @Kd_Satker, Norek = @Norek, Kd_Pangkat = @Kd_Pangkat, Piwp1 = @Piwp1, Nm_Skpd = @Nm_Skpd, Pagu_TppBk = @Pagu_TppBk, Pagu_TppKk = @Pagu_TppKk " +
                               "WHERE Nip = @Nip;";

                using var command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@Nip", nip);
                command.Parameters.AddWithValue("@Nama", nama);
                command.Parameters.AddWithValue("@Kd_Satker", kdSatker);
                command.Parameters.AddWithValue("@Norek", norek);
                command.Parameters.AddWithValue("@Kd_Pangkat", kdPangkat);
                command.Parameters.AddWithValue("@Piwp1", piwp1);
                command.Parameters.AddWithValue("@Nm_Skpd", nmSkpd);
                command.Parameters.AddWithValue("@Pagu_TppBk", paguTppBk);
                command.Parameters.AddWithValue("@Pagu_TppKk", paguTppKk);

                command.ExecuteNonQuery();
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

        public static void DeletePegawai(string nip)
        {
            using var connection = OpenConnection();

            try
            {
                string query = "DELETE FROM data_pegawai WHERE Nip = @Nip";

                using var command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@Nip", nip);

                command.ExecuteNonQuery();
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
