using System;
using System.Collections.Generic;
using NUnit.Framework;
using OpenQA.Selenium;

namespace KurbSideTest
{
    [TestFixture]
    class AuthenticationTests : BaseTest
    {
        private string randomString = RandomString(5) + "_";
        
        /// <summary>
        /// UC03 - Register as Business
        /// Tests registering as a business with invalid details.
        /// </summary>
        [Test]
        [Order(1)]
        public void UC03_Authentication_RegisterAsBusiness_InvalidDetails_ShouldFail()
        {
            // Arrange
            // Fields & Buttons
            string navbarRegisterButtonID = "navbar-register";
            string registerAsBusinessID = "register-page-business-register";
            string registerAsBusinessButtonID = "submit";

            // Titles
            string homePageTitle = "Home Page - KurbSide";
            string registerPageTitle = "Register - KurbSide";
            string businessRegisterPageTitle = "Register as Business - KurbSide";

            // Expected Result
            int numberOfExpectedErrors = 11;

            //Act
            KSTitleContains(homePageTitle); // Wait until home page is visible
            KSClick(navbarRegisterButtonID); // Click the register button in navbar
            KSTitleContains(registerPageTitle); // Wait until register page is visible
            KSClick(registerAsBusinessID); // Click "Register as a Business" button
            KSTitleContains(businessRegisterPageTitle); // Wait until business registration page is visible
            KSClick(registerAsBusinessButtonID);
            
            // Gets every IWebElement with an ID which has a suffix of "-error".
            IReadOnlyList<IWebElement> numberOfErrors = _driver.FindElements(By.CssSelector("[id$='-error']")); 
            
            //Assert
            Assert.IsTrue(numberOfErrors.Count == numberOfExpectedErrors);
        }

        /// <summary>
        /// UC03 - Register as Business
        /// Tests registering a new business account with valid details.
        /// </summary>
        [Test]
        [Order(2)]
        public void UC03_Authentication_RegisterAsBusiness_ValidDetails_ShouldPass()
        {
            // Arrange
            // Registration Details
            string email = randomString + "TESTMEMBER@mail.com";
            string firstName = randomString + "FIRSTNAME";
            string lastName = randomString + "LASTNAME";
            string contactPhoneNumber = "519-885-0300";
            string password = randomString + "PASSWORD12345";

            string businessName = randomString + "BUSINESSNAME";
            string businessPhoneNumber = "321-321-4321";
            string businessNumber = "123456789";

            string businessAddress = randomString + "ADDRESS1";
            string businessAddress2 = randomString + "ADDRESS2";
            string city = randomString + "CITY";
            string postalCode = "A1B-2C3";
            string province = "ONTARIO";
            string country = "Canada";

            string eightAM = "800A";
            string tenAM = "1000A";
            string noon = "1200A";
            string sixPM = "600P";

            // Fields & Buttons
            string navbarRegisterButtonID = "navbar-register";
            string registerAsBusinessID = "register-page-business-register";
            string sysMessageID = "sysMessage";

            string emailID = "Input_Email";
            string contactFirstID = "Input_ContactFirst";
            string contactLastID = "Input_ContactLast";
            string contactPhoneID = "Input_ContactPhone";
            string passwordID = "Input_Password";
            string confrimPasswordID = "Input_ConfirmPassword";

            string businessNameID = "Input_BusinessName";
            string businessPhoneID = "Input_PhoneNumber";
            string businessNumberID = "Input_BusinessNumber";
            string businessStreetID = "Input_Street";
            string businessStreet2ID = "Input_StreetLn2";
            string businessCityID = "Input_City";
            string businessPostalID = "Input_Postal";
            string businessProvinceID = "Input_ProvinceCode";
            string businessCountryID = "Input_CountryCode";
            string businessAdvancedHoursCheckboxID = "Input_UseGeneric";

            string monOpenID = "Input_MonOpen";
            string monClosedID = "Input_MonClose";
            string tuesOpenID = "Input_TuesOpen";
            string tuesClosedID = "Input_TuesClose";
            string wedOpenID = "Input_WedOpen";
            string wedClosedID = "Input_WedClose";
            string thurOpenID = "Input_ThuOpen";
            string thurClosedID = "Input_ThuClose";
            string friOpenID = "Input_FriOpen";
            string friClosedID = "Input_FriClose";
            string satOpenID = "Input_SatOpen";
            string satClosedID = "Input_SatClose";
            string sunOpenID = "Input_SunOpen";
            string sunClosedID = "Input_SunClose";

            string submitID = "submit";

            // Titles
            string homePageTitle = "Home Page - KurbSide";
            string registerPageTitle = "Register - KurbSide";
            string businessRegisterPageTitle = "Register as Business - KurbSide";
            string registrationConfirmationPageTitle = "Register confirmation - KurbSide";

            // Expected Result
            string expectedResult = $"We've sent an email to {email}, Please confirm your account to continue.";

            //Act
            KSTitleContains(homePageTitle); // Wait until home page is visible
            KSClick(navbarRegisterButtonID); // Click the register button in navbar
            KSTitleContains(registerPageTitle); // Wait until register page is visible
            KSClick(registerAsBusinessID); // Click "Register as a Business" button
            KSTitleContains(businessRegisterPageTitle); // Wait until business registration page is visible

            // Fill out the form
            KSSendKeys(emailID, email);
            KSSendKeys(contactFirstID, firstName);
            KSSendKeys(contactLastID, lastName);
            KSSendKeys(contactPhoneID, contactPhoneNumber);
            KSSendKeys(passwordID, password);
            KSSendKeys(confrimPasswordID, password);

            KSSendKeys(businessNameID, businessName);
            KSSendKeys(businessPhoneID, businessPhoneNumber);
            KSSendKeys(businessNumberID, businessNumber);
            KSSendKeys(businessStreetID, businessAddress);
            KSSendKeys(businessStreet2ID, businessAddress2);
            KSSendKeys(businessCityID, city);
            KSSendKeys(businessPostalID, postalCode);
            KSSendKeys(businessProvinceID, province);
            KSSendKeys(businessCountryID, country);

            KSClick(businessAdvancedHoursCheckboxID);

            KSSendKeys(monOpenID, eightAM);
            KSSendKeys(monClosedID, sixPM);
            KSSendKeys(tuesOpenID, eightAM);
            KSSendKeys(tuesClosedID, sixPM);
            KSSendKeys(wedOpenID, eightAM);
            KSSendKeys(wedClosedID, sixPM);
            KSSendKeys(thurOpenID, eightAM);
            KSSendKeys(thurClosedID, sixPM);
            KSSendKeys(friOpenID, eightAM);
            KSSendKeys(friClosedID, sixPM);
            KSSendKeys(friOpenID, eightAM);
            KSSendKeys(friClosedID, sixPM);
            KSSendKeys(satOpenID, tenAM);
            KSSendKeys(satClosedID, sixPM);
            KSSendKeys(sunOpenID, noon);
            KSSendKeys(sunClosedID, sixPM);

            KSClick(submitID);

            KSTitleContains(registrationConfirmationPageTitle); // Wait until confirmation page is visible

            string result = _driver.FindElement(By.Id(sysMessageID)).Text;

            //Assert
            Assert.That(result.Contains(expectedResult));
        }
        
        /// <summary>
        /// UC04 - Register as Member
        /// Tests registering for a new member account with invalid details.
        /// </summary>
        [Test]
        [Order(3)]
        public void UC04_Authentication_RegisterAsMember_InvalidInfo_ShouldFail()
        {
            // Arrange
            // Fields & Buttons
            string navbarRegisterButtonID = "navbar-register";
            string registerAsMemberID = "register-page-member-register";
            string registerAsMemberSubmit = "memberSubmit";

            // Titles
            string homePageTitle = "Home Page - KurbSide";
            string registerPageTitle = "Register - KurbSide";
            string memberRegisterPageTitle = "Register for KurbSide - KurbSide";

            // Expected Result
            int numberOfExpectedErrors = 10;

            //Act
            KSTitleContains(homePageTitle); // Wait until home page is visible
            KSClick(navbarRegisterButtonID); // Click the register button in navbar
            KSTitleContains(registerPageTitle); // Wait until register page is visible
            KSClick(registerAsMemberID); // Click "Register as a Member" button
            KSTitleContains(memberRegisterPageTitle); // Wait until business registration page is visible
            KSClick(registerAsMemberSubmit);

            // Gets every IWebElement with an ID which has a suffix of "-error".
            IReadOnlyList<IWebElement> numberOfErrors = _driver.FindElements(By.CssSelector("[id$='-error']"));

            //Assert
            Assert.IsTrue(numberOfErrors.Count == numberOfExpectedErrors);
        }
        
        /// <summary>
        /// UC04 - Register as Member
        /// Tests registering for a new member account with valid details.
        /// </summary>
        [Test]
        [Order(4)]
        public void UC04_Authentication_RegisterAsMember_ValidDetails_ShouldPass()
        {
            //Arrange
            // Registration Details
            string email = randomString + "TESTMEMBER@mail.com";
            string firstName = randomString + "FIRSTNAME";
            string lastName = randomString + "LASTNAME";
            string contactPhoneNumber = "519-885-0300";
            string password = randomString + "PASSWORD12345";
            string gender = "male";
            string birthdayYear = "1999";
            string birthMonth = "09";
            string birthday = "09";

            string memberAddress = randomString + "ADDRESS1";
            string memberAddress2 = randomString + "ADDRESS2";
            string city = randomString + "CITY";
            string postalCode = "A1B-2C3";
            string province = "ONTARIO";
            string country = "Canada";

            // Fields & Buttons
            string navbarRegisterButtonID = "navbar-register";
            string registerAsMemberID = "register-page-member-register";
            string sysMessageID = "sysMessage";
            
            string memberEmailID = "Input_Email";
            string memberPasswordID = "Input_Password";
            string memberConfirmPasswordID = "Input_ConfirmPassword";
            string memberPhoneID = "Input_Phone";
            string memberFirstNameID = "Input_FirstName";
            string memberLastNameID = "Input_LastName";
            string memberGenderID = "Input_Gender";
            string memberBirthdayID = "Input_Birthday";
            string memberStreetID = "Input_Street";
            string memberStreetLn2ID = "Input_StreetLn2";
            string memberCityID = "Input_City";
            string memberPostalID = "Input_Postal";
            string memberProvinceCodeID = "Input_ProvinceCode";
            string memberCountryCodeID = "Input_CountryCode";
            string memberSubmitButtonID = "memberSubmit";

            // Titles
            string homePageTitle = "Home Page - KurbSide";
            string registerPageTitle = "Register - KurbSide";
            string memberRegisterPageTitle = "Register for KurbSide - KurbSide";
            string registrationConfirmationPageTitle = "Register confirmation - KurbSide";

            // Expected Result
            string expectedResult = $"We've sent an email to {email}, Please confirm your account to continue.";

            // Act
            KSTitleContains(homePageTitle); // Wait until home page is visible
            KSClick(navbarRegisterButtonID); // Click the register button in navbar
            KSTitleContains(registerPageTitle); // Wait until register page is visible
            KSClick(registerAsMemberID); // Click "Register as a Member" button
            KSTitleContains(memberRegisterPageTitle); // Wait until business registration page is visible

            // Fill out the form
            KSSendKeys(memberEmailID,email);
            KSSendKeys(memberPasswordID,  password);
            KSSendKeys(memberConfirmPasswordID, password);
            KSSendKeys(memberPhoneID, contactPhoneNumber);
            KSSendKeys(memberFirstNameID, firstName);
            KSSendKeys(memberLastNameID, lastName);
            KSSendKeys(memberGenderID, gender);
            KSSendKeys(memberBirthdayID,birthdayYear);
            KSSendKeys(memberBirthdayID, Keys.Tab);
            KSSendKeys(memberBirthdayID, birthMonth);
            KSSendKeys(memberBirthdayID, birthday);
            KSSendKeys(memberStreetID, memberAddress);
            KSSendKeys(memberStreetLn2ID, memberAddress2);                      
            KSSendKeys(memberCityID,city);                     
            KSSendKeys(memberPostalID,postalCode);
            KSSendKeys(memberProvinceCodeID,province);
            KSSendKeys(memberCountryCodeID, country);

            KSClick(memberSubmitButtonID);

            KSTitleContains(registrationConfirmationPageTitle); // Wait until confirmation page is visible

            string result = _driver.FindElement(By.Id(sysMessageID)).Text;

            //Assert
            Assert.That(result.Contains(expectedResult));
        }

        /// <summary>
        /// UC05 - Login
        /// Tests logging in to the specified account type.
        /// </summary>
        /// <param name="accountType">The account type to be used in the unit test.</param>
        /// <param name="testCaseNavbarLinks">The ID for the list of links in the navbar for the given account type.</param>
        [TestCase(AccountType.MEMBER, "navbar-member-links")]
        [TestCase(AccountType.BUSINESS, "navbar-business-links")]
        [Order(5)]
        public void UC05_Authentication_Login_ShouldPass(AccountType accountType, string testCaseNavbarLinks)
        {
            // Arrange
            // Fields & Buttons
            string navbarLinks = testCaseNavbarLinks;

            //Act
            KSUnitTestLogin(accountType);
            var result = _driver.FindElement(By.Id(navbarLinks));

            //Assert
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// UC06 - Logout
        /// Tests logging in to a test account, then logging out.
        /// </summary>
        [Test]
        [Order(6)]
        public void UC06_Authentication_Logout_ShouldPass()
        {
            // Arrange
            // Fields & Buttons
            string navbarlogoutButtonID = "navbar-logout";
            string sysMessageID = "sysMessage";

            // Titles
            string homePageTitle = "Home Page - KurbSide";

            // Expected Result
            string expectedResult = "You have been logged out of your account.";

            //Act
            KSUnitTestLogin(AccountType.TEST);
            KSClick(navbarlogoutButtonID); // Click the logout button
            KSTitleContains(homePageTitle); // Wait until home page is visible.

            string result = _driver.FindElement(By.Id(sysMessageID)).Text;

            //Assert
            Assert.That(result.Contains(expectedResult));
        }

        /// <summary>
        /// UC07 - Change Password
        /// Tests logging in to a test account, then changing the password.
        /// </summary>
        [Test]
        [Order(7)]
        public void UC07_Authentication_ChangePassword_ShouldPass()
        {
            // Arrange
            // Login Details
            string loginPassword = "Password12345";

            // Fields & Buttons
            string navbarAccountSettingsID = "navbar-account-settings";
            string accountSettingsChangePasswordID = "change-password";
            string currentPasswordID = "Input_OldPassword";
            string newPasswordID = "Input_NewPassword";
            string confirmNewPasswordID = "Input_ConfirmPassword";
            string updatePasswordButtonID = "update-password";
            string sysMessageID = "sysMessage";

            // Titles
            string accountSettingsPageTitle = "My Account - KurbSide";
            string changePasswordPageTitle = "Change password - KurbSide";

            // Expected Result
            string expectedResult = "Your password has been changed.";

            //Act
            KSUnitTestLogin(AccountType.TEST);
            KSClick(navbarAccountSettingsID); // Click the logout button.
            KSTitleContains(accountSettingsPageTitle); // Wait until account settings page is visible.
            KSClick(accountSettingsChangePasswordID); // Click the change password button / "Password" link in account settings.
            KSTitleContains(changePasswordPageTitle); // Wait until account settings page is visible.

            KSSendKeys(currentPasswordID, loginPassword); // Enter the current password.
            KSSendKeys(newPasswordID, loginPassword); // Enter the "new" password, its the current password for the sake of the test case.
            KSSendKeys(confirmNewPasswordID, loginPassword); // confirm the "new" password, its the current password for the sake of the test case.
            KSClick(updatePasswordButtonID); // Change the password.
            KSTitleContains(changePasswordPageTitle); // Wait until account settings page is visible.

            string result = _driver.FindElement(By.Id(sysMessageID)).Text;

            //Assert
            Assert.That(result.Contains(expectedResult));
        }
    }
}