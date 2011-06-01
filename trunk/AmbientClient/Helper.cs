using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace AmbientClient
{
    public class Helper
    {
    }
    public class App
    {
        static public bool ByPassLogin
        {
            get
            {
                return bool.Parse(ConfigurationManager.AppSettings["ByPassLogin"]);
            }
        }

        public static string Username
        {
            get
            {
                return HttpContext.Current.User.Identity.Name;
            }
        }
    }
}