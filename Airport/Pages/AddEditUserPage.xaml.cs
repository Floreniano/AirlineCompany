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
using Airport.Classes;
using Airport.Windows;
using Airport.Data;
using Airport.Pages;
using System.Text.RegularExpressions;

namespace Airport.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddEditPageUser.xaml
    /// </summary>
    public partial class AddEditPageUser : Page
    {
        private Users currentUser = new Users();

        public AddEditPageUser(Users selectedUser)
        {
            InitializeComponent(); 
            comboActive.Items.Add("Активный");
            comboActive.Items.Add("Не активный");
            if (selectedUser != null)
            {
                currentUser = selectedUser;
                if (selectedUser.Active == true)
                    comboActive.SelectedValue = "Активный";
                if (selectedUser.Active == false)
                    comboActive.SelectedValue = "Не активный";
            }
            else
            {
                currentUser.Birthdate = DateTime.Now;
                comboActive.Visibility = Visibility.Collapsed;
                txtStatus.Visibility = Visibility.Collapsed;
            }
            DataContext = currentUser;
            comboOffice.ItemsSource = AirlineCompanyEntities.getContext().Offices.ToList();
            comboRole.ItemsSource = AirlineCompanyEntities.getContext().Roles.ToList();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(currentUser.Login))
                errors.AppendLine("Введите логин");
            if (string.IsNullOrWhiteSpace(currentUser.FirstName))
                errors.AppendLine("Введите имя");
            if (string.IsNullOrWhiteSpace(currentUser.LastName))
                errors.AppendLine("Введите фамилию");
            if (string.IsNullOrEmpty(comboOffice.Text))
                errors.AppendLine("Выберите офис");
            if (currentUser.Birthdate == null)
                errors.AppendLine("Укажите дату рождения");
            if (string.IsNullOrWhiteSpace(currentUser.Password))
                errors.AppendLine("Введите пароль");
            if (string.IsNullOrEmpty(comboRole.Text))
                errors.AppendLine("Выберите роль");
            if (comboActive.Visibility == Visibility.Visible)
            {
                if (string.IsNullOrEmpty(comboActive.Text))
                    errors.AppendLine("Выберите статус");
                currentUser.Active = comboActive.SelectedItem.Equals("Активный") ? true : false;
            }
            else
                currentUser.Active = true;

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            

            if (currentUser.ID == 0)
                AirlineCompanyEntities.getContext().Users.Add(currentUser);
            try
            {
                AirlineCompanyEntities.getContext().SaveChanges();
                MessageBox.Show("Информация сохранена");
                Manager.mainFrame.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка" + ex.Message.ToString());
            }
        }


        private void txtFirstName_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^а-яА-Яa-zA-Z]");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void txtLastName_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^а-яА-Яa-zA-Z]");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
