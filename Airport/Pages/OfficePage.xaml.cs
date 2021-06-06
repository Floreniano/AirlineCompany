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
    /// Логика взаимодействия для OfficePage.xaml
    /// </summary>
    public partial class OfficePage : Page
    {
        public OfficePage()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Manager.mainFrame.Navigate(new AddEditOfficeAndCountriesPage(null));
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            Manager.mainFrame.Navigate(new AddEditOfficeAndCountriesPage((sender as Button).DataContext as Offices));
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var officesForRemoving = dGridOffice.SelectedItems.Cast<Offices>().ToList();
            if (MessageBox.Show($"Вы точно хотите удалить выделенный(ые) оффис(ы)?\nКоличество выбранных элементов = {officesForRemoving.Count}", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    AirlineCompanyEntities.getContext().Offices.RemoveRange(officesForRemoving);
                    AirlineCompanyEntities.getContext().SaveChanges();
                    MessageBox.Show("Данные удалены!");
                    dGridOffice.ItemsSource = AirlineCompanyEntities.getContext().Offices.ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                AirlineCompanyEntities.getContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                dGridOffice.ItemsSource = AirlineCompanyEntities.getContext().Offices.ToList();
            }
            catch
            {
                MessageBox.Show("Ошибка соединения с БД", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void txtSearchOffice_TextChanged(object sender, TextChangedEventArgs e)
        {
            List<Offices> currentOffice = AirlineCompanyEntities.getContext().Offices.ToList();
            currentOffice = currentOffice.Where(p => p.Title.ToLower().Contains(txtSearchOffice.Text.ToLower()) || p.Contact.ToLower().Contains(txtSearchOffice.Text.ToLower()) || p.Countries.Name.ToLower().Contains(txtSearchOffice.Text.ToLower()) || p.Phone.Contains(txtSearchOffice.Text) ).ToList();
            dGridOffice.ItemsSource = currentOffice.ToList();
            if (dGridOffice.Items.Count == 0)
            {
                labelNotFound.Visibility = Visibility.Visible;
                dGridOffice.Visibility = Visibility.Collapsed;
            }
            else
            {
                dGridOffice.Visibility = Visibility.Visible;
                labelNotFound.Visibility = Visibility.Collapsed;
            }
        }
    }
}