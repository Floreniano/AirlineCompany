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
    /// Логика взаимодействия для AddEditTicketPage.xaml
    /// </summary>
    public partial class AddEditTicketPage : Page
    {
        private Tickets currentTicket = new Tickets();
        public AddEditTicketPage(Tickets selectedTicket)
        {
            InitializeComponent();
            comboConfirmed.Items.Add("Подтвержден");
            comboConfirmed.Items.Add("Не подтвержден");
            if (selectedTicket != null)
            {
                currentTicket = selectedTicket;
                if (selectedTicket.Confirmed == true)
                    comboConfirmed.SelectedValue = "Подтвержден";
                if (selectedTicket.Confirmed == false)
                    comboConfirmed.SelectedValue = "Не подтвержден";
            }
            else
            {
                txtStatus.Visibility = Visibility.Collapsed;
                comboConfirmed.Visibility = Visibility.Collapsed;
            }
            try
            {
                comboCabinTypes.ItemsSource = AirlineCompanyEntities.getContext().CabinTypes.ToList();
                comboSchedule.ItemsSource = AirlineCompanyEntities.getContext().Schedules.ToList();

            }
            catch
            {
                System.Windows.MessageBox.Show("Ошибка соединения с БД", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
            DataContext = currentTicket;
        }

        private void txtPassportNumber_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(currentTicket.Firstname))
                errors.AppendLine("Введите имя");
            if (string.IsNullOrWhiteSpace(currentTicket.Lastname))
                errors.AppendLine("Введите фамилия");
            if (comboSchedule.SelectedItem == null)
                errors.AppendLine("Выберите маршрут");
            else
            {
                var schedules = (Schedules)comboSchedule.SelectedItem;
                currentTicket.ScheduleID = schedules.ID;
            }
            if (string.IsNullOrWhiteSpace(currentTicket.Phone))
                errors.AppendLine("Введите номер телефона");
            if (string.IsNullOrWhiteSpace(currentTicket.PassportNumber))
                errors.AppendLine("Введите номер паспорта");
            if (string.IsNullOrWhiteSpace(comboCabinTypes.Text))
                errors.AppendLine("Выберите класс");
            if (string.IsNullOrWhiteSpace(currentTicket.BookingReference))
                errors.AppendLine("Введите номер брони");
            if (comboConfirmed.Visibility == Visibility.Visible)
            {
                if (string.IsNullOrEmpty(comboConfirmed.Text))
                    errors.AppendLine("Выберите статус");
                currentTicket.Confirmed = comboConfirmed.SelectedItem.Equals("Активный") ? true : false;
            }
            else
                currentTicket.Confirmed = true;
            currentTicket.UserID = Manager.user.ID;

            if (errors.Length > 0)
            {
                System.Windows.MessageBox.Show(errors.ToString());
                return;
            }



            if (currentTicket.ID == 0)
                AirlineCompanyEntities.getContext().Tickets.Add(currentTicket);
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

        private void txtLastNameClient_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^а-яА-Яa-zA-Z]");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void txtFirstNameClient_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^а-яА-Яa-zA-Z]");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void txtPhone_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void txtBookingReference_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
