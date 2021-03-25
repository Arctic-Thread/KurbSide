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
        /// UC23 - Member Changes Email Preferences
        /// Tests changing email preferences.
        /// </summary>
        [Order(2)]
        [Test]
        public void UC23_Account_MemberChangesEmailPreferences_ShouldPass()
        {
            // Arrange
            // Fields & Buttons
            string preferencesID = "preferences";
            string promotionalEmailsCheckboxID = "Input_PromotionalEmails";
            string saveChanges = "update-profile-button";
            
            // Act
            UC22_Account_AccountViewsAccountSettings_ShouldPass(AccountType.MEMBER);
            KSClick(preferencesID);
            IWebElement emailCheckBox = _driver.FindElement(By.Id(promotionalEmailsCheckboxID));

            bool isChecked = emailCheckBox.Selected;
            KSClick(promotionalEmailsCheckboxID);
            KSClick(saveChanges);

            // The instance of the checkbox changes when refreshing the page so we must find it again.
            emailCheckBox = _driver.FindElement(By.Id(promotionalEmailsCheckboxID)); 
            
            //Assert
            // If the previous status of the checkbox is different, the changes were saved.
            Assert.IsTrue(emailCheckBox.Selected == !isChecked); 
        }
    }
}
