using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace KurbSideTest
{
    [TestFixture]
    class GenericTests : BaseTest
    {
        /// <summary>
        /// UC01 - View Home Page
        /// Tests if the Home Page loads
        /// </summary>
        [Order(1)]
        [Test]
        public void UC01_Generic_ViewHomePage_ShouldPass()
        {
            // Arrange
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));

            //Fields & Buttons
            string navbarHomeID = "navbar-home";

            // Titles
            string homePageTitle = "Home Page - KurbSide";

            //Act
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.TitleContains(homePageTitle));
            _driver.FindElement(By.Id(navbarHomeID)).Click();
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.TitleContains(homePageTitle));

            var result = _driver.Title.Contains(homePageTitle);

            //Assert
            Assert.IsTrue(result);
        }

        /// <summary>
        /// UC02 - View Privacy Policy
        /// Tests if the Privacy Policy Page loads
        /// </summary>
        [Order(2)]
        [Test]
        public void UC02_Generic_ViewPrivacyPlicy_ShouldPass()
        {
            // Arrange
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));

            // Fields & Buttons
            string footerPrivacyID = "footer-privacy";

            // Titles
            string homePageTitle = "Home Page - KurbSide";
            string privacyPolicyPageTitle = "Privacy Policy - KurbSide";

            //Act
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.TitleContains(homePageTitle));
            _driver.FindElement(By.Id(footerPrivacyID)).Click();
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.TitleContains(privacyPolicyPageTitle));

            var result = _driver.Title.Contains(privacyPolicyPageTitle);

            //Assert
            Assert.IsTrue(result);
        }
    }
}
