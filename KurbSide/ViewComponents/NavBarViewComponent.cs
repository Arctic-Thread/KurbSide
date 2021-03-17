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
                    .Where(b => b.AspNetId.Equals(user.Id))
                    .FirstOrDefaultAsync();

                TempData["loggedInBusiness"] = business;
                TempData["openForBusiness"] = await OpenForBusiness();
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

        public async Task<bool> OpenForBusiness()
        {
            var user = await KSCurrentUser.KSGetCurrentUserAsync(_userManager, HttpContext);
            var accountType = await KSCurrentUser.KSGetAccountType(_context, _userManager, HttpContext);

            if (accountType == KSCurrentUser.AccountType.BUSINESS)
            {
                var business = await _context.Business
                    .Where(b => b.AspNetId.Equals(user.Id))
                    .FirstOrDefaultAsync();

                var businessHours = await _context.BusinessHours
                    .FirstOrDefaultAsync(b => b.BusinessId == business.BusinessId);

                DayOfWeek dayOfWeek = DateTime.Now.DayOfWeek;

                switch (dayOfWeek)
                {
                    case DayOfWeek.Sunday:
                        return businessHours.SunOpen < DateTime.Now.TimeOfDay && DateTime.Now.TimeOfDay < businessHours.SunClose;
                    case DayOfWeek.Monday:
                        return businessHours.MonOpen < DateTime.Now.TimeOfDay && DateTime.Now.TimeOfDay < businessHours.MonClose;
                    case DayOfWeek.Tuesday:
                        return businessHours.TuesOpen < DateTime.Now.TimeOfDay && DateTime.Now.TimeOfDay < businessHours.TuesClose;
                    case DayOfWeek.Wednesday:
                        return businessHours.WedOpen < DateTime.Now.TimeOfDay && DateTime.Now.TimeOfDay < businessHours.WedClose;
                    case DayOfWeek.Thursday:
                        return businessHours.ThuOpen < DateTime.Now.TimeOfDay && DateTime.Now.TimeOfDay < businessHours.ThuClose;
                    case DayOfWeek.Friday:
                        return businessHours.FriOpen < DateTime.Now.TimeOfDay && DateTime.Now.TimeOfDay < businessHours.FriClose;
                    case DayOfWeek.Saturday:
                        return businessHours.SatOpen < DateTime.Now.TimeOfDay && DateTime.Now.TimeOfDay < businessHours.SatClose;
                    default:
                        return false;
                }
            }
            return false;
        }

    }

}
