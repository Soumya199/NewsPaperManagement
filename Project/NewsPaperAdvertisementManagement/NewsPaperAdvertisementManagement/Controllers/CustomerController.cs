using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using NewsPaperAdvertisementManagement.Models;

namespace NewsPaperAdvertisementManagement.Controllers
{     
    public class CustomerController : Controller
    {
        public NewsPaperAgencyEntities newePaperAgencyContext = new NewsPaperAgencyEntities();

        public Customer admin = new Customer();
        [HttpGet]
        [AllowAnonymous]
        public ActionResult CustomerSignup()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult CustomerSignup(Customer customersignup)
        {
            if (ModelState.IsValid)
            {
                 var findCustomer = newePaperAgencyContext.Customers.FirstOrDefault(s => s.CustomerEmailId == customersignup.CustomerEmailId);
                if (findCustomer == null)
                {
                    customersignup.CustomerPssword = Encryption.GetMD5(customersignup.CustomerPssword);
                    newePaperAgencyContext.Configuration.ValidateOnSaveEnabled = false;
                    newePaperAgencyContext.Customers.Add(customersignup);
                    newePaperAgencyContext.SaveChanges();
                    FormsAuthentication.SetAuthCookie(customersignup.CustomerEmailId, false);
                    Session["CustomerEmail"] = customersignup.CustomerEmailId;
                    Session["CustomerName"] = customersignup.CustomerName;
                    MessageBox.Show("Registration Successful");
                    return RedirectToAction("CustomerLogin");
                }
                else
                {
                    ViewBag.error = "Email already exists";
                    return View();
                }


            }
            return View();
        }


        [HttpGet]
        [AllowAnonymous]
        public ActionResult CustomerLogin()
        {
            
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult CustomerLogin(Customer customer)
        {
            if (customer.CustomerEmailId!=null)
            {
                var decryptPassword =Encryption.GetMD5(customer.CustomerPssword);
                var IsValid = newePaperAgencyContext.Customers.Where(s => s.CustomerEmailId.Equals(customer.CustomerEmailId) && s.CustomerPssword.Equals(decryptPassword)).ToList();
                if (IsValid.Count() > 0)
                {
                    FormsAuthentication.SetAuthCookie(customer.CustomerEmailId,false);
                    //add session
                    Session["CustomerName"] = IsValid.FirstOrDefault().CustomerName;
                    Session["CustomerEmail"] = IsValid.FirstOrDefault().CustomerEmailId;
                    return RedirectToAction("ViewRequestStatus", "CustomerDashboard");
                }
                else
                {
                    MessageBox.Show("Email id or Password incorrect!");
                    return RedirectToAction("CustomerLogin");
                }
            }
            return View();
        }
        [AllowAnonymous]
        [HttpGet]
        public ActionResult EditPassword()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult EditPassword(Customer customer)
        {
            Customer ResetPassword = newePaperAgencyContext.Customers.ToList().Find(x => x.CustomerEmailId.Equals(customer.CustomerEmailId) && x.CustomerPhone.Equals(customer.CustomerPhone));
            if(ResetPassword!=null)
            {
                customer.CustomerPssword = Encryption.GetMD5(customer.CustomerPssword);
                newePaperAgencyContext.Configuration.ValidateOnSaveEnabled = false;
                ResetPassword.CustomerPssword = customer.CustomerPssword;
                newePaperAgencyContext.SaveChanges();
                MessageBox.Show("Password updated");
                return RedirectToAction("CustomerLogin");
            }
            else
            {
                MessageBox.Show("Invalid Credential!");
                return View();
            }
        }


        public ActionResult CustomerLogout()
        {
            Session.Abandon();
            FormsAuthentication.SignOut();
            MessageBox.Show("Successfully Logout!!!!");
            return RedirectToAction("CustomerLogin","Customer");
        }
    }
}