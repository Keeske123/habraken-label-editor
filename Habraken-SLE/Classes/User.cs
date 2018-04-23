using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Habraken_SLE
{
    public class User
    {
        public bool isLoggedIn { get; set; }
        public string currentUser { get; set; }
        public string currentProfile { get; set; }

        public User()
        {

        }

        public void Login(string input)
        {
            try
            {
                var db = new HLE_LinqtoSQLDataContext();
                tbl_User t = null;

                t = db.tbl_Users.Single(p => p.UserID == input);

                if (t != null)
                {
                    isLoggedIn = true;

                    var query = from q in db.tbl_Users
                                where q.UserID == input
                                select q;

                   //var abc = db.tbl_Users.Where(user => user.UserID == input).FirstOrDefault();

                    foreach (var user in query)
                    {
                        currentUser = user.UserName;
                        currentProfile = user.UserProfile;
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