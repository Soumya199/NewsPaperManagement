using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using NewsPaperAdvertisementManagement.Models;

namespace NewsPaperAdvertisementUnitTest
{
    
    [TestFixture]
    public class UnitTest1
    {
        public NewsPaperAgencyEntities Context = new NewsPaperAgencyEntities();
        [Test]
        public void RequestAds()
        {
            Advertisement advertisement = new Advertisement();
            advertisement.AdvertisementName = "Shop Opening";
            advertisement.AdvertisementDate = DateTime.Today;
            advertisement.AdvertisementDescription = "welcome";
            advertisement.AdvertisementDimension = "4 x 5";
            advertisement.AdvertisementImage = "Dummypath/image.jpg";
            advertisement.AdvertisementPrice = 150;
            advertisement.AdvertisementStatus = "Pending";
            advertisement.CustomerEmailId = "Soumya@gmail.com";
            bool isAdded = false;
            string errorMessage = null;
            Context.Advertisements.Add(advertisement);
            try
            {
                Advertisement adsDetail = Context.Advertisements.Find(advertisement.AdvertisementName);
                if (adsDetail.Equals(1))
                {
                    isAdded = true;
                }

            }
            catch (Exception ex)
            {
                isAdded = false;
                errorMessage = ex.Message;
            }
            finally
            {

                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(isAdded, errorMessage);
            }

        }
    }
}
