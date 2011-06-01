using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AmbientClient.Models;

namespace AmbientClient.BLL
{
    public class UserBLL : BaseBLL<UserBLL>
    {
        public User Create_ByPassLogin()
        {
            string email = "bypasslogin@gmail.com";

            var v = Get(email);

            if (v == null)
            {
                v = new User() { Email = email };
                DB.Users.Add(v);
                Save();
            }

            return v;
        }

        public User Get(string email)
        {
            return DB.Users.Where(r => r.Email == email).FirstOrDefault();
        }
    }
}