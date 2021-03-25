using NUnit.Framework;
using OpenQA.Selenium;

namespace KurbSideTest
{
    [TestFixture]
    class AccountTest : BaseTest
    {
        /// <summary>
        /// UC22 - Business or Member Views Account Settings
        /// Tests viewing the account settings for the specified <see cref="AccountType"/>.
        /// </summary>
        [Order(1)]
        [TestCase(AccountType.MEMBER)]
        [TestCase(AccountType.BUSINESS)]
        public void UC22_Account_AccountViewsAccountSettings_ShouldPass(AccountType accountType)
        {
            // Arrange
            // Fields & Buttons
            string navbarAccountSettingsID = "navbar-account-settings";
            
            // Titles
            string myAccountPageTitle = "My Account - KurbSide";
            
            // Act
            KSUnitTestLogin(accountType);
            KSClick(navbarAccountSettingsID);
            
            var result = _driver.Title.Contains(myAccountPageTitle);
            
            //Assert
            Assert.IsTrue(result);
        }
        
        /// <summary>
        /// UC23 Email preferences Tests
        /// </summary>
        [Order(3)]
        [Test]
        public void UC23_Account_MemberChangesEmailPreferences_ShouldPass()
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
