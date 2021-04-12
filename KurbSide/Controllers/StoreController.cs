using KurbSide.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using KurbSide.Service;
using KurbSide.Utilities;

namespace KurbSide.Controllers
{
    /// <summary>
    /// If the user trying to view the store front is not a logged in they are redirected to the login page.
    /// If the currently logged in user is not a member they are redirected to the business dashboard.
    /// </summary>
    [Authorize]
    public class StoreController : Controller
    {
        private readonly KSContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<Item> _logger;

        public StoreController(KSContext context, UserManager<IdentityUser> userManager, ILogger<Item> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        /// <summary>
        /// Displays the store index page.
        /// </summary>
        /// <param name="md">The maximum distance a business can be from the customer.</param>
        /// <param name="filter">The query entered by the member, e.g. item name or category.</param>
        /// <returns>A redirect to the store front index page.</returns>
        [HttpGet]
        public async Task<IActionResult> IndexAsync(int md = 25, string filter = "")
        {
            var accountType = await KSUserUtilities.KSGetAccountType(_context, _userManager, HttpContext);

            //If the currently logged in user is not a member they can not access the store.
            if (accountType != KSUserUtilities.AccountType.MEMBER)
            {
                TempData["sysMessage"] = "Error: You're not signed in as a member.";
                return RedirectToAction("Index", "Home");
            }

            //Get the current logged in member
            var member = await KSUserUtilities.KSGetCurrentMemberAsync(_context, _userManager, HttpContext);

            // prepare the current members location
            var memberLocation = new Location(member.Lat, member.Lng, "");

            // The list of businesses and their business hours.
            var businesses = _context.Business
                .Include(b => b.BusinessHours)
                .AsEnumerable();

            if (!string.IsNullOrWhiteSpace(filter))
            {
                TempData["catalogueFilter"] = filter;

                businesses = businesses
                    .Where(i => i.BusinessName.ToLower().Contains(filter.ToLower()))
                    .ToList();
            }

            if (md <= 0)
                md = 5;

            //Get all stores that exist within 'md' or Max Distance defaulting to 25km
            var businessListings = businesses
                .Where(b => GetDistance(new Location(b.Lat, b.Lng, b.Street), memberLocation) <= md)
                .OrderBy(b => GetDistance(new Location(b.Lat, b.Lng, b.Street), memberLocation))
                .Select(b => Tuple.Create(b, GetDistance(new Location(b.Lat, b.Lng, b.Street), memberLocation)));

            TempData["md"] = md;
            return View(businessListings);
        }

        /// <summary>
        /// Displays the specified businesses catalogue.
        /// </summary>
        /// <param name="id">The ID of the specified <see cref="Business"/>.</param>
        /// <param name="filter">The query entered by the business, e.g. item name or category.</param>
        /// <returns>A redirect to the specified businesses catalogue.</returns>
        [HttpGet]
        public async Task<IActionResult> Catalogue(Guid? id, string filter = "")
        {
            var accountType = await KSUserUtilities.KSGetAccountType(_context, _userManager, HttpContext);

            //If the currently logged in user is not a member they can not access the store.
            if (accountType != KSUserUtilities.AccountType.MEMBER)
            {
                TempData["sysMessage"] = "Error: You're not signed in as a member.";
                return RedirectToAction("Index", "Home");
            }

            if (id == null)
            {
                _logger.LogDebug("Business ID Not found.");
                return RedirectToAction("Index");
            }

            var business = await _context.Business
                .Where(i => i.BusinessId.Equals(id))
                .FirstOrDefaultAsync();

            var items = await _context.Item
                .Where(i => i.BusinessId.Equals(id))
                .Where(i => i.Removed != null && i.Removed == false)
                .Include(s => s.SaleItem)
                .ToListAsync();

            if (items.Count <= 0)
            {
                 ViewData["NoItemsFoundReason"] = $"Sorry, {business.BusinessName} has no items for sale right now. Please check back later!";
            }

            var categories = items
                .Select(i => i.Category)
                .Distinct()
                .ToList();

            if (!string.IsNullOrWhiteSpace(filter))
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
                    
                    ViewData["NoItemsFoundReason"] = $"Sorry, no results found for {filter}.";
                }
            }

            var categorizedItems = items
                .GroupBy(i => KurbSideUtils.KSStringManipulation.KSTitleCase(i.Category))
                .ToDictionary(i => i.Key, i => i.AsEnumerable());

            var sales = await _context.Sale
                .Where(b => b.BusinessId.Equals(business.BusinessId))
                .Include(s => s.SaleItem)
                .ToListAsync();

            ViewData["sales"] = sales;

            TempData["itemCategories"] = categories;
            return View(Tuple.Create(business, categorizedItems));
        }

        /// <summary>
        /// Displays an <see cref="Item"/>s details page.
        /// </summary>
        /// <param name="id">The ID of the specified <see cref="Item"/> to view.</param>
        /// <returns>A redirect to the items details page.</returns>
        [HttpGet]
        public async Task<IActionResult> ViewItem(Guid? id)
        {
            if (id == null)
            {
                _logger.LogDebug("Item ID Not found.");
                return RedirectToAction("Index");
            }

            var accountType = await KSUserUtilities.KSGetAccountType(_context, _userManager, HttpContext);
            //If the currently logged in user is not a member they can not access the store.
            if (accountType != KSUserUtilities.AccountType.MEMBER)
            {
                TempData["sysMessage"] = "Error: You're not signed in as a member.";
                return RedirectToAction("Index", "Home");
            }

            var item = await _context.Item
                .Include(si => si.SaleItem)
                .FirstOrDefaultAsync(i => i.ItemId == id);
            
            var business = await _context.Business
                .Where(b => b.BusinessId == item.BusinessId)
                .Include(b => b.BusinessHours)
                .FirstOrDefaultAsync();

            var sales = await _context.Sale
                .Where(b => b.BusinessId.Equals(business.BusinessId))
                .Include(si => si.SaleItem)
                .ToListAsync();

            if (item == null)
            {
                _logger.LogDebug("ID Mismatch. Item does not exist.");
                return RedirectToAction("Index");
            }

            ViewData["sales"] = sales;

            return View(Tuple.Create(business, item));
        }

        /// <summary>
        /// Returns the distance between two locations.
        /// </summary>
        /// <param name="location1">The first location e.g. The customers location.</param>
        /// <param name="location2">The second location e.g. The businesses location.</param>
        /// <returns>The distance between the two locations.</returns>
        public static double GetDistance(Location location1, Location location2) =>
            GeoCode.CalculateDistanceLocal(location1, location2).distance / 1000;
    }
}