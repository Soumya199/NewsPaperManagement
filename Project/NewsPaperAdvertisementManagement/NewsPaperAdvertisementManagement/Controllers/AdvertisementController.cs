using NewsPaperAdvertisementManagement.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Windows.Forms;

namespace NewsPaperAdvertisementManagement.Controllers
{
    public class AdvertisementController : Controller
    {
        public NewsPaperAgencyEntities newePaperAgencyContext = new NewsPaperAgencyEntities();

        public List<Advertisement> AdvertisementList = new List<Advertisement>();
        public Advertisement advertisementDetails = new Advertisement();
        // GET: Advertisement
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult RequestForAds()
        {
            return View();
        }
        [HttpPost]
        public ActionResult RequestForAds(Advertisement advertisementDetails)
        {

            advertisementDetails.CustomerEmailId = Session["CustomerEmail"].ToString();
            advertisementDetails.AdvertisementStatus = "Pending";
            string fileName = Path.GetFileNameWithoutExtension(advertisementDetails.imageFile.FileName);
            string extension = Path.GetExtension(advertisementDetails.imageFile.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            advertisementDetails.AdvertisementImage = "~/Image/" + fileName;
            fileName = Path.Combine(Server.MapPath("~/Image/"), fileName);
            advertisementDetails.imageFile.SaveAs(fileName);

            int checkdate = newePaperAgencyContext.Advertisements.ToList().FindAll(check => check.AdvertisementDate == advertisementDetails.AdvertisementDate).Count();
            if (advertisementDetails.AdvertisementDimension == "4 x 5")
            {
                advertisementDetails.AdvertisementPrice = 50;
            }
            else if(advertisementDetails.AdvertisementDimension == "8 x 4")
            {
                advertisementDetails.AdvertisementPrice = 100;
            }
            else
            {
                advertisementDetails.AdvertisementPrice = 150;
            }
            if (checkdate <= 25)
            {
                if (MessageBox.Show("Your cost=" + advertisementDetails.AdvertisementPrice + " " + "Are You Sure You Want Proceed", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    newePaperAgencyContext.Advertisements.Add(advertisementDetails);
                    newePaperAgencyContext.SaveChanges();
                    MessageBox.Show($"Dear {advertisementDetails.CustomerEmailId} \n Your Advertisement No is {advertisementDetails.AdvertisementNo} ");
                    return RedirectToAction("ViewRequestStatus", "CustomerDashboard");
                }
                else
                {
                    MessageBox.Show("Payment Cancel!!!");
                    return View();
                }
            }
            else
            {
                    MessageBox.Show($"Dear {advertisementDetails.CustomerEmailId} \n Please Select another Date");
                    return View();
            }
           
            

        }
        [HttpGet]
        public ActionResult ViewRequestedAds()
        {
            AdvertisementList = newePaperAgencyContext.Advertisements.ToList();
            if (AdvertisementList == null)
            {
                return HttpNotFound();
            }
            return View(AdvertisementList);

        }
        // GET: tblProducts/Delete/5
        [HttpGet]
        public ActionResult DeleteRequestedAds(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            advertisementDetails = newePaperAgencyContext.Advertisements.ToList().Find(x => x.AdvertisementNo == id);
            if (MessageBox.Show("Are You Sure You Want To delete", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                newePaperAgencyContext.Advertisements.Remove(advertisementDetails);
                newePaperAgencyContext.SaveChanges();
                return RedirectToAction("ViewRequestedAds", "Advertisement");
            }
            else
            {
                AdvertisementList = newePaperAgencyContext.Advertisements.ToList();
                return RedirectToAction("ViewRequestedAds", "Advertisement");
            }

        }
        // GET: tblProducts/Confirm/5
        [HttpGet]
        public ActionResult ConfirmRequestedAds(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            string status = "Approved";
            advertisementDetails = newePaperAgencyContext.Advertisements.ToList().Find(x => x.AdvertisementNo == id);
            if (advertisementDetails == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            {
                if(advertisementDetails.AdvertisementStatus.Equals(status))
                {
                    MessageBox.Show("Already Approved");
                }
                if(advertisementDetails.AdvertisementStatus.Equals("Pending"))
                {
                    advertisementDetails.AdvertisementStatus = status;
                    newePaperAgencyContext.SaveChanges();
                    MessageBox.Show("Request Approved!");
                    return RedirectToAction("ViewRequestedAds", "Advertisement");
                }
            }
            return RedirectToAction("ViewRequestedAds", "Advertisement");
        }
        [HttpGet]
        public ActionResult EditRequestedAds(int? id)
        {
            if (id != null)
            {
                var editAds = newePaperAgencyContext.Advertisements.Find(id);
                return View(editAds);
            }
            else
            {
                return RedirectToAction("ViewRequestedAds");
            }
        }
        [HttpPost]
        public ActionResult EditRequestedAds(Advertisement advertisement)
        {
            Advertisement TubeUpdated = newePaperAgencyContext.Advertisements.ToList().Find(data => data.AdvertisementNo == advertisement.AdvertisementNo);
            TubeUpdated.AdvertisementName = advertisement.AdvertisementName;
            TubeUpdated.AdvertisementDate = advertisement.AdvertisementDate;
            TubeUpdated.AdvertisementDescription = advertisement.AdvertisementDescription;
            TubeUpdated.AdvertisementDimension = advertisement.AdvertisementDimension;
            TubeUpdated.AdvertisementPrice = advertisement.AdvertisementPrice;
            TubeUpdated.AdvertisementStatus = advertisement.AdvertisementStatus;
            newePaperAgencyContext.SaveChanges();
            MessageBox.Show("Edit Successful");
            return RedirectToAction("ViewRequestedAds");
        }


    }
}