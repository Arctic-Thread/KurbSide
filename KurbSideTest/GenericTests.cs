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
            string registerPageTitle = "Register - KurbSide";

            //Act
            KSTitleContains(registerPageTitle);
            KSClick(navbarHomeID);
            KSTitleContains(registerPageTitle);

            var result = _driver.Title.Contains(registerPageTitle);

            //Assert
            Assert.IsTrue(result);
        }

        /// <summary>
        /// UC02 - View Privacy Policy
        /// Tests if the Privacy Policy Page loads
        /// </summary>
        [Order(2)]
        [Test]
        public void UC02_Generic_ViewPrivacyPolicy_ShouldPass()
        {
            // Arrange
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));

            // Fields & Buttons
            string footerPrivacyID = "footer-privacy";

            // Titles
            string registerPageTitle = "Register - KurbSide";
            string privacyPolicyPageTitle = "Privacy Policy - KurbSide";

            //Act
            KSTitleContains(registerPageTitle);
            KSClick(footerPrivacyID);
            KSTitleContains(privacyPolicyPageTitle);

            var result = _driver.Title.Contains(privacyPolicyPageTitle);

            //Assert
            Assert.IsTrue(result);
        }
        
        /// <summary>
        /// UC22
        /// Member views account settings
        /// </summary>
        [Order(3)]
        [Test]
        public void UC22_BusinessViewsAccountSettings_ShouldPass()
        {
            string accountTitle = "My Account - KurbSide";
            
            KSUnitTestLogin(AccountType.BUSINESS);
            
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));
            KSClick("navbar-account-settings");

            var result = _driver.Title.Contains(accountTitle);
            
            Assert.IsTrue(result);
        }
        
        /// <summary>
        /// UC22
        /// Member views account settings
        /// </summary>
        [Order(3)]
        [Test]
        public void UC22_MemberViewsAccountSettings_ShouldPass()
        {
            string accountTitle = "My Account - KurbSide";
            
            KSUnitTestLogin(AccountType.MEMBER);
            
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));
            KSClick("navbar-account-settings");

            var result = _driver.Title.Contains(accountTitle);
            
            Assert.IsTrue(result);
        }
    }
}
