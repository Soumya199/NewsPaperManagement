using NewsPaperAdvertisementManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Windows.Forms;

namespace NewsPaperAdvertisementManagement.Controllers
{
    public class AdminController : Controller
    {
        NewsPaperAgencyEntities newePaperAgencyContext = new NewsPaperAgencyEntities();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpGet]
        public ActionResult AdminLogin()
        {

            return View();

        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult AdminLogin(Admin admindata)
        {
            if (admindata.AdminEmailId != null)
            {
                var IsValid = newePaperAgencyContext.Admins.Where(s => s.AdminEmailId.Equals(admindata.AdminEmailId) && admindata.AdminPassword.Equals("Admin@123")).ToList();
                if (IsValid.Count() > 0)
                {
                    FormsAuthentication.SetAuthCookie(admindata.AdminEmailId, false);
                    //add session
                    Session["AdminName"] = IsValid.FirstOrDefault().AdminName;
                    Session["AdminEmail"] = IsValid.FirstOrDefault().AdminEmailId;
                    return RedirectToAction("Dashboard", "AdminDashboard");
                }
                else
                {
                    MessageBox.Show("Email id or Password incorrect!");
                    return RedirectToAction("AdminLogin");
                }
            }
            return View();
        }

        public ActionResult AdminLogout()
        {
            Session.Abandon();
            FormsAuthentication.SignOut();
            MessageBox.Show("Successfully Logout!!!!");
            return RedirectToAction("AdminLogin");
        }

    }
}