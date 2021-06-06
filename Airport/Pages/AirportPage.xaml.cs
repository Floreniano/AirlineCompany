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
    /// Логика взаимодействия для AirportPage.xaml
    /// </summary>
    public partial class AirportPage : Page
    {
        public AirportPage()
        {
            InitializeComponent();
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                AirlineCompanyEntities.getContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                dGridAirport.ItemsSource = AirlineCompanyEntities.getContext().Airports.ToList();
            }
            catch
            {
                MessageBox.Show("Ошибка соединения с БД", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Manager.mainFrame.Navigate(new AddEditAirportPage(null));

        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var airportsForRemoving = dGridAirport.SelectedItems.Cast<Airports>().ToList();
            if (MessageBox.Show($"Вы точно хотите удалить выделенный(ые) аиропорт(ы)?\nКоличество выбранных элементов = {airportsForRemoving.Count}", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    AirlineCompanyEntities.getContext().Airports.RemoveRange(airportsForRemoving);
                    AirlineCompanyEntities.getContext().SaveChanges();
                    MessageBox.Show("Данные удалены!");
                    dGridAirport.ItemsSource = AirlineCompanyEntities.getContext().Tickets.ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void txtSearchAirport_TextChanged(object sender, TextChangedEventArgs e)
        {
            List<Airports> currentTicket = AirlineCompanyEntities.getContext().Airports.ToList();
            currentTicket = currentTicket.Where(p => p.IATACode.ToLower().Contains(txtSearchAirport.Text.ToLower()) || p.Name.ToLower().Contains(txtSearchAirport.Text.ToLower()) || p.Countries.Name.ToLower().Contains(txtSearchAirport.Text.ToLower())).ToList();
            dGridAirport.ItemsSource = currentTicket.ToList();
            if (dGridAirport.Items.Count == 0)
            {
                labelNotFound.Visibility = Visibility.Visible;
                dGridAirport.Visibility = Visibility.Collapsed;
            }
            else
            {
                dGridAirport.Visibility = Visibility.Visible;
                labelNotFound.Visibility = Visibility.Collapsed;
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            Manager.mainFrame.Navigate(new AddEditAirportPage((sender as Button).DataContext as Airports));
        }
    }
}
