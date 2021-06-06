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
    /// Логика взаимодействия для RoutePage.xaml
    /// </summary>
    public partial class RoutePage : Page
    {
        public RoutePage()
        {
            InitializeComponent();
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                AirlineCompanyEntities.getContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                dGridRoute.ItemsSource = AirlineCompanyEntities.getContext().Routes.ToList();
            }
            catch
            {
                MessageBox.Show("Ошибка соединения с БД", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void txtSearchRoute_TextChanged(object sender, TextChangedEventArgs e)
        {
            List<Routes> currentRoute = AirlineCompanyEntities.getContext().Routes.ToList();
            currentRoute = currentRoute.Where(p => p.Airports.IATACode.ToLower().Contains(txtSearchRoute.Text.ToLower()) || p.Airports1.IATACode.ToLower().Contains(txtSearchRoute.Text.ToLower()) || p.Distance.ToString().Contains(txtSearchRoute.Text) || p.FlightTime.ToString().Contains(txtSearchRoute.Text)).ToList();
            dGridRoute.ItemsSource = currentRoute.ToList();
            if (dGridRoute.Items.Count == 0)
            {
                labelNotFound.Visibility = Visibility.Visible;
                dGridRoute.Visibility = Visibility.Collapsed;
            }
            else
            {
                dGridRoute.Visibility = Visibility.Visible;
                labelNotFound.Visibility = Visibility.Collapsed;
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            Manager.mainFrame.Navigate(new AddEditRoutePage((sender as Button).DataContext as Routes));
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Manager.mainFrame.Navigate(new AddEditRoutePage(null));
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var routeForRemoving = dGridRoute.SelectedItems.Cast<Routes>().ToList();
            if (MessageBox.Show($"Вы точно хотите удалить выделенный(ые) маршрут(ы)?\nКоличество выбранных элементов = {routeForRemoving.Count}", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    AirlineCompanyEntities.getContext().Routes.RemoveRange(routeForRemoving);
                    AirlineCompanyEntities.getContext().SaveChanges();
                    MessageBox.Show("Данные удалены!");
                    dGridRoute.ItemsSource = AirlineCompanyEntities.getContext().Routes.ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }
    }
}
