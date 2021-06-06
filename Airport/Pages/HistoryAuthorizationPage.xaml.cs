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
namespace Airport.Pages
{
    /// <summary>
    /// Логика взаимодействия для HistoryAuthorizationPage.xaml
    /// </summary>
    public partial class HistoryAuthorizationPage : Page
    {
        public HistoryAuthorizationPage()
        {
            InitializeComponent();
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                AirlineCompanyEntities.getContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                dGridHistoryAuthorization.ItemsSource = AirlineCompanyEntities.getContext().HistoryAuthorization.ToList();
            }
            catch
            {
                MessageBox.Show("Ошибка соединения с БД", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void txtSearchAuthorization_TextChanged(object sender, TextChangedEventArgs e)
        {
            List<HistoryAuthorization> currentAuthorization = AirlineCompanyEntities.getContext().HistoryAuthorization.ToList();
            currentAuthorization = currentAuthorization.Where(p => p.Users.Login.ToLower().Contains(txtSearchAuthorization.Text.ToLower()) || p.Date.ToShortDateString().Contains(txtSearchAuthorization.Text) || p.Status.ToString().Contains(txtSearchAuthorization.Text)).ToList();
            dGridHistoryAuthorization.ItemsSource = currentAuthorization.ToList();
            if (dGridHistoryAuthorization.Items.Count == 0)
            {
                labelNotFound.Visibility = Visibility.Visible;
                dGridHistoryAuthorization.Visibility = Visibility.Collapsed;
            }
            else
            {
                dGridHistoryAuthorization.Visibility = Visibility.Visible;
                labelNotFound.Visibility = Visibility.Collapsed;
            }
        }
    }
}
