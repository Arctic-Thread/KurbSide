using System.Collections.Generic;
using KurbSide.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Linq;
using KurbSide.Utilities;
using Microsoft.EntityFrameworkCore;

namespace KurbSide.Areas.Identity.Pages.Account.Manage
{
    public partial class OrdersModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly KSContext _context;

        public OrdersModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            KSContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        public Dictionary<int, IEnumerable<Order>> Orders { get; set; }
        public IEnumerable<OrderStatus> Status { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var currentMember = await KSUserUtilities.KSGetCurrentMemberAsync(_context, _userManager, HttpContext);
            var accountType = await KSUserUtilities.KSGetAccountType(_context, _userManager, HttpContext);
            //If the currently logged in user is not a member they can not access the store.
            if (accountType != KSUserUtilities.AccountType.MEMBER)
            {
                TempData["sysMessage"] = "Error: You're not signed in as a member.";
                return RedirectToAction("Index", "Home");
            }

            var userOrders = await _context.Order
                .Where(o => o.MemberId.Equals(currentMember.MemberId))
                .Include(o => o.Member)
                .Include(o => o.Business)
                .Include(o => o.StatusNavigation)
                .Include(o => o.OrderItem)
                .ThenInclude(oi => oi.Item)
                .ToListAsync();

            var activeCategories = userOrders
                .Select(o => o.StatusNavigation)
                .Distinct()
                .ToList()
                .OrderBy(s => s.StatusId);
            
            var orderDict = userOrders
                .GroupBy(o => o.Status)
                .ToDictionary(i => i.Key, i => i.AsEnumerable());;

            Orders = orderDict;
            Status = activeCategories;
            
            return Page();
        }
    }
}
