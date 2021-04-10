using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using OpenQA.Selenium;

namespace KurbSideTest
{
    [TestFixture]
    class ReportTests : BaseTest
    {
        /// <summary>
        /// UC21- Business views Reports
        /// Tests error message generation when adding an item with invalid details.
        /// Note: To run this test you may have to modify the download path
        /// Note: For Best results ensure the download path is clear on you device
        /// </summary>
        [Test]
        [Order(1)]
        public void UC21_Reports_DownLoadReport_AllItems_ShouldPass()
        {
            //strings needed for the test
            string dashboard = "Business Dashboard - KurbSide";
            string reportsListing = "Business Report Listing - KurbSide";
            string allItemsReport = "All Items Report - KurbSide";
            string downLoadPath = @"C:\Users\User\Downloads\All Items Report.pdf";//You may need to modify based on the file location
            
            //logins to the business account
            KSUnitTestLogin(AccountType.BUSINESS);
            
            //From the dashboard navigates to report listings
            KSTitleContains(dashboard);
            System.Threading.Thread.Sleep(200);
            KSClick("dashboard-reports");
            
            //navigates to all items reports
            KSTitleContains(reportsListing);
            System.Threading.Thread.Sleep(200);
            KSClick("reports-allItems");
            
            //Waits till intended page displays
            KSTitleContains(allItemsReport);
            KSClick("btnDownloadGameListReport");
            
            //Checks if the file was downloaded

            System.Threading.Thread.Sleep(10000);
            
            Assert.IsTrue(File.Exists(downLoadPath));//checks to make sure that there are items in the list
        }
        
        /// <summary>
        /// UC21- Business views Reports
        /// Tests error message generation when adding an item with invalid details.
        /// Note: To run this test you may have to modify the download path
        /// Note: For Best results ensure the download path is clear on you device
        /// </summary>
        [Test]
        [Order(2)]
        public void UC21_Reports_DownLoadReport_AvailableItems_ShouldPass()
        {
            //strings needed for the test
            string dashboard = "Business Dashboard - KurbSide";
            string reportsListing = "Business Report Listing - KurbSide";
            string allItemsReport = "Available Items Report - KurbSide";
            string downLoadPath = @"C:\Users\User\Downloads\Available Items Report.pdf";//You may need to modify based on the file location
            
            //logins to the business account
            KSUnitTestLogin(AccountType.BUSINESS);
            
            //From the dashboard navigates to report listings
            KSTitleContains(dashboard);
            System.Threading.Thread.Sleep(200);
            KSClick("dashboard-reports");
            
            //navigates to all items reports
            KSTitleContains(reportsListing);
            System.Threading.Thread.Sleep(200);
            KSClick("reports-availableItems");
            
            //Waits till intended page displays
            KSTitleContains(allItemsReport);
            KSClick("btnDownloadGameListReport");
            
            //Checks if the file was downloaded

            System.Threading.Thread.Sleep(10000);
            
            Assert.IsTrue(File.Exists(downLoadPath));//checks to make sure that there are items in the list
        }
        
        
        
        
    }    
}
