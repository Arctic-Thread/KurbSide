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

        //We can change the ordering when we decide on it later.
        /// <summary>
        /// Logs you into the test user
        /// </summary>
        public void Login()
        {
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));

            // Login Details
            string loginEmail = "test@kurbsi.de";
            string loginPassword = "Password12345";

            // Fields & Buttons
            string navbarLoginButtonID = "navbar-login";
            string navbarlogoutButtonID = "navbar-logout";
            string loginEmailFieldID = "Input_Email";
            string loginPasswordFieldID = "Input_Password";
            string loginButtonID = "login-button";
            string sysMessageID = "sysMessage";

            // Titles
            string homePageTitle = "Home Page - KurbSide";
            string loginPageTitle = "Log in - KurbSide";

            // Expected Result
            string expectedResult = "× You have been logged out of your account.";

            //Act
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.TitleContains(homePageTitle)); // Wait until home page is visible.
            _driver.FindElement(By.Id(navbarLoginButtonID)).Click(); // Click login button in navbar.
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.TitleContains(loginPageTitle)); // Wait until login page is visible
            _driver.FindElement(By.Id(loginEmailFieldID)).SendKeys(loginEmail); // Send email to email field.
            _driver.FindElement(By.Id(loginPasswordFieldID)).SendKeys(loginPassword); // Send password to password field.
            _driver.FindElement(By.Id(loginButtonID)).Click(); // Click the login button.
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.TitleContains(homePageTitle)); // Wait until home page is visible.
        }

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
            IWebElement itemFound = null;
            
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));

            Login();

            _driver.FindElement(By.Id("navbar-catalogue")).Click();//Clicks the catalogue button in the nav bar

            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("catalogue-AddItem")));
            _driver.FindElement(By.Id("catalogue-AddItem")).Click();

            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ItemName")));
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
