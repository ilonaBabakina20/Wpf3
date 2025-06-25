using System.Windows;
using Microsoft.Data.Sqlite;

namespace WpfApp3
{
    public partial class AddClientWindow : Window
    {
        private string connectionString = "Data Source=new.db;";

        public AddClientWindow()
        {
            InitializeComponent();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(FullNameTextBox.Text))
            {
                MessageBox.Show("ФИО клиента обязательно для заполнения", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = @"
                        INSERT INTO Clients (FullName, Phone, Email)
                        VALUES (@fullName, @phone, @email)";
                    
                    command.Parameters.AddWithValue("@fullName", FullNameTextBox.Text.Trim());
                    command.Parameters.AddWithValue("@phone", PhoneTextBox.Text.Trim());
                    command.Parameters.AddWithValue("@email", EmailTextBox.Text.Trim());
                    
                    command.ExecuteNonQuery();
                }

                DialogResult = true;
                Close();
            }
            catch (SqliteException ex)
            {
                MessageBox.Show($"Ошибка базы данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
