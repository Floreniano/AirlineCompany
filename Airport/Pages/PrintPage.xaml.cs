using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using Airport.Data;
using Airport.Classes;

namespace Airport.Pages
{
    /// <summary>
    /// Логика взаимодействия для PrintPage.xaml
    /// </summary>
    public partial class PrintPage : Page
    {
        public PrintPage()
        {
            InitializeComponent();
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

       

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            InvokePrintReport(flowDocPrint);
        }
        public static void InvokePrintReport(FlowDocument doctoprint)
        {

            PrintDialog printDialog = new PrintDialog();
            printDialog.PageRangeSelection = PageRangeSelection.AllPages;
            printDialog.UserPageRangeEnabled = true;
            Nullable<Boolean> print = printDialog.ShowDialog();

            if (print == true)
            {
                doctoprint.PageWidth = printDialog.PrintableAreaWidth;
                doctoprint.ColumnGap = 0;
                doctoprint.ColumnWidth = printDialog.PrintableAreaWidth;
                IDocumentPaginatorSource idp = doctoprint as IDocumentPaginatorSource; printDialog.PrintDocument(idp.DocumentPaginator, "Report");
                Manager.mainFrame.GoBack();
            }
        }
        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (datePickerStartDate.SelectedDate != null && datePickerEndDate.SelectedDate != null)
            {
                if (datePickerStartDate.SelectedDate < datePickerEndDate.SelectedDate)
                {
                    dGridTickets.ItemsSource = AirlineCompanyEntities.getContext().Tickets.Where(p => p.Schedules.Date < datePickerEndDate.SelectedDate && p.Schedules.Date > datePickerStartDate.SelectedDate).ToList();
                    if (dGridTickets.Items.Count == 0)
                    {
                        labelNotFound.Visibility = Visibility.Visible;
                        dGridTickets.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        labelNotFound.Visibility = Visibility.Collapsed;
                        dGridTickets.Visibility = Visibility.Visible;                        
                    }
                }
                else
                {
                    MessageBox.Show("Начальная дата не можеть быть меньше конечной", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                
            }
            else
            {
                dGridTickets.Visibility = Visibility.Visible;
                labelNotFound.Visibility = Visibility.Collapsed;
                dGridTickets.ItemsSource = AirlineCompanyEntities.getContext().Tickets.ToList();
            }


            if (datePickerStartDate.SelectedDate == null)
            {
                dGridTickets.ItemsSource = AirlineCompanyEntities.getContext().Tickets.Where(p => p.Schedules.Date < datePickerEndDate.SelectedDate).ToList();
                if (dGridTickets.Items.Count == 0)
                {
                    labelNotFound.Visibility = Visibility.Visible;
                    dGridTickets.Visibility = Visibility.Collapsed;
                }
                else
                {
                    labelNotFound.Visibility = Visibility.Collapsed;
                    dGridTickets.Visibility = Visibility.Visible;
                }
            }
            else
            {
                dGridTickets.Visibility = Visibility.Visible;
                labelNotFound.Visibility = Visibility.Collapsed;
                dGridTickets.ItemsSource = AirlineCompanyEntities.getContext().Tickets.ToList();
            }
            if (datePickerEndDate.SelectedDate == null)
            {
                dGridTickets.ItemsSource = AirlineCompanyEntities.getContext().Tickets.Where(p => p.Schedules.Date > datePickerStartDate.SelectedDate).ToList();
                if (dGridTickets.Items.Count == 0)
                {
                    labelNotFound.Visibility = Visibility.Visible;
                    dGridTickets.Visibility = Visibility.Collapsed;
                }
                else
                {
                    labelNotFound.Visibility = Visibility.Collapsed;
                    dGridTickets.Visibility = Visibility.Visible;
                }
            }
            else
            {
                dGridTickets.Visibility = Visibility.Visible;
                labelNotFound.Visibility = Visibility.Collapsed;
                dGridTickets.ItemsSource = AirlineCompanyEntities.getContext().Tickets.ToList();
            }

        }
        
    }
}
