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
using Xceed.Wpf.Toolkit;

namespace Airport.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddEditScheduleFlightPage.xaml
    /// </summary>
    public partial class AddEditScheduleFlightPage : Page
    {
        private Schedules currentFlight = new Schedules();
        public AddEditScheduleFlightPage(Schedules selectedFlight)
        {
            InitializeComponent(); comboConfirmed.Items.Add("Подтвержден");
            comboConfirmed.Items.Add("Отклонен");
            if (selectedFlight != null)
            {
                txtFromAirports.Text += selectedFlight.Routes.Airports1.IATACode;
                txtToAirports.Text += selectedFlight.Routes.Airports.IATACode;
                txtAircarft.Text += selectedFlight.Aircrafts.Name;

                currentFlight = selectedFlight;
                if (selectedFlight.Confirmed == true)
                    comboConfirmed.SelectedValue = "Подтвержден";
                if (selectedFlight.Confirmed == false)
                    comboConfirmed.SelectedValue = "Отклонен";
            }

            else
            {
                currentFlight.Date = DateTime.Now.Date;
                currentFlight.Confirmed = true;
                try
                {
                    comboRoute.ItemsSource = AirlineCompanyEntities.getContext().Routes.ToList();
                    comboAircraft.ItemsSource = AirlineCompanyEntities.getContext().Aircrafts.ToList();
                    
                }
                catch
                {
                    System.Windows.MessageBox.Show("Ошибка соединения с БД", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                comboAircraft.Visibility = Visibility.Visible;
                comboRoute.Visibility = Visibility.Visible;
                txtRoute.Visibility = Visibility.Visible;


                comboConfirmed.Visibility = Visibility.Collapsed;
                txtFromAirports.Visibility = Visibility.Collapsed;
                txtToAirports.Visibility = Visibility.Collapsed;

            }
            DataContext = currentFlight;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (comboRoute.Visibility == Visibility.Visible)
            {
                if (comboRoute.SelectedItem == null)
                    errors.AppendLine("Выберите маршрут");
                else
                {
                    var route = (Routes)comboRoute.SelectedItem;
                    currentFlight.RouteID = route.ID;
                }
            }

            if (datePickerDate.SelectedDate == null || datePickerDate.Text.Length == 0)
                errors.AppendLine("Введите дату");
            if (string.IsNullOrWhiteSpace(currentFlight.Time.ToString()))
                errors.AppendLine("Введите время");
            if (txtEconomyPrice.Text == "" || currentFlight.EconomyPrice == 0)
                errors.AppendLine("Введите цену");
            if (string.IsNullOrWhiteSpace(currentFlight.FlightNumber))
                errors.AppendLine("Введите номер рейса");
            if(comboAircraft.Visibility == Visibility.Visible)
            {
                if (string.IsNullOrWhiteSpace(comboAircraft.Text))
                    errors.AppendLine("Выберите самолет");
            }

            if (errors.Length > 0)
            {
                System.Windows.MessageBox.Show(errors.ToString());
                return;
            }

            if (comboConfirmed.Visibility == Visibility.Visible)
                currentFlight.Confirmed = comboConfirmed.SelectedItem.Equals("Подтвержден") ? true : false;
            else
                currentFlight.Confirmed = true;



            if (currentFlight.ID == 0)
                AirlineCompanyEntities.getContext().Schedules.Add(currentFlight);
            try
            {
                AirlineCompanyEntities.getContext().SaveChanges();
                System.Windows.MessageBox.Show("Информация сохранена");
                Manager.mainFrame.GoBack();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Ошибка" + ex.Message.ToString());
            }
        }

        private void txtTime_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void txtFlightNumber_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void txtEconomyPrice_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}