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
using Microsoft.Win32;
using System.IO;

namespace Airport.Pages
{
    /// <summary>
    /// Логика взаимодействия для ScheduleFligthPage.xaml
    /// </summary>
    public partial class ScheduleFligthPage : Page
    {
        public ScheduleFligthPage()
        {
            InitializeComponent();
            try
            {
                comboFromAirport.ItemsSource = AirlineCompanyEntities.getContext().Airports.ToList();
                comboToAirport.ItemsSource = AirlineCompanyEntities.getContext().Airports.ToList();
            }
            catch
            {
                MessageBox.Show("Ошибка соединения с БД", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }


        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            Manager.mainFrame.Navigate(new AddEditScheduleFlightPage((sender as Button).DataContext as Schedules));
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var fligthForRemoving = dGridSheduleFlight.SelectedItems.Cast<Schedules>().ToList();
            if (MessageBox.Show($"Вы точно хотите удалить выбранное(ые) рассписание(ия)? \nКоличество выбранных элементов = {fligthForRemoving.Count}", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    AirlineCompanyEntities.getContext().Schedules.RemoveRange(fligthForRemoving);
                    AirlineCompanyEntities.getContext().SaveChanges();
                    MessageBox.Show("Данные удалены!");
                    dGridSheduleFlight.ItemsSource = AirlineCompanyEntities.getContext().Schedules.ToList();
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
                dGridSheduleFlight.ItemsSource = AirlineCompanyEntities.getContext().Schedules.ToList();
            }
            catch
            {
                MessageBox.Show("Ошибка соединения с БД", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void comboFromAirport_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboFromAirport.SelectedItem != null)
            {
                try
                {
                    Airports currentAirport = (Airports)comboFromAirport.SelectedItem;
                    List<Airports> availableAirports = new List<Airports>();
                    foreach (var row in AirlineCompanyEntities.getContext().Routes.Where(p => p.DepartureAirportID == currentAirport.ID).ToList())
                    {
                        availableAirports.Add(AirlineCompanyEntities.getContext().Airports.Where(p => p.ID == row.ArrivalAirportID).FirstOrDefault());
                    }
                    comboToAirport.ItemsSource = availableAirports;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка " + ex.Message.ToString());
                }
            }
        }

        private void comboToAirport_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboToAirport.SelectedItem != null)
            {
                try
                {
                    Airports currentAirport = (Airports)comboToAirport.SelectedItem;
                    List<Airports> availableAirports = new List<Airports>();
                    foreach (var row in AirlineCompanyEntities.getContext().Routes.Where(p => p.ArrivalAirportID == currentAirport.ID).ToList())
                    {
                        availableAirports.Add(AirlineCompanyEntities.getContext().Airports.Where(p => p.ID == row.DepartureAirportID).FirstOrDefault());
                    }
                    comboFromAirport.ItemsSource = availableAirports;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка " + ex.Message.ToString());
                }
            }
        }

        private void btnResetParameters_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                comboFromAirport.SelectedItem = null;
                comboToAirport.SelectedItem = null;
                txtBoxSearchSchedule.Text = "";
                comboFromAirport.ItemsSource = AirlineCompanyEntities.getContext().Airports.ToList();
                comboToAirport.ItemsSource = AirlineCompanyEntities.getContext().Airports.ToList();
                dGridSheduleFlight.ItemsSource = AirlineCompanyEntities.getContext().Schedules.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка" + ex.Message.ToString());
            }
        }

        private decimal costEconomy = 0;
        private decimal costBusiness = 0;
        private decimal costFirstClass = 0;
        Airports airportFrom;
        Airports airportTo;
        private void txtBoxSearchSchedule_TextChanged(object sender, TextChangedEventArgs e)
        {
            Decimal.TryParse(txtBoxSearchSchedule.Text, out costEconomy);
            Decimal.TryParse(txtBoxSearchSchedule.Text, out costBusiness);
            Decimal.TryParse(txtBoxSearchSchedule.Text, out costFirstClass);
            List<Schedules> currentSchedule = AirlineCompanyEntities.getContext().Schedules.ToList();
            if (comboFromAirport.SelectedItem == null && comboToAirport.SelectedItem == null)
            {
                currentSchedule = currentSchedule.Where(p => p.Aircrafts.Name.ToLower().Contains(txtBoxSearchSchedule.Text.ToLower()) || p.EconomyPrice.Equals(costEconomy) || p.BusinessPrice.Equals(costBusiness) || p.FirstClassPrice.Equals(costFirstClass) || p.Date.ToShortDateString().Contains(txtBoxSearchSchedule.Text) || p.Time.ToString().Contains(txtBoxSearchSchedule.Text)).ToList();
            }
            if (comboFromAirport.SelectedItem != null && comboToAirport.SelectedItem == null)
            {
                currentSchedule = currentSchedule.Where(p => (p.Routes.DepartureAirportID == airportFrom.ID) && (p.Aircrafts.Name.ToLower().Contains(txtBoxSearchSchedule.Text.ToLower()) || p.EconomyPrice.Equals(costEconomy) || p.BusinessPrice.Equals(costBusiness) || p.FirstClassPrice.Equals(costFirstClass) || p.Date.ToShortDateString().Contains(txtBoxSearchSchedule.Text) || p.Time.ToString().Contains(txtBoxSearchSchedule.Text))).ToList();
            }
            if (comboFromAirport.SelectedItem == null && comboToAirport.SelectedItem != null)
            {
                currentSchedule = currentSchedule.Where(p => (p.Routes.ArrivalAirportID == airportTo.ID) && (p.Aircrafts.Name.ToLower().Contains(txtBoxSearchSchedule.Text.ToLower()) || p.EconomyPrice.Equals(costEconomy) || p.BusinessPrice.Equals(costBusiness) || p.FirstClassPrice.Equals(costFirstClass) || p.Date.ToShortDateString().Contains(txtBoxSearchSchedule.Text) || p.Time.ToString().Contains(txtBoxSearchSchedule.Text))).ToList();
            }
            if (comboFromAirport.SelectedItem != null && comboToAirport.SelectedItem != null)
            {
                currentSchedule = currentSchedule.Where(p => (p.Routes.ArrivalAirportID == airportTo.ID && p.Routes.DepartureAirportID == airportFrom.ID) && (p.Aircrafts.Name.ToLower().Contains(txtBoxSearchSchedule.Text.ToLower()) || p.EconomyPrice.Equals(costEconomy) || p.BusinessPrice.Equals(costBusiness) || p.FirstClassPrice.Equals(costFirstClass) || p.Date.ToShortDateString().Contains(txtBoxSearchSchedule.Text) || p.Time.ToString().Contains(txtBoxSearchSchedule.Text))).ToList();
            }
            dGridSheduleFlight.ItemsSource = currentSchedule.ToList();
            if (dGridSheduleFlight.Items.Count == 0)
            {
                labelNotFound.Visibility = Visibility.Visible;
                dGridSheduleFlight.Visibility = Visibility.Collapsed;
            }
            else
            {
                dGridSheduleFlight.Visibility = Visibility.Visible;
                labelNotFound.Visibility = Visibility.Collapsed;
            }
        }

        private void btnApplyParameters_Click(object sender, RoutedEventArgs e)
        {
            if (comboFromAirport.SelectedItem != null && comboToAirport.SelectedItem != null)
            {
                if (comboFromAirport.SelectedItem != comboToAirport.SelectedItem)
                {
                    airportFrom = (Airports)comboFromAirport.SelectedItem;
                    airportTo = (Airports)comboToAirport.SelectedItem;
                    dGridSheduleFlight.ItemsSource = AirlineCompanyEntities.getContext().Schedules.Where(p => p.Routes.ArrivalAirportID == airportTo.ID 
                    && p.Routes.DepartureAirportID == airportFrom.ID).ToList();
                }
            }
            if (comboFromAirport.SelectedItem != null && comboToAirport.SelectedItem == null)
            {
                if (comboFromAirport.SelectedItem != comboToAirport.SelectedItem)
                {
                    airportFrom = (Airports)comboFromAirport.SelectedItem;
                    airportTo = (Airports)comboToAirport.SelectedItem;
                    dGridSheduleFlight.ItemsSource = AirlineCompanyEntities.getContext().Schedules.Where
                        (p => p.Routes.DepartureAirportID == airportFrom.ID).ToList();
                }
            }
            if (comboFromAirport.SelectedItem == null && comboToAirport.SelectedItem != null)
            {
                if (comboFromAirport.SelectedItem != comboToAirport.SelectedItem)
                {
                    airportFrom = (Airports)comboFromAirport.SelectedItem;
                    airportTo = (Airports)comboToAirport.SelectedItem;
                    dGridSheduleFlight.ItemsSource = AirlineCompanyEntities.getContext().Schedules.Where
                        (p => p.Routes.ArrivalAirportID == airportTo.ID).ToList();
                }
            }

        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Manager.mainFrame.Navigate(new AddEditScheduleFlightPage(null));
        }
    }
}
