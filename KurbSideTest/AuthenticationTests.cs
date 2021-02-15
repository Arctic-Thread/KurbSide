using System;
using KurbSideTest;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace KurbSideTest
{
    [TestFixture]
    class AuthenticationTests : BaseTest
    {
        private string randomUser = RandomString(5);

        //We can change the ordering when we decide on it later.

        //[Order(1)]
        [Test]
        public void Unit_Template_ShouldPass()
        {
            // Arrange
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));

            // Login Details
            var loginEmail = "test@kurbsi.de";
            var loginPassword = "Password12345";

            // Fields & Buttons

            // Titles

            // Expected Result
            var expectedResult = "";

            //Act
            var result = "";

            //Assert
            Assert.AreEqual(expectedResult, result);
        }


        //[Order(1)]
        [Test]
        public void Authentication_LoginWithValidInformation_ShouldPass()
        {
            // Arrange
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));

            // Login Details
            var loginEmail = "test@kurbsi.de";
            var loginPassword = "Password12345";

            // Fields & Buttons
            var navbarLoginButtonID = "navbar-login";
            var loginEmailFieldID = "Input_Email";
            var loginPasswordFieldID = "Input_Password";
            var loginButtonID = "login-button";
            var navbarGreetingID = "navbar-greeting";

            // Titles
            var homePageTitle = "Home Page - KurbSide";
            var loginPageTitle = "Log in - KurbSide";

            // Expected Result
            var expectedResult = "Hello test@kurbsi.de!";

            //Act
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.TitleContains(homePageTitle)); // Wait until home page is visible.
            _driver.FindElement(By.Id(navbarLoginButtonID)).Click(); // Click login buttin in navbar.
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.TitleContains(loginPageTitle)); // Wait until login page is visible
            _driver.FindElement(By.Id(loginEmailFieldID)).SendKeys(loginEmail); // Send email to email field.
            _driver.FindElement(By.Id(loginPasswordFieldID)).SendKeys(loginPassword); // Send password to password field.
            _driver.FindElement(By.Id(loginButtonID)).Click(); // Click the login button.
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.TitleContains(homePageTitle)); // Wait until home page is visible.
            var result = _driver.FindElement(By.Id(navbarGreetingID)).Text; // Should be "Hello test@kurbsi.de!"

            //Assert
            Assert.AreEqual(expectedResult, result);
        }
    }
}
