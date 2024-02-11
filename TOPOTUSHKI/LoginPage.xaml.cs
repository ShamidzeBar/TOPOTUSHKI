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

namespace TOPOTUSHKI
{
    /// <summary>
    /// Логика взаимодействия для LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void LogInBtn_Click(object sender, RoutedEventArgs e)
        {
            var currentUsers = TradeEntities.GetContext().User.ToList();
            bool is_autorized = false;
            string log = LogIn_TextBox.Text,
                   pass = Password_TextBox.Text;

            if (LogIn_TextBox.Text == "")
                MessageBox.Show("Введите логин пользователя!");
            else
            {
                if (Password_TextBox.Text == "")
                    MessageBox.Show("Введите пароль пользователя!");
                else
                {
                    foreach (var user in currentUsers)
                    {
                        if (user.UserLogin == log)
                        {
                            is_autorized = true;
                            if (user.UserPassword == pass)
                            {
                                Manager.MainFrame.Navigate(new TopotushkiPage());
                            }
                            else
                            {
                                MessageBox.Show("Неправильный пароль!");
                            }
                        }
                    }
                    if (is_autorized == false)
                    {
                        MessageBox.Show("Пользователя с таким логином не существует");
                    }
                }
            }
        }
    }
}
