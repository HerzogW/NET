using BusinessEntities;
using BusinessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace WebAppEFTest.Controllers
{
    public class AuthenticationController : Controller
    {
        // GET: Authentication
        public ActionResult Login()
        {
            return View("Login");
        }

        [HttpPost]
        public ActionResult DoLogin(UserDetails u)
        {
            if (ModelState.IsValid)
            {
                EmployeeBusinesslayer bal = new EmployeeBusinesslayer();

                UserStatus status = bal.GetUserValidity(u);

                bool IsAdmin = false;
                switch (status)
                {
                    case UserStatus.AuthenticatedAdmin:
                        IsAdmin = true;
                        break;
                    case UserStatus.AuthentuatedUser:
                        IsAdmin = false;
                        break;
                    case UserStatus.NonAuthenticatedUser:
                        ModelState.AddModelError("CredentialError", "Invalid UserName or Password");
                        return View("Login");
                }

                FormsAuthentication.SetAuthCookie(u.UserName, false);
                Session["IsAdmin"] = IsAdmin;
                return RedirectToAction("Index", "SPA/Main");
            }
            else
            {
                return View("Login");
            }
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}