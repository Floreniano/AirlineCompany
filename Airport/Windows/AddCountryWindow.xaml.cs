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
using System.Windows.Shapes;
using Airport.Classes;
using Airport.Windows;
using Airport.Data;
using Airport.Pages;
using System.Text.RegularExpressions;

namespace Airport.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddCountryWindow.xaml
    /// </summary>
    public partial class AddCountryWindow : Window
    {
        private AddEditOfficeAndCountriesPage parentPage;
        private AddEditAirportPage parentPage2;
        private Countries currentCountry;
        public AddCountryWindow(AddEditOfficeAndCountriesPage country, AddEditAirportPage country2)
        {
            InitializeComponent();
            currentCountry = new Countries(); 
            DataContext = currentCountry;
            if (country != null)
                parentPage = country;
            else
                parentPage2 = country2;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(txtAddCountry.Text))
            {
                if (AirlineCompanyEntities.getContext().Countries.Where(p => p.Name == txtAddCountry.Text).Count() == 0)
                {
                    if (currentCountry.ID == 0) 
                    { 
                        AirlineCompanyEntities.getContext().Countries.Add(currentCountry); 
                    }
                    AirlineCompanyEntities.getContext().SaveChanges();
                    MessageBox.Show("Данные успешно сохранены!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                    if(parentPage != null)
                        parentPage.comboOfficeCounty.ItemsSource = AirlineCompanyEntities.getContext().Countries.ToList();
                    else
                        parentPage2.comboCounty.ItemsSource = AirlineCompanyEntities.getContext().Countries.ToList();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Текущая страна уже существует", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Введите название страны", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void txtAddCountry_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^a-zA-Z]");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
