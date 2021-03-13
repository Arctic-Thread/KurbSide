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

namespace KurbSide.Controllers
{
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
        public async Task<IActionResult> IndexAsync()
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

            var business = await _context.Business.FirstOrDefaultAsync(b => b.AspNetId.Equals(user.Id));

            return View(business);
        }

        public async Task<IActionResult> EditBusiness()
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

            ViewData["CountryCode"] = new SelectList(_context.Country, "CountryCode", "FullName", business.ProvinceCode);
            ViewData["ProvinceCode"] = new SelectList(_context.Province, "ProvinceCode", "FullName", business.ProvinceCode);

            return View(business);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditBusiness(Guid id, Business business)
        {
            //Check that the accessing user is a business type account
            var accountType = await KSCurrentUser.KSGetAccountType(_context, _userManager, HttpContext);

            //If the currently logged in user is not a business they can not access business controllers.
            if (accountType != KSCurrentUser.AccountType.BUSINESS)
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
                    try
                    {
                        string address = $"{business.Street} {business.City} {business.ProvinceCode} {business.CountryCode} {business.Postal}";
                        Service.Location location = await Service.GeoCode.GetLocationAsync(address);

                        business.Lng = location.lng;
                        business.Lat = location.lat;

                        _context.Update(business);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        _logger.LogError("Business does not exist, Update Failed.");
                        return RedirectToAction("Index");
                    }
                    _logger.LogDebug($"Edit succeeded. {business.BusinessId}");
                    return RedirectToAction("Index");
                }
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
        public async Task<IActionResult> Catalogue(string filter = "", int page = 1, int perPage = 5)
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
            var paginatedList = KurbSideUtils.KSPaginatedList<Item>.Create(items.AsQueryable(), page, perPage);

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
        public async Task<IActionResult> RemoveItem(Guid id, string filter = "", int page = 1, int perPage = 5)
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

            var item = await _context.Item
                .Where(i => i.BusinessId.Equals(business.BusinessId))
                .Where(i => i.ItemId.Equals(id))
                .FirstOrDefaultAsync();

            try
            {
                //Throw an exception forcing items to be hidden instead of removed
                throw new Exception("");
                //_context.Item.Remove(item);
                //_logger.LogDebug("Debug: Item not present in any orders, Removed From Database.");
            }
            catch (Exception)
            {
                try
                {
                    if (item == null) throw new Exception();
                    item.Removed = true;
                    _context.Item.Update(item);
                    _logger.LogDebug("Debug: Item is present in orders, Marked as Hidden/Removed.");
                }
                catch (Exception)
                { }
            }
            finally
            {
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Catalogue", new {filter, page, perPage});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddItem(Guid id, Item item, IFormFile itemImage)
        {
            //Check that the accessing user is a business type account
            var accountType = await KSCurrentUser.KSGetAccountType(_context, _userManager, HttpContext);

            //If the currently logged in user is not a business they can not access business controllers.
            if (accountType != KSCurrentUser.AccountType.BUSINESS)
            {
                TempData["sysMessage"] = "Error: You're not signed in as a business.";
                return RedirectToAction("Index", "Home");
            }

            //Ensure that the business can only add items for themselves
            if (id != item.BusinessId)
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

                    _context.Item.Add(item);
                    await _context.SaveChangesAsync();
                    _logger.LogDebug("Debug: Add succeeded. {item.ItemId}");
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
        public async Task<IActionResult> AddItem()
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

            var blankItem = new Item
            {
                BusinessId = business.BusinessId,
                Business = business
            };

            return View(blankItem);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditItem(Guid id, Item item, IFormFile itemImageEdit)
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

            //Ensure that the business can only edit items for themselves
            if (id != item.BusinessId)
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
                        string uploadResults = await KSImgur.KSUploadImageToImgur(itemImageEdit);
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
                        string existingImage = existingItem.ImageLocation;
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
        public async Task<IActionResult> EditItem(Guid id)
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

            var item = await _context.Item
                .Where(i => i.BusinessId.Equals(business.BusinessId))
                .Where(i => i.ItemId.Equals(id))
                .FirstOrDefaultAsync();

            if (item == null)
            {
                return RedirectToAction("Index");
            }

            return View(item);
        }
        #endregion

        #region Business Hours
        public async Task<IActionResult> EditBusinessHours()
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

            var businessHours = await _context.BusinessHours
                .FirstOrDefaultAsync(b => b.BusinessId == business.BusinessId);

            if (businessHours == null)
            {
                TempData["sysMessage"] = $"Error: Business hours not found.";
                return RedirectToAction("Index");
            }

            return View(businessHours);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditBusinessHours(Guid id, BusinessHours businessHours)
        {
            //Check that the accessing user is a business type account
            var accountType = await KSCurrentUser.KSGetAccountType(_context, _userManager, HttpContext);

            //If the currently logged in user is not a business they can not access business controllers.
            if (accountType != KSCurrentUser.AccountType.BUSINESS)
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
    }
}
