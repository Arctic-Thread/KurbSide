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

        public async Task<IActionResult> IndexAsync(int md = 25)
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

            //Get all stores that exist within 'md' or Max Distance defaulting to 25km
            //var stores = await _context.Business.Where(b =>
            //    Service.GeoCode.CalculateDistanceLocal(
            //        memberLocation,
            //        new Service.Location((float)b.Lat, (float)b.Lng, "")).distance
            //        <= md)
            //    .ToListAsync();
            var stores = _context.Business
                .AsEnumerable()
                .Where(b => getDistance(b.Lat, b.Lng, memberLocation.lat, memberLocation.lng) <= md);

            var storeDistances = _context.Business
                .AsEnumerable()
                .Where(b => getDistance(b.Lat, b.Lng, memberLocation.lat, memberLocation.lng) <= md)
                .Select(b => getDistance(b.Lat, b.Lng, memberLocation.lat, memberLocation.lng));

            return View(Tuple.Create(stores, storeDistances));
        }

        public static float getDistance(double lat1, double lng1, double lat2, double lng2)
        {
            var location1 = new Service.Location((float)lat1, (float)lng1, "");
            var location2 = new Service.Location((float)lat2, (float)lng2, "");

            return Service.GeoCode.CalculateDistanceLocal(location1, location2).distance / 1000;
        }
    }
}
