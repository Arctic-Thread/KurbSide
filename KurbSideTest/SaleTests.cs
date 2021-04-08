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
            string saleListTitle = "Sale List - KurbSide";
            string createSaleTitle = "Create Sale - KurbSide";
            
            // Expected Result
            int numberOfExpectedErrors = 3;
            
            // Act
            KSUnitTestLogin(AccountType.BUSINESS);
            
            KSTitleContains(businessDashboardTitle);
            KSClick(manageSalesButtonID);
            
            KSTitleContains(saleListTitle);
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
            string saleListTitle = "Sale List - KurbSide";
            string createSaleTitle = "Create Sale - KurbSide";

            string expectedTitle = $"Add Items To {saleName} - KurbSide";
            
            // Act
            KSUnitTestLogin(AccountType.BUSINESS);
            
            KSTitleContains(businessDashboardTitle);
            KSClick(manageSalesButtonID);
            
            KSTitleContains(saleListTitle);
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
    }
}