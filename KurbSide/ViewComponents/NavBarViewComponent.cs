using KurbSide.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace KurbSide.ViewComponents
{
    [ViewComponent(Name = "NavBar")]
    public class NavBarViewComponent : ViewComponent
    {
        private readonly KSContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public NavBarViewComponent(KSContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        private Task<IdentityUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        private async Task<string> GetLoggedInEmailAsync()
        {
            var user = await GetCurrentUserAsync();
            return user == null ? "" : user.Email;
        }

        private string GetAccountType(IdentityUser? IUser)
        {
            if (IUser == null)
            {
                return "";
            }

            bool hasBusiness = _context.Business.Where(b => b.AspNetId.Equals(IUser.Id)).Count() > 0;
            bool hasMember = _context.Member.Where(b => b.AspNetId.Equals(IUser.Id)).Count() > 0;

            if (hasBusiness)
            {
                return "business";
            }
            else if (hasMember)
            {
                return "member";
            }
            else
            {
                return "";
            }

        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await GetCurrentUserAsync();
            var accountType = GetAccountType(user);

            TempData["email"] = await GetLoggedInEmailAsync();
            TempData["loggedInType"] = accountType;

            if (accountType.Equals("business"))
            {
                var business = await _context.Business
                    .Where(b => b.AspNetId.Equals(user.Id))
                    .FirstOrDefaultAsync();

                TempData["loggedInBusiness"] = business;
            }

            return await Task.FromResult((IViewComponentResult)View("Default"));
        }
    }
}
