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
        private string randomString = RandomString(5) + "_";

        //We can change the ordering when we decide on it later.

        //[Order(1)]
        [Test]
        public void UC00_Category_ThingsUnderTest_ShouldPass()
        {
            // Arrange
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));

            // Login Details
            string loginEmail = "test@kurbsi.de";
            string loginPassword = "Password12345";

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
        /// Registers a new business account
        /// </summary>
        //[Order(1)]
        [Test]
        public void UC03_Authentication_BusinessRegister_ShouldPass()
        {
            // Arrange
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));

            // Registraton Details
            string email = randomString + "TESTMEMBER@mail.com";
            string firstName = randomString + "FIRSTNAME";
            string lastName = randomString + "LASTNAME";
            string contactPhoneNumber = "123-123-1234";
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
            string expectedResult = $"× We've sent an email to {email}, Please confirm your account to continue.";

            //Act
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.TitleContains(homePageTitle)); // Wait until home page is visible
            _driver.FindElement(By.Id(navbarRegisterButtonID)).Click(); // Click the register button in navbar
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.TitleContains(registerPageTitle)); // Wait until register page is visible
            _driver.FindElement(By.Id(registerAsBusinessID)).Click(); // Click "Register as a Business" button
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.TitleContains(businessRegisterPageTitle)); // Wait until business registration page is visible

            // Fill out the form
            _driver.FindElement(By.Id(emailID)).SendKeys(email);
            _driver.FindElement(By.Id(contactFirstID)).SendKeys(firstName);
            _driver.FindElement(By.Id(contactLastID)).SendKeys(lastName);
            _driver.FindElement(By.Id(contactPhoneID)).SendKeys(contactPhoneNumber);
            _driver.FindElement(By.Id(passwordID)).SendKeys(password);
            _driver.FindElement(By.Id(confrimPasswordID)).SendKeys(password);

            _driver.FindElement(By.Id(businessNameID)).SendKeys(businessName);
            _driver.FindElement(By.Id(businessPhoneID)).SendKeys(businessPhoneNumber);
            _driver.FindElement(By.Id(businessNumberID)).SendKeys(businessNumber);
            _driver.FindElement(By.Id(businessStreetID)).SendKeys(businessAddress);
            _driver.FindElement(By.Id(businessStreet2ID)).SendKeys(businessAddress2);
            _driver.FindElement(By.Id(businessCityID)).SendKeys(city);
            _driver.FindElement(By.Id(businessPostalID)).SendKeys(postalCode);
            _driver.FindElement(By.Id(businessProvinceID)).SendKeys(province);
            _driver.FindElement(By.Id(businessCountryID)).SendKeys(country);

            _driver.FindElement(By.Id(businessAdvancedHoursCheckboxID)).Click();

            _driver.FindElement(By.Id(monOpenID)).SendKeys(eightAM);
            _driver.FindElement(By.Id(monClosedID)).SendKeys(sixPM);
            _driver.FindElement(By.Id(tuesOpenID)).SendKeys(eightAM);
            _driver.FindElement(By.Id(tuesClosedID)).SendKeys(sixPM);
            _driver.FindElement(By.Id(wedOpenID)).SendKeys(eightAM);
            _driver.FindElement(By.Id(wedClosedID)).SendKeys(sixPM);
            _driver.FindElement(By.Id(thurOpenID)).SendKeys(eightAM);
            _driver.FindElement(By.Id(thurClosedID)).SendKeys(sixPM);
            _driver.FindElement(By.Id(friOpenID)).SendKeys(eightAM);
            _driver.FindElement(By.Id(friClosedID)).SendKeys(sixPM);
            _driver.FindElement(By.Id(friOpenID)).SendKeys(eightAM);
            _driver.FindElement(By.Id(friClosedID)).SendKeys(sixPM);
            _driver.FindElement(By.Id(satOpenID)).SendKeys(tenAM);
            _driver.FindElement(By.Id(satClosedID)).SendKeys(sixPM);
            _driver.FindElement(By.Id(sunOpenID)).SendKeys(noon);
            _driver.FindElement(By.Id(sunClosedID)).SendKeys(sixPM);

            _driver.FindElement(By.Id(submitID)).Click();

            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.TitleContains(registrationConfirmationPageTitle)); // Wait until confirmation page is visible

            string result = _driver.FindElement(By.Id(sysMessageID)).Text;

            //Assert
            Assert.AreEqual(expectedResult, result);
        }

        /// <summary>
        /// UC05 - Login
        /// Logs in with both account types, member and business.
        /// </summary>
        /// <param name="testCaseEmail">The email for each account type</param>
        /// <param name="testCaseNavbarLinks">The ID to the list of links in the navbar for the given account type</param>
        //[Order(1)]
        [TestCase("member@kurbsi.de", "navbar-member-links")]
        [TestCase("business@kurbsi.de", "navbar-business-links")]
        public void UC05_Authentication_Login_ShouldPass(string testCaseEmail, string testCaseNavbarLinks)
        {
            // Arrange
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));

            // Login Details
            string loginEmail = testCaseEmail;
            string loginPassword = "Password12345";

            // Fields & Buttons
            string navbarLoginButtonID = "navbar-login";
            string loginEmailFieldID = "Input_Email";
            string loginPasswordFieldID = "Input_Password";
            string loginButtonID = "login-button";
            string navbarLinks = testCaseNavbarLinks;

            // Titles
            string homePageTitle = "Home Page - KurbSide";
            string loginPageTitle = "Log in - KurbSide";

            //Act
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.TitleContains(homePageTitle)); // Wait until home page is visible.
            _driver.FindElement(By.Id(navbarLoginButtonID)).Click(); // Click login buttin in navbar.
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.TitleContains(loginPageTitle)); // Wait until login page is visible
            _driver.FindElement(By.Id(loginEmailFieldID)).SendKeys(loginEmail); // Send email to email field.
            _driver.FindElement(By.Id(loginPasswordFieldID)).SendKeys(loginPassword); // Send password to password field.
            _driver.FindElement(By.Id(loginButtonID)).Click(); // Click the login button.
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.TitleContains(homePageTitle)); // Wait until home page is visible.

            var result = _driver.FindElement(By.Id(navbarLinks));

            //Assert
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// UC06 - Logout
        /// Logs into a test account then logs out.
        /// </summary>
        //[Order(1)]
        [Test]
        public void UC06_Authentication_Logout_ShouldPass()
        {
            // Arrange
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
            _driver.FindElement(By.Id(navbarLoginButtonID)).Click(); // Click login buttin in navbar.
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.TitleContains(loginPageTitle)); // Wait until login page is visible
            _driver.FindElement(By.Id(loginEmailFieldID)).SendKeys(loginEmail); // Send email to email field.
            _driver.FindElement(By.Id(loginPasswordFieldID)).SendKeys(loginPassword); // Send password to password field.
            _driver.FindElement(By.Id(loginButtonID)).Click(); // Click the login button.
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.TitleContains(homePageTitle)); // Wait until home page is visible.
            _driver.FindElement(By.Id(navbarlogoutButtonID)).Click(); // Click the logout button
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.TitleContains(homePageTitle)); // Wait until home page is visible.

            string result = _driver.FindElement(By.Id(sysMessageID)).Text;

            //Assert
            Assert.AreEqual(expectedResult, result);
        }
    }
}
