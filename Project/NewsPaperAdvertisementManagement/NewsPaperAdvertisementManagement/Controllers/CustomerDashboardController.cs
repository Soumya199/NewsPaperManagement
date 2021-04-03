using NewsPaperAdvertisementManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Windows.Forms;

namespace NewsPaperAdvertisementManagement.Controllers
{
    public class CustomerDashboardController : Controller
    {
        public NewsPaperAgencyEntities newePaperAgencyContext = new NewsPaperAgencyEntities();
        public  List<Advertisement> AdvertisementList = new List<Advertisement>();
        public Advertisement advertisementDetails = new Advertisement();
        public Customer customerDetail = new Customer();

        // GET: CustomerDashboard
        public ActionResult NotFound()
        {
            ViewBag.message = "Dear" +" "+ Session["CustomerName"].ToString() +" "+ "You dont have any Request!";
            return View();
        }

        public ActionResult NoConfirmAds()
        {
            ViewBag.message= "Dear" + " " + Session["CustomerName"].ToString() + " " + "You dont have any Confirm Ads";
            return View();
        }
        [HttpGet]
        public ActionResult ViewConfirmAds()
        {
            string User = Session["CustomerEmail"].ToString();
            string status = "Approved";
            AdvertisementList = newePaperAgencyContext.Advertisements.Where(s => s.CustomerEmailId.Equals(User) && s.AdvertisementStatus.Equals(status)).ToList();
            if(AdvertisementList.Count.Equals(0))
            {
                return RedirectToAction("NoConfirmAds");
            }
            else
            {
                return View(AdvertisementList);
            }
        }

        public ActionResult ViewRequestStatus()
        {
            string User = Session["CustomerEmail"].ToString();
            AdvertisementList = newePaperAgencyContext.Advertisements.Where(s => s.CustomerEmailId.Equals(User)).ToList();
            if (AdvertisementList.Count.Equals(0))
            {
                
                return RedirectToAction("NotFound");
            }
            else
            {
                return View(AdvertisementList);
            }
            
        }
        [HttpGet]
        public ActionResult CancelRequestedAds(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            advertisementDetails = newePaperAgencyContext.Advertisements.ToList().Find(x => x.AdvertisementNo == id);
            if (MessageBox.Show("Are You Sure You Want To Cancel Request", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                newePaperAgencyContext.Advertisements.Remove(advertisementDetails);
                newePaperAgencyContext.SaveChanges();
                return RedirectToAction("ViewRequestStatus", "CustomerDashboard");
            }
            else
            {
                AdvertisementList = newePaperAgencyContext.Advertisements.ToList();
                return RedirectToAction("ViewRequestStatus", "CustomerDashboard");
            }
        }

        public ActionResult ViewProfile()
        {
            string user = Session["CustomerEmail"].ToString();
            string status = "Approved";
            int requestAds = newePaperAgencyContext.Advertisements.Where(check => check.CustomerEmailId.Equals(user)).Count();
            int approveAds= newePaperAgencyContext.Advertisements.Where(check => check.CustomerEmailId.Equals(user)&& check.AdvertisementStatus.Equals(status)).Count();
            ViewBag.requestad = requestAds;
            ViewBag.approved = approveAds;
            return View();
        }
        [HttpGet]
        public ActionResult EditProfile()
        {
            string user = Session["CustomerEmail"].ToString();
            if (user != null)
            {
                customerDetail = newePaperAgencyContext.Customers.Find(user);
                return View(customerDetail);
            }
            else
            {
                return RedirectToAction("ViewProfile");
            }

        }
        [HttpPost]
        public ActionResult EditProfile(Customer customer)
        {
            Customer TubeUpdated = newePaperAgencyContext.Customers.ToList().Find(data => data.CustomerEmailId.Equals(customer.CustomerEmailId));
            TubeUpdated.CustomerName = customer.CustomerName;
            TubeUpdated.CustomerAddress = customer.CustomerAddress;
            TubeUpdated.CustomerPhone = customer.CustomerPhone;
            TubeUpdated.CustomerPssword = TubeUpdated.CustomerPssword;
            newePaperAgencyContext.Configuration.ValidateOnSaveEnabled = false;
            newePaperAgencyContext.SaveChanges();
            MessageBox.Show("Edit Successful");
            return RedirectToAction("ViewProfile");

        }

    }
}