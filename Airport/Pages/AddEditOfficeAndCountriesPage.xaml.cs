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
    /// Логика взаимодействия для AddEditOfficeAndCountriesPage.xaml
    /// </summary>
    public partial class AddEditOfficeAndCountriesPage : Page
    {
        private Offices currentOffice = new Offices();
        public AddEditOfficeAndCountriesPage(Offices selectedOffice)
        {
            InitializeComponent();
            if (selectedOffice != null)
                currentOffice = selectedOffice;
            try
            {
                comboOfficeCounty.ItemsSource = AirlineCompanyEntities.getContext().Countries.ToList();

            }
            catch
            {
                System.Windows.MessageBox.Show("Ошибка соединения с БД", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
            DataContext = currentOffice;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrEmpty(comboOfficeCounty.Text))
                errors.AppendLine("Выберите страну");
            if (string.IsNullOrWhiteSpace(currentOffice.Title))
                errors.AppendLine("Введите название");
            if (string.IsNullOrWhiteSpace(currentOffice.Phone) && txtPhone.Text == "000-000-0000" && txtPhone.Text == "" && txtPhone.Text.Length < 2)
                errors.AppendLine("Введите правильный номер");
            if (string.IsNullOrEmpty(currentOffice.Contact))
                errors.AppendLine("Введите контакт");

            if (errors.Length > 0)
            {
                System.Windows.MessageBox.Show(errors.ToString());
                return;
            }



            if (currentOffice.ID == 0)
                AirlineCompanyEntities.getContext().Offices.Add(currentOffice);
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

        private void btnAddCountry_Click(object sender, RoutedEventArgs e)
        {
            AddCountryWindow addCountryWindow = new AddCountryWindow(this, null);
            addCountryWindow.ShowDialog();
        }

        private void txtCity_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^а-яА-Яa-zA-Z]");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void txtPhone_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void txtContact_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^а-яА-Яa-zA-Z]");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
