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

namespace Habraken_SLE.Overlays
{
    /// <summary>
    /// Interaction logic for Usermanagement.xaml
    /// </summary>
    public partial class Usermanagement : UserControl
    {
        string action;

        public Usermanagement()
        {
            InitializeComponent();

        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            if (ucUsers.Visibility == Visibility.Visible)
            {
                ucUsers.gbAddEditUsers.IsEnabled = true;
                action = "New";

                btnNew.IsEnabled = false;
                btnEdit.IsEnabled = false;
                btnDelete.IsEnabled = false;
                btnSave.IsEnabled = true;
                btnCancel.IsEnabled = true;
            }            
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                switch (action)
                {
                    case "New":
                        if (ucUsers.gbAddEditUsers.IsEnabled == true)
                        {
                            try
                            {
                                ucUsers.SaveNewUser();
                            }
                            catch
                            {
                                MessageBox.Show("Something went wrong while saving the NEW User");
                            }
                            finally
                            {
                                ucUsers.gbAddEditUsers.IsEnabled = false;
                                btnNew.IsEnabled = true;
                                btnEdit.IsEnabled = true;
                                btnDelete.IsEnabled = true;
                                btnSave.IsEnabled = false;
                                btnCancel.IsEnabled = false;
                            }
                        }
                        break;

                    case "Edit":

                        if (ucUsers.gbAddEditUsers.IsEnabled == true)
                        {
                            try
                            {
                                if (ucUsers.lvUsers.SelectedItem != null)
                                {
                                    string[] selectedUser = ucUsers.lvUsers.SelectedItem.ToString().Split(',');

                                    ucUsers.UpdateUser(selectedUser[0]);
                                }
                            }
                            catch
                            {
                                MessageBox.Show("Something went wrong while saving the UPDATED User");
                            }
                            finally
                            {
                                ucUsers.gbAddEditUsers.IsEnabled = false;
                                btnNew.IsEnabled = true;
                                btnEdit.IsEnabled = true;
                                btnDelete.IsEnabled = true;
                                btnSave.IsEnabled = false;
                                btnCancel.IsEnabled = false;
                            }

                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong while Saving Changes");
            }
            finally
            {
                action = "";
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (ucUsers.lvUsers.SelectedItem != null)
            {
                ucUsers.gbAddEditUsers.IsEnabled = true;
                
                string[] selectedUser = ucUsers.lvUsers.SelectedItem.ToString().Split(',');

                ucUsers.RewriteUser(selectedUser[0]);

                action = "Edit";
                btnNew.IsEnabled = false;
                btnEdit.IsEnabled = false;
                btnDelete.IsEnabled = false;
                btnSave.IsEnabled = true;
                btnCancel.IsEnabled = true;
            }
            else
            {
                MessageBox.Show("Select a User First");
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (ucUsers.lvUsers.SelectedItem != null)
            {
                if (MessageBox.Show("Do you really want to DELETE this User?", "Attention", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                { 
                    ucUsers.gbAddEditUsers.IsEnabled = true;
                    string[] selectedUser = ucUsers.lvUsers.SelectedItem.ToString().Split(',');

                    ucUsers.DeleteUser(selectedUser[0]);
                }
            }
            else
            {
                MessageBox.Show("Select a User First");
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Do you really want to Cancel these changes?", "Attention", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
            {
                action = "";

                ucUsers.CancelAddEditUser();

                btnNew.IsEnabled = true;
                btnEdit.IsEnabled = true;
                btnDelete.IsEnabled = true;               
                btnSave.IsEnabled = false;
                btnCancel.IsEnabled = false;
            }
        }
    }
}
