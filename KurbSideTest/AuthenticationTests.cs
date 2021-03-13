using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace KurbSideTest
{
    [TestFixture]
    class AuthenticationTests : BaseTest
    {
        private string randomString = RandomString(5) + "_";

        //We can change the ordering when we decide on it later.
        //[Order(1)]
        [Test]
        public void UC00_Category_ThingsUnderTest_ShouldPass()
        {
            // Arrange
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));

            // Login Details
            KSUnitTestLogin(AccountType.TEST);

            // Fields & Buttons

            // Titles

            // Expected Result
            var expectedResult = "";

            //Act
            var result = "";

            //Assert
            Assert.AreEqual(expectedResult, result);
        }

        /// <summary>
        /// UC03 - Register as Business
        /// Tests registering a new business account
        /// </summary>
        //[Order(1)]
        [Test]
        public void UC03_Authentication_BusinessRegister_ShouldPass()
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
        /// UC05 - Login
        /// Tests logging in to the specified account type.
        /// </summary>
        /// <param name="accountType">The account type to be used in the unit test.</param>
        /// <param name="testCaseNavbarLinks">The ID for the list of links in the navbar for the given account type.</param>
        //[Order(1)]
        //[TestCase(AccountType.MEMBER, "navbar-member-links")]
        [TestCase(AccountType.BUSINESS, "navbar-business-links")]
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
        //[Order(1)]
        [Test]
        public void UC06_Authentication_Logout_ShouldPass()
        {
            // Arrange

            // Fields & Buttons
            string navbarlogoutButtonID = "navbar-logout";
            string sysMessageID = "sysMessage";

            // Titles
            string homePageTitle = "Home Page - KurbSide";

            // Expected Result
            string expectedResult = "× You have been logged out of your account.";

            //Act
            KSUnitTestLogin(AccountType.TEST);
            KSClick(navbarlogoutButtonID); // Click the logout button
            KSTitleContains(homePageTitle); // Wait until home page is visible.

            string result = _driver.FindElement(By.Id(sysMessageID)).Text;

            //Assert
            Assert.AreEqual(expectedResult, result);
        }

        /// <summary>
        /// UC07 - Change Password
        /// Tests logging in to a test account, then changing the password.
        /// </summary>
        //[Order(1)]
        [Test]
        public void UC07_Authentication_ChangePassword_ShouldPass()
        {
            // Arrange

            // Login Details
            string loginPassword = "Password12345";

            // Fields & Buttons
            string navbarAccountSettingsID = "navbar-account-settings";
            string accountSettingschangePasswordID = "change-password";
            string currentPasswordID = "Input_OldPassword";
            string newPasswordID = "Input_NewPassword";
            string confirmNewPasswordID = "Input_ConfirmPassword";
            string updatePasswordButtonID = "update-password";
            string sysMessageID = "sysMessage";

            // Titles
            string accountSettingsPageTitle = "Profile - KurbSide";
            string changePasswordPageTitle = "Change password - KurbSide";

            // Expected Result
            string expectedResult = "× Your password has been changed.";

            //Act
            KSUnitTestLogin(AccountType.TEST);
            KSClick(navbarAccountSettingsID); // Click the logout button.
            KSTitleContains(accountSettingsPageTitle); // Wait until account settings page is visible.
            KSClick(accountSettingschangePasswordID); // Click the change password button / "Password" link in account settings.
            KSTitleContains(changePasswordPageTitle); // Wait until account settings page is visible.

            KSSendKeys(currentPasswordID, loginPassword); // Enter the current password.
            KSSendKeys(newPasswordID, loginPassword); // Enter the "new" password, its the current password for the sake of the test case.
            KSSendKeys(confirmNewPasswordID, loginPassword); // confirm the "new" password, its the current password for the sake of the test case.
            KSClick(updatePasswordButtonID); // Change the password.
            KSTitleContains(changePasswordPageTitle); // Wait until account settings page is visible.

            string result = _driver.FindElement(By.Id(sysMessageID)).Text;

            //Assert
            Assert.AreEqual(expectedResult, result);
        }
        /// <summary>
        /// Test for the member registration page using a valid member 
        /// </summary>
        [Test]
        public void UC04_RegisterAsMember_ShouldPass()
        {

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

            string navbarRegisterButtonID = "navbar-register";
            string registerAsMemberID = "register-page-member-register";
            string sysMessageID = "sysMessage";

            string homePageTitle = "Home Page - KurbSide";
            string registerPageTitle = "Register - KurbSide";
            string memberRegisterPageTitle = "Register for KurbSide - KurbSide";
            string registrationConfirmationPageTitle = "Register confirmation - KurbSide";

            // Expected Result
            string expectedResult = $"×\r\nWe've sent an email to {email}, Please confirm your account to continue.";

            KSTitleContains(homePageTitle); // Wait until home page is visible
            KSClick(navbarRegisterButtonID); // Click the register button in navbar
            KSTitleContains(registerPageTitle); // Wait until register page is visible
            KSClick(registerAsMemberID); // Click "Register as a Member" button
            KSTitleContains(memberRegisterPageTitle); // Wait until business registration page is visible

            KSSendKeys("Input_Email",email);
            KSSendKeys("Input_Password", password);
            KSSendKeys("Input_ConfirmPassword",password);
            KSSendKeys("Input_Phone",contactPhoneNumber);
            KSSendKeys("Input_FirstName",firstName);
            KSSendKeys("Input_LastName",lastName);
            KSSendKeys("Input_Gender",gender);
            KSSendKeys("Input_Birthday",birthdayYear);
            KSSendKeys("Input_Birthday",Keys.Tab);
            KSSendKeys("Input_Birthday", birthMonth);
            KSSendKeys("Input_Birthday", birthday);
            KSSendKeys("Input_Street", memberAddress);
            KSSendKeys("Input_StreetLn2", memberAddress2);                      
            KSSendKeys("Input_City",city);                     
            KSSendKeys("Input_Postal",postalCode);
            KSSendKeys("Input_ProvinceCode",province);
            KSSendKeys("Input_CountryCode",country);

            KSClick("memberSubmit");

            KSTitleContains(registrationConfirmationPageTitle); // Wait until confirmation page is visible

            string result = _driver.FindElement(By.Id(sysMessageID)).Text;

            //Assert
            Assert.AreEqual(expectedResult, result);
        }
        
        [Test]
        public void UC04_RegisterAsMember_InvalidInfo_ShouldPass()
        {

            string email = randomString + "TESTMEMBER@mail.com";
            string firstName = randomString + "FIRSTNAME";
            string lastName = randomString + "LASTNAME";
            string contactPhoneNumber = "519-885-0300";
            string password = randomString + "Test";
            string gender = "attack helicopter";
            string birthdayYear = "2100";
            string birthMonth = "45";
            string birthday = "89";

            string memberAddress = randomString + "ADDRESS1";
            string memberAddress2 = randomString + "ADDRESS2";
            string city = randomString + "CITY";
            string postalCode = "A1B-2C3";
            string province = "ONTARIO";
            string country = "Canada";

            string navbarRegisterButtonID = "navbar-register";
            string registerAsMemberID = "register-page-member-register";
            string sysMessageID = "sysMessage";

            string homePageTitle = "Home Page - KurbSide";
            string registerPageTitle = "Register - KurbSide";
            string memberRegisterPageTitle = "Register for KurbSide - KurbSide";
            string registrationConfirmationPageTitle = "Register confirmation - KurbSide";

            // Expected Result
            string expectedResult = $"×\r\nWe've sent an email to {email}, Please confirm your account to continue.";

            KSTitleContains(homePageTitle); // Wait until home page is visible
            KSClick(navbarRegisterButtonID); // Click the register button in navbar
            KSTitleContains(registerPageTitle); // Wait until register page is visible
            KSClick(registerAsMemberID); // Click "Register as a Member" button
            KSTitleContains(memberRegisterPageTitle); // Wait until business registration page is visible

            KSSendKeys("Input_Email",email);
            KSSendKeys("Input_Password", password);
            KSSendKeys("Input_ConfirmPassword",password);
            KSSendKeys("Input_Phone",contactPhoneNumber);
            KSSendKeys("Input_FirstName",firstName);
            KSSendKeys("Input_LastName",lastName);
            KSSendKeys("Input_Gender",gender);
            KSSendKeys("Input_Birthday",birthdayYear);
            KSSendKeys("Input_Birthday",Keys.Tab);
            KSSendKeys("Input_Birthday", birthMonth);
            KSSendKeys("Input_Birthday", birthday);
            KSSendKeys("Input_Street", memberAddress);
            KSSendKeys("Input_StreetLn2", memberAddress2);                      
            KSSendKeys("Input_City",city);                     
            KSSendKeys("Input_Postal",postalCode);
            KSSendKeys("Input_ProvinceCode",province);
            KSSendKeys("Input_CountryCode",country);

            KSClick("memberSubmit");

            KSTitleContains(registrationConfirmationPageTitle); // Wait until confirmation page is visible

            string result = _driver.FindElement(By.Id(sysMessageID)).Text;

            //Assert
            Assert.AreEqual(expectedResult, result);
        }
    }
}