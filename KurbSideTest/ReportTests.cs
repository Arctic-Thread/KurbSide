using System.IO;
using NUnit.Framework;
using Syroot.Windows.IO;

namespace KurbSideTest
{
    [TestFixture]
    class ReportTests : BaseTest
    {
        /// <summary>
        /// UC21 - Business Views Report
        /// Tests downloading of generated reports.
        /// Note: For Best results ensure the download path is clear on you device
        /// </summary>
        [TestCase("reports-availableItems", "Available Items Report - KurbSide", "KurbSide-Available Items Report-*.pdf")]
        [TestCase("reports-allItems", "All Items Report - KurbSide", "KurbSide-All Items Report-*.pdf")]
        [TestCase("reports-RemovedItems", "Removed Items Report - KurbSide", "KurbSide-Removed Items Report-*.pdf")]
        [TestCase("reports-allOrders", "All Orders Report - KurbSide", "KurbSide-All Orders Report-*.pdf")]
        [TestCase("reports-completedOrders", "All Completed Orders Report - KurbSide", "KurbSide-All Completed Orders Report-*.pdf")]
        [TestCase("reports-pendingOrders", "All Pending Orders Report - KurbSide", "KurbSide-All Pending Orders Report-*.pdf")]
        [TestCase("reports-canceledOrders", "All Canceled Orders Report - KurbSide", "KurbSide-All Canceled Orders Report-*.pdf")]
        [Order(1)]
        public void UC21_Reports_DownloadReport_ShouldPass(string reportButtonId, string reportTitle, string fileName)
        {
            // Arrange
            // Fields & Buttons
            string dashboardReportsButtonID = "dashboard-reports";
            string downloadReportButtonID = "btnDownloadReport";

            // Titles
            string dashboard = "Business Dashboard - KurbSide";
            string reportsListing = "Business Report Listing - KurbSide";

            // Other
            string downLoadPath = new KnownFolder(KnownFolderType.Downloads).Path;

            // Act
            // Logs in to the business account.
            KSUnitTestLogin(AccountType.BUSINESS);

            // From the dashboard, navigates to report listings page.
            KSTitleContains(dashboard);
            KSClick(dashboardReportsButtonID);

            // Navigates to the specified report.
            
            KSTitleContains(reportsListing);
            KSClick(reportButtonId);

            // Downloads the specified report.
            KSTitleContains(reportTitle);
            KSClick(downloadReportButtonID);

            // Assert
            // Checks if the file was downloaded.
            if (KSCheckForFileInDirectory(downLoadPath, fileName))
            {
                //Deletes the file from the folder.
                KSDeleteFilesInDirectory(downLoadPath, fileName);
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsTrue(false);
            }
        }

        private static bool KSCheckForFileInDirectory(string directory, string filePattern)
        {
            var files = Directory.GetFiles(directory, filePattern, SearchOption.TopDirectoryOnly);
            return files.Length > 0;
        }

        private static void KSDeleteFilesInDirectory(string directory, string filePattern)
        {
            foreach (var file in Directory.EnumerateFiles(directory, filePattern))
            {
                File.Delete(file);
            }
        }
    }
}