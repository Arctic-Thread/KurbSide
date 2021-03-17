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
    [ViewComponent(Name = "MemberHead")]
    public class MemberHeadViewComponent : ViewComponent
    {
        private readonly KSContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public MemberHeadViewComponent(KSContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await GetCurrentUserAsync();

            var member = await _context.Member
                .Where(m => m.AspNetId.Equals(user.Id))
                .Include(m => m.ProvinceCodeNavigation)
                .FirstOrDefaultAsync();

            return await Task.FromResult((IViewComponentResult)View("Default", member));
        }

        //Current User Utils 1.0
        private Task<IdentityUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);
        //End Current User Utils 1.0
    }
}
