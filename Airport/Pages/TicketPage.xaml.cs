using Airport.Classes;
using Airport.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace Airport.Pages
{
    /// <summary>
    /// Логика взаимодействия для TicketPage.xaml
    /// </summary>
    public partial class TicketPage : Page
    {
        public TicketPage()
        {
            InitializeComponent();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var ticketsForRemoving = dGridTickets.SelectedItems.Cast<Tickets>().ToList();
            if (MessageBox.Show($"Вы точно хотите удалить выделенный(ые) билет(ы)?\nКоличество выбранных элементов = {ticketsForRemoving.Count}", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    AirlineCompanyEntities.getContext().Tickets.RemoveRange(ticketsForRemoving);
                    AirlineCompanyEntities.getContext().SaveChanges();
                    MessageBox.Show("Данные удалены!");
                    dGridTickets.ItemsSource = AirlineCompanyEntities.getContext().Tickets.ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Manager.mainFrame.Navigate(new AddEditTicketPage(null));
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            Manager.mainFrame.Navigate(new AddEditTicketPage((sender as Button).DataContext as Tickets));
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                AirlineCompanyEntities.getContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                dGridTickets.ItemsSource = AirlineCompanyEntities.getContext().Tickets.ToList();
            }
            catch
            {
                MessageBox.Show("Ошибка соединения с БД", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void txtSearchTicket_TextChanged(object sender, TextChangedEventArgs e)
        {
            List<Tickets> currentTicket = AirlineCompanyEntities.getContext().Tickets.ToList();
            currentTicket = currentTicket.Where(p => p.Firstname.ToLower().Contains(txtSearchTicket.Text.ToLower()) 
            || p.Lastname.ToLower().Contains(txtSearchTicket.Text.ToLower())
            || p.Users.LastName.ToLower().Contains(txtSearchTicket.Text.ToLower())
            || p.Users.FirstName.ToLower().Contains(txtSearchTicket.Text.ToLower())
            || p.Schedules.Routes.Airports.IATACode.ToLower().Contains(txtSearchTicket.Text.ToLower())
            || p.Schedules.Routes.Airports1.IATACode.ToLower().Contains(txtSearchTicket.Text.ToLower())
            || p.Schedules.Date.ToShortDateString().Contains(txtSearchTicket.Text)).ToList();
            dGridTickets.ItemsSource = currentTicket.ToList();
            if (dGridTickets.Items.Count == 0)
            {
                labelNotFound.Visibility = Visibility.Visible;
                dGridTickets.Visibility = Visibility.Collapsed;
            }
            else
            {                
                dGridTickets.Visibility = Visibility.Visible;
                labelNotFound.Visibility = Visibility.Collapsed;                
            }
        }
    }
}
