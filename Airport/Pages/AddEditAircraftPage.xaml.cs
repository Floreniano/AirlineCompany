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
    /// Логика взаимодействия для AddEditAircraftPage.xaml
    /// </summary>
    public partial class AddEditAircraftPage : Page
    {
        private Aircrafts currentAircraft = new Aircrafts();
        
        public AddEditAircraftPage(Aircrafts selectedAircraft)
        {
            InitializeComponent();
            if (selectedAircraft != null)
                currentAircraft = selectedAircraft;
            DataContext = currentAircraft;
        }
        int totalSeats = 0;
        int economSeats = 0;
        int businessSeats = 0;
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            int.TryParse(txtTotalSeats.Text, out totalSeats);
            int.TryParse(txtEconomySeats.Text, out economSeats);
            int.TryParse(txtBusinessSeats.Text, out businessSeats);

            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(currentAircraft.Name))
                errors.AppendLine("Введите название");
            if (string.IsNullOrWhiteSpace(currentAircraft.MakeModel))
                errors.AppendLine("Введите модель");
            if (txtTotalSeats.Text == "" || currentAircraft.TotalSeats == 0)
                errors.AppendLine("Введите количество мест(общее)");

            if(txtBusinessSeats.Text != txtTotalSeats.Text)
                if (txtEconomySeats.Text == "" || currentAircraft.EconomySeats == 0)
                    errors.AppendLine("Введите количество мест(економ)");

            if (txtEconomySeats.Text != txtTotalSeats.Text)
                if (txtBusinessSeats.Text == "" || currentAircraft.BusinessSeats == 0)
                    errors.AppendLine("Введите количество мест(бизнесс)");

            if (totalSeats < (economSeats + businessSeats))
                errors.AppendLine("Уменьшите количество мест економ/бизнесс класа");

            if(totalSeats != (economSeats + businessSeats))
                errors.AppendLine("Сумма економ и бизнесс класса не равняется общей сумме мест");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }



            if (currentAircraft.ID == 0)
                AirlineCompanyEntities.getContext().Aircrafts.Add(currentAircraft);
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

        private void txtTotalSeats_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void txtBusinessSeats_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void txtEconomySeats_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void txtTotalSeats_TextChanged(object sender, TextChangedEventArgs e)
        {
            int totalSeats = 0;
            if (txtTotalSeats.Text != "")
                totalSeats = Convert.ToInt32(txtTotalSeats.Text);
            else
            {
                txtEconomySeats.Text = "";
                txtBusinessSeats.Text = "";
            }
            if(totalSeats > 0)
            {
                txtEconomySeats.IsEnabled = true;
                txtBusinessSeats.IsEnabled = true;
            }
        }
    }
}
