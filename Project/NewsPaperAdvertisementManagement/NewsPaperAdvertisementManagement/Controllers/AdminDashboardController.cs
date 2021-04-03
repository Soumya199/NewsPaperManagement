using NewsPaperAdvertisementManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Windows.Forms;

namespace NewsPaperAdvertisementManagement.Controllers
{
    public class AdminDashboardController : Controller
    {
       public List<Customer> customerList = new List<Customer>();
        public Customer customerDetail= new Customer();
       public  NewsPaperAgencyEntities newePaperAgencyContext = new NewsPaperAgencyEntities();
        public List<Advertisement> advertisementList = new List<Advertisement>();
        public Advertisement advertisement = new Advertisement();
        // GET: AdminDashboard
        public ActionResult Dashboard()
        {
            int adsCount = newePaperAgencyContext.Advertisements.ToList().Count();
            int confirmAds= newePaperAgencyContext.Advertisements.Where(d=>d.AdvertisementStatus.Equals("Approved")).Count();
            int totalregistration = newePaperAgencyContext.Customers.ToList().Count();
            var TotlRevenue = newePaperAgencyContext.Advertisements.Select(x => x.AdvertisementPrice);
            decimal revenue = 0;
            if(TotlRevenue !=null)
            {
                foreach(decimal item in TotlRevenue)
                {
                    revenue = item+revenue;
                }

            }
            ViewBag.revenue = revenue;
            ViewBag.adsCount = adsCount;
            ViewBag.adsConfirm = confirmAds;
            ViewBag.totalUser = totalregistration;

            return View();
        }
        [HttpGet]
        public ActionResult ViewCustomer(string searchString, string search)
        {
            if (searchString == "Name")
            {
                customerList = newePaperAgencyContext.Customers.Where(p => p.CustomerName.Contains(search)).ToList();
                return View(customerList);
            }
            else if(searchString == "Email")
            {
                customerList = newePaperAgencyContext.Customers.Where(p => p.CustomerEmailId.Contains(search)).ToList();
                return View(customerList);
            }
            else
            {
                return View(newePaperAgencyContext.Customers.ToList());
            }
            
        }
        [HttpGet]
        public async Task <ActionResult> EditCustomer(string id)
        {
            if (id != null)
            {
                customerDetail = await newePaperAgencyContext.Customers.FindAsync(id);
                return View(customerDetail);
            }
            else
            {
                return RedirectToAction("ViewCustomer");
            }
        }
        [HttpPost]
        public ActionResult EditCustomer(Customer customer)
        {
              customerDetail = newePaperAgencyContext.Customers.ToList().Find(data => data.CustomerEmailId.Equals(customer.CustomerEmailId));
                customerDetail.CustomerName = customer.CustomerName;
                customerDetail.CustomerAddress = customer.CustomerAddress;
                customerDetail.CustomerPhone = customer.CustomerPhone;
                customerDetail.CustomerEmailId = customerDetail.CustomerEmailId;
                customerDetail.CustomerPssword = customerDetail.CustomerPssword;
                newePaperAgencyContext.Configuration.ValidateOnSaveEnabled = false;
                newePaperAgencyContext.SaveChanges();
                MessageBox.Show("Edit Successful");
                return RedirectToAction("ViewCustomer");
            
              
        }


    }

}