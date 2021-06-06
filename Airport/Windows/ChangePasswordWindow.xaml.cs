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
using System.Windows.Shapes;
using Airport.Classes;
using Airport.Windows;
using Airport.Data;
using Airport.Pages;

namespace Airport.Windows
{
    /// <summary>
    /// Логика взаимодействия для ChangePasswordWindow.xaml
    /// </summary>
    public partial class ChangePasswordWindow : Window
    {
        private int counter = 0;
        public StartWindow baseWindow;
        private Users currentUser = Manager.user;
        public ChangePasswordWindow()
        {
            InitializeComponent();
        }

        private void btnConfirmPassword_Click(object sender, RoutedEventArgs e)
        {
            counter = 0;
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrEmpty(passOld.Password))
                errors.AppendLine("Старый пароль не введен");
            if (passOld.Password != currentUser.Password)
                errors.AppendLine("Старый пароль введен не правильно");
            else
                counter++;
            if (string.IsNullOrEmpty(passNew.Password))
                errors.AppendLine("Новый пароль не введен");
            if (passNew.Password != passNewConfirm.Password)
                errors.AppendLine("Новый пароль не совпадает");
            else
                counter++;

            if (errors.Length != 0)
            {
                MessageBox.Show(errors.ToString(), "Внимание", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                if (counter == 2)
                {
                    currentUser.Password = passNew.Password;
                    AirlineCompanyEntities.getContext().SaveChanges();
                    MessageBox.Show("Пароль сохранен", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                    AuthorizationWindow authorizationWindow = new AuthorizationWindow();
                    baseWindow.Close();
                    this.Close();
                    authorizationWindow.Show();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка" + ex.Message.ToString());
            }
        }
    }
}
