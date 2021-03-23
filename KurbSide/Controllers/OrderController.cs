using KurbSide.Models;
using KurbSide.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KurbSide.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly KSContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<Item> _logger;

        public OrderController(KSContext context, UserManager<IdentityUser> userManager, ILogger<Item> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        public void checkCart(IdentityUser Iuser, KSContext context)
        {
            var member = context.Member
                .Where(u => u.AspNetId.Equals(Iuser.Id))
                .FirstOrDefault();

            var cart = context.Cart
                .Where(c => c.MemberId.Equals(member.MemberId))
                .Include(c => c.CartItem)
                .FirstOrDefault();

            if (cart.ExpiryDate < DateTime.Now)
            {
                context.CartItem.RemoveRange(cart.CartItem);
                context.Cart.Remove(cart);
                context.SaveChanges();
            }
        }

        [HttpPost]
        public async Task<IActionResult> CartAddAsync(Guid id, string r = "", int q = 1)
        {
            var currentUser = await KSCurrentUser.KSGetCurrentUserAsync(_userManager, HttpContext);
            var currentMember = await _context.Member
                .Where(m => m.AspNetId.Equals(currentUser.Id))
                .FirstOrDefaultAsync();
            var accountType = await KSCurrentUser.KSGetAccountType(_context, _userManager, HttpContext);
            //If the currently logged in user is not a member they can not access the store.
            if (accountType != KSCurrentUser.AccountType.MEMBER)
            {
                TempData["sysMessage"] = "Error: You're not signed in as a member.";
                return RedirectToAction("Index", "Home");
            }

            //Check that the requested item exists
            var item = _context.Item
                .Where(i => i.ItemId.Equals(id))
                .FirstOrDefault();
            if (item == null) return RedirectToAction("Index", "Home");

            //Retreive the store
            var store = _context.Business
                .Where(b => b.BusinessId.Equals(item.BusinessId))
                .FirstOrDefault();

            //Retreive or create the cart
            //TODO Re-visit cart expiry
            //checkCart(currentUser, _context);
            var cart = _context.Cart
                .Where(c => c.MemberId.Equals(currentMember.MemberId))
                .Include(c => c.Business)
                .Include(c => c.CartItem)
                .FirstOrDefault() ??
                new Cart
                {
                    BusinessId = item.BusinessId,
                    MemberId = currentMember.MemberId,
                    ExpiryDate = DateTime.Now.AddDays(1)
                };

            //Validate the cart
            if (!cart.BusinessId.Equals(store.BusinessId))
            {
                TempData["sysMessage"] = $"Error: Your current cart is with {cart.Business.BusinessName}," +
                    $" please clear your cart to shop with {store.BusinessName}.";
                return string.IsNullOrWhiteSpace(r) ?
                    RedirectToAction("Catalogue", "Store", new { id = store.BusinessId }) :
                    (IActionResult)Redirect(r);
            }
            else
            {
                _context.Cart.Update(cart);

                //Add quantity/Create cart item
                var cartItem = cart.CartItem
                    .Where(ci => ci.ItemId.Equals(id))
                    .Select(ci => { ci.Quantity += q; return ci; })
                    .FirstOrDefault() ??
                    new CartItem
                    {
                        CartId = cart.CartId,
                        ItemId = item.ItemId,
                        Quantity = 1
                    };
                try
                { 
                    _context.CartItem.Add(cartItem);
                    await _context.SaveChangesAsync();
                }
                catch (Exception)
                {
                    _context.CartItem.Update(cartItem);
                    await _context.SaveChangesAsync();
                }

                TempData["sysMessage"] = $"{item.ItemName} has been added to cart.";
            }


            return string.IsNullOrWhiteSpace(r)?
                RedirectToAction("Catalogue", "Store", new { id = store.BusinessId }) :
                (IActionResult)Redirect(r);
        }

    }
}
