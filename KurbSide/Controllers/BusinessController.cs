using KurbSide.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
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

        public async Task<IActionResult> Edit()
        {
            var user = await GetCurrentUserAsync();
            var accountType = GetAccountType(user);
            var isAllowed = accountType.Equals("business");

            if (!isAllowed) return RedirectToAction("index");

            var business = await _context.Business
                .Where(b => b.AspNetId.Equals(user.Id))
                .FirstOrDefaultAsync();

            ViewData["CountryCode"] = new SelectList(_context.Country, "CountryCode", "FullName", "CA");
            ViewData["ProvinceCode"] = new SelectList(_context.Province, "ProvinceCode", "FullName", "ON");

            return View(business);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //TODO Needs to remove debug messages
        public async Task<IActionResult> Edit(Guid id, Business business)
        {
            var user = await GetCurrentUserAsync();
            var accountType = GetAccountType(user);
            var isAllowed = accountType.Equals("business");

            if (!isAllowed) return RedirectToAction("index");

            if (id != business.BusinessId)
            {
                TempData["sysMessage"] = $"Debug: Id Mismatch. Edit not performed.";
                return RedirectToAction("index");
            }

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
                    return RedirectToAction("index");
                }
            }

            TempData["sysMessage"] = $"Debug: Edit succeeded. {business.BusinessId}";
            return RedirectToAction("index");
        }


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
    }
}
