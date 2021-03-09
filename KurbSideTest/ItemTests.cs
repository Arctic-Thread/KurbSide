using System;
using System.Collections.Generic;
using KurbSideTest;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace KurbSideTest
{
    [TestFixture]
    class ItemTests : BaseTest
    {
        private string randomString = RandomString(5) + "_";

        /// <summary>
        /// Test for adding a item to the business catalogue
        /// </summary>
        //[Order(1)]
        [Test]
        public void UC15_AddItem_ShouldPass()
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
        /// Edits the test item from the business test user
        /// For the test to run there needs to be a test item from UC15 in the website
        /// </summary>
        [Test]
        public void UC16_EditItem_ShouldPass()
        {
            string ItemNameTest = " Edit";
            string DetailTest = " Edit";
            string PriceTest = "5";
            string SkuTest = " Edit";
            string UpcTest = "";
            string CategoryTest = " Edit";

            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));

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

            var confirm_edit = _driver.SwitchTo().Alert();
            confirm_edit.Accept();

            IWebElement itemFounds = _driver.FindElement(By.XPath("//td[contains(.,'Test Item Pls Delete Edit')]"));

            Assert.AreEqual("Test Item Pls Delete Edit", itemFounds.Text);//checks to make sure that there are items in the list
        }
        /// <summary>
        /// This test deletes the test item from the test business
        /// </summary>
        [Test]
        public void UC17_DeleteItem_ShouldPass()
        {
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));

            KSUnitTestLogin(AccountType.BUSINESS);

            KSClick("navbar-catalogue"); //Clicks the catalogue button in the nav bar

            KSClick("catalogue-DeleteItem");

            var confirm_delete = _driver.SwitchTo().Alert();
            confirm_delete.Accept();

            IReadOnlyList<IWebElement> itemNames = _driver.FindElements(By.Id("catalogue-allItems"));//gets all the items and store them in a list

            Assert.IsTrue(itemNames.Count == 0);//checks to make sure that there are no items in the list
        }

        /// <summary>
        /// This test inputs invalid data into the item field and checks if the errors are caught
        /// </summary>
        [Test]
        public void UC15_AddItem_InvalidInfo_ShouldPass()
        {
            string ItemNameTest = "N";
            string PriceTest = "-89";
            string UpcTest = "10111";
            string CategoryTest = "a";

            string ItemErrrorMsg = "The entered Product Name is too short. A minimum of 2 characters is required.";
            string PriceErrorMsg = "The entered price is too low. The minimum price is $0.01.";
            string CategoryErrorMsg = "The entered Product Category is too short. A minimum of 2 characters is required.";
            string UpcErrorMsg = "The entered UPC/EAN is too short. 11 numbers min.";
            
            bool AddFailed = false;

            KSUnitTestLogin(AccountType.BUSINESS);

            KSClick("navbar-catalogue"); //Clicks the catalogue button in the nav bar

            KSClick("catalogue-AddItem");

            KSSendKeys("ItemName", ItemNameTest);
            KSSendKeys("Price", PriceTest);
            KSSendKeys("Category", CategoryTest);
            KSSendKeys("Upc", UpcTest);

            KSClick("catalogue-SubmitItem");

            string ItemMsg = _driver.FindElement(By.XPath("//span[contains(.,'The entered Product Name is too short. A minimum of 2 characters is required.')]")).Text;
            string PriceMsg = _driver.FindElement(By.XPath("(//span[contains(.,'The entered price is too low. The minimum price is $0.01.')])")).Text;
            string UpcMsg = _driver.FindElement(By.XPath("(//span[contains(.,'The entered UPC/EAN is too short. 11 numbers min.')])")).Text;
            string CategoryMsg = _driver.FindElement(By.XPath("(//span[contains(.,'The entered Product Category is too short. A minimum of 2 characters is required.')])")).Text;

            if (ItemMsg==ItemErrrorMsg && PriceMsg==PriceErrorMsg && UpcMsg == UpcErrorMsg && CategoryMsg==CategoryErrorMsg)
            {
                AddFailed = true;
            }

            Assert.AreEqual(AddFailed, true);//checks to make sure that there are items in the list
        }
        /// <summary>
        /// Test for inputing invalid info when you are editing a field
        /// </summary>
        [Test]
        public void UC15_EditItem_InvalidInfo_ShouldPass()
        {
            string ItemNameTest = "N";
            string PriceTest = "-89";
            string UpcTest = "10111";
            string CategoryTest = "a";

            string ItemErrrorMsg = "The entered Product Name is too short. A minimum of 2 characters is required.";
            string PriceErrorMsg = "The entered price is too low. The minimum price is $0.01.";
            string CategoryErrorMsg = "The entered Product Category is too short. A minimum of 2 characters is required.";
            string UpcErrorMsg = "The entered UPC/EAN is too short. 11 numbers min.";

            bool AddFailed = false;

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
            var confirm_edit = _driver.SwitchTo().Alert();
            confirm_edit.Accept();

            string ItemMsg = _driver.FindElement(By.XPath("//span[contains(.,'The entered Product Name is too short. A minimum of 2 characters is required.')]")).Text;
            string PriceMsg = _driver.FindElement(By.XPath("(//span[contains(.,'The entered price is too low. The minimum price is $0.01.')])")).Text;
            string UpcMsg = _driver.FindElement(By.XPath("(//span[contains(.,'The entered UPC/EAN is too short. 11 numbers min.')])")).Text;
            string CategoryMsg = _driver.FindElement(By.XPath("(//span[contains(.,'The entered Product Category is too short. A minimum of 2 characters is required.')])")).Text;

            if (ItemMsg == ItemErrrorMsg && PriceMsg == PriceErrorMsg && UpcMsg == UpcErrorMsg && CategoryMsg == CategoryErrorMsg)
            {
                AddFailed = true;
            }

            Assert.AreEqual(AddFailed, true);//checks to make sure that there are items in the list
        }
    }    
}
