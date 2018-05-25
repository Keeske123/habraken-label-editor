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
    public partial class Users : UserControl
    {
        //Usermanagement usermanagement;
        //HLE_LinqtoSQLDataContext db;
        public string con;

        public Users()
        { }

        public Users(ref Button button)
        {
            con = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\keese_000\Desktop\AFSTUDEER STAGE!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!\Periode 11-12\Habraken\Habraken-SLE\dbLabelEditor.mdf;Integrated Security=True";

            InitializeComponent();

            LoadUsersToListView();
            LoadProfiles();
            
           // db = new HLE_LinqtoSQLDataContext(con);
        }

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            
            LoadUsersToListView();
            if (tbSearch.Text != "" || tbSearch.Text != null)
            {
                IEnumerable<string> lv = lvUsers.Items.Cast<string>();

                var query = from q in lv
                            where q.ToLower().Split(',')[0].Contains(tbSearch.Text.ToLower())
                            select q;

                string[] results = query.ToArray();

                lvUsers.Items.Clear();


                foreach (var item in results)
                {
                    lvUsers.Items.Add(String.Format(item.ToString()));
                }
            }
            else
            {
                LoadUsersToListView();
            }
        }

        public void LoadProfiles()
        {
            try
            {
                var db = new HLE_LinqtoSQLDataContext(con);

                var query =
                     from q in db.tbl_userProfiles
                     select q.Userprofile;

                foreach (var item in query)
                {
                    cbRights.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Something went wrong while loading User Profiles to Combobox");
            }
        }

        public void UpdateUser(string username)
        {
            try
            {
                var db = new HLE_LinqtoSQLDataContext(con);

                var query = (from q in db.tbl_Users
                             where q.UserName == username
                             select q).SingleOrDefault();

                query.UserName = tbName.Text;
                query.UserID = tbOperatortag.Text;
                query.UserProfileID = GetUserProfileID();
                query.IsActive = checkActive.IsChecked.Value;

                db.SubmitChanges();
                Resetvalues();
            }
            catch
            {
                MessageBox.Show("Something went wrong While Updating this user");
            }
            finally
            {
                LoadUsersToListView();
            }
        }
        
        public void DeleteUser(string username)
        {
            var db = new HLE_LinqtoSQLDataContext(con);

            var query = from q in db.tbl_Users
                        where q.UserName == username
                        select q;

            foreach (var item in query)
            {
                db.tbl_Users.DeleteOnSubmit(item);
            }

            try
            {
                db.SubmitChanges();
                LoadUsersToListView();
                Resetvalues();
            }
            catch
            {
                MessageBox.Show("Something went wrong while Deleting User");
            }
        }

        public void SaveNewUser()
        {
            try
            {
                var db = new HLE_LinqtoSQLDataContext(con);

                tbl_User user = new tbl_User
                {
                    UserName = tbName.Text,
                    UserProfileID = GetUserProfileID(),
                    IsActive = checkActive.IsChecked.Value,
                    UserID = tbOperatortag.Text
                };

                db.tbl_Users.InsertOnSubmit(user);

                db.SubmitChanges();
            }
            catch
            {
                MessageBox.Show("Something went wrong While Adding this user");
            }
            finally
            {
                LoadUsersToListView();
            }
        }

        public void CancelAddEditUser()
        {
            Resetvalues();           
        }

        private void Resetvalues()
        {
            tbName.Clear();
            tbOperatortag.Clear();
            cbRights.Text = "";
            checkActive.IsChecked = false;
            gbAddEditUsers.IsEnabled = false;            
        }

        private void LoadUsersToListView()
        {
            lvUsers.Items.Clear();
            try
            {
                var db = new HLE_LinqtoSQLDataContext(con);

                var query =
                    (from q in db.tbl_Users
                     select q).ToList();

                foreach (var item in query)
                {
                    string isActive;

                    if (!item.IsActive)
                    {
                        isActive = "Not active";
                    }
                    else
                    {
                        isActive = "Active";
                    }

                    lvUsers.Items.Add(String.Format(item.UserName.ToUpper() + ", " + item.UserID + "\n" + item.tbl_userProfile.Userprofile + "\n" + isActive));
                }
            }
            catch
            {
                MessageBox.Show("Something went wrong while Loading users into the Listview");
            }
        }

        public void RewriteUser(string username)
        {
            var db = new HLE_LinqtoSQLDataContext(con);

            var query = (from q in db.tbl_Users
                         where q.UserName == username
                         select q).SingleOrDefault();

            tbOperatortag.Text = query.UserID;
            tbName.Text = query.UserName;
            cbRights.SelectedItem = query.tbl_userProfile.Userprofile;
            checkActive.IsChecked = query.IsActive;           
        }

        private int GetUserProfileID()
        {
            int result = 0;

            try
            {
                var db = new HLE_LinqtoSQLDataContext(con);

                var query =
                    from q in db.tbl_userProfiles
                    where q.Userprofile == cbRights.SelectedItem.ToString()
                    select q.Id;

                foreach (var item in query)
                {
                    result = item;
                }
            }
            catch (Exception)
            {
                result = 0;
            }

            return result;
        }

    }
}
