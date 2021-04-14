using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using OpenQA.Selenium;

namespace KurbSideTest
{
    [TestFixture]
    class OrderTests : BaseTest
    {
        /// <summary>
        /// UC24 - Member View list of previous orders
        /// Tests to see if member can see list of orders they purchased previously 
        /// </summary>
        [Test]
        [Order(1)]
        public void UC24_ViewsListOfPreviousOrders_ShouldPass()
        {
            //Arrange
            //Titles
            string loginInPageTitle = "Stores - KurbSide";
            
            //Login as a member 
            KSUnitTestLogin(AccountType.MEMBER);
            
            //Go to orders page
            KSTitleContains(loginInPageTitle);
            KSClick("myOrders");
            
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
        [Order(2)]
        public void UC25_ViewOrderDetails_ShouldPass()
        {
            
        }
        
        /// <summary>
        /// UC30 - Member cancels order
        /// </summary>
        [Test]
        [Order(4)]
        public void UC30_CancelOrder_ShouldPass()
        {

        }
        
        /// <summary>
        /// UC36 - Update Order status
        /// Tests to see if a business can update a order
        /// </summary>
        [Test]
        [Order(3)]
        public void UC36_UpdateOrderStatus_ShouldPass()
        {

        }
        

        
        
    }    
}
