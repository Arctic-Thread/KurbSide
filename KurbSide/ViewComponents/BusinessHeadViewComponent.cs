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
            var business = await KSUserUtilities.KSGetCurrentBusinessAsync(_context, _userManager, HttpContext);
            return await Task.FromResult((IViewComponentResult) View("Default", business));
        }
    }
}