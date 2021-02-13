using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace KurbSideTest
{
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
    }
}