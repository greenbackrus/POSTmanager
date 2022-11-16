using POSTmanager.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace POSTmanager
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Exit_Button_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void Login_Button_Click(object sender, RoutedEventArgs e)
        {
            string _loginText = LoginTextBox.Text.ToLower();
            string _passwordText = PasswordTextBox.Password;
            
            if (_loginText.Length < 3)
            {
                ShowWarning("Введите логин, не менее 3-х символов");
                return;
            }
            
            if (_passwordText.Length < 4)
            {
                ShowWarning("Введите пароль, не менее 4-х символов");
                return;
            }
            
            AuthHelper _auth = new AuthHelper(_loginText, _passwordText);
            if (AuthHelper.CurrentUser == null)
            {
                PasswordTextBox.Password = null;
                ShowWarning("Пользователь с таким логином и паролем не найден");
                return;
            }

            Window RestsList = new RestsListWindow();
            Close();
            RestsList.Show();
        }

        private void LoginTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            CleanWarning();
        }

        private void PasswordTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            CleanWarning();
        }

        private void CleanWarning()
        {
            WarningLabel.Content = "";
            WarningLabel.Visibility = Visibility.Hidden;
        }
       
        private void ShowWarning(string text)
        {
            WarningLabel.Content = text;
            WarningLabel.Visibility = Visibility.Visible;
        }
    }
}
