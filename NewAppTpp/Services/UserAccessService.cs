using MySqlConnector;
using System.Windows;
using System;
using System.Configuration;
using Microsoft.Win32;
using System.IO;
using System.Collections.Generic;
using NewAppTpp.MVVM.Model;

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
        public byte[] CurrentProfileImage { get; private set; }

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
                MessageBox.Show($"Error during execute: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public bool GetUserPhoto(string Username)
        {
            using var connection = OpenConnection();

            try
            {
                string query = "SELECT Profile_Image FROM user_photo WHERE Username = @Username";

                using MySqlCommand command = new(query, connection);

                command.Parameters.AddWithValue("@Username", Username);

                using MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    CurrentProfileImage = !string.IsNullOrEmpty(reader["Profile_image"].ToString()) ? (byte[])reader["Profile_image"] : null;
                }

                return reader.HasRows;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during execute: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public void SaveImageToDB(OpenFileDialog openFileDialog)
        {
            using var connection = OpenConnection();

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    string filePath = openFileDialog.FileName;
                    byte[] fileBytes = File.ReadAllBytes(filePath);

                    string query = "UPDATE user_photo SET Profile_Image = @FileData WHERE Nip = @Nip;";

                    using var command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@FileData", fileBytes);
                    command.Parameters.AddWithValue("@Nip", CurrentNip);

                    command.ExecuteNonQuery();

                    Instance.CurrentProfileImage = fileBytes;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error during execute: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
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
                MessageBox.Show($"Error during execute: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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

                string secondQuery = "INSERT INTO user_photo (Nip, Username) VALUES (@Nip, @Username)";

                using MySqlCommand command = new(query, connection);
                command.Parameters.AddWithValue("@Nip", nip);
                command.Parameters.AddWithValue("@Nama", nama);
                command.Parameters.AddWithValue("@Jabatan", jabatan);
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Password", password);
                command.Parameters.AddWithValue("@Privilege", privilege);

                using MySqlCommand secondCommand = new(secondQuery, connection);
                secondCommand.Parameters.AddWithValue("@Nip", nip);
                secondCommand.Parameters.AddWithValue("@Username", username);

                command.ExecuteNonQuery();
                secondCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during execute: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static void UpdateUser(string nip, string nama, string jabatan, string username, string password, string privilege)
        {
            using var connection = OpenConnection();

            try
            {
                string query = "UPDATE daftar_user SET Nama = @Nama, Jabatan = @Jabatan, Username = @Username, Password = @Password, Privilege = @Privilege " +
                               "WHERE Nip = @Nip;";

                string secondQuery = "UPDATE user_photo SET Username = @Username WHERE Nip = @Nip";

                using var command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@Nama", nama);
                command.Parameters.AddWithValue("@Jabatan", jabatan);
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Password", password);
                command.Parameters.AddWithValue("@Privilege", privilege);
                command.Parameters.AddWithValue("@Nip", nip);

                using var secondCommand = new MySqlCommand(secondQuery, connection);
                secondCommand.Parameters.AddWithValue("@Username", username);
                secondCommand.Parameters.AddWithValue("@Nip", nip);

                secondCommand.ExecuteNonQuery();
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during execute: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static void DeleteUser(string username)
        {
            using var connection = OpenConnection();

            try
            {
                string query = "DELETE FROM daftar_user WHERE Username = @Username";

                string secondQuery = "DELETE FROM user_photo WHERE Username = @Username";

                using var command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@Username", username);

                using var secondCommand = new MySqlCommand(secondQuery, connection);
                secondCommand.Parameters.AddWithValue("@Username", username);

                secondCommand.ExecuteNonQuery();
                command.ExecuteNonQuery();
            }
            catch ( Exception ex )
            {
                MessageBox.Show($"Error during execute: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
