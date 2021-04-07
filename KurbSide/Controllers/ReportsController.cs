using KurbSide.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KurbSide.Service;
using KurbSide.Utilities;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Drawing;
using System.IO;
using System.Reflection.Metadata;
using Syncfusion.Pdf.Grid;


namespace KurbSide.Controllers
{
    /// <summary>
    /// Controller for Reports 
    /// </summary>
    [Authorize]
    public class ReportsController : Controller
    {
        private readonly KSContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<Order> _logger;

        public ReportsController(KSContext context, UserManager<IdentityUser> userManager, ILogger<Order> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        /// <summary>
        /// Shows all the reports you can have
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> ViewAllReportList()
        {
            //Check that the accessing user is a business type account
            var user = await KSCurrentUser.KSGetCurrentUserAsync(_userManager, HttpContext);
            var accountType = await KSCurrentUser.KSGetAccountType(_context, _userManager, HttpContext);

            //If the currently logged in user is not a business they can not access business controllers.
            if (accountType != KSCurrentUser.AccountType.BUSINESS)
            {
                TempData["sysMessage"] = "Error: You're not signed in as a business.";
                return RedirectToAction("Index", "Home");
            }

            var business = await _context.Business
                .Where(b => b.AspNetId.Equals(user.Id))
                .FirstOrDefaultAsync();

            return View();

        }
        
        /// <summary>
        /// Shows a report for all the items
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> ViewAllItemsReport()
        {
            //Check that the accessing user is a business type account
            var user = await KSCurrentUser.KSGetCurrentUserAsync(_userManager, HttpContext);
            var accountType = await KSCurrentUser.KSGetAccountType(_context, _userManager, HttpContext);

            //If the currently logged in user is not a business they can not access business controllers.
            if (accountType != KSCurrentUser.AccountType.BUSINESS)
            {
                TempData["sysMessage"] = "Error: You're not signed in as a business.";
                return RedirectToAction("Index", "Home");
            }

            var business = await _context.Business
                .Where(b => b.AspNetId.Equals(user.Id))
                .FirstOrDefaultAsync();

            TempData["businessId"] = business.BusinessId;
            
            //Gets all the items
            var items = await _context.Item
                .Where(b => b.BusinessId.Equals(business.BusinessId))
                .ToListAsync();

            
            
            return View(items);
        }
        
        /// <summary>
        /// Shows report for the items that have been removed from the catalogue 
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> ViewRemovedItemsReport()
        {
            //Check that the accessing user is a business type account
            var user = await KSCurrentUser.KSGetCurrentUserAsync(_userManager, HttpContext);
            var accountType = await KSCurrentUser.KSGetAccountType(_context, _userManager, HttpContext);

            //If the currently logged in user is not a business they can not access business controllers.
            if (accountType != KSCurrentUser.AccountType.BUSINESS)
            {
                TempData["sysMessage"] = "Error: You're not signed in as a business.";
                return RedirectToAction("Index", "Home");
            }

            var business = await _context.Business
                .Where(b => b.AspNetId.Equals(user.Id))
                .FirstOrDefaultAsync();

            //Gets removed items
            var items = await _context.Item
                .Where(b => b.BusinessId.Equals(business.BusinessId))
                .Where(i => i.Removed==true)
                .ToListAsync();
            
            return View(items);
        }
        
        /// <summary>
        /// shows report for the item that are currently for sale
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> ViewAvailableItemsReport()
        {
            //Check that the accessing user is a business type account
            var user = await KSCurrentUser.KSGetCurrentUserAsync(_userManager, HttpContext);
            var accountType = await KSCurrentUser.KSGetAccountType(_context, _userManager, HttpContext);

            //If the currently logged in user is not a business they can not access business controllers.
            if (accountType != KSCurrentUser.AccountType.BUSINESS)
            {
                TempData["sysMessage"] = "Error: You're not signed in as a business.";
                return RedirectToAction("Index", "Home");
            }

            var business = await _context.Business
                .Where(b => b.AspNetId.Equals(user.Id))
                .FirstOrDefaultAsync();

            //Get for sale items
            var items = await _context.Item
                .Where(b => b.BusinessId.Equals(business.BusinessId))
                .Where(i => i.Removed==false)
                .ToListAsync();
            
            return View(items);
        }
        
        /// <summary>
        /// The function for downloading report Pdf's
        /// </summary>
        /// <param name="pdfName"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<FileResult> CreateItemsReportsPdf(string pdfName, string name, Guid businessId)
        {
            MemoryStream workStream = new MemoryStream();
            DateTime currentDate= DateTime.Now;
            
            //Create a new document
            PdfDocument itemsReportDocument = new PdfDocument();
            
            //Adding a page to the itemsReportDocument 
            PdfPage page = itemsReportDocument.Pages.Add();
            
            //Pdf Graphics for the page
            PdfGraphics graphics = page.Graphics;
            
            //Pdf grid for the table
            PdfGrid pdfGrid = new PdfGrid();
            
            //Set the standard font
            PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 20);
            
            var itemsList =  await _context.Item
                .Where(b => b.BusinessId.Equals(businessId))
                .Select(i=> new{i.ItemName, i.Price,i.Category,i.Sku,i.Upc,i.Removed })
                .ToListAsync();

            //Make the list to IEnumerable
            IEnumerable<object> itemTable = itemsList;
            
            //Assign to data source
            pdfGrid.DataSource = itemTable;
            
            //Draw the table to a pdf page
            pdfGrid.Draw(page, new Syncfusion.Drawing.PointF(10, 10));
            
            //Saves the Pdf to the stream
            itemsReportDocument.Save(workStream);
            workStream.Position = 0;
            
            //close the stream
            itemsReportDocument.Close(true);
            
            //Content type for pdf as well as file name
            string type = "application/pdf";
            string fileName = "All Items.pdf";

            return File(workStream, type, fileName);
        }
        

    }
}