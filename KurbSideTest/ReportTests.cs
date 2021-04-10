using System;
using System.Collections.Generic;
using NUnit.Framework;
using OpenQA.Selenium;

namespace KurbSideTest
{
    [TestFixture]
    class ReportTests : BaseTest
    {
        /// <summary>
        /// UC15 - Business Add Item
        /// Tests error message generation when adding an item with invalid details.
        /// </summary>
        [Test]
        [Order(1)]
        public void UC15_Items_AddItem_InvalidDetails_ShouldFail()
        {
            KSUnitTestLogin(AccountType.BUSINESS);
            
            
            
            
            Assert.AreEqual(addFailed, true);//checks to make sure that there are items in the list
        }
        
        
    }    
}
