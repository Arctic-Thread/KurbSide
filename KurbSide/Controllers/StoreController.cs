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
            //Check that the accessing user is a member type account
            var user = await GetCurrentUserAsync();
            var accountType = GetAccountType(user);
            var isAllowed = accountType.Equals("member");

            if (!isAllowed)
            {
                return RedirectToAction("index", "home");
            }

            //Get the current logged in member, and prepare location var
            var member = _context.Member
                .Where(m => m.AspNetId.Equals(user.Id))
                .FirstOrDefault();
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
