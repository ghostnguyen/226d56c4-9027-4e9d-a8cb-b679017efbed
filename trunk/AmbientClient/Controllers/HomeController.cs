using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AmbientClient.Models;

namespace AmbientClient.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            using (var db = new DB())
            {
                User a = new User();
                a.Emaila = "bypasslogin@gmail.com";

                db.Users.Add(a);
                db.SaveChanges();
            }
            return View();
        }

    }
}
