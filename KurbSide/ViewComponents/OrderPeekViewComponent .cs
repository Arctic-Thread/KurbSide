using KurbSide.Models;
using KurbSide.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace KurbSide.ViewComponents
{
    [ViewComponent(Name = "OrderPeek")]
    public class OrderPeekViewComponent : ViewComponent
    {
        private readonly KSContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public OrderPeekViewComponent(KSContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(int view = 0)
        {
            var currentUser = await KSCurrentUser.KSGetCurrentUserAsync(_userManager, HttpContext);
            var currentBusiness = await _context.Business
                .Where(b => b.AspNetId.Equals(currentUser.Id))
                .Include(b => b.Order)
                .ThenInclude(o => o.StatusNavigation)
                .Include(b => b.Order)
                .ThenInclude(o => o.Member)
                .Include(b => b.Order)
                .ThenInclude(o => o.OrderItem)
                .ThenInclude(oi => oi.Item)
                .FirstOrDefaultAsync();

            var businessOrders = currentBusiness.Order
                .OrderBy(o => o.CreationDate)
                .OrderBy(o => o.Status)
                .ToList();

            switch (view)
            {
                case 1: //Pending
                    businessOrders = businessOrders
                        .Where(bo => bo.Status.Equals(0))
                        .ToList();
                    break;
                case 2: //Open
                    businessOrders = businessOrders
                        .Where(bo => !bo.Status.Equals(0))
                        .Where(bo => !bo.Status.Equals(4))
                        .Where(bo => !bo.Status.Equals(5))
                        .Where(bo => !bo.Status.Equals(6))
                        .ToList();
                    break;
                case 3: //Closed
                    businessOrders = businessOrders
                        .Where(bo => bo.Status.Equals(4) || bo.Status.Equals(5) || bo.Status.Equals(6))
                        .ToList();
                    break;
                default: //Broken
                    businessOrders = new List<Order>();
                    break;
            }

            return await Task.FromResult((IViewComponentResult)View("Default", businessOrders.Take(10)));
        }
    }
}
