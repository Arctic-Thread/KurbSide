using NUnit.Framework;

namespace KurbSideTest
{
    [TestFixture]
    public class NotificationTests : BaseTest
    {
        /// <summary>
        /// UC31 - Business / Member Views Notifications
        /// Tests the viewing of the notifications page for each account type.
        /// </summary>
        /// <param name="accountType">The account type of the user to test.</param>
        [TestCase(AccountType.MEMBER)]
        [TestCase(AccountType.BUSINESS)]
        public void UC31_Notifications_ViewNotifications_ShouldPass(AccountType accountType)
        {
            // Arrange
            // Fields & Buttons
            var navbarNotificationsID = "navbar-notifications";

            //Expected Title
            var expectedTitle = "Notifications - KurbSide";

            // Act
            KSUnitTestLogin(accountType);
            KSClick(navbarNotificationsID);

            KSTitleContains(expectedTitle);

            var actualTitle = _driver.Title;

            // Assert
            Assert.AreEqual(expectedTitle, actualTitle);
        }
    }
}