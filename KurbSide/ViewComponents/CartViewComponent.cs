﻿using KurbSide.Models;
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
    [ViewComponent(Name = "Cart")]
    public class CartViewComponent : ViewComponent
    {
        private readonly KSContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public CartViewComponent(KSContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var currentMember = await KSUserUtilities.KSGetCurrentMemberAsync(_context, _userManager, HttpContext);

            var cart = await _context.Cart
                .Where(c => c.MemberId.Equals(currentMember.MemberId))
                .Include(c => c.Business)
                .Include(c => c.CartItem)
                .ThenInclude(ci => ci.Item)
                .FirstOrDefaultAsync();
            
            List<Sale> sales = null;
            if (cart != null)
            {
                if (cart.CartItem.Count > 0)
                {
                    sales = await _context.Sale
                        .Where(b => b.BusinessId.Equals(cart.BusinessId))
                        .ToListAsync();

                    var removedItems = cart.CartItem.Where(i => i.Item.Removed == true).ToList();

                    _context.CartItem.RemoveRange(removedItems);
                    await _context.SaveChangesAsync();
                }
                
                if (cart.CartItem.Count <= 0)
                {
                    _context.Cart.Remove(cart);
                    await _context.SaveChangesAsync();
                }
            }

            if (cart != null && cart.ExpiryDate < DateTime.Today)
            {
                _context.CartItem.RemoveRange(cart.CartItem);
                _context.Cart.RemoveRange(cart);
                await _context.SaveChangesAsync();
            }

            var updatedCart = await _context.Cart
                .Where(c => c.MemberId.Equals(currentMember.MemberId))
                .Include(c => c.Business)
                .Include(c => c.CartItem)
                .ThenInclude(ci => ci.Item)
                .FirstOrDefaultAsync();

            return await Task.FromResult((IViewComponentResult)View("Default", Tuple.Create(updatedCart, sales)));
        }
    }
}
