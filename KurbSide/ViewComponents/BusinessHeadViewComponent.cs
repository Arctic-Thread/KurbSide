using KurbSide.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace KurbSide.ViewComponents
{
    [ViewComponent(Name = "BusinessHead")]
    public class BusinessHeadViewComponent : ViewComponent
    {
        private readonly KSContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public BusinessHeadViewComponent(KSContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await GetCurrentUserAsync();

            var business = await _context.Business
                .Where(b => b.AspNetId.Equals(user.Id))
                .FirstOrDefaultAsync();

            return await Task.FromResult((IViewComponentResult)View("Default", business));
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
