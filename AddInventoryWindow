using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Data.Sqlite;

namespace WpfApp3
{
    public partial class AddInventoryWindow : Window
    {
        private readonly string _connectionString = "Data Source=new.db;";

        public AddInventoryWindow()
        {
            InitializeComponent();
            this.Loaded += (s, e) => ArticleTextBox.Focus();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ArticleTextBox.Text))
                    throw new Exception("Артикул не может быть пустым");
                
                if (string.IsNullOrWhiteSpace(NameTextBox.Text))
                    throw new Exception("Название не может быть пустым");

                if (!int.TryParse(QuantityTextBox.Text, out int quantity) || quantity <= 0)
                    throw new Exception("Количество должно быть целым положительным числом");

                if (!decimal.TryParse(PriceTextBox.Text, out decimal price) || price <= 0)
                    throw new Exception("Цена должна быть положительным числом");

                using (var connection = new SqliteConnection(_connectionString))
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = @"
                        INSERT INTO SpareParts (Articul, Name, Quantity, Price)
                        VALUES (@article, @name, @quantity, @price)";
                    
                    command.Parameters.AddWithValue("@article", ArticleTextBox.Text.Trim());
                    command.Parameters.AddWithValue("@name", NameTextBox.Text.Trim());
                    command.Parameters.AddWithValue("@quantity", quantity);
                    command.Parameters.AddWithValue("@price", price);
                    
                    command.ExecuteNonQuery();
                }
                
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void NumericTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            foreach (char c in e.Text)
            {
                if (!char.IsDigit(c))
                {
                    e.Handled = true;
                    return;
                }
            }
        }

        private void DecimalTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;
            string newText = textBox.Text.Insert(textBox.SelectionStart, e.Text);
            
            if (!decimal.TryParse(newText, out _))
            {
                e.Handled = true;
            }
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }
    }
}
