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

namespace Airport.Pages
{
    /// <summary>
    /// Логика взаимодействия для StartAdminPage.xaml
    /// </summary>
    public partial class StartAdminPage : Page
    {
        public StartAdminPage()
        {
            InitializeComponent();
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                AirlineCompanyEntities.getContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                dGridUser.ItemsSource = AirlineCompanyEntities.getContext().Users.ToList();
            }
            catch
            {
                MessageBox.Show("Ошибка соединения с БД", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            Manager.mainFrame.Navigate(new AddEditPageUser((sender as Button).DataContext as Users));
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Manager.mainFrame.Navigate(new AddEditPageUser(null));
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var usersForRemoving = dGridUser.SelectedItems.Cast<Users>().ToList();
            if (MessageBox.Show($"Вы точно хотите удалить выбранного(ых) пользователя(ей)?\nКоличество выбранных элементов = {usersForRemoving.Count}", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    AirlineCompanyEntities.getContext().Users.RemoveRange(usersForRemoving);
                    AirlineCompanyEntities.getContext().SaveChanges();
                    MessageBox.Show("Данные удалены!");
                    dGridUser.ItemsSource = AirlineCompanyEntities.getContext().Users.ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void txtSearchUser_TextChanged(object sender, TextChangedEventArgs e)
        {
            List<Users> currentUser = AirlineCompanyEntities.getContext().Users.ToList();
            currentUser = currentUser.Where(p => p.FirstName.ToLower().Contains(txtSearchUser.Text.ToLower()) || p.LastName.ToLower().Contains(txtSearchUser.Text.ToLower()) || p.Login.ToLower().Contains(txtSearchUser.Text.ToLower()) || p.Offices.Title.ToLower().Contains(txtSearchUser.Text.ToLower())).ToList();
            dGridUser.ItemsSource = currentUser.ToList();
            if (dGridUser.Items.Count == 0)
            {
                labelNotFound.Visibility = Visibility.Visible;
                dGridUser.Visibility = Visibility.Collapsed;
            }
            else
            {
                dGridUser.Visibility = Visibility.Visible;
                labelNotFound.Visibility = Visibility.Collapsed;
            }
        }
    }
}
