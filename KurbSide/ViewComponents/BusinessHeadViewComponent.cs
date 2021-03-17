using KurbSide.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KurbSide.Utilities;

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
            var user = await KSCurrentUser.KSGetCurrentUserAsync(_userManager, HttpContext);

            var business = await _context.Business
                .Where(b => b.AspNetId.Equals(user.Id))
                .FirstOrDefaultAsync();

            return await Task.FromResult((IViewComponentResult)View("Default", business));
        }
    }
}
