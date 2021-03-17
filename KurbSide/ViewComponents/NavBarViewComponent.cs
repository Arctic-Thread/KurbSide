using KurbSide.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using KurbSide.Utilities;

#nullable enable
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

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await KSCurrentUser.KSGetCurrentUserAsync(_userManager, HttpContext);
            var accountType = await KSCurrentUser.KSGetAccountType(_context, _userManager, HttpContext);
            var email = await KSCurrentUser.KSGetLoggedInEmailAsync(_userManager, HttpContext);

            TempData["email"] = email;
            TempData["loggedInType"] = accountType;

            if (accountType == KSCurrentUser.AccountType.BUSINESS)
            {
                var business = await _context.Business
                    .Include(b => b.BusinessHours)
                    .Where(b => b.AspNetId.Equals(user.Id))
                    .FirstOrDefaultAsync();

                TempData["loggedInBusiness"] = business;
                TempData["openForBusiness"] = KSStoreUtilities.CheckIfOpenForBusiness(business.BusinessHours, DateTime.Now.DayOfWeek);
            }
            else if (accountType == KSCurrentUser.AccountType.MEMBER)
            {
                var member = await _context.Member
                    .Where(m => m.AspNetId.Equals(user.Id))
                    .FirstOrDefaultAsync();

                TempData["loggedInMember"] = member;
            }

            return await Task.FromResult((IViewComponentResult)View("Default"));
        }
    }
}
