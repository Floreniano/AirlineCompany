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
using Airport.Data;
using Airport.Pages;
using Airport.Windows;

namespace Airport.Windows
{
    /// <summary>
    /// Логика взаимодействия для StartWindow.xaml
    /// </summary>
    public partial class StartWindow : Window
    {
        public StartWindow()
        {
            InitializeComponent(); 
            if (Manager.user.Roles.Title == "Administrator")
            {
                MainFrame.Navigate(new StartAdminPage());
                Manager.mainFrame = MainFrame;
                btnSheduleFlight.Visibility = Visibility.Collapsed;
                panelHelloUser.Visibility = Visibility.Collapsed;
                btnOffice.Visibility = Visibility.Collapsed;
                btnRoute.Visibility = Visibility.Collapsed;
                btnAircraft.Visibility = Visibility.Collapsed;
                btnTicket.Visibility = Visibility.Collapsed;
                btnAirport.Visibility = Visibility.Collapsed;
                btnReport.Visibility = Visibility.Collapsed;
            }
            if (Manager.user.Roles.Title == "Manager")
            {
                Manager.mainFrame = MainFrame;
                btnHistoryAuthorization.Visibility = Visibility.Collapsed;
                btnAddUser.Visibility = Visibility.Collapsed;
                btnTicket.Visibility = Visibility.Collapsed;
            }
            if (Manager.user.Roles.Title == "Booker")
            {
                Manager.mainFrame = MainFrame;
                btnHistoryAuthorization.Visibility = Visibility.Collapsed;
                btnAddUser.Visibility = Visibility.Collapsed;
                btnOffice.Visibility = Visibility.Collapsed;
                btnAirport.Visibility = Visibility.Collapsed;
                btnAircraft.Visibility = Visibility.Collapsed;
            }
            labelFirstName.Content = Manager.user.FirstName;
            labelLastName.Content = Manager.user.LastName + ".";
            btnBack.Visibility = Visibility.Collapsed;
        }

        private void btnLogOut_Click(object sender, RoutedEventArgs e)
        {
            AuthorizationWindow authorizationWindow = new AuthorizationWindow();
            this.Close();
            authorizationWindow.Show();
        }

        private void btnHistoryAuthorization_Click(object sender, RoutedEventArgs e)
        {
            Manager.mainFrame.Navigate(new HistoryAuthorizationPage());
            panelHelloUser.Visibility = Visibility.Collapsed;
        }

        private void MainFrame_ContentRendered(object sender, EventArgs e)
        {
            if (Manager.mainFrame.CanGoBack)
            {
                btnBack.Visibility = Visibility.Visible;
            }
            else
            {
                btnBack.Visibility = Visibility.Hidden;
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            Manager.mainFrame.GoBack();
        }

        private void btnAddUser_Click(object sender, RoutedEventArgs e)
        {
            Manager.mainFrame.Navigate(new StartAdminPage());
            panelHelloUser.Visibility = Visibility.Collapsed;
        }

        private void btnSheduleFlight_Click(object sender, RoutedEventArgs e)
        {
            Manager.mainFrame.Navigate(new ScheduleFligthPage());
            panelHelloUser.Visibility = Visibility.Collapsed;
        }

        private void btnEditPassword_Click(object sender, RoutedEventArgs e)
        {
            ChangePasswordWindow changePasswordWindow = new ChangePasswordWindow();
            changePasswordWindow.ShowDialog();
        }

        private void btnOffice_Click(object sender, RoutedEventArgs e)
        {
            Manager.mainFrame.Navigate(new OfficePage());
            panelHelloUser.Visibility = Visibility.Collapsed;
        }

        private void btnAircraft_Click(object sender, RoutedEventArgs e)
        {
            Manager.mainFrame.Navigate(new AircraftsPage());
            panelHelloUser.Visibility = Visibility.Collapsed;
        }

        private void btnRoute_Click(object sender, RoutedEventArgs e)
        {
            Manager.mainFrame.Navigate(new RoutePage());
            panelHelloUser.Visibility = Visibility.Collapsed;
        }

        private void btnTicket_Click(object sender, RoutedEventArgs e)
        {
            Manager.mainFrame.Navigate(new TicketPage());
            panelHelloUser.Visibility = Visibility.Collapsed;
        }

        private void btnAirport_Click(object sender, RoutedEventArgs e)
        {
            Manager.mainFrame.Navigate(new AirportPage());
            panelHelloUser.Visibility = Visibility.Collapsed;
        }

        private void btnReport_Click(object sender, RoutedEventArgs e)
        {
            Manager.mainFrame.Navigate(new PrintPage());
            panelHelloUser.Visibility = Visibility.Collapsed;
        }
    }
}
