using MySqlConnector;
using System.Windows;
using System;
using System.Configuration;
using System.Collections.Generic;
using NewAppTpp.MVVM.Model;
using HandyControl.Data;

namespace NewAppTpp.Services
{
    internal class UserAccessService
    {
        private static UserAccessService _instance = new();
        public static UserAccessService Instance
        {
            get
            {
                _instance ??= new UserAccessService();

                return _instance;
            }
        }

        public string CurrentNip { get ; private set; }
        public string CurrentNama { get; private set; }
        public string CurrentUsername { get; private set; }
        public string CurrentPrivilege { get; private set; }

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

        public bool GetUserLoginData(string Username, string Password)
        {
            using var connection = OpenConnection();

            try
            {
                string query = "SELECT Nip, Username, Nama, Privilege FROM daftar_user WHERE Username = @Username AND Password = @Password";

                using MySqlCommand command = new(query, connection);

                command.Parameters.AddWithValue("@Username", Username);
                command.Parameters.AddWithValue("@Password", Password);

                using MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    CurrentNip = reader["Nip"].ToString() ?? string.Empty;
                    CurrentUsername = reader["Username"].ToString() ?? string.Empty;
                    CurrentNama = reader["Nama"].ToString() ?? string.Empty;
                    CurrentPrivilege = reader["Privilege"].ToString() ?? string.Empty;
                }

                return reader.HasRows;
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

                return false;
            }
        }

        public static List<UserAccessModel> GetAllUsers()
        {
            using var connection = OpenConnection();

            try
            {
                string query = "SELECT Nip, Nama, Jabatan, Username, Privilege FROM daftar_user WHERE Privilege != 'Super User' ORDER BY Privilege ASC";

                using MySqlCommand command = new(query, connection);

                List<UserAccessModel> userList = new();

                using MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    UserAccessModel userModel = new()
                    {
                        Nip = reader["Nip"].ToString(),
                        Nama = reader["Nama"].ToString(),
                        Jabatan = reader["Jabatan"].ToString(),
                        Username = reader["Username"].ToString(),
                        Privilege = reader["Privilege"].ToString(),
                    };

                    userList.Add(userModel);
                }

                return userList;
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

        public static void InsertNewUser(string nip, string nama, string jabatan, string username, string password, string privilege)
        {
            using var connection = OpenConnection();

            try
            {
                string query = "INSERT INTO daftar_user (Nip, Nama, Jabatan, Username, Password, Privilege) " +
                               "VALUES (@Nip, @Nama, @Jabatan, @Username, @Password, @Privilege)";

                using MySqlCommand command = new(query, connection);
                command.Parameters.AddWithValue("@Nip", nip);
                command.Parameters.AddWithValue("@Nama", nama);
                command.Parameters.AddWithValue("@Jabatan", jabatan);
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Password", password);
                command.Parameters.AddWithValue("@Privilege", privilege);

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

        public static void UpdateUser(string nip, string nama, string jabatan, string username, string privilege)
        {
            using var connection = OpenConnection();

            try
            {
                string query = "UPDATE daftar_user SET Nama = @Nama, Jabatan = @Jabatan, Username = @Username, Privilege = @Privilege " +
                               "WHERE Nip = @Nip;";

                using var command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@Nama", nama);
                command.Parameters.AddWithValue("@Jabatan", jabatan);
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Privilege", privilege);
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

        public static void DeleteUser(string username)
        {
            using var connection = OpenConnection();

            try
            {
                string query = "DELETE FROM daftar_user WHERE Username = @Username";

                using var command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@Username", username);

                command.ExecuteNonQuery();
            }
            catch ( Exception ex )
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
