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
using System.Windows.Threading;
using Airport.Classes;
using Airport.Data;
using Airport.Pages;
using Airport.Windows;

namespace Airport.Windows
{
    /// <summary>
    /// Логика взаимодействия для AuthorizationWindow.xaml
    /// </summary>
    public partial class AuthorizationWindow : Window
    {
        private int countOfAuthorization = 0;
        private DispatcherTimer timer = new DispatcherTimer();
        public AuthorizationWindow()
        {
            InitializeComponent(); 
            txtLogin.Text = "manager";
            passPassword.Password = "manager";
            txtPassword.Visibility = Visibility.Hidden;
            imgShowHidePassword.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/HiddenPassword.png"));

        }

        private void btnAuthorization_Click(object sender, RoutedEventArgs e)
        {
            int counter = 0;
            int userId = 0;
            var userAuth = new HistoryAuthorization();
            try
            {
                foreach (var user in AirlineCompanyEntities.getContext().Users)
                {
                    counter++;
                    if (txtLogin.Text == user.Login && (txtPassword.Text == user.Password || passPassword.Password == user.Password) && user.Active == true)
                    {
                        counter = 0;
                        userAuth.Date = DateTime.Now;
                        userAuth.Status = "Успешно";
                        userAuth.IdUser = user.ID;

                        Manager.user = user;

                        StartWindow mainWindow = new StartWindow();
                        MessageBox.Show("Вы успешно авторизовались!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                        this.Close();
                        mainWindow.Show();
                        countOfAuthorization = 0;
                        break;
                    }                  
                    if (txtLogin.Text == user.Login)
                    {
                        userId = user.ID;
                    }
                }
                if (counter == 0)
                {
                    try
                    {
                        AirlineCompanyEntities.getContext().HistoryAuthorization.Add(userAuth);
                        AirlineCompanyEntities.getContext().SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка" + ex.Message.ToString());
                    }
                }
                if (counter != 0)
                {
                    MessageBox.Show("Вы ввели некорректные данные или ваша учетная запись не активна", "Информация", MessageBoxButton.OK, MessageBoxImage.Error);
                    countOfAuthorization++;
                    if (txtLogin.Text != "")
                    {
                        try
                        {
                            if (userId != 0)
                            {
                                userAuth.Date = DateTime.Now;
                                userAuth.Status = "Не успешно";
                                userAuth.IdUser = userId;

                                AirlineCompanyEntities.getContext().HistoryAuthorization.Add(userAuth);
                                AirlineCompanyEntities.getContext().SaveChanges();
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Ошибка" + ex.Message.ToString());
                        }
                    }
                    if (countOfAuthorization == 3)
                    {
                        btnAuthorization.Visibility = Visibility.Hidden;
                        countOfAuthorization = 0;
                        labelWait.Content = $"Повторите попытку через 10 секунд";
                        timer.Tick += new EventHandler(dispatcherTimer_Tick);
                        timer.Interval = new TimeSpan(0, 0, 10);
                        timer.Start();
                    }

                }
            }
            catch
            {
                MessageBox.Show("Ошибка соединения с БД", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            btnAuthorization.Visibility = Visibility.Visible;
            labelWait.Content = "";
        }

        private void imgShowHidePassword_MouseLeave(object sender, MouseEventArgs e)
        {
            HidePassword();
        }

        private void imgShowHidePassword_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            ShowPassword();
        }

        private void imgShowHidePassword_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            HidePassword();
        }
        void ShowPassword()
        {
            imgShowHidePassword.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/ShowPassword.png"));
            txtPassword.Visibility = Visibility.Visible;
            passPassword.Visibility = Visibility.Hidden;
            txtPassword.Text = passPassword.Password;
        }
        void HidePassword()
        {
            imgShowHidePassword.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/HiddenPassword.png"));
            txtPassword.Visibility = Visibility.Hidden;
            passPassword.Visibility = Visibility.Visible;
            passPassword.Focus();
        }
    }
}
