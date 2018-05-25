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

namespace Habraken_SLE
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Database db;
        User user;

        public MainWindow()
        {
            db = new Database();
            user = new User();

            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (user.isLoggedIn)
            {
                if (MessageBox.Show("Do you really want to log out?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    home.Visibility = Visibility.Collapsed;
                    logo.Visibility = Visibility.Visible;
                    pb_Login.Visibility = Visibility.Visible;

                    pb_Login.Clear();

                    user.isLoggedIn = false;

                    imgLogin.Source = new BitmapImage(new Uri(@"\Images\Login.png", UriKind.RelativeOrAbsolute));
                }
            }
            else
            {
                user.Login(pb_Login.Password);

                if (user.isLoggedIn)
                {
                    home.Visibility = Visibility.Visible;
                    logo.Visibility = Visibility.Collapsed;
                    pb_Login.Visibility = Visibility.Collapsed;

                    imgLogin.Source = new BitmapImage(new Uri(@"\Images\Logout.png", UriKind.RelativeOrAbsolute));

                    lblUserName.Content = user.currentUser;
                    lblUserProfile.Content = user.currentProfile;

                    if (!user.canEdit)
                    {
                        home.btnDesigner.Visibility = Visibility.Hidden;
                        home.btnUsermanagement.Visibility = Visibility.Hidden;
                    }
                    else
                    {
                        home.btnDesigner.Visibility = Visibility.Visible;
                        home.btnUsermanagement.Visibility = Visibility.Visible;
                    }
                }
                else
                {
                    MessageBox.Show("Wrong ID");
                }
            }
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Close Application?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }

        private void btnHome_MouseUp(object sender, MouseButtonEventArgs e)
        {
            home.gridUsermanagement.Visibility = Visibility.Collapsed;
            home.gridDesigner.Visibility = Visibility.Collapsed;
            home.gridNav.Visibility = Visibility.Visible;
        }
    }
}
