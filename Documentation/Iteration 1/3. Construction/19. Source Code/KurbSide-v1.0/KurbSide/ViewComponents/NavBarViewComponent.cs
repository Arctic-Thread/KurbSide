using KurbSide.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

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
                TempData["openForBusiness"] = await OpenForBusiness();
            }

            return await Task.FromResult((IViewComponentResult)View("Default"));
        }

        public async Task<bool> OpenForBusiness()
        {
            var user = await GetCurrentUserAsync();
            if (GetAccountType(user) == "business")
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
