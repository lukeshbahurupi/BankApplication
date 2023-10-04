using BankApplication.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace BankApplication.Controllers
{
    public class AccountsController : Controller
    {
        private BankApplication_dbContext db = new BankApplication_dbContext();
        //private readonly BankApplication_dbContext _context = new BankApplication_dbContext();
        // GET: Accounts
        public ActionResult UserProfile()
        {
            ViewModel model = new ViewModel();
            model.user = db.Users.FirstOrDefault(el => el.UserName == User.Identity.Name);
            model.form = db.ApplicationForms.FirstOrDefault(el => el.CustemerName == model.user.UserName);
            return View(model);
        } 
        #region/**********< USER >***********/
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Login_Signup model)
        {
            using(BankApplication_dbContext _context = new BankApplication_dbContext())
            {
                try
                {
                    User usr = _context.Users.FirstOrDefault(el => el.Email == model.Login.Email);
                    bool tokenvalid = false;
                    if (usr != null) 
                    { 
                        string token = usr.UniqueId.ToString().Split('0')[0];
                        if (token != null && token != "adminn") tokenvalid = true;
                        bool isValidUser = _context.Users.Any(user => user.Email == model.Login.Email && user.UserPassword == model.Login.Password);
                        if (isValidUser && tokenvalid)
                        {
                            FormsAuthentication.SetAuthCookie(usr.UserName, false);
                            //TempData["Success"] = $"Welcome{User.Identity.Name}";
                            //TempData.Keep("Success");
                            return RedirectToAction("Create", "ApplicationForm");
                        }
                    }
                }
                catch (Exception ex)
                {
                    return Content($"<h1 style = 'color:red'>Error : {ex.Message}</h1>");
                }
                TempData["Error"] = "Invalid user!";
                return View("Login");
            }
            
        }
        [HttpPost]
        public ActionResult Signup(Login_Signup model)
        {
            using (BankApplication_dbContext _context = new BankApplication_dbContext())
            {

                try
                {
                    bool isValidUser = _context.Users.Any(user =>
                        user.UserName.ToLower() == model.Signup.UserName.ToLower() ||
                        user.Email == model.Signup.Email ||
                        user.MobileNumber == model.Signup.MobileNumber ||
                        user.UniqueId == model.Signup.UniqueId);
                    if (!isValidUser)
                    {
                        string token = model.Signup.UniqueId.ToString().Split('0')[0];
                        if (token == "adminn")
                        {
                            TempData["Error"] = "pancard Number is Not Valid!";
                            return View("Login");
                        }
                        List<UserRolesMapping> role = new List<UserRolesMapping>()
                        {
                            new UserRolesMapping(){RoleID = 2}
                        };
                        model.Signup.UserRolesMappings = role;
                        _context.Users.Add(model.Signup);
                        _context.SaveChanges();
                        return RedirectToAction("Login");
                    }
                    TempData["Error"] = "User is allready exist!";
                    return View("Login");
                }
                catch (Exception ex)
                {
                    return Content($"<h1 style = 'color:red'>Error : {ex.Message}</h1>");
                }
            }
        }
        #endregion

        #region/**********< ADMIN >***********/
        public ActionResult AdminLogin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AdminLogin(Login_Signup model)
        {
            using (BankApplication_dbContext _context = new BankApplication_dbContext())
            {

                try
                {
                    User usr = _context.Users.FirstOrDefault(el => el.Email == model.Login.Email);
                    bool tokenvalid = false;
                    if(usr != null) { string token = usr.UniqueId.ToString().Split('0')[0];
                    
                    if ( token == "adminn") tokenvalid = true;
                    bool isValidUser = _context.Users.Any(user => user.Email == model.Login.Email && user.UserPassword == model.Login.Password);
                    if (isValidUser && tokenvalid)
                    {
                        FormsAuthentication.SetAuthCookie(usr.UserName, false);
                        //TempData["Success"] = $"Welcome{User.Identity.Name}";
                        //TempData.Keep("Success");
                        return RedirectToAction("Index","Home");
                    }
                    }
                }
                catch (Exception ex)
                {
                    return Content($"<h1 style = 'color:red'>Error : {ex.Message}</h1>");
                }
                TempData["Error"] = "Invalid User!";
                return View("AdminLogin");
            }

        }
        [HttpPost]
        public ActionResult AdminSignup(Login_Signup model)
        {
            using (BankApplication_dbContext _context = new BankApplication_dbContext())
            {

                try
                {
                    bool isValidUser = _context.Users.Any(user =>                     
                        user.UserName.ToLower() == model.Signup.UserName.ToLower() ||                    
                        user.Email == model.Signup.Email ||
                        user.MobileNumber == model.Signup.MobileNumber ||
                        user.UniqueId == model.Signup.UniqueId);
                    if (!isValidUser)
                    {
                        string token = model.Signup.UniqueId.ToString().Split('0')[0];
                        if (token != "adminn")
                        {
                            TempData["Error"] = "Admin Token is Not Valid";
                            return View("AdminLogin");
                        }
                        List<UserRolesMapping> role = new List<UserRolesMapping>()
                        {
                            new UserRolesMapping(){RoleID = 1}
                        };
                        model.Signup.UserRolesMappings = role;
                        _context.Users.Add(model.Signup);
                        _context.SaveChanges();
                        return RedirectToAction("AdminLogin");
                    }
                    TempData["Error"] = "User is allready exist!";
                    return View("AdminLogin");
                }
                catch (Exception ex)
                {
                    return Content($"<h1 style = 'color:red'>Error : {ex.Message}</h1>");
                }
            }
        }
        #endregion
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return Redirect("/");
        }
        
    }
}