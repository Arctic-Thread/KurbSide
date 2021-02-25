using KurbSide.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KurbSide.Controllers
{
    [Authorize]
    public class BusinessController : Controller
    {
        private readonly KSContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public BusinessController(KSContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        #region BusinessRU
        public async Task<IActionResult> IndexAsync()
        {
            var user = await GetCurrentUserAsync();
            var accountType = GetAccountType(user);
            var isAllowed = accountType.Equals("business");

            if (!isAllowed)
            {
                TempData["sysMessage"] = "Error: You're not signed in as a business.";
                return RedirectToAction("index", "home");
            }

            var business = await _context.Business
                .Where(b => b.AspNetId.Equals(user.Id))
                .FirstOrDefaultAsync();

            return View(business);
        }

        public async Task<IActionResult> EditBusiness()
        {
            var user = await GetCurrentUserAsync();
            var accountType = GetAccountType(user);
            var isAllowed = accountType.Equals("business");

            if (!isAllowed) return RedirectToAction("index");

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
            var user = await GetCurrentUserAsync();
            var accountType = GetAccountType(user);
            var isAllowed = accountType.Equals("business");

            if (!isAllowed) return RedirectToAction("Index");

            if (id != business.BusinessId)
            {
                //TODO Remove Debug messages
                TempData["sysMessage"] = $"Debug: Id Mismatch. Edit not performed.";
                return RedirectToAction("Index");
            }

            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(business);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        TempData["sysMessage"] = $"Error: Business does not exist, Update Failed.";
                        return RedirectToAction("Index");
                    }
                    //TODO more debug messages!
                    TempData["sysMessage"] = $"Debug: Edit succeeded. {business.BusinessId}";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                //TODO even more debug messages!
                TempData["sysMessage"] = $"Error: {ex.GetBaseException().Message}. Edit not performed.";
                return RedirectToAction("Index");
            }

            ViewData["CountryCode"] = new SelectList(_context.Country, "CountryCode", "FullName", business.ProvinceCode);
            ViewData["ProvinceCode"] = new SelectList(_context.Province, "ProvinceCode", "FullName", business.ProvinceCode);
            return View(business);
        }
        #endregion

        #region ItemCRUD
        public async Task<IActionResult> Catalogue(string? filter, int page = 1, int perPage = 5)
        {
            //if (filter != null) page = 1;

            var user = await GetCurrentUserAsync();
            var accountType = GetAccountType(user);
            var isAllowed = accountType.Equals("business");

            if (!isAllowed) return RedirectToAction("Index");

            var business = await _context.Business
                .Where(b => b.AspNetId.Equals(user.Id))
                .FirstOrDefaultAsync();

            var items = await _context.Item
                .Where(i => i.BusinessId.Equals(business.BusinessId))
                .Include(i => i.Business)
                .Where(i => i.Removed==false)
                .OrderByDescending(i => i.Category)
                .ToListAsync();

            var categories = items
                .Select(i => i.Category)
                .Distinct()
                .ToList();

            if (filter != null)
            {
                TempData["catalogueFilter"] = filter;
                if (categories.Contains(filter))
                {
                    items = items
                        .Where(i => i.Category.Equals(filter))
                        .ToList();
                }
            else
                {
                    items = items
                        .Where(i => i.ItemName.ToLower().Contains(filter.ToLower()))
                        .ToList();
                }
            }

            TempData["itemCategories"] = categories;
            //return View(items);
            var paginatedList = KurbSideUtils.KSPaginatedList<Item>.Create(items.AsQueryable(), page, perPage);
            //TempData["maxPage"] = paginatedList.TotalPages;
            TempData["currentPage"] = page;
            TempData["totalPage"] = paginatedList.TotalPages;
            TempData["hasNextPage"] = paginatedList.HasNextPage;
            TempData["hasPrevPage"] = paginatedList.HasPreviousPage;
            return View(paginatedList);
        }
        public async Task<IActionResult> RemoveItem(Guid id)
        {
            var user = await GetCurrentUserAsync();
            var accountType = GetAccountType(user);
            var isAllowed = accountType.Equals("business");

            if (!isAllowed) return RedirectToAction("Index");

            var business = await _context.Business
                .Where(b => b.AspNetId.Equals(user.Id))
                .FirstOrDefaultAsync();

            var item = await _context.Item
                .Where(i => i.BusinessId.Equals(business.BusinessId))
                .Where(i => i.ItemId.Equals(id))
                .FirstOrDefaultAsync();

            try
            {
                //HACK throw an exception forcing items to be hidden instead of removed
                //throw new Exception("Test Exception, force hide");
                _context.Item.Remove(item);
                //TODO even more debug messages!
                TempData["sysMessage"] = $"Debug: Item not present in any orders, Removed From Database.";
            }
            catch (Exception)
            {
                try
                {
                    if (item == null) throw new Exception();
                    item.Removed = true;
                    _context.Item.Update(item);
                    //TODO even more debug messages!
                    TempData["sysMessage"] = $"Debug: Item is present in orders, Marked as Hidden/Removed.";
                }
                catch (Exception)
                {
                    //TempData["sysMessage"] = $"Error: Item does not exist or does not belong to buisness.";
                }
            }
            finally
            {
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Catalogue");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddItem(Guid id, Item item)
        {
            var user = await GetCurrentUserAsync();
            var accountType = GetAccountType(user);
            var isAllowed = accountType.Equals("business");

            if (!isAllowed) return RedirectToAction("index");

            if (id != item.BusinessId)
            {
                //TODO Remove Debug messages
                TempData["sysMessage"] = $"Debug: Id Mismatch. Edit not performed.";
                return RedirectToAction("index");
            }

            try
            {
                if (ModelState.IsValid)
                {
                    _context.Item.Add(item);
                    await _context.SaveChangesAsync();
                    
                    //TODO more debug messages!
                    TempData["sysMessage"] = $"Debug: Add succeeded. {item.ItemId}";
                    return RedirectToAction("catalogue");
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                TempData["sysMessage"] = $"Error: Business does not exist, Add Failed.";
                return RedirectToAction("index");
            }
            catch (Exception ex)
            {
                //TODO even more debug messages!

                TempData["sysMessage"] = $"Error: {ex.GetBaseException().Message}. Add not performed.";
                return RedirectToAction("index");
            }
            return View(item);
        }
        public async Task<IActionResult> AddItem()
        {
            var user = await GetCurrentUserAsync();
            var accountType = GetAccountType(user);
            var isAllowed = accountType.Equals("business");

            if (!isAllowed) return RedirectToAction("index");

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
        public async Task<IActionResult> EditItem(Guid id, Item item)
        {
            var user = await GetCurrentUserAsync();
            var accountType = GetAccountType(user);
            var isAllowed = accountType.Equals("business");

            if (!isAllowed) return RedirectToAction("Index");

            if (id != item.BusinessId)
            {
                //TODO Remove Debug messages
                TempData["sysMessage"] = $"Debug: Id Mismatch. Update not performed.";
                return RedirectToAction("Index");
            }

            try
            {
                if (ModelState.IsValid)
                {
                    _context.Item.Update(item);
                    await _context.SaveChangesAsync();

                    //TODO more debug messages!
                    TempData["sysMessage"] = $"Debug: Update succeeded. {item.ItemId}";
                    return RedirectToAction("Catalogue");
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                TempData["sysMessage"] = $"Error: Business does not exist, Update Failed.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                //TODO even more debug messages!
                TempData["sysMessage"] = $"Error: {ex.GetBaseException().Message}. Update not performed.";
                return RedirectToAction("Index");
            }

            return View(item);
        }
        public async Task<IActionResult> EditItem(Guid id)
        {
            var user = await GetCurrentUserAsync();
            var accountType = GetAccountType(user);
            var isAllowed = accountType.Equals("business");

            if (!isAllowed) return RedirectToAction("Index");

            var business = await _context.Business
                .Where(b => b.AspNetId.Equals(user.Id))
                .FirstOrDefaultAsync();

            var item = await _context.Item
                .Where(i => i.BusinessId.Equals(business.BusinessId))
                .Where(i => i.ItemId.Equals(id))
                .FirstOrDefaultAsync();

            if (item==null)
            {
                return RedirectToAction("Index");
            }

            return View(item);
        }
        #endregion

        #region Business Hours
        public async Task<IActionResult> EditBusinessHours()
        {
            var user = await GetCurrentUserAsync();
            var accountType = GetAccountType(user);
            var isAllowed = accountType.Equals("business");

            if (!isAllowed) return RedirectToAction("Index");

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
            var user = await GetCurrentUserAsync();
            var accountType = GetAccountType(user);
            var isAllowed = accountType.Equals("business");

            if (!isAllowed) return RedirectToAction("Index");

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

        #region CurrentUserUtils
        //Current User Utils 1.0
        private Task<IdentityUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        private async Task<string> GetLoggedInEmailAsync()
        {
            var user = await GetCurrentUserAsync();
            return user == null ? "" : user.Email;
        }

        private string GetAccountType(IdentityUser? IUser)
        {
            if (IUser == null) return "";

            bool hasBusiness = _context.Business.Where(b => b.AspNetId.Equals(IUser.Id)).Count() > 0;
            bool hasMember = _context.Member.Where(b => b.AspNetId.Equals(IUser.Id)).Count() > 0;

            /*
             * HACK maybe make this an enum rather than a string to prevent future issues
             *      actually nevermind because C# enums aren't like Java enums
             *      for now keeping it standard to lowercase strings works :)
             */
            if (hasBusiness) return "business";
            else if (hasMember) return "member";
            else return "";

        }
        //End Current User Utils 1.0
        #endregion
    }
}
