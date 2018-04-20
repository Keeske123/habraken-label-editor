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
                designer.Visibility = Visibility.Collapsed;
                Logo.Visibility = Visibility.Visible;
                pb_Login.Visibility = Visibility.Visible;

                pb_Login.Clear();

                user.isLoggedIn = false;

                imgLogin.Source = new BitmapImage(new Uri(@"\Images\Login.png", UriKind.RelativeOrAbsolute));
            }
            else
            {
                user.Login(pb_Login.Password);

                if (user.isLoggedIn)
                {
                    designer.Visibility = Visibility.Visible;
                    Logo.Visibility = Visibility.Collapsed;
                    pb_Login.Visibility = Visibility.Collapsed;

                    imgLogin.Source = new BitmapImage(new Uri(@"\Images\Logout.png", UriKind.RelativeOrAbsolute));

                    lblUserName.Content = user.currentUser;
                    lblUserProfile.Content = user.currentProfile;

                }
                else
                {
                    MessageBox.Show("Wrong ID");
                }
            }
        }
    }
}
