using MVCBootstrapDemo.App_Start;
using MVCBootstrapDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MVCBootstrapDemo.Controllers
{
    public class HomeController : Controller
    {
        private BootstrapMVCEntities db = new BootstrapMVCEntities();
        

        [HttpGet]
        public ActionResult Login(string returnurl)
        {
            var userinfo = new LoginModel();
            try
            {
                // We do not want to use any existing identity information
                EnsureLoggedOut();
                // Store the originating URL so we can attach it to a form field
                userinfo.ReturnURL = returnurl;
                return View(userinfo);
            }
            catch
            {
                throw;
            }
            
        }

        private void EnsureLoggedOut()
        {
            if (Request.IsAuthenticated)
                Logout();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        private ActionResult Logout()
        {
            try
            {
                FormsAuthentication.SignOut();
                HttpContext.User = new GenericPrincipal(new GenericIdentity(string.Empty),null);
                Session.Clear();
                System.Web.HttpContext.Current.Session.RemoveAll();
                return RedirectToLocal();
            }
            catch
            {
                throw;

            }
        }

        private ActionResult RedirectToLocal(string returnURL = "")
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(returnURL) && Url.IsLocalUrl(returnURL))
                    return Redirect(returnURL);
                    return RedirectToAction("Dashboard", "Admin");      
             }
            catch
            {
                throw;
            }
        }


        //GET: SignInAsync   
        private void SignInRemember(string userName, bool isPersistent = false)
        {
            // Clear any lingering authencation data
            FormsAuthentication.SignOut();
            // Write the authentication cookie
            FormsAuthentication.SetAuthCookie(userName, isPersistent);
        }



        #region --> Login POST Method

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel entity)
        {
            string OldHASHValue = string.Empty;
            byte[] SALT = new byte[Common.saltLengthLimit];
            try
            {
                using (db = new BootstrapMVCEntities())
                {
                    // Ensure we have a valid viewModel to work with
                    if (!ModelState.IsValid)
                        return View(entity);

                    //Retrive Stored HASH Value From Database According To Username (one unique field)
                    var userInfo = db.users.Where(s => s.U_Name == entity.Username.Trim()).FirstOrDefault();

                    //Assign HASH Value
                    if (userInfo != null)
                    {
                        OldHASHValue = userInfo.U_Password;                      
                    }                  
                    bool isLogin = CompareHashValue(entity.Password,OldHASHValue);

                    if (isLogin)
                    {
                        //Login Success
                        //For Set Authentication in Cookie (Remeber ME Option)
                        SignInRemember(entity.Username, entity.isRemember);

                        //Set A Unique ID in session
                        Session["UserID"] = userInfo.U_ID;

                        // If we got this far, something failed, redisplay form
                        // return RedirectToAction("Index", "Dashboard");
                        return RedirectToLocal(entity.ReturnURL);
                    }
                    else
                    {
                        //Login Fail
                        TempData["ErrorMSG"] = "Access Denied! Please Try Again";
                        return View(entity);
                    }
                }
            }
            catch
            {
                throw;
            }

        }

        #endregion

       

       

        #region --> Comapare HASH Value
        public static bool CompareHashValue(string password, string OldHASHValue)
        {
            try
            {
                string expectedHashString = Common.Encrypt(password);

                return (OldHASHValue == expectedHashString);
            }
            catch
            {
                return false;
            }
        }
        #endregion



        public ActionResult Index()
        {          
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Movie()
        {
            return View();
        }

        public ActionResult Users()
        {
            return RedirectToAction("Users", "User");

        }

        public ActionResult MainCategories()
        {
            return RedirectToAction("MainCategories", "MainCategories");

        }

        public ActionResult Categories()
        {
            return RedirectToAction("Categories", "Categories");

        }

        public ActionResult SubCategories()
        {
            return RedirectToAction("SubCategories", "SubCategories");

        }

        public ActionResult UnitTypes()
        {
            return RedirectToAction("UnitTypes", "UnitTypes");

        }

        public ActionResult Currencies()
        {
            return RedirectToAction("Currencies", "Currencies");

        }

        public ActionResult Positions()
        {
            return RedirectToAction("Positions", "Positions");

        }

        public ActionResult Departments()
        {
            return RedirectToAction("Departments", "Departments");

        }

        public ActionResult Employees()
        {
            return RedirectToAction("Employees", "Employees");

        }

    }
}