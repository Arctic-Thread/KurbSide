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
            IReadOnlyList<IWebElement> originalNumberOfBusinessListings =
                _driver.FindElements(By.CssSelector(businessListingsCSSSelector));

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

            // Assert 
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

            // Assert 
            Assert.IsTrue(result);
        }

        /// <summary>
        /// UC34 - Member Views Business Catalogue
        /// Tests that the selected businesses catalogue is displayed with their items.
        /// </summary>
        [Test]
        [Order(4)]
        public void UC34_Store_ViewBusinessCatalogue_ShouldPass()
        {
            // Arrange
            // Fields & Buttons
            string searchBarID = "filter2";
            string viewBusinessCatalogueButtonID = "view-test-catalogue";
            string catalogueItems = "[id^='catalogue-item-']";

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
            IReadOnlyList<IWebElement> catalogueItemsList = _driver.FindElements(By.CssSelector(catalogueItems)); 

            // There are items found on the business catalogue page.
            bool result = catalogueItemsList.Count > 0;

            // Assert 
            Assert.IsTrue(result);
        }

        /// <summary>
        /// UC35 - Member Views Item
        /// Tests that the selected item page is displayed when clicked in a business catalogue.
        /// </summary>
        [Test]
        [Order(5)]
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

            // Assert 
            Assert.IsTrue(result);
        }

        /// <summary>
        /// UC26 - Member Adds To Cart
        /// Tests adding an item to a shopping cart.
        /// </summary>
        [Test]
        [Order(6)]
        public void UC26_Store_MemberAddsToCart_ShouldPass()
        {
            // Arrange
            // Fields & Buttons
            string searchBarID = "filter2";
            string searchCatalogueID = "filter";
            string viewBusinessCatalogueButtonID = "view-test-catalogue";
            string addToCartButtonID = "addToCart";
            string cartItemsID = "cartItems";

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
            KSReplaceText(searchCatalogueID, "test");
            KSSendKeys(searchCatalogueID, Keys.Enter);

            KSClick(addToCartButtonID); //adds a item to the cart to make the cart visible

            IReadOnlyList<IWebElement> cartItems = _driver.FindElements(By.Id(cartItemsID));

            bool result = cartItems.Count > 0; //checks to see if there are items in the cart

            // Assert 
            Assert.IsTrue(result);
        }

        /// <summary>
        /// UC27 - Member Removes Item From Cart
        /// Tests removing all items from a shopping cart.
        /// </summary>
        [Test]
        [Order(7)]
        public void UC27_Store_MemberClearsCart_ShouldPass()
        {
            // Arrange
            // Fields & Buttons
            string clearCartButtonID = "clear-cart";
            string cartItemsID = "cartItems";

            // Act
            UC26_Store_MemberAddsToCart_ShouldPass();

            KSClick(clearCartButtonID); //clears the cart to make the test work for other test
            var confirmClearCart = _driver.SwitchTo().Alert();
            confirmClearCart.Accept();
            
            IReadOnlyList<IWebElement> cartItems = _driver.FindElements(By.Id(cartItemsID));
            //checks to see if there are items in the cart
            bool result = cartItems.Count == 0; 

            // Assert 
            Assert.IsTrue(result);
        }

        /// <summary>
        /// UC27 - Member Removes Item From Cart
        /// Tests removing a specific item from a shopping cart.
        /// </summary>
        [Test]
        [Order(8)]
        public void UC27_Store_MemberRemovesFromCart_ShouldPass()
        {
            // Arrange
            // Fields & Buttons
            string removeTestItemFromCartButtonID = "remove-from-cart-Test Item";
            string cartItemsID = "cartItems";
            bool result = false;

            // Titles

            // Act
            UC26_Store_MemberAddsToCart_ShouldPass();

            IReadOnlyList<IWebElement> cartItems = _driver.FindElements(By.Id(cartItemsID));

            KSClick(removeTestItemFromCartButtonID); //clears the cart to make the test work for other test

            var confirmRemoveFromCart = _driver.SwitchTo().Alert();
            confirmRemoveFromCart.Accept();
            IReadOnlyList<IWebElement> cartItems2 = _driver.FindElements(By.Id(cartItemsID));
            //checks to see if there are items in the cart

            if (cartItems.Count - 1 == cartItems2.Count)
            {
                result = true;
            }
            // Assert 
            Assert.IsTrue(result);
        }

        /// <summary>
        /// UC28 - Member Checkout
        /// Tests adding an item to a shopping cart and then purchasing it
        ///  passes when the order confirmation page is displayed.
        /// TODO Revisit when order page is completed.
        /// </summary>
        [Test]
        [Order(9)]
        public void UC28_Store_MemberCheckout_ShouldPass()
        {
            // Arrange
            // Fields & Buttons
            string checkoutButtonID = "checkout";
            string placeOrderFormID = "PlaceOrderForm";

            // Titles
            string businessPageTitle = "test - KurbSide";
            string orderConfirmationPageTitle = "My Orders - KurbSide";

            // Act
            UC26_Store_MemberAddsToCart_ShouldPass();

            KSTitleContains(businessPageTitle);
            KSClick(checkoutButtonID);

            var placeOrderForm = _driver.FindElement(By.Id(placeOrderFormID));
            placeOrderForm.Submit();
            
            // Go back to orders page
            KSClick("backToOrders");

            bool result = _driver.Title.Contains(orderConfirmationPageTitle);

            // Assert
            Assert.IsTrue(result);
        }
        
        /// <summary>
        /// UC24 - Member View list of previous orders
        /// Tests to see if member can see list of orders they purchased previously 
        /// </summary>
        [Test]
        [Order(10)]
        public void UC24_ViewsListOfPreviousOrders_ShouldPass()
        {
            //Login as a member 
            UC28_Store_MemberCheckout_ShouldPass();

            //gets a list of orders to process
            IReadOnlyList<IWebElement> orders = _driver.FindElements(By.Id("allOrders"));

            //Assert
            Assert.IsTrue(orders.Count > 0);
        }
        
        /// <summary>
        /// UC25 - View Order Details
        /// Tests to see if a user can see the details of a order
        /// </summary>
        [Test]
        [Order(11)]
        public void UC25_ViewOrderDetails_ShouldPass()
        {
            
        }
        
        /// <summary>
        /// UC30 - Member cancels order
        /// </summary>
        [Test]
        [Order(12)]
        public void UC30_CancelOrder_ShouldPass()
        {

        }
        
        /// <summary>
        /// UC36 - Update Order status
        /// Tests to see if a business can update a order
        /// </summary>
        [Test]
        [Order(13)]
        public void UC36_UpdateOrderStatus_ShouldPass()
        {

        }
    }
}