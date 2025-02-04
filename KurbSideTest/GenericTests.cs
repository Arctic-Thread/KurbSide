﻿using NUnit.Framework;

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
    }
}
