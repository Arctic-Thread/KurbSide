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
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ExcelDataReader;
using KurbSide.Service;
using KurbSide.Utilities;
using KurbSideUtils;

namespace KurbSide.Controllers
{
    /// <summary>
    /// If the user trying to view any business actions is not logged in, they are redirected to the login page.
    /// If the currently logged in user is not a business they are redirected to the store front.
    /// </summary>
    [Authorize]
    public class BusinessController : Controller
    {
        private readonly KSContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<Business> _logger;

        public BusinessController(KSContext context, UserManager<IdentityUser> userManager, ILogger<Business> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        #region BusinessRU

        /// <summary>
        /// Displays the business dashboard.
        /// </summary>
        /// <returns>A redirect to the index page.</returns>
        [HttpGet]
        public async Task<IActionResult> IndexAsync()
        {
            //Check that the accessing user is a business type account
            var accountType = await KSUserUtilities.KSGetAccountType(_context, _userManager, HttpContext);

            //If the currently logged in user is not a business they can not access business controllers.
            if (accountType != KSUserUtilities.AccountType.BUSINESS)
            {
                TempData["sysMessage"] = "Error: You're not signed in as a business.";
                return RedirectToAction("Index", "Home");
            }

            var business = await KSUserUtilities.KSGetCurrentBusinessAsync(_context, _userManager, HttpContext);

            return View(business);
        }

        /// <summary>
        /// Displays the edit business information page.
        /// </summary>
        /// <returns>A redirect to the edit business page.</returns>
        [HttpGet]
        public async Task<IActionResult> EditBusiness()
        {
            //Check that the accessing user is a business type account
            var accountType = await KSUserUtilities.KSGetAccountType(_context, _userManager, HttpContext);

            //If the currently logged in user is not a business they can not access business controllers.
            if (accountType != KSUserUtilities.AccountType.BUSINESS)
            {
                TempData["sysMessage"] = "Error: You're not signed in as a business.";
                return RedirectToAction("Index", "Home");
            }

            var business = await KSUserUtilities.KSGetCurrentBusinessAsync(_context, _userManager, HttpContext);

            ViewData["CountryCode"] = new SelectList(_context.Country, "CountryCode", "FullName", business.ProvinceCode);
            ViewData["ProvinceCode"] = new SelectList(_context.Province, "ProvinceCode", "FullName", business.ProvinceCode);

            return View(business);
        }

        /// <summary>
        /// Upon submission of the EditBusiness page, if the entered information is valid,
        /// the businesses information is updated in the database.
        /// </summary>
        /// <param name="id">The ID of the <see cref="Business"/> to be updated.</param>
        /// <param name="business">The new <see cref="Business"/> information.</param>
        /// <param name="businessLogo">The businesses logo.</param>
        /// <returns>If successful A redirect to the business dashboard, otherwise the index.</returns>
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> EditBusiness(Guid id, Business business, IFormFile businessLogo)
        {
            //Check that the accessing user is a business type account
            var accountType = await KSUserUtilities.KSGetAccountType(_context, _userManager, HttpContext);

            //If the currently logged in user is not a business they can not access business controllers.
            if (accountType != KSUserUtilities.AccountType.BUSINESS)
            {
                TempData["sysMessage"] = "Error: You're not signed in as a business.";
                return RedirectToAction("Index", "Home");
            }

            //Check that the business being edited is the signed in business
            if (id != business.BusinessId)
            {
                _logger.LogDebug("ID Mismatch. Edit not performed.");
                return RedirectToAction("Index");
            }

            try
            {
                if (ModelState.IsValid)
                {
                    if(businessLogo != null) // If the business has added an image, it is uploaded to imgur and the link is prepped to be saved to the DB
                    {
                        string uploadResults = await KSImgur.KSUploadImageToImgur(businessLogo);

                        //If the upload results from imgur do not start with "Error: " the link is saved.
                        if (!uploadResults.StartsWith("Error: "))
                        {
                            business.LogoLocation = uploadResults;
                        }
                        //If the upload failed, the previous image is used instead.
                        else
                        {
                            var existingBusiness = await _context.Business.AsNoTracking().Where(b => b.BusinessId == business.BusinessId).FirstOrDefaultAsync();
                            string existingImage = existingBusiness.LogoLocation;
                            business.LogoLocation = existingImage;
                            TempData["sysMessage"] = uploadResults + ", Logo not changed";
                        }
                    }
                    else // If they are not adding a new image, it uses the pre-existing image.
                    {
                        var existingBusiness = await _context.Business.AsNoTracking().Where(b => b.BusinessId == business.BusinessId).FirstOrDefaultAsync();
                        string existingImage = existingBusiness.LogoLocation;
                        business.LogoLocation = existingImage;
                    }
                    
                    string address = $"{business.Street} {business.City} {business.ProvinceCode} {business.CountryCode} {business.Postal}";
                    Location location = await GeoCode.GetLocationAsync(address);

                    business.Lng = location.lng;
                    business.Lat = location.lat;

                    _context.Update(business);
                    await _context.SaveChangesAsync();
                    _logger.LogDebug($"Edit succeeded. {business.BusinessId}");
                    return RedirectToAction("Index");
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                _logger.LogError("Business does not exist, Update Failed.");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.GetBaseException().Message}. Edit not performed.");
                return RedirectToAction("Index");
            }

            ViewData["CountryCode"] = new SelectList(_context.Country, "CountryCode", "FullName", business.ProvinceCode);
            ViewData["ProvinceCode"] = new SelectList(_context.Province, "ProvinceCode", "FullName", business.ProvinceCode);
            return View(business);
        }
        #endregion

        #region ItemCRUD

        /// <summary>
        /// Displays the item catalogue page, listing all of the businesses items.
        /// </summary>
        /// <param name="filter">The query entered by the business, e.g. item name or category.</param>
        /// <param name="page">Pagination: The page number the business would like to view.</param>
        /// <param name="perPage">Pagination: The number of items the business would like to see per page.</param>
        /// <returns> A redirect to the business catalogue page.</returns>
        [HttpGet]
        public async Task<IActionResult> Catalogue(string filter = "", int page = 1, int perPage = 5)
        {
            //Check that the accessing user is a business type account
            var accountType = await KSUserUtilities.KSGetAccountType(_context, _userManager, HttpContext);

            //If the currently logged in user is not a business they can not access business controllers.
            if (accountType != KSUserUtilities.AccountType.BUSINESS)
            {
                TempData["sysMessage"] = "Error: You're not signed in as a business.";
                return RedirectToAction("Index", "Home");
            }

            var business = await KSUserUtilities.KSGetCurrentBusinessAsync(_context, _userManager, HttpContext);

            //Retrieve items associated with the editing business
            //  skip items that are marked as removed
            //  TODO separate items to a separate list somewhere to be re-added
            var items = await _context.Item
                .Where(i => i.BusinessId.Equals(business.BusinessId))
                .Include(i => i.Business)
                .Where(i => i.Removed == false)
                .OrderByDescending(i => i.Category)
                .ToListAsync();

            //Retrieve the user-defined categories
            var categories = items
                .Select(i => i.Category)
                .Distinct()
                .ToList();

            //Filtering functions
            if (!string.IsNullOrEmpty(filter))
            {
                TempData["catalogueFilter"] = filter;
                //sort by category if the search term/filter
                //  is contained in the categories list
                if (categories.Contains(filter))
                {
                    items = items
                        .Where(i => i.Category.Equals(filter))
                        .ToList();
                }
                //sort by name if the search term/filter
                //  is not contained in the categories list
                else
                {
                    items = items
                        .Where(i => i.ItemName.ToLower().Contains(filter.ToLower()))
                        .ToList();
                }
            }

            //Create the paginated list for return
            var paginatedList = KSPaginatedList<Item>.Create(items.AsQueryable(), page, perPage);

            //Gather temp data and pagination/filter info
            //  all in to one place for use 
            TempData["itemCategories"] = categories;
            TempData["currentPage"] = page;
            TempData["totalPage"] = paginatedList.TotalPages;
            TempData["perPage"] = perPage;
            TempData["hasNextPage"] = paginatedList.HasNextPage;
            TempData["hasPrevPage"] = paginatedList.HasPreviousPage;

            return View(paginatedList);
        }

        /// <summary>
        /// Displays the add item page.
        /// </summary>
        /// <returns>A redirect to the add item page.</returns>
        [HttpGet]
        public async Task<IActionResult> AddItem()
        {
            //Check that the accessing user is a business type account
            var accountType = await KSUserUtilities.KSGetAccountType(_context, _userManager, HttpContext);

            //If the currently logged in user is not a business they can not access business controllers.
            if (accountType != KSUserUtilities.AccountType.BUSINESS)
            {
                TempData["sysMessage"] = "Error: You're not signed in as a business.";
                return RedirectToAction("Index", "Home");
            }

            var business = await KSUserUtilities.KSGetCurrentBusinessAsync(_context, _userManager, HttpContext);

            var blankItem = new Item
            {
                BusinessId = business.BusinessId,
                Business = business
            };

            return View(blankItem);
        }

        /// <summary>
        /// Upon submission of the AddItem page, if the entered information is valid,
        /// the <see cref="Item"/> is created and saved to the database.
        /// </summary>
        /// <param name="item">The new <see cref="Item"/>s information.</param>
        /// <param name="itemImage">The image for the new item.</param>
        /// <returns>If successful A redirect to the businesses catalogue, otherwise the index.</returns>
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> AddItem(Item item, IFormFile itemImage)
        {
            //Check that the accessing user is a business type account
            var accountType = await KSUserUtilities.KSGetAccountType(_context, _userManager, HttpContext);

            //If the currently logged in user is not a business they can not access business controllers.
            if (accountType != KSUserUtilities.AccountType.BUSINESS)
            {
                TempData["sysMessage"] = "Error: You're not signed in as a business.";
                return RedirectToAction("Index", "Home");
            }

            var business = await KSUserUtilities.KSGetCurrentBusinessAsync(_context, _userManager, HttpContext);

            //Ensure that the business can only add items for themselves
            if (item.BusinessId != business.BusinessId)
            {
                _logger.LogDebug("Debug: Id Mismatch. Edit not performed.");
                return RedirectToAction("Index");
            }

            try
            {
                if (ModelState.IsValid)
                {
                    if(itemImage != null) // If the business has added an image, it is uploaded to imgur and the link is prepped to be saved to the DB
                    {
                        string linkToImage = await KSImgur.KSUploadImageToImgur(itemImage);
                        if(!linkToImage.StartsWith("Error: "))
                        {
                            item.ImageLocation = linkToImage;
                        }
                        else
                        {
                            TempData["sysMessage"] = linkToImage + ", Image not added";
                        }
                    }

                    await _context.Item.AddAsync(item);
                    await _context.SaveChangesAsync();
                    _logger.LogDebug($"Debug: Add succeeded. {item.ItemId}");
                    return RedirectToAction("Catalogue");
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                _logger.LogError("Error: Business does not exist, Add Failed.");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.GetBaseException().Message}. Add not performed.");
                return RedirectToAction("Index");
            }
            return View(item);
        }

        /// <summary>
        /// Displays the edit item information page.
        /// </summary>
        /// <param name="id">The ID of the <see cref="Item"/> to be edited.</param>
        /// <returns>A redirect to the edit item page for the selected item.</returns>
        [HttpGet]
        public async Task<IActionResult> EditItem(Guid id)
        {
            //Check that the accessing user is a business type account
            var accountType = await KSUserUtilities.KSGetAccountType(_context, _userManager, HttpContext);

            //If the currently logged in user is not a business they can not access business controllers.
            if (accountType != KSUserUtilities.AccountType.BUSINESS)
            {
                TempData["sysMessage"] = "Error: You're not signed in as a business.";
                return RedirectToAction("Index", "Home");
            }

            var business = await KSUserUtilities.KSGetCurrentBusinessAsync(_context, _userManager, HttpContext);

            var item = await KSCatalogueUtilities.GetSpecificItem(_context, business, id);

            if (item == null)
            {
                return RedirectToAction("Index");
            }

            return View(item);
        }
        
        /// <summary>
        /// Upon submission of the EditItem page, if the entered information is valid,
        /// the <see cref="Item"/>s information is updated in the database.
        /// </summary>
        /// <param name="item">The new <see cref="Item"/>s information.</param>
        /// <param name="itemImageEdit">The image for the new item.</param>
        /// <returns>If successful A redirect to the businesses catalogue, otherwise the index.</returns>
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> EditItem(Item item, IFormFile itemImageEdit)
        {
            //Check that the accessing user is a business type account
            var accountType = await KSUserUtilities.KSGetAccountType(_context, _userManager, HttpContext);

            //If the currently logged in user is not a business they can not access business controllers.
            if (accountType != KSUserUtilities.AccountType.BUSINESS)
            {
                TempData["sysMessage"] = "Error: You're not signed in as a business.";
                return RedirectToAction("Index", "Home");
            }

            var business = await KSUserUtilities.KSGetCurrentBusinessAsync(_context, _userManager, HttpContext);

            //Ensure that the business can only edit items for themselves
            if (business.BusinessId != item.BusinessId)
            {
                _logger.LogDebug("Debug: Id Mismatch. Update not performed.");
                return RedirectToAction("Index");
            }

            try
            {
                if (ModelState.IsValid)
                {
                    if(itemImageEdit != null) // If the business has added an image, it is uploaded to imgur and the link is prepped to be saved to the DB
                    {
                        var uploadResults = await KSImgur.KSUploadImageToImgur(itemImageEdit);
                        if (!uploadResults.StartsWith("Error: "))
                        {
                            item.ImageLocation = uploadResults;
                        }
                        else
                        {
                            var existingItem = await _context.Item.AsNoTracking().Where(i => i.ItemId == item.ItemId).FirstOrDefaultAsync();
                            string existingImage = existingItem.ImageLocation;
                            item.ImageLocation = existingImage;
                            TempData["sysMessage"] = uploadResults + ", Image not changed";
                        }
                    }
                    else // If they are not adding a new image, it uses the pre-existing image.
                    {
                        var existingItem = await _context.Item.AsNoTracking().Where(i => i.ItemId == item.ItemId).FirstOrDefaultAsync();
                        var existingImage = existingItem.ImageLocation;
                        item.ImageLocation = existingImage;
                    }

                    _context.Item.Update(item);
                    await _context.SaveChangesAsync();
                    _logger.LogDebug($"Debug: Update succeeded. {item.ItemId}");
                    return RedirectToAction("Catalogue");
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                _logger.LogError($"Error: Business does not exist, Update Failed.");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.GetBaseException().Message}. Update not performed.");
                return RedirectToAction("Index");
            }

            return View(item);
        }

        /// <summary>
        /// Removes the item from the businesses catalogue.
        /// Note: The <see cref="Item"/> is not deleted from the database, but it can no longer be purchased anymore.
        /// </summary>
        /// <param name="id">The ID of the item to be removed.</param>
        /// <param name="filter">The query entered by the business, e.g. item name or category.</param>
        /// <param name="page">Pagination: The page number the business would like to view.</param>
        /// <param name="perPage">Pagination: The number of items the business would like to see per page.</param>
        /// <returns>If successful A redirect to the business catalogue, otherwise the index.</returns>
        public async Task<IActionResult> RemoveItem(Guid id, string filter = "", int page = 1, int perPage = 5)
        {
            //Check that the accessing user is a business type account
            var accountType = await KSUserUtilities.KSGetAccountType(_context, _userManager, HttpContext);

            //If the currently logged in user is not a business they can not access business controllers.
            if (accountType != KSUserUtilities.AccountType.BUSINESS)
            {
                TempData["sysMessage"] = "Error: You're not signed in as a business.";
                return RedirectToAction("Index", "Home");
            }

            var business = await KSUserUtilities.KSGetCurrentBusinessAsync(_context, _userManager, HttpContext);

            var item = await KSCatalogueUtilities.GetSpecificItem(_context, business, id);

            if (item != null)
            {
                try
                {
                    item.Removed = true;
                    _context.Item.Update(item);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    TempData["sysMessage"] = $"Error: Business does not exist, Add Failed.";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    //TODO even more debug messages!
                    TempData["sysMessage"] = $"Error: {ex.GetBaseException().Message}. Add not performed.";
                    return RedirectToAction("Index");
                }
            }

            return RedirectToAction("Catalogue", new {filter, page, perPage});
        }
        
        /// <summary>
        /// Displays the Import tool page.
        /// </summary>
        public async Task<IActionResult> ViewImportItems()
        {
            //Check that the accessing user is a business type account
            var accountType = await KSUserUtilities.KSGetAccountType(_context, _userManager, HttpContext);

            //If the currently logged in user is not a business they can not access business controllers.
            if (accountType != KSUserUtilities.AccountType.BUSINESS)
            {
                TempData["sysMessage"] = "Error: You're not signed in as a business.";
                return RedirectToAction("Index", "Home");
            }
            
            return View();
        }

        /// <summary>
        /// Downloads the Import tool template.
        /// </summary>
        public async Task<FileResult> DownloadTemplate()
        {
            string filePath = "wwwroot/misc/KurbSide Import.xlsx";
            string fileName = "KurbSide Import.xlsx";

            byte[] fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);

            return File(fileBytes, "application/force-download", fileName);
        }

        /// <summary>
        /// Accepts a .xlsx or .xls file for importing items.
        /// </summary>
        /// <param name="itemImport">The .xlsx or .xls template file from the business.</param>
        public async Task<IActionResult> ImportItems(IFormFile itemImport)
        {
            var business = await KSUserUtilities.KSGetCurrentBusinessAsync(_context, _userManager, HttpContext);
            
            //Check that the accessing user is a business type account
            var accountType = await KSUserUtilities.KSGetAccountType(_context, _userManager, HttpContext);

            //If the currently logged in user is not a business they can not access business controllers.
            if (accountType != KSUserUtilities.AccountType.BUSINESS)
            {
                TempData["sysMessage"] = "Error: You're not signed in as a business.";
                return RedirectToAction("Index", "Home");
            }

            List<string> validFileExtensions = new List<string>
            {
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "application/vnd.ms-excel"
            };
            
            // If the file extension is not valid (not in validFileExtensions)
            if (!validFileExtensions.Contains(itemImport.ContentType))
            {
                TempData["sysMessage"] = "Error: Invalid file type.";
                return RedirectToAction("ViewImportItems", "Business");
            }
            
            // If File size is greater than 20MB
            if (itemImport.Length > 20971520)
            {
                TempData["sysMessage"] = "Error: File size is too large.";
                return RedirectToAction("ViewImportItems", "Business");
            }

            try
            {
                await using var memoryStream = new MemoryStream();
                
                // Copy file to memory.
                await itemImport.CopyToAsync(memoryStream);

                // Create Excel file reader
                using var excelDataReader = ExcelReaderFactory.CreateReader(memoryStream);
                
                // Ignore headers
                var excelDataSetConfiguration = new ExcelDataSetConfiguration
                {
                    ConfigureDataTable = a => new ExcelDataTableConfiguration
                    {
                        UseHeaderRow = true
                    }
                };

                // Create the dataset
                DataSet dataSet = excelDataReader.AsDataSet(excelDataSetConfiguration);

                // Get all rows from Sheet1
                DataRowCollection row = dataSet.Tables["Sheet1"].Rows;
                
                foreach (DataRow tempRow in row)
                {
                    // convert DataRow tempRow to List a list of strings
                    var rowValues = tempRow.ItemArray.ToList().Select(s => s.ToString()).ToList();

                    string itemName = rowValues[0].KSTitleCase();
                    string category = rowValues[1].KSTitleCase();
                    string stringPrice = rowValues[2];
                    
                    string sku = rowValues[3].KSRemoveWhitespace().Length <= 0
                        ? null
                        : rowValues[3].KSRemoveWhitespace();

                    string upc = rowValues[4].KSRemoveWhitespace().Length <= 0
                        ? null
                        : rowValues[4].KSRemoveWhitespace();

                    string details = rowValues[5];
                    
                    string imageLocation = rowValues[6].KSRemoveWhitespace().Length <= 0
                        ? null
                        : rowValues[6].KSRemoveWhitespace();
                    
                    if (itemName.Length < 2)
                        throw new Exception();

                    if (category.Length < 2)
                        throw new Exception();

                    if (decimal.Parse(rowValues[2]) < 0.01m)
                        throw new Exception();

                    Item item = new Item
                    {
                        BusinessId = business.BusinessId,
                        ItemName = itemName,                                
                        Category = category,                                
                        Price = decimal.Parse(stringPrice),
                        Sku = sku,
                        Upc = upc,
                        Details = details,
                        ImageLocation = imageLocation
                    };

                    await _context.AddAsync(item);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                TempData["sysMessage"] = "Error: Something went wrong while importing your items.";
            }

            return RedirectToAction("Catalogue");
        }

        #endregion
        
        #region Orders

        public async Task<IActionResult> Orders(string filter = "", int page = 1, int perPage = 5)
        {
            //Check that the accessing user is a business type account
            var user = await KSUserUtilities.KSGetCurrentUserAsync(_userManager, HttpContext);
            var accountType = await KSUserUtilities.KSGetAccountType(_context, _userManager, HttpContext);

            //If the currently logged in user is not a business they can not access business controllers.
            if (accountType != KSUserUtilities.AccountType.BUSINESS)
            {
                TempData["sysMessage"] = "Error: You're not signed in as a business.";
                return RedirectToAction("Index", "Home");
            }

            var business = await _context.Business
                .Where(b => b.AspNetId.Equals(user.Id))
                .FirstOrDefaultAsync();

            var orders = await _context.Order
                .Where(o => o.Business.BusinessId.Equals(business.BusinessId))
                .Include(o => o.Member)
                .Include(o => o.StatusNavigation)
                .OrderBy(o => o.Status)
                .ToListAsync();
            
            //Retrieve the existing status(es) that are relevant
            //to the current business
            var status = orders
                .Select(o => o.StatusNavigation.StatusName.ToUpper().Trim())
                .Distinct()
                .ToList();

            //Filtering functions
            if (!string.IsNullOrEmpty(filter))
            {
                filter = filter.Trim();
                TempData["filter"] = filter;
                //sort by category if the search term/filter
                //  is contained in the categories list
                if (status.Contains(filter.ToUpper()))
                {
                    orders = orders
                        .Where(o => o.StatusNavigation.StatusName.ToUpper().Equals(filter))
                        .ToList();
                }
                //try filtering orders in the following order
                // 1. OrderId
                // 2. FirstName
                // 3. LastName
                //HACK: this is a ham-fisted way to do this, but it is unlikely
                // that any user will be named 22BBF0/etc so there should be very little
                // collision.
                else
                {
                    orders = orders
                        .Where
                            (o =>
                            o.OrderId.ToString().ToUpper().Contains(filter.ToUpper()) ||
                            o.Member.FirstName.ToUpper().Contains(filter.ToUpper()) ||
                            o.Member.LastName.ToUpper().Contains(filter.ToUpper()))
                        .ToList();
                    
                    if (orders.Count() == 1)
                    {
                        //this seems like an excellent idea :)
                        TempData["sysMessage"] = $"Only one order found for {filter}, redirecting to order.";
                        return RedirectToAction("ViewOrder","Order", new {id = orders.First().OrderId});
                    }
                }
            }
            
            var paginatedList = KSPaginatedList<Order>.Create(orders.AsQueryable(), page, perPage);

            //Gather temp data and pagination/filter info
            //  all in to one place for use 
            TempData["status"] = status;
            TempData["statusFull"] = await _context.OrderStatus.ToListAsync();
            TempData["currentPage"] = page;
            TempData["totalPage"] = paginatedList.TotalPages;
            TempData["perPage"] = perPage;
            TempData["hasNextPage"] = paginatedList.HasNextPage;
            TempData["hasPrevPage"] = paginatedList.HasPreviousPage;
            
            return View("Orders/Index", paginatedList);
        }
        #endregion

        #region Business Hours

        /// <summary>
        /// Displays the edit business hours page.
        /// </summary>
        /// <returns>A redirect to the edit business hours page.</returns>
        [HttpGet]
        public async Task<IActionResult> EditBusinessHours()
        {
            //Check that the accessing user is a business type account
            var accountType = await KSUserUtilities.KSGetAccountType(_context, _userManager, HttpContext);

            //If the currently logged in user is not a business they can not access business controllers.
            if (accountType != KSUserUtilities.AccountType.BUSINESS)
            {
                TempData["sysMessage"] = "Error: You're not signed in as a business.";
                return RedirectToAction("Index", "Home");
            }

            var business = await KSUserUtilities.KSGetCurrentBusinessAsync(_context, _userManager, HttpContext);

            //Get the businesses business hours, if no businesses hours are found, display a blank form.
            var businessHours = await _context.BusinessHours
                .FirstOrDefaultAsync(b => b.BusinessId == business.BusinessId) ?? new BusinessHours();
            
            return View(businessHours);
        }

        /// <summary>
        /// Upon submission of the EditBusinessHours page, if the entered information is valid,
        /// the businesses <see cref="BusinessHours"/> are updated in the database.
        /// </summary>
        /// <param name="id">The ID of the business.</param>
        /// <param name="businessHours">The business hour information.</param>
        /// <returns>If successful A redirect to the business dashboard, otherwise the index,</returns>
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> EditBusinessHours(Guid id, BusinessHours businessHours)
        {
            //Check that the accessing user is a business type account
            var accountType = await KSUserUtilities.KSGetAccountType(_context, _userManager, HttpContext);

            //If the currently logged in user is not a business they can not access business controllers.
            if (accountType != KSUserUtilities.AccountType.BUSINESS)
            {
                TempData["sysMessage"] = "Error: You're not signed in as a business.";
                return RedirectToAction("Index", "Home");
            }

            if (id != businessHours.BusinessId)
            {
                //TODO Remove Debug messages
                TempData["sysMessage"] = $"Debug: Id Mismatch. Edit not performed.";
                return RedirectToAction("Index");
            }

            try
            {
                if (ModelState.IsValid)
                {
                    _context.Update(businessHours);
                    await _context.SaveChangesAsync();

                    //TODO Remove Debug messages
                    TempData["sysMessage"] = $"Debug: Business Hours Edit succeeded. {businessHours.BusinessId}";
                    return RedirectToAction("Index");
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                TempData["sysMessage"] = $"Error: Business does not exist, Add Failed.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                //TODO even more debug messages!
                TempData["sysMessage"] = $"Error: {ex.GetBaseException().Message}. Add not performed.";
                return RedirectToAction("Index");
            }
            return View(businessHours);
        }
        #endregion

        #region Sales

        /// <summary>
        /// Displays the sale listing page.
        /// </summary>
        /// <param name="filter">The query entered by the business, e.g. item name or category.</param>
        /// <param name="page">Pagination: The page number the business would like to view.</param>
        /// <param name="perPage">Pagination: The number of items the business would like to see per page.</param>
        /// <param name="viewInactive">If the business would like to view inactive or active sales only.</param>
        /// <returns>A redirect to the sale listing page.</returns>
        [HttpGet]
        public async Task<IActionResult> ViewSales(string filter = "", int page = 1, int perPage = 5, string viewInactive = "false")
        {
            //Check that the accessing user is a business type account
            var accountType = await KSUserUtilities.KSGetAccountType(_context, _userManager, HttpContext);

            //If the currently logged in user is not a business they can not access business controllers.
            if (accountType != KSUserUtilities.AccountType.BUSINESS)
            {
                TempData["sysMessage"] = "Error: You're not signed in as a business.";
                return RedirectToAction("Index", "Home");
            }

            var business = await KSUserUtilities.KSGetCurrentBusinessAsync(_context, _userManager, HttpContext);

            bool viewInactiveSales = bool.Parse(viewInactive.KSTitleCase());

            var sales = await _context.Sale
                .Where(s => s.BusinessId.Equals(business.BusinessId))
                .Where(s => s.Active == !viewInactiveSales)
                .OrderBy(s => s.SaleName)
                .ToListAsync();

            var categories = sales
                .Select(s => s.SaleCategory)
                .Distinct()
                .ToList();

            if (!string.IsNullOrWhiteSpace(filter))
            {
                TempData["saleFilter"] = filter;
                if (categories.Contains(filter.KSTitleCase()))
                {
                    sales = sales
                        .Where(i => i.SaleCategory.ToLower().Contains(filter.ToLower()))
                        .ToList();
                }
                else
                {
                    sales = sales
                        .Where(i => i.SaleName.ToLower().Contains(filter.ToLower()))
                        .ToList();
                }
                    
                ViewData["NoItemsFoundReason"] = $"Sorry, no results found for {filter}.";
            }

            var paginatedSales = KSPaginatedList<Sale>.Create(sales.AsQueryable(), page, perPage);

            //Gather temp data and pagination/filter info
            //  all in to one place for use 
            TempData["saleCategories"] = categories;
            TempData["currentPage"] = page;
            TempData["totalPage"] = paginatedSales.TotalPages;
            TempData["perPage"] = perPage;
            TempData["hasNextPage"] = paginatedSales.HasNextPage;
            TempData["hasPrevPage"] = paginatedSales.HasPreviousPage;
            TempData["viewInactiveSales"] = viewInactiveSales;

            return View("SalesList", paginatedSales);
        }

        /// <summary>
        /// Displays the sale creation page.
        /// </summary>
        /// <returns>A redirect to the create sale page.</returns>
        [HttpGet]
        public async Task<IActionResult> CreateSale()
        {
            //Check that the accessing user is a business type account
            var accountType = await KSUserUtilities.KSGetAccountType(_context, _userManager, HttpContext);

            //If the currently logged in user is not a business they can not access business controllers.
            if (accountType != KSUserUtilities.AccountType.BUSINESS)
            {
                TempData["sysMessage"] = "Error: You're not signed in as a business.";
                return RedirectToAction("Index", "Home");
            }

            var business = await KSUserUtilities.KSGetCurrentBusinessAsync(_context, _userManager, HttpContext);

            var blankSale = new Sale
            {
                BusinessId = business.BusinessId
            };

            return View(blankSale);
        }

        /// <summary>
        /// Upon submission of the CreateSale page, if the entered information is valid,
        /// the <see cref="Sale"/> is created and saved to the database.
        /// The manage sale items page is then displayed.
        /// </summary>
        /// <param name="businessId">The ID of the business</param>
        /// <param name="sale">The sale information.</param>
        /// <returns>If successful A redirect to the manage sale item page, otherwise the index,</returns>
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSale(Guid businessId, Sale sale)
        {
            //Check that the accessing user is a business type account
            var accountType = await KSUserUtilities.KSGetAccountType(_context, _userManager, HttpContext);

            //If the currently logged in user is not a business they can not access business controllers.
            if (accountType != KSUserUtilities.AccountType.BUSINESS)
            {
                TempData["sysMessage"] = "Error: You're not signed in as a business.";
                return RedirectToAction("Index", "Home");
            }

            //Ensure that the business can only add items for themselves
            if (businessId != sale.BusinessId)
            {
                _logger.LogDebug("Debug: Id Mismatch. Edit not performed.");
                return RedirectToAction("Index");
            }

            try
            {
                if (ModelState.IsValid)
                {
                    sale.SaleDiscountPercentage /= 100; //User enters discount as whole numbers(15), they are saved as decimals(0.15).
                    await _context.AddAsync(sale);
                    await _context.SaveChangesAsync();
                    _logger.LogDebug($"Debug: Sale created. {sale.BusinessId} - {sale.SaleId}");
                    return RedirectToAction("ManageSaleItem", new{ sale.SaleId});
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                _logger.LogError("Error: Business does not exist, sale creation failed.");
                return RedirectToAction("ViewSales");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.GetBaseException().Message}. Sale creation not performed.");
                return RedirectToAction("ViewSales");
            }

            return View(sale);
        }

        /// <summary>
        /// Displays the manage sale item page.
        /// </summary>
        /// <param name="saleId">The ID of the <see cref="Sale"/> being managed.</param>
        /// <param name="filter">The query entered by the business, e.g. item name or category.</param>
        /// <param name="page">Pagination: The page number the business would like to view.</param>
        /// <param name="perPage">Pagination: The number of items the business would like to see per page.</param>
        /// <returns>A redirect to the sale item management page.</returns>
        [HttpGet]
        public async Task<IActionResult> ManageSaleItem(Guid saleId, string filter = "", int page = 1, int perPage = 5)
        {
            //Check that the accessing user is a business type account
            var accountType = await KSUserUtilities.KSGetAccountType(_context, _userManager, HttpContext);

            //If the currently logged in user is not a business they can not access business controllers.
            if (accountType != KSUserUtilities.AccountType.BUSINESS)
            {
                TempData["sysMessage"] = "Error: You're not signed in as a business.";
                return RedirectToAction("Index", "Home");
            }

            var business = await KSUserUtilities.KSGetCurrentBusinessAsync(_context, _userManager, HttpContext);

            var sale = await _context.Sale
                .Where(s => s.BusinessId.Equals(business.BusinessId))
                .Where(s => s.SaleId.Equals(saleId))
                .FirstOrDefaultAsync();

            if (sale == null)
            {
                _logger.LogDebug($"Debug: Sale ID mismatch. Sale {saleId} not found for business {business.BusinessId}.");
                TempData["sysMessage"] = "Error: Something went wrong while finding your sale.";
                return RedirectToAction("ViewSales");
            }

            var items = await _context.Item
                .Where(i => i.BusinessId.Equals(business.BusinessId))
                .Where(si => si.Removed == false)
                .Include(si => si.SaleItem)
                .ToListAsync();
            
            var categories = items
                .Select(i => i.Category)
                .Distinct()
                .ToList();
            
            if (!string.IsNullOrWhiteSpace(filter))
            {
                TempData["saleFilter"] = filter;
                if (categories.Contains(filter.KSTitleCase()))
                {
                    items = items
                        .Where(i => i.Category.Equals(filter.KSTitleCase()))
                        .ToList();
                }
                else
                {
                    items = items
                        .Where(i => i.ItemName.ToLower().Contains(filter.ToLower()))
                        .ToList();
                }
            }
            
            var paginatedItems = KSPaginatedList<Item>.Create(items.AsQueryable(), page, perPage);
            
            //Gather temp data and pagination/filter info
            //  all in to one place for use 
            TempData["itemCategories"] = categories;
            TempData["currentPage"] = page;
            TempData["totalPage"] = paginatedItems.TotalPages;
            TempData["perPage"] = perPage;
            TempData["hasNextPage"] = paginatedItems.HasNextPage;
            TempData["hasPrevPage"] = paginatedItems.HasPreviousPage;

            ViewData["sale"] = sale;
            return View(paginatedItems);
        }

        /// <summary>
        /// Adding or Removing item from a sale.
        /// </summary>
        public enum AddOrRemove
        {
            ADD,
            REMOVE
        }

        /// <summary>
        /// Adds or removes, specified by <see cref="AddOrRemove"/>,
        /// the <see cref="Item"/>, specified by the itemId from the
        /// <see cref="Sale"/>, specified by the saleId.
        /// </summary>
        /// <param name="saleId">The ID of the <see cref="Sale"/> being managed.</param>
        /// <param name="itemId">The ID of the <see cref="Item"/> being managed.</param>
        /// <param name="addOrRemove">Adding or removing the <see cref="Item"/> from the <see cref="Sale"/>.</param>
        /// <returns>If successful A redirect to the businesses catalogue, otherwise the index.</returns>
        [HttpPost]
        public async Task<IActionResult> ManageSaleItem(Guid saleId, Guid itemId, AddOrRemove addOrRemove)
        {
            //Check that the accessing user is a business type account
            var accountType = await KSUserUtilities.KSGetAccountType(_context, _userManager, HttpContext);

            //If the currently logged in user is not a business they can not access business controllers.
            if (accountType != KSUserUtilities.AccountType.BUSINESS)
            {
                TempData["sysMessage"] = "Error: You're not signed in as a business.";
                return RedirectToAction("Index", "Home");
            }

            var business = await KSUserUtilities.KSGetCurrentBusinessAsync(_context, _userManager, HttpContext);

            var sale = await KSCatalogueUtilities.GetSpecificSale(_context, business, saleId);

            var item = await KSCatalogueUtilities.GetSpecificItem(_context, business, itemId);

            if (sale == null)
            {
                _logger.LogDebug($"Debug: Sale ID mismatch. Sale {saleId} not found for business {business.BusinessId}.");
                TempData["sysMessage"] = "Error: Something went wrong while finding your sale.";
                return RedirectToAction("ViewSales");
            }

            if (item == null)
            {
                _logger.LogDebug($"Debug: Item ID mismatch. Sale {itemId} not found for business {business.BusinessId}.");
                TempData["sysMessage"] = "Error: Something went wrong while finding your sale.";
                return RedirectToAction("ViewSales");
            }

            try
            {
                var saleItem = await KSCatalogueUtilities.GetSpecificSaleItem(_context, saleId, itemId);
                
                if (addOrRemove == AddOrRemove.ADD)
                {
                    if (saleItem == null)
                    {
                        SaleItem saleItemAdd = new SaleItem
                        {
                            SaleId = saleId,
                            ItemId = itemId
                        };

                        await _context.SaleItem.AddAsync(saleItemAdd);
                        await _context.SaveChangesAsync();
                    }
                }
                else if (addOrRemove == AddOrRemove.REMOVE)
                {
                    if (saleItem != null)
                    {
                        _context.Remove(saleItem);
                        await _context.SaveChangesAsync();
                    }
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                _logger.LogError($"Error: Sale {sale.SaleId} does not exist, Update Failed.");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.GetBaseException().Message}. Update not performed.");
                return RedirectToAction("Index");
            }

            return RedirectToAction("ManageSaleItem", new{saleId});
        }

        /// <summary>
        /// Displays the edit sale page.
        /// </summary>
        /// <param name="saleId">The ID of the <see cref="Sale"/> to be edited.</param>
        /// <returns>A redirect to the sale edit page.</returns>
        [HttpGet]
        public async Task<IActionResult> EditSale(Guid saleId)
        {
            //Check that the accessing user is a business type account
            var accountType = await KSUserUtilities.KSGetAccountType(_context, _userManager, HttpContext);

            //If the currently logged in user is not a business they can not access business controllers.
            if (accountType != KSUserUtilities.AccountType.BUSINESS)
            {
                TempData["sysMessage"] = "Error: You're not signed in as a business.";
                return RedirectToAction("Index", "Home");
            }

            var business = await KSUserUtilities.KSGetCurrentBusinessAsync(_context, _userManager, HttpContext);

            var sale = await KSCatalogueUtilities.GetSpecificSale(_context, business, saleId);

            if (sale == null)
            {
                _logger.LogDebug($"Debug: Sale ID mismatch. Sale {saleId} not found for business {business.BusinessId}.");
                TempData["sysMessage"] = "Error: Something went wrong while finding your sale.";
                return RedirectToAction("ViewSales");
            }
            
            sale.SaleDiscountPercentage = Math.Round(sale.SaleDiscountPercentage * 100, 2); //User enters discount as whole numbers(15), they are saved as decimals(0.15).

            return View(sale);
        }
        
        /// <summary>
        /// Upon submission of the EditSale page, if the entered information is valid,
        /// the <see cref="Sale"/> is edited in the database.
        /// </summary>
        /// <param name="sale">The <see cref="Sale"/> information.</param>
        /// <returns>If successful A redirect to the businesses catalogue, otherwise the index.</returns>
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSale(Sale sale)
        {
            //Check that the accessing user is a business type account
            var accountType = await KSUserUtilities.KSGetAccountType(_context, _userManager, HttpContext);

            //If the currently logged in user is not a business they can not access business controllers.
            if (accountType != KSUserUtilities.AccountType.BUSINESS)
            {
                TempData["sysMessage"] = "Error: You're not signed in as a business.";
                return RedirectToAction("Index", "Home");
            }

            var business = await KSUserUtilities.KSGetCurrentBusinessAsync(_context, _userManager, HttpContext);

            //Ensure that the business can only add items for themselves
            if (business.BusinessId != sale.BusinessId)
            {
                _logger.LogDebug("Debug: Id Mismatch. Edit not performed.");
                return RedirectToAction("Index");
            }

            try
            {
                if (ModelState.IsValid)
                {
                    sale.SaleDiscountPercentage /= 100; //User enters discount as whole numbers(15), they are saved as decimals(0.15).
                    _context.Update(sale);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("ViewSales");
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                _logger.LogError($"Error: Sale {sale.SaleId} does not exist, Update Failed.");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.GetBaseException().Message}. Update not performed.");
                return RedirectToAction("Index");
            }

            return View(sale);
        }

        /// <summary>
        /// Swaps the status of the <see cref="Sale"/>.
        /// Disabled -> Active | Active -> Disabled.
        /// </summary>
        /// <param name="saleId">The id of the <see cref="Sale"/> being edited.</param>
        /// <returns>A redirect to the sale listing page.</returns>
        [HttpPost]
        public async Task<IActionResult> SwapSaleStatus(Guid saleId)
        {
            //Check that the accessing user is a business type account
            var accountType = await KSUserUtilities.KSGetAccountType(_context, _userManager, HttpContext);

            //If the currently logged in user is not a business they can not access business controllers.
            if (accountType != KSUserUtilities.AccountType.BUSINESS)
            {
                TempData["sysMessage"] = "Error: You're not signed in as a business.";
                return RedirectToAction("Index", "Home");
            }

            var business = await KSUserUtilities.KSGetCurrentBusinessAsync(_context, _userManager, HttpContext);

            var sale = await KSCatalogueUtilities.GetSpecificSale(_context, business, saleId);
            
            try
            {
                sale.Active = !sale.Active;
                _context.Update(sale);
                await _context.SaveChangesAsync();
                TempData["sysMessage"] = "Sale status changed successfully.";
            }
            catch (DbUpdateConcurrencyException)
            {
                _logger.LogError($"Error: Sale {sale.SaleId} does not exist, status change Failed.");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.GetBaseException().Message}. status change not performed.");
                return RedirectToAction("Index");
            }

            return RedirectToAction("ViewSales");
        }

        /// <summary>
        /// Deletes the <see cref="Sale"/> from the businesses account.
        /// </summary>
        /// <param name="saleId">The ID of the <see cref="Sale"/> to be deleted.</param>
        /// <returns>If successful A redirect to the sale listing page, otherwise the index.</returns>
        [HttpGet]
        public async Task<IActionResult> DeleteSale(Guid saleId)
        {
            //Check that the accessing user is a business type account
            var accountType = await KSUserUtilities.KSGetAccountType(_context, _userManager, HttpContext);

            //If the currently logged in user is not a business they can not access business controllers.
            if (accountType != KSUserUtilities.AccountType.BUSINESS)
            {
                TempData["sysMessage"] = "Error: You're not signed in as a business.";
                return RedirectToAction("Index", "Home");
            }

            var business = await KSUserUtilities.KSGetCurrentBusinessAsync(_context, _userManager, HttpContext);

            var sale = await KSCatalogueUtilities.GetSpecificSale(_context, business, saleId);

            try
            {
                _context.SaleItem.RemoveRange(sale.SaleItem);
                _context.Sale.Remove(sale);
                await _context.SaveChangesAsync();

                TempData["sysMessage"] = $"The sale: {sale.SaleName} has been deleted";
                return RedirectToAction("ViewSales");
            }
            catch (DbUpdateConcurrencyException)
            {
                TempData["sysMessage"] = "Error: Something went wrong. Your sale was not deleted";
                _logger.LogError($"Error: Sale {sale.SaleId} does not exist, status change Failed.");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["sysMessage"] = "Error: Something went wrong. Your sale was not deleted";
                _logger.LogError($"Error: {ex.GetBaseException().Message}. status change not performed.");
                return RedirectToAction("Index");
            }
        }

        #endregion
    }
}
