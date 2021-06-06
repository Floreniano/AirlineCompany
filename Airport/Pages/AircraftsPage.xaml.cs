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
    /// Логика взаимодействия для AircraftsPage.xaml
    /// </summary>
    public partial class AircraftsPage : Page
    {
        public AircraftsPage()
        {
            InitializeComponent();
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                AirlineCompanyEntities.getContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                dGridAircraft.ItemsSource = AirlineCompanyEntities.getContext().Aircrafts.ToList();
            }
            catch
            {
                MessageBox.Show("Ошибка соединения с БД", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            Manager.mainFrame.Navigate(new AddEditAircraftPage((sender as Button).DataContext as Aircrafts));
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Manager.mainFrame.Navigate(new AddEditAircraftPage(null));
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var aircraftsForRemoving = dGridAircraft.SelectedItems.Cast<Aircrafts>().ToList();
            if (MessageBox.Show($"Вы точно хотите удалить выделенный(ые) самолет(ы)?\nКоличество выбранных элементов = {aircraftsForRemoving.Count}", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    AirlineCompanyEntities.getContext().Aircrafts.RemoveRange(aircraftsForRemoving);
                    AirlineCompanyEntities.getContext().SaveChanges();
                    MessageBox.Show("Данные удалены!");
                    dGridAircraft.ItemsSource = AirlineCompanyEntities.getContext().Aircrafts.ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void txtSearchAircraft_TextChanged(object sender, TextChangedEventArgs e)
        {
            List<Aircrafts> currentAircraft = AirlineCompanyEntities.getContext().Aircrafts.ToList();
            currentAircraft = currentAircraft.Where(p => p.Name.ToLower().Contains(txtSearchAircraft.Text.ToLower()) || p.MakeModel.ToLower().Contains(txtSearchAircraft.Text.ToLower()) || p.TotalSeats.ToString().Contains(txtSearchAircraft.Text) || p.EconomySeats.ToString().Contains(txtSearchAircraft.Text) || p.BusinessSeats.ToString().Contains(txtSearchAircraft.Text)).ToList();
            dGridAircraft.ItemsSource = currentAircraft.ToList();
            if (dGridAircraft.Items.Count == 0)
            {
                labelNotFound.Visibility = Visibility.Visible;
                dGridAircraft.Visibility = Visibility.Collapsed;
            }
            else
            {
                dGridAircraft.Visibility = Visibility.Visible;
                labelNotFound.Visibility = Visibility.Collapsed;
            }
        }
    }
}
