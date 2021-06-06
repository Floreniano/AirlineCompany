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
    /// Логика взаимодействия для AddEditRoutePage.xaml
    /// </summary>
    public partial class AddEditRoutePage : Page
    {
        private Routes currentRoute = new Routes();
        public AddEditRoutePage(Routes selectedRoute)
        {
            InitializeComponent();
            if (selectedRoute != null)
                currentRoute = selectedRoute;
            try
            {
                comboArrivalAirport.ItemsSource = AirlineCompanyEntities.getContext().Airports.ToList();
                comboDepartureAirport.ItemsSource = AirlineCompanyEntities.getContext().Airports.ToList();
            }
            catch
            {
                MessageBox.Show("Ошибка соединения с БД", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            DataContext = currentRoute;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrEmpty(comboDepartureAirport.Text))
                errors.AppendLine("Выберите страну вылета");
            if (string.IsNullOrWhiteSpace(comboArrivalAirport.Text))
                errors.AppendLine("Введите страну прилета");
            if (txtDistance.Text == "")
                errors.AppendLine("Введите дистанцию");
            if (txtFligthTime.Text == "")
                errors.AppendLine("Введите время полета");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }



            if (currentRoute.ID == 0)
                AirlineCompanyEntities.getContext().Routes.Add(currentRoute);
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
    }
}
