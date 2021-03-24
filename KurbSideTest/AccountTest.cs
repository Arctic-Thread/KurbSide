using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace KurbSideTest
{
    [TestFixture]
    class AccountTest : BaseTest
    {
        /// <summary>
        /// UC22
        /// Member views account settings
        /// </summary>
        [Order(1)]
        [Test]
        public void UC22_BusinessViewsAccountSettings_ShouldPass()
        {
            //var needed for the test
            string accountTitle = "My Account - KurbSide";
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));
            
            //getting to the page we need
            KSUnitTestLogin(AccountType.BUSINESS);
            KSClick("navbar-account-settings");
            
            //checking to see if we are on the right page
            var result = _driver.Title.Contains(accountTitle);
            
            //Assert
            Assert.IsTrue(result);
        }
        
        /// <summary>
        /// UC22
        /// Member views account settings
        /// </summary>
        [Order(2)]
        [Test]
        public void UC22_MemberViewsAccountSettings_ShouldPass()
        {
            //var needed for the test
            string accountTitle = "My Account - KurbSide";
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));
            
            //getting to the page we need
            KSUnitTestLogin(AccountType.MEMBER);
            KSClick("navbar-account-settings");
            
            //checking to see if we are on the right page
            var result = _driver.Title.Contains(accountTitle);
            
            //Assert
            Assert.IsTrue(result);
        }
        
        /// <summary>
        /// UC23 Email preferences Tests
        /// </summary>
        [Order(3)]
        [Test]
        public void UC23_MemberChangesEmailPreferences_ShouldPass()
        {
            //var for the test
            bool isChecked = false;
            
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));
            
            //logging and getting to page we want
            KSUnitTestLogin(AccountType.MEMBER);
            KSClick("navbar-account-settings");
            KSClick("preferences");
            IWebElement emailCheckBox = _driver.FindElement(By.XPath("//input[contains(@id,'Input_PromotionalEmails')]"));
            
            //checking to make sure promotional emails is checked
            if(!emailCheckBox.Selected)
            {
                KSClick("Input_PromotionalEmails");//if unchecked check it
            }
            if (emailCheckBox.Selected)
            {
                isChecked = true;
            }
            //Assert
            Assert.IsTrue(isChecked);
        }
    }
}
