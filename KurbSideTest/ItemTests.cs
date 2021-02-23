using System;
using System.Collections.Generic;
using KurbSideTest;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace KurbSideTest
{
    [TestFixture]
    class ItemTests : BaseTest
    {
        private string randomString = RandomString(5) + "_";

        /// <summary>
        /// Test for adding a item to the business catalogue
        /// </summary>
        //[Order(1)]
        [Test]
        public void UC15_AddItem_ShouldPass()
        {
            string ItemNameTest = "Test Item pls delete";
            string DetailTest = "This item is to be deleted";
            string PriceTest = "75";
            string SkuTest = "TestSKU";
            string UpcTest = "012345678905";
            string CategoryTest = "This is a test category";
            
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));

            KSUnitTestLogin(AccountType.BUSINESS);

            _driver.FindElement(By.Id("navbar-catalogue")).Click();//Clicks the catalogue button in the nav bar

            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("catalogue-AddItem")));
            _driver.FindElement(By.Id("catalogue-AddItem")).Click();

            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("ItemName")));
            _driver.FindElement(By.Id("ItemName")).SendKeys(ItemNameTest);
            _driver.FindElement(By.Id("Price")).SendKeys(PriceTest);
            _driver.FindElement(By.Id("Details")).SendKeys(DetailTest);
            _driver.FindElement(By.Id("Category")).SendKeys(CategoryTest);
            _driver.FindElement(By.Id("Sku")).SendKeys(SkuTest);
            _driver.FindElement(By.Id("Upc")).SendKeys(UpcTest);

            _driver.FindElement(By.Id("catalogue-SubmitItem")).Click();

            IReadOnlyList<IWebElement> itemNames = _driver.FindElements(By.Id("catalogue-allItems"));//gets all the items and store them in a list

            Assert.IsTrue(itemNames.Count > 0);//checks to make sure that there are items in the list
        }
    }    
}
