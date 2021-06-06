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
    /// Логика взаимодействия для AddEditAirportPage.xaml
    /// </summary>
    public partial class AddEditAirportPage : Page
    {
        private Airports currentAirport = new Airports();
        public AddEditAirportPage(Airports selectedAirport)
        {
            InitializeComponent();
            if (selectedAirport != null)
                currentAirport = selectedAirport;
            try
            {
                comboCounty.ItemsSource = AirlineCompanyEntities.getContext().Countries.ToList();
            }
            catch
            {
                MessageBox.Show("Ошибка соединения с БД", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            DataContext = currentAirport;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrEmpty(comboCounty.Text))
                errors.AppendLine("Выберите страну");
            if (string.IsNullOrWhiteSpace(currentAirport.IATACode))
                errors.AppendLine("Введите IATACode");
            if (string.IsNullOrWhiteSpace(currentAirport.Name))
                errors.AppendLine("Введите название");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }



            if (currentAirport.ID == 0)
                AirlineCompanyEntities.getContext().Airports.Add(currentAirport);
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

        private void txtName_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^а-яА-Яa-zA-Z]");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void txtIATACode_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^А-ЯA-Z]");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void btnAddCountry_Click(object sender, RoutedEventArgs e)
        {
            AddCountryWindow addCountryWindow = new AddCountryWindow(null, this);
            addCountryWindow.ShowDialog();
        }
    }
}
