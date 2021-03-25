using System;
using System.Collections.Generic;
using NUnit.Framework;
using OpenQA.Selenium;

namespace KurbSideTest
{
    [TestFixture]
    class ItemTests : BaseTest
    {
        /// <summary>
        /// UC15 - Business Add Item
        /// Tests error message generation when adding an item with invalid details.
        /// </summary>
        [Test]
        [Order(1)]
        public void UC15_Items_AddItem_InvalidDetails_ShouldFail()
        {
            string ItemNameTest = "N";
            string PriceTest = "-89";
            string UpcTest = "10111";
            string CategoryTest = "a";

            string ItemErrrorMsg = "The entered Product Name is too short. A minimum of 2 characters is required.";
            string PriceErrorMsg = "The entered price is too low. The minimum price is $0.01.";
            string CategoryErrorMsg = "The entered Product Category is too short. A minimum of 2 characters is required.";
            string UpcErrorMsg = "The entered UPC/EAN is too short. 11 numbers min.";
            
            bool addFailed = false;

            KSUnitTestLogin(AccountType.BUSINESS);

            KSClick("navbar-catalogue"); //Clicks the catalogue button in the nav bar

            KSClick("catalogue-AddItem");

            KSSendKeys("ItemName", ItemNameTest);
            KSSendKeys("Price", PriceTest);
            KSSendKeys("Category", CategoryTest);
            KSSendKeys("Upc", UpcTest);

            KSClick("catalogue-SubmitItem");

            string itemMsg = _driver.FindElement(By.XPath("//span[contains(.,'The entered Product Name is too short. A minimum of 2 characters is required.')]")).Text;
            string priceMsg = _driver.FindElement(By.XPath("(//span[contains(.,'The entered price is too low. The minimum price is $0.01.')])")).Text;
            string upcMsg = _driver.FindElement(By.XPath("(//span[contains(.,'The entered UPC/EAN is too short. 11 numbers min.')])")).Text;
            string categoryMsg = _driver.FindElement(By.XPath("(//span[contains(.,'The entered Product Category is too short. A minimum of 2 characters is required.')])")).Text;

            if (itemMsg==ItemErrrorMsg && priceMsg==PriceErrorMsg && upcMsg == UpcErrorMsg && categoryMsg==CategoryErrorMsg)
            {
                addFailed = true;
            }

            Assert.AreEqual(addFailed, true);//checks to make sure that there are items in the list
        }
        
        /// <summary>
        /// UC15 - Business Add Item
        /// Tests adding an item with valid details to the business catalogue
        /// </summary>
        [Test]
        [Order(2)]
        public void UC15_Items_AddItem_ValidDetails_ShouldPass()
        {
            string ItemNameTest = "Test Item Pls Delete";
            string DetailTest = "This item is to be deleted";
            string PriceTest = "75";
            string SkuTest = "TestSKU";
            string UpcTest = "012345678905";
            string CategoryTest = "This is a test category";

            KSUnitTestLogin(AccountType.BUSINESS);

            KSClick("navbar-catalogue"); //Clicks the catalogue button in the nav bar

            KSClick("catalogue-AddItem");

            KSSendKeys("ItemName", ItemNameTest);
            KSSendKeys("Price", PriceTest);
            KSSendKeys("Details", DetailTest);
            KSSendKeys("Category", CategoryTest);
            KSSendKeys("Sku", SkuTest);
            KSSendKeys("Upc", UpcTest);

            KSClick("catalogue-SubmitItem");

            IReadOnlyList<IWebElement> itemNames = _driver.FindElements(By.Id("catalogue-allItems"));//gets all the items and store them in a list

            Assert.IsTrue(itemNames.Count > 0);//checks to make sure that there are items in the list
        }
        
        
        /// <summary>
        /// UC16 - Business Edit Item
        /// Tests error message generation when editing an item with invalid details.
        /// </summary>
        [Test]
        [Order(3)]
        public void UC16_Items_EditItem_InvalidDetails_ShouldFail()
        {
            string ItemNameTest = "N";
            string PriceTest = "-89";
            string UpcTest = "10111";
            string CategoryTest = "a";

            string ItemErrrorMsg = "The entered Product Name is too short. A minimum of 2 characters is required.";
            string PriceErrorMsg = "The entered price is too low. The minimum price is $0.01.";
            string CategoryErrorMsg = "The entered Product Category is too short. A minimum of 2 characters is required.";
            string UpcErrorMsg = "The entered UPC/EAN is too short. 11 numbers min.";

            bool addFailed = false;

            KSUnitTestLogin(AccountType.BUSINESS);

            KSClick("navbar-catalogue"); //Clicks the catalogue button in the nav bar

            KSClick("catalogue-EditItem");

            _driver.FindElement(By.Id("ItemName")).Clear();
            _driver.FindElement(By.Id("Price")).Clear();
            _driver.FindElement(By.Id("Category")).Clear();
            _driver.FindElement(By.Id("Upc")).Clear();

            KSSendKeys("ItemName", ItemNameTest);
            KSSendKeys("Price", PriceTest);
            KSSendKeys("Category", CategoryTest);
            KSSendKeys("Upc", UpcTest);

            KSClick("catalogue-saveItem");
            var confirmEdit = _driver.SwitchTo().Alert();
            confirmEdit.Accept();

            string itemMsg = _driver.FindElement(By.XPath("//span[contains(.,'The entered Product Name is too short. A minimum of 2 characters is required.')]")).Text;
            string priceMsg = _driver.FindElement(By.XPath("(//span[contains(.,'The entered price is too low. The minimum price is $0.01.')])")).Text;
            string upcMsg = _driver.FindElement(By.XPath("(//span[contains(.,'The entered UPC/EAN is too short. 11 numbers min.')])")).Text;
            string categoryMsg = _driver.FindElement(By.XPath("(//span[contains(.,'The entered Product Category is too short. A minimum of 2 characters is required.')])")).Text;

            if (itemMsg == ItemErrrorMsg && priceMsg == PriceErrorMsg && upcMsg == UpcErrorMsg && categoryMsg == CategoryErrorMsg)
            {
                addFailed = true;
            }

            Assert.AreEqual(addFailed, true);//checks to make sure that there are items in the list
        }
        
        /// <summary>
        /// UC16 - Business Edit Item
        /// Tests saving a business catalogue item with valid details.
        /// </summary>
        [Test]
        [Order(4)]
        public void UC16_Items_EditItem_ShouldPass()
        {
            // Arrange
            // Input
            string ItemNameTest = " Edit";
            string DetailTest = " Edit";
            string PriceTest = "5";
            string SkuTest = " Edit";
            string UpcTest = "";
            string CategoryTest = " Edit";
            
            // Titles
            string cataloguePageTitle = "Business Catalogue - KurbSide";

            KSUnitTestLogin(AccountType.BUSINESS);

            KSClick("navbar-catalogue"); //Clicks the catalogue button in the nav bar

            KSClick("catalogue-EditItem");

            KSSendKeys("ItemName", ItemNameTest);
            KSSendKeys("Price", PriceTest);
            KSSendKeys("Details", DetailTest);
            KSSendKeys("Category", CategoryTest);
            KSSendKeys("Sku", SkuTest);
            KSSendKeys("Upc", UpcTest);

            KSClick("catalogue-saveItem");

            var confirmEdit = _driver.SwitchTo().Alert();
            confirmEdit.Accept();
            
            KSTitleContains(cataloguePageTitle);

            IReadOnlyList<IWebElement> itemNames = _driver.FindElements(By.Id("catalogue-allItems"));//gets all the items and store them in a list

            Assert.IsTrue(itemNames.Count == 1);//checks to make sure that there are no items in the list
        }
        
        /// <summary>
        /// UC17 - Business Remove Item
        /// Tests the deletion of an item from a business catalogue. test deletes the test item from the test business.
        /// </summary>
        [Test]
        [Order(5)]
        public void UC17_Items_DeleteItem_ShouldPass()
        {
            KSUnitTestLogin(AccountType.BUSINESS);
            KSClick("navbar-catalogue"); //Clicks the catalogue button in the nav bar
            KSClick("catalogue-DeleteItem");

            var confirmDelete = _driver.SwitchTo().Alert();
            confirmDelete.Accept();

            IReadOnlyList<IWebElement> itemNames = _driver.FindElements(By.Id("catalogue-allItems"));//gets all the items and store them in a list

            Assert.IsTrue(itemNames.Count == 0);//checks to make sure that there are no items in the list
        }
    }    
}
