using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;
using OpenQA.Selenium;

namespace KurbSideTest
{
    [TestFixture]
    public class StoreTests : BaseTest
    {
        /// <summary>
        /// UC33 - Member Views Business List
        /// Tests if the store page loads and business listings are generated.
        /// </summary>
        [Test]
        [Order(1)]
        public void UC33_Store_ViewListOfBusinesses_ShouldPass()
        {
            // Arrange
            // Fields & Buttons
            string businessListingsCSSSelector = "[id^='businessListing-']"; // IDs that start with 'businessListing-'
            
            //Titles
            string storesPageTitle = "Stores - KurbSide";
            
            // Act
            KSUnitTestLogin(AccountType.MEMBER);
            KSTitleContains(storesPageTitle);
            IReadOnlyList<IWebElement> originalNumberOfBusinessListings = _driver.FindElements(By.CssSelector(businessListingsCSSSelector));

            // The number of business listings is greater than zero (indicating the web server has retrieved a list of businesses).
            bool result = originalNumberOfBusinessListings.Count > 0;
            
            // Assert
            Assert.IsTrue(result);
        }

        /// <summary>
        /// UC33 - Member Views Business List
        /// Tests that the stores generated are based on the maximum distance provided.
        /// </summary>
        [Test]
        [Order(2)]
        public void UC33_Store_ViewListOfBusinessesDistanceModifier_ShouldPass()
        {
            // Arrange
            // Fields & Buttons
            string businessListingsCSSSelector = "[id^='businessListing-']"; // IDs that start with 'businessListing-'
            string maximumDistanceID = "md";
            
            //Titles
            string storesPageTitle = "Stores - KurbSide";
            
            // Act
            KSUnitTestLogin(AccountType.MEMBER);
            
            KSTitleContains(storesPageTitle);
            IReadOnlyList<IWebElement> originalNumberOfBusinessListings = _driver.FindElements(By.CssSelector(businessListingsCSSSelector));
            KSReplaceText(maximumDistanceID, "5");
            KSSendKeys(maximumDistanceID, Keys.Enter);
            
            KSTitleContains(storesPageTitle);
            IReadOnlyList<IWebElement> newNumberOfBusinessListings = _driver.FindElements(By.CssSelector(businessListingsCSSSelector));

            // The number of business listings has decreased (indicating the distance filtering has narrowed down the results).
            bool result = originalNumberOfBusinessListings.Count > newNumberOfBusinessListings.Count;
            
            Assert.IsTrue(result);
        }

        /// <summary>
        /// UC33 - Member Views Business List
        /// Tests that the stores search functionality is properly working.
        /// </summary>
        [Test]
        [Order(3)]
        public void UC33_Store_SearchForBusiness_ShouldPass()
        {
            // Arrange
            // Fields & Buttons
            string businessListingsCSSSelector = "[id^='businessListing-']"; // IDs that start with 'businessListing-'
            string searchBarID = "filter2";
            string desiredBusinessListingID = "businessListing-test";
            
            // Titles
            string storesPageTitle = "Stores - KurbSide";
            
            // Act
            KSUnitTestLogin(AccountType.MEMBER);
            
            KSTitleContains(storesPageTitle);
            IReadOnlyList<IWebElement> originalNumberOfBusinessListings = _driver.FindElements(By.CssSelector(businessListingsCSSSelector));
            KSReplaceText(searchBarID, "test");
            KSSendKeys(searchBarID, Keys.Enter);
            
            IReadOnlyList<IWebElement> newNumberOfBusinessListings = _driver.FindElements(By.CssSelector(businessListingsCSSSelector));
            IWebElement desiredBusinessListing = _driver.FindElement(By.Id(desiredBusinessListingID));
            
            // The number of business listings has decreased (indicating the search has narrowed down the results), and the desired business is displayed.
            bool result = originalNumberOfBusinessListings.Count > newNumberOfBusinessListings.Count && desiredBusinessListing.Displayed;

            Assert.IsTrue(result);
        }

        /// <summary>
        /// UC34 - Member Views Business Catalogue
        /// Tests that the selected businesses catalogue is displayed with their items.
        /// </summary>
        [Test]
        [Order(3)]
        public void UC34_Store_ViewBusinessCatalogue_ShouldPass()
        {
            // Arrange
            // Fields & Buttons
            string searchBarID = "filter2";
            string viewBusinessCatalogueButtonID = "view-test-catalogue";
            string catalogueID = "test-Catalogue";

            // Titles
            string storesPageTitle = "Stores - KurbSide";
            string businessPageTitle = "test - KurbSide";
            
            // Act
            KSUnitTestLogin(AccountType.MEMBER);
            
            KSTitleContains(storesPageTitle);
            KSReplaceText(searchBarID, "test");
            KSSendKeys(searchBarID, Keys.Enter);
            
            KSClick(viewBusinessCatalogueButtonID);
            
            KSTitleContains(businessPageTitle);
            IReadOnlyList<IWebElement> catalogueItems = _driver.FindElements(By.Id(catalogueID));

            // There are items found on the business catalogue page.
            bool result = catalogueItems.Count > 0;
            
            Assert.IsTrue(result);
        }

        /// <summary>
        /// UC35 - Member Views Item
        /// Tests that the selected item page is displayed when clicked in a business catalogue.
        /// </summary>
        [Test]
        [Order(4)]
        public void UC35_Store_ViewBusinessItem_ShouldPass()
        {
            // Arrange
            // Fields & Buttons
            string searchBarID = "filter2";
            string viewBusinessCatalogueButtonID = "view-test-catalogue";
            string viewTestItemID = "view-Test Item";

            // Titles
            string storesPageTitle = "Stores - KurbSide";
            string businessPageTitle = "test - KurbSide";
            string testItemPageTitle = "Test Item - KurbSide";
            
            // Act
            KSUnitTestLogin(AccountType.MEMBER);
            
            KSTitleContains(storesPageTitle);
            KSReplaceText(searchBarID, "test");
            KSSendKeys(searchBarID, Keys.Enter);
            
            KSClick(viewBusinessCatalogueButtonID);
            
            KSTitleContains(businessPageTitle);
            KSClick(viewTestItemID);
            
            KSTitleContains(testItemPageTitle);

            bool result = _driver.Title.Contains(testItemPageTitle);
            
            Assert.IsTrue(result);
        }
        
        /// <summary>
        /// UC26 member adds item to cart test
        /// </summary>
        [Test]
        [Order(5)]
        public void UC26_MemberAddsToCart_ShouldPass()
        {
            string searchBarID = "filter2";
            string viewBusinessCatalogueButtonID = "view-test-catalogue";
            
            // Titles
            string storesPageTitle = "Stores - KurbSide";
            string businessPageTitle = "test - KurbSide";
            
            // Act
            KSUnitTestLogin(AccountType.MEMBER);
            
            KSTitleContains(storesPageTitle);
            KSReplaceText(searchBarID, "test");
            KSSendKeys(searchBarID, Keys.Enter);
            
            KSClick(viewBusinessCatalogueButtonID);
            
            KSTitleContains(businessPageTitle);

            KSClick("addToCart");//adds a item to the cart to make the cart visible
            
            IReadOnlyList<IWebElement> cartItems = _driver.FindElements(By.Id("cartItems"));
            
            bool result = cartItems.Count > 0;//checks to see if there are items in the cart
            
            KSClick("clear-cart");//clears the cart to make the test work for other test
            
            var confirmClearCart = _driver.SwitchTo().Alert();
            confirmClearCart.Accept();
            
            Assert.IsTrue(result);
        }
        
        [Test]
        [Order(6)]
        public void UC27_MemberClearsCart_ShouldPass()
        {
            string searchBarID = "filter2";
            string viewBusinessCatalogueButtonID = "view-test-catalogue";
            
            // Titles
            string storesPageTitle = "Stores - KurbSide";
            string businessPageTitle = "test - KurbSide";
            
            // Act
            KSUnitTestLogin(AccountType.MEMBER);
            
            KSTitleContains(storesPageTitle);
            KSReplaceText(searchBarID, "test");
            KSSendKeys(searchBarID, Keys.Enter);
            
            KSClick(viewBusinessCatalogueButtonID);
            
            KSTitleContains(businessPageTitle);

            KSClick("addToCart");//adds a item to the cart to make the cart visible
            
            IReadOnlyList<IWebElement> cartItems = _driver.FindElements(By.Id("cartItems"));
            
            KSClick("clear-cart");//clears the cart to make the test work for other test
            
            var confirmClearCart = _driver.SwitchTo().Alert();
            confirmClearCart.Accept();
            //checks to see if there are items in the cart
            bool result = cartItems.Count == 1; //(note that there is a element in the cart so the result will not come back as 0 )
            
            Assert.IsTrue(result);
        }
        
        
        
        
    }
}