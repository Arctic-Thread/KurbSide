using KurbSide.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using KurbSide.Utilities;

namespace KurbSide.Controllers
{
    [Authorize]
    public class StoreController : Controller
    {
        private readonly KSContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<Business> _logger;

        public StoreController(KSContext context, UserManager<IdentityUser> userManager, ILogger<Business> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<IActionResult> IndexAsync(int md = 25, string filter = "")
        {
            var user = await KSCurrentUser.KSGetCurrentUserAsync(_userManager, HttpContext);
            var accountType = await KSCurrentUser.KSGetAccountType(_context, _userManager, HttpContext);

            //If the currently logged in user is not a member they can not access the store.
            if (accountType != KSCurrentUser.AccountType.MEMBER)
            {
                TempData["sysMessage"] = "Error: You're not signed in as a member.";
                return RedirectToAction("Index", "Home");
            }

            //Get the current logged in member, and prepare location var
            var member = await _context.Member.FirstOrDefaultAsync(m => m.AspNetId.Equals(user.Id));

            var memberLocation = new Service.Location((float)member.Lat, (float)member.Lng, "");

            var business = _context.Business
                .AsEnumerable();

            if (!string.IsNullOrWhiteSpace(filter))
            {
                TempData["catalogueFilter"] = filter;

                business = business
                    .Where(i => i.BusinessName.ToLower().Contains(filter.ToLower()))
                    .ToList();
            }


            //Get all stores that exist within 'md' or Max Distance defaulting to 25km
            var rtnbusiness = business
                .Where(b => GetDistance(b.Lat, b.Lng, memberLocation.lat, memberLocation.lng) <= md)
                .OrderBy(b => GetDistance(b.Lat, b.Lng, memberLocation.lat, memberLocation.lng))
                .Select(b => Tuple.Create(b, GetDistance(b.Lat, b.Lng, memberLocation.lat, memberLocation.lng)));

            TempData["md"] = md;
            return View(rtnbusiness);
        }

        public async Task<IActionResult> ViewCatalogue(Guid? id, string filter = "")
        {
            var accountType = await KSCurrentUser.KSGetAccountType(_context, _userManager, HttpContext);

            //If the currently logged in user is not a member they can not access the store.
            if (accountType != KSCurrentUser.AccountType.MEMBER)
            {
                TempData["sysMessage"] = "Error: You're not signed in as a member.";
                return RedirectToAction("Index", "Home");
            }

            if (id == null) //TODO
            {
                return NotFound();
            }
            var business = await _context.Business
                .Where(i => i.BusinessId.Equals(id))
                .FirstOrDefaultAsync();

            var items = await _context.Item
                .Where(i => i.BusinessId.Equals(id))
                //.Where(i => i.Removed != null && i.Removed == false)
                .ToListAsync();

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
                }
            }

            var categorizedItems = items
                .GroupBy(i => KurbSideUtils.KSStringManipulation.KSTitleCase(i.Category))
                .ToDictionary(i => i.Key, i => i.AsEnumerable());

            if (items == null) //TODO
            {
                return NotFound();
            }

            //return View(items);
            //return View(categorizedItems);
            TempData["itemCategories"] = categories;
            return View(Tuple.Create(business, categorizedItems));
        }

        public async Task<IActionResult> ViewItem(Guid? id)
        {
            var accountType = await KSCurrentUser.KSGetAccountType(_context, _userManager, HttpContext);

            //If the currently logged in user is not a member they can not access the store.
            if (accountType != KSCurrentUser.AccountType.MEMBER)
            {
                TempData["sysMessage"] = "Error: You're not signed in as a member.";
                return RedirectToAction("Index", "Home");
            }

            if (id == null) //TODO
            {
                return NotFound();
            }

            var item = await _context.Item.FirstOrDefaultAsync(i => i.ItemId == id);

            if (item == null) //TODO
            {
                return NotFound();
            }

            return View(item);
        }

        public static float GetDistance(double lat1, double lng1, double lat2, double lng2)
        {
            var location1 = new Service.Location((float)lat1, (float)lng1, "");
            var location2 = new Service.Location((float)lat2, (float)lng2, "");

            return Service.GeoCode.CalculateDistanceLocal(location1, location2).distance / 1000;
        }
    }
}
