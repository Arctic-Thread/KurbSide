using System.Collections.Generic;
using NUnit.Framework;
using OpenQA.Selenium;

namespace KurbSideTest
{
    [TestFixture]
    public class SaleTests : BaseTest
    {
        /// <summary>
        /// UC18 - Business Create Sale
        /// Tests error message generation when creating a sale with invalid details.
        /// </summary>
        [Test]
        [Order(1)]
        public void UC18_Sales_CreateSale_InvalidDetails_ShouldFail()
        {
            // Arrange
            // Fields & Buttons
            string manageSalesButtonID = "dashboard-sales";
            string newSaleButtonID = "saleList-CreateSale";
            string createSaleButtonID = "sales-createSale";

            // Titles
            string businessDashboardTitle = "Business Dashboard - KurbSide";
            string activeSalesListTitle = "Active Sales List - KurbSide";
            string createSaleTitle = "Create Sale - KurbSide";

            // Expected Result
            int numberOfExpectedErrors = 3;

            // Act
            KSUnitTestLogin(AccountType.BUSINESS);

            KSTitleContains(businessDashboardTitle);
            KSClick(manageSalesButtonID);

            KSTitleContains(activeSalesListTitle);
            KSClick(newSaleButtonID);

            KSTitleContains(createSaleTitle);
            KSClick(createSaleButtonID);

            // Result
            IReadOnlyList<IWebElement> numberOfErrors = _driver.FindElements(By.CssSelector("[id$='-error']"));

            // Assert
            Assert.AreEqual(numberOfExpectedErrors, numberOfErrors.Count);
        }

        /// <summary>
        /// UC18 - Business Create Sale
        /// Tests creating a sale with valid details.
        /// </summary>
        [Test]
        [Order(2)]
        public void UC18_Sales_CreateSale_ValidDetails_ShouldPass()
        {
            // Arrange
            // Input
            string saleName = "This Is A Test Sale";
            string saleCategory = "Test Category";
            string saleDescription = "This is a Sale Description";
            string saleDiscountAmount = "50";

            // Fields & Buttons
            string manageSalesButtonID = "dashboard-sales";
            string newSaleButtonID = "saleList-CreateSale";
            string createSaleButtonID = "sales-createSale";

            string saleNameFieldID = "SaleName";
            string saleCategoryFieldID = "SaleCategory";
            string saleDescriptionFieldID = "SaleDescription";
            string saleDiscountFieldID = "SaleDiscountPercentage";

            // Titles
            string businessDashboardTitle = "Business Dashboard - KurbSide";
            string activeSalesListTitle = "Active Sales List - KurbSide";

            string createSaleTitle = "Create Sale - KurbSide";

            string expectedTitle = $"Add Items To {saleName} - KurbSide";

            // Act
            KSUnitTestLogin(AccountType.BUSINESS);

            KSTitleContains(businessDashboardTitle);
            KSClick(manageSalesButtonID);

            KSTitleContains(activeSalesListTitle);
            KSClick(newSaleButtonID);

            KSTitleContains(createSaleTitle);
            KSSendKeys(saleNameFieldID, saleName);
            KSSendKeys(saleCategoryFieldID, saleCategory);
            KSSendKeys(saleDescriptionFieldID, saleDescription);
            KSReplaceText(saleDiscountFieldID, saleDiscountAmount);
            KSClick(createSaleButtonID);

            var actualTitle = _driver.Title;

            // Assert
            Assert.AreEqual(expectedTitle, actualTitle);
        }

        /// <summary>
        /// UC19 - Business Edits Sale
        /// Tests error message generation when editing a sale with invalid details.
        /// </summary>
        [Test]
        [Order(3)]
        public void UC19_Sales_EditSale_InvalidDetails_ShouldFail()
        {
            // Arrange
            // Fields & Buttons
            string manageSalesButtonID = "dashboard-sales";
            string saveSaleButtonID = "sales-saveSale";
            string swapViewingModeButtonID = "swapViewingMode";
            string editSaleButtonID = "editSale-This Is A Test Sale";

            string saleNameFieldID = "SaleName";
            string saleCategoryFieldID = "SaleCategory";
            string saleDescriptionFieldID = "SaleDescription";
            string saleDiscountFieldID = "SaleDiscountPercentage";

            // Titles
            string businessDashboardTitle = "Business Dashboard - KurbSide";
            string activeSalesListTitle = "Active Sales List - KurbSide";
            string inactiveSalesListTitle = "Inactive Sales List - KurbSide";
            string editSalePageTitle = "Edit Sale: This Is A Test Sale - KurbSide";

            // Expected Result
            int numberOfExpectedErrors = 3;

            // Act
            KSUnitTestLogin(AccountType.BUSINESS);

            KSTitleContains(businessDashboardTitle);
            KSClick(manageSalesButtonID);

            KSTitleContains(activeSalesListTitle);
            KSClick(swapViewingModeButtonID);

            KSTitleContains(inactiveSalesListTitle);
            KSClick(editSaleButtonID);

            KSTitleContains(editSalePageTitle);
            KSClearInput(saleNameFieldID);
            KSClearInput(saleCategoryFieldID);
            KSClearInput(saleDescriptionFieldID);
            KSClearInput(saleDiscountFieldID);

            KSClick(saveSaleButtonID);

            // Result
            IReadOnlyList<IWebElement> numberOfErrors = _driver.FindElements(By.CssSelector("[id$='-error']"));

            // Assert
            Assert.AreEqual(numberOfExpectedErrors, numberOfErrors.Count);
        }

        /// <summary>
        /// UC19 - Business Edits Sale
        /// Tests editing a sale with valid details.
        /// </summary>
        [Test]
        [Order(4)]
        public void UC19_Sales_EditSale_ValidDetails_ShouldPass()
        {
            // Arrange
            // Input
            string saleName = "This Is An Edited Test Sale";
            string saleCategory = "Edited Test Category";
            string saleDescription = "This Is An Edited Sale Description";
            string saleDiscountAmount = "05";

            // Fields & Buttons
            string manageSalesButtonID = "dashboard-sales";
            string saveSaleButtonID = "sales-saveSale";
            string swapViewingModeButtonID = "swapViewingMode";
            string editSaleButtonID = "editSale-This Is A Test Sale";

            string saleNameFieldID = "SaleName";
            string saleCategoryFieldID = "SaleCategory";
            string saleDescriptionFieldID = "SaleDescription";
            string saleDiscountFieldID = "SaleDiscountPercentage";
            string saleCurrentlyActiveLabelID = "activeLabel";

            string editedSaleID = "saleList-sale-This Is An Edited Test Sale";

            // Titles
            string businessDashboardTitle = "Business Dashboard - KurbSide";
            string activeSalesListTitle = "Active Sales List - KurbSide";
            string inactiveSalesListTitle = "Inactive Sales List - KurbSide";
            string editSalePageTitle = "Edit Sale: This Is A Test Sale - KurbSide";

            // Act
            KSUnitTestLogin(AccountType.BUSINESS);

            KSTitleContains(businessDashboardTitle);
            KSClick(manageSalesButtonID);

            KSTitleContains(activeSalesListTitle);
            KSClick(swapViewingModeButtonID);

            KSTitleContains(inactiveSalesListTitle);
            KSClick(editSaleButtonID);

            KSTitleContains(editSalePageTitle);
            KSReplaceText(saleNameFieldID, saleName);
            KSReplaceText(saleCategoryFieldID, saleCategory);
            KSReplaceText(saleDescriptionFieldID, saleDescription);
            KSReplaceText(saleDiscountFieldID, saleDiscountAmount);
            KSClick(saleCurrentlyActiveLabelID);
            KSClick(saveSaleButtonID);

            KSTitleContains(activeSalesListTitle);

            var result = _driver.FindElement(By.Id(editedSaleID));

            // Assert
            Assert.IsTrue(result != null);
        }

        /// <summary>
        /// UC20 - Business Ends Sale
        /// Tests ending a sale.
        /// Note: Does not delete the sale, can still be re-activated later.
        /// </summary>
        [Test]
        [Order(5)]
        public void UC20_Sales_EndSale_ShouldPass()
        {
            // Arrange
            // Fields & Buttons
            string manageSalesButtonID = "dashboard-sales";
            string swapViewingModeButtonID = "swapViewingMode";
            string saleSwapStatusButtonID = "saleSwapStatus-This Is An Edited Test Sale";
            string sysMessageID = "sysMessage";

            // Titles
            string businessDashboardTitle = "Business Dashboard - KurbSide";
            string activeSalesListTitle = "Active Sales List - KurbSide";

            // Act
            KSUnitTestLogin(AccountType.BUSINESS);

            KSTitleContains(businessDashboardTitle);
            KSClick(manageSalesButtonID);

            KSTitleContains(activeSalesListTitle);
            KSClick(saleSwapStatusButtonID);
            var confirmEnd = _driver.SwitchTo().Alert();
            confirmEnd.Accept();

            KSTitleContains(activeSalesListTitle);
            KSWaitUntilElementIsVisible(sysMessageID);
            KSClick(swapViewingModeButtonID);
            KSClick(saleSwapStatusButtonID);
            var confirmActivate = _driver.SwitchTo().Alert();
            confirmActivate.Accept();

            KSTitleContains(activeSalesListTitle);
            KSWaitUntilElementIsVisible(saleSwapStatusButtonID);

            var result = _driver.FindElement(By.Id(saleSwapStatusButtonID));

            // Assert
            Assert.That(result != null);
        }

        /// <summary>
        /// UC20 - Business Ends Sale
        /// Tests deleting a sale.
        /// </summary>
        [Test]
        [Order(6)]
        public void UC20_Sales_DeleteSale_ShouldPass()
        {
            // Arrange
            // Fields & Buttons
            string manageSalesButtonID = "dashboard-sales";
            string deleteSaleButtonID = "sales-deleteSale";
            string editSaleButtonID = "editSale-This Is An Edited Test Sale";

            string sysMessageID = "sysMessage";

            // Titles
            string businessDashboardTitle = "Business Dashboard - KurbSide";
            string activeSalesListTitle = "Active Sales List - KurbSide";
            string editSalePageTitle = "Edit Sale: This Is An Edited Test Sale - KurbSide";

            // Expected Result
            string expectedMessage = "The sale: This Is An Edited Test Sale has been deleted";

            // Act
            KSUnitTestLogin(AccountType.BUSINESS);

            KSTitleContains(businessDashboardTitle);
            KSClick(manageSalesButtonID);

            KSTitleContains(activeSalesListTitle);
            KSClick(editSaleButtonID);

            KSTitleContains(editSalePageTitle);
            KSClick(deleteSaleButtonID);
            var confirmDelete = _driver.SwitchTo().Alert();
            confirmDelete.Accept();

            KSTitleContains(activeSalesListTitle);
            string actualMessage = _driver.FindElement(By.Id(sysMessageID)).Text;

            // Assert
            Assert.That(actualMessage.Contains(expectedMessage));
        }
    }
}