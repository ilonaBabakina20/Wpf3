using System.Windows;

namespace WpfApp3
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            PasswordBox.Focus(); 
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string password = PasswordBox.Password;

            if (string.IsNullOrWhiteSpace(password))
            {
                ShowError("Введите пароль");
                return;
            }
            
            UserRole role = DetermineUserRole(password);
            
            if (role == UserRole.None)
            {
                ShowError("Неверный пароль");
                return;
            }
            
            var mainWindow = new MainWindow(role);
            mainWindow.Show();
            this.Close();
        }

        private UserRole DetermineUserRole(string password)
        {
            if (password.Contains("admin"))
                return UserRole.Admin;
            
            if (password.Contains("m"))
                return UserRole.Master;
            
            return UserRole.None;
        }

        private void ShowError(string message)
        {
            ErrorTextBlock.Text = message;
            ErrorTextBlock.Visibility = Visibility.Visible;
        }
    }

    public enum UserRole
    {
        None,
        Admin,
        Master
    }
}
