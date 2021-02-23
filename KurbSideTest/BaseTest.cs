using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace KurbSideTest
{
    /// <summary>
    /// Specifies which account type should be used. MEMBER, BUSINESS or TEST
    /// </summary>
    public enum AccountType
    {
        /// <summary>
        /// A Member account: member@kurbsi.de - Note: Should be used when testing member functionality.
        /// </summary>
        MEMBER,
        /// <summary>
        /// A Business account: business@kurbsi.de - Note: Should be used when testing business functionality.
        /// </summary>
        BUSINESS,
        /// <summary>
        /// A Business account: test@kurbsi.de - Note: Should be used for anything that doesn't fall under business or member specifically.
        /// </summary>
        TEST
    }

    [TestFixture]
    public partial class BaseTest
    {
        public IWebDriver _driver;
        public Process _application;

        [SetUp]
        public void SetUpMethod()
        {
            var newPath = Directory.GetParent(
                Directory.GetParent(
                    Directory.GetParent(
                        Directory.GetParent(
                            Directory.GetCurrentDirectory())
                        .ToString())
                    .ToString())
                .ToString())
                .ToString();

            newPath = newPath + "\\KurbSide\\bin\\Debug\\netcoreapp3.1\\";

            Console.WriteLine(newPath);

            var startInfo = new ProcessStartInfo(@$"{newPath}KurbSide.exe");
            startInfo.WorkingDirectory = @$"{newPath}";
            _application = Process.Start(startInfo);

            var chrome = new ChromeOptions
            {
                AcceptInsecureCertificates = true
            };
            _driver = new ChromeDriver(newPath, chrome);
            _driver.Navigate().GoToUrl("http://localhost:5000/");
        }

        [TearDown]
        public void TearDownMethod()
        {
            if (_driver != null)
            {
                _driver.Quit();
                _application.Kill();
            }
        }

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        /// <summary>
        /// Logs in to the specified account type for the unit test.
        /// </summary>
        /// <param name="accountType">The account type to be used in the unit test.</param>
        public void KSUnitTestLogin(AccountType accountType)
        {
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));

            // Login Details
            string loginEmail;
            string loginPassword = "Password12345";

            switch (accountType)
            {
                case AccountType.MEMBER:
                    loginEmail = "member@kurbsi.de";
                    break;
                case AccountType.BUSINESS:
                    loginEmail = "business@kurbsi.de";
                    break;
                case AccountType.TEST:
                    loginEmail = "test@kurbsi.de";
                    break;
                default:
                    throw new NotImplementedException("KurbSideTest.BaseTest.KSUnitTestLogin - Account Type Not Implemented");
            }



            // Fields & Buttons
            string navbarLoginButtonID = "navbar-login";
            string loginEmailFieldID = "Input_Email";
            string loginPasswordFieldID = "Input_Password";
            string loginButtonID = "login-button";

            // Titles
            string homePageTitle = "Home Page - KurbSide";
            string loginPageTitle = "Log in - KurbSide";

            //Act
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.TitleContains(homePageTitle)); // Wait until home page is visible.
            _driver.FindElement(By.Id(navbarLoginButtonID)).Click(); // Click login button in navbar.
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.TitleContains(loginPageTitle)); // Wait until login page is visible
            _driver.FindElement(By.Id(loginEmailFieldID)).SendKeys(loginEmail); // Send email to email field.
            _driver.FindElement(By.Id(loginPasswordFieldID)).SendKeys(loginPassword); // Send password to password field.
            _driver.FindElement(By.Id(loginButtonID)).Click(); // Click the login button.
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.TitleContains(homePageTitle)); // Wait until home page is visible.
        }
    }
}