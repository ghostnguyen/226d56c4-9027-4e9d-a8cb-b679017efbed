using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AmbientClient.Models;
using AmbientClient.BLL;
using System.Web.Security;
using DotNetOpenAuth.OpenId.RelyingParty;
using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;
using DotNetOpenAuth.Messaging;

namespace AmbientClient.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            DB context = new DB();

            var l = new List<User>() 
            { 
                new User() { Email = "bypasslogin@gmail.com" }, 
            };
            l.ForEach(r => context.Users.Add(r));

            context.SaveChanges();

            return View();
        }

        [ValidateInput(false)]
        public ActionResult OpenIdLogOn(string returnUrl)
        {
            if (App.ByPassLogin && !Request.IsAuthenticated)
            {
                string email = UserBLL.I.Create_ByPassLogin().Email;
                this.IssueAuthTicket(email, true);

                if (string.IsNullOrEmpty(returnUrl))
                    returnUrl = "~/";

                return Redirect(returnUrl);
            }
            else
            {
                var openid = new OpenIdRelyingParty();
                var response = openid.GetResponse();
                if (response == null)  // Initial operation
                {
                    // Step 1 - Send the request to the OpenId provider server
                    string openid_identifier = "https://www.google.com/accounts/o8/id";
                    //Identifier id;

                    try
                    {
                        var req = openid.CreateRequest(openid_identifier);

                        var fetch = new FetchRequest();
                        fetch.Attributes.AddRequired(WellKnownAttributes.Contact.Email);
                        fetch.Attributes.AddRequired(WellKnownAttributes.Name.First);
                        fetch.Attributes.AddRequired(WellKnownAttributes.Name.Last);

                        req.AddExtension(fetch);


                        return req.RedirectingResponse.AsActionResult();
                    }
                    catch (ProtocolException)
                    {
                        // display error by showing original LogOn view
                        //this.ErrorDisplay.ShowError("Unable to authenticate: " + ex.Message);
                        return View("Logon");
                    }

                }
                else  // OpenId redirection callback
                {
                    // Step 2: OpenID Provider sending assertion response
                    switch (response.Status)
                    {
                        case AuthenticationStatus.Authenticated:
                            string identifier = response.ClaimedIdentifier;

                            var fetch = response.GetExtension<FetchResponse>();
                            string email = string.Empty;
                            string fullname = string.Empty;
                            if (fetch != null)
                            {
                                email = fetch.GetAttributeValue(WellKnownAttributes.Contact.Email);
                                fullname = fetch.GetAttributeValue(WellKnownAttributes.Name.FullName);
                            }

                            //if (repo.Exist(email, identifier))
                            //{

                            //}
                            //else
                            //{
                            //    repo.Create(email, identifier);
                            //}

                            //this.IssueAuthTicket(email, true);

                            //RoleRepository roleRepo = new RoleRepository();
                            //if (roleRepo.GetRolesList(email).Count() == 0)
                            //{
                            //    returnUrl = "~/Account/Guest";
                            //}

                            if (string.IsNullOrEmpty(returnUrl))
                                returnUrl = "~/";

                            return Redirect(returnUrl);

                        case AuthenticationStatus.Canceled:
                            //this.ErrorDisplay.ShowMessage("Canceled at provider");
                            //return View("LogOn", this.ViewModel);
                            return View("LogOn");
                        case AuthenticationStatus.Failed:
                            //this.ErrorDisplay.ShowError(response.Exception.Message);
                            //return View("LogOn", this.ViewModel);
                            return View("LogOn");
                    }
                }
            }
            return new EmptyResult();
        }

        private void IssueAuthTicket(string userId, bool rememberMe)
        {
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, userId,
                                                                 DateTime.Now, DateTime.Now.AddDays(10),
                                                                 rememberMe, userId);

            string ticketString = FormsAuthentication.Encrypt(ticket);
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, ticketString);
            if (rememberMe)
                cookie.Expires = DateTime.Now.AddDays(10);

            HttpContext.Response.Cookies.Add(cookie);
        }

    }
}
