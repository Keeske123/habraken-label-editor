using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Habraken_SLE
{
    public class User
    {
        string con = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\keese_000\Desktop\OPLEVERING KT 2\20180525_P12_KT2_03_Elsman-Kees\Habraken\Habraken-SLE\dbLabelEditor.mdf;Integrated Security=True";
        public bool isLoggedIn { get; set; }

        public string currentUser { get; set; }
        public string currentProfile { get; set; }

        public bool canEdit { get; set; }
        public bool canPrint { get; set; }
        public bool canSave { get; set; }

        public User()
        {

        }

        public void Login(string input)
        {
            try
            {
                var db = new HLE_LinqtoSQLDataContext(con);
                tbl_User t = null;

                var query = from q in db.tbl_Users
                            where q.UserID == input
                            select q;


                if (query.Any())
                {
                    isLoggedIn = true;

                    foreach (var user in query)
                    {
                        currentUser = user.UserName;

                        var selectProfile = from q in db.tbl_userProfiles
                                            where q.Id == user.UserProfileID
                                            select q;

                        foreach (var profile in selectProfile)
                        {
                            currentProfile = profile.Userprofile;
                            canEdit = profile.CanEdit;
                            canPrint = profile.CanPrint;
                            canSave = profile.CanSave;
                        }
                    }
                }                
            }
            catch
            {
                isLoggedIn = false;
            }            
        }

        public void GetRights()
        {
            throw new System.NotImplementedException();
        }

      
    }
}