﻿using KurbSide.Models;
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
        private readonly ILogger<Order> _logger;

        public OrderController(KSContext context, UserManager<IdentityUser> userManager, ILogger<Order> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CartUpdateAsync(Guid id, int q = 0)
        {
            var currentMember = await KSCurrentUser.KSGetCurrentMemberAsync(_context, _userManager, HttpContext);
            var accountType = await KSCurrentUser.KSGetAccountType(_context, _userManager, HttpContext);
            //If the currently logged in user is not a member they can not access the store.
            if (accountType != KSCurrentUser.AccountType.MEMBER)
            {
                TempData["sysMessage"] = "Error: You're not signed in as a member.";
                return RedirectToAction("Index", "Home");
            }

            //Check that the requested item exists
            var item = await _context.Item.FirstOrDefaultAsync(i => i.ItemId.Equals(id));
            if (item == null) return RedirectToAction("Index", "Home");

            if (q == null || q <= 0) return Redirect(HttpContext.Request.Headers["Referer"]);

            //Retrieve the store
            var business = await _context.Business.FirstOrDefaultAsync(b => b.BusinessId.Equals(item.BusinessId));

            if (business != null)
            {
                var cart = await _context.Cart
                               .Where(c => c.MemberId.Equals(currentMember.MemberId))
                               .Include(c => c.Business)
                               .Include(c => c.CartItem)
                               .FirstOrDefaultAsync() ??
                           new Cart
                           {
                               BusinessId = item.BusinessId,
                               MemberId = currentMember.MemberId,
                               ExpiryDate = DateTime.Now.AddDays(1)
                           };

                //Validate the cart
                if (!cart.BusinessId.Equals(business.BusinessId))
                {
                    TempData["sysMessage"] = $"Error: Your current cart is with {cart.Business.BusinessName}," +
                                             $" please clear your cart to shop with {business.BusinessName}.";
                    return Redirect(HttpContext.Request.Headers["Referer"]);
                }

                _context.Cart.Update(cart);
                await _context.SaveChangesAsync();

                //Add quantity/Create cart item
                var cartItem = cart.CartItem
                                   .Where(ci => ci.ItemId.Equals(id))
                                   .Select(ci => { ci.Quantity = q; return ci; })
                                   .FirstOrDefault() ??
                               new CartItem
                               {
                                   CartId = cart.CartId,
                                   ItemId = item.ItemId,
                                   Quantity = q
                               };
                try
                {
                    await _context.CartItem.AddAsync(cartItem);
                    await _context.SaveChangesAsync();
                }
                catch (Exception)
                {
                    _context.CartItem.Update(cartItem);
                    await _context.SaveChangesAsync();
                }

            }

            return Redirect(HttpContext.Request.Headers["Referer"]);
        }

        [HttpPost]
        public async Task<IActionResult> CartAddAsync(Guid id, int q = 1)
        {
            var currentMember = await KSCurrentUser.KSGetCurrentMemberAsync(_context, _userManager, HttpContext);
            var accountType = await KSCurrentUser.KSGetAccountType(_context, _userManager, HttpContext);
            //If the currently logged in user is not a member they can not access the store.
            if (accountType != KSCurrentUser.AccountType.MEMBER)
            {
                TempData["sysMessage"] = "Error: You're not signed in as a member.";
                return RedirectToAction("Index", "Home");
            }

            //Check that the requested item exists
            var item = await _context.Item.FirstOrDefaultAsync(i => i.ItemId.Equals(id));
            if (item == null) return RedirectToAction("Index", "Home");

            //Retrieve the store
            var business = await _context.Business.FirstOrDefaultAsync(b => b.BusinessId.Equals(item.BusinessId));

            if (business != null)
            {
                var cart = await _context.Cart
                               .Where(c => c.MemberId.Equals(currentMember.MemberId))
                               .Include(c => c.Business)
                               .Include(c => c.CartItem)
                               .FirstOrDefaultAsync() ??
                           new Cart
                           {
                               BusinessId = item.BusinessId,
                               MemberId = currentMember.MemberId,
                               ExpiryDate = DateTime.Now.AddDays(1)
                           };

                //Validate the cart
                if (!cart.BusinessId.Equals(business.BusinessId))
                {
                    TempData["sysMessage"] = $"Error: Your current cart is with {cart.Business.BusinessName}," +
                                             $" please clear your cart to shop with {business.BusinessName}.";
                    return Redirect(HttpContext.Request.Headers["Referer"]);
                }

                _context.Cart.Update(cart);
                await _context.SaveChangesAsync();

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
                    await _context.CartItem.AddAsync(cartItem);
                    await _context.SaveChangesAsync();
                }
                catch (Exception)
                {
                    _context.CartItem.Update(cartItem);
                    await _context.SaveChangesAsync();
                }

                TempData["sysMessage"] = $"{item.ItemName} has been added to cart.";
            }

            return Redirect(HttpContext.Request.Headers["Referer"]);
        }

        public async Task<IActionResult> CartRemoveAsync(Guid id)
        {
            var currentMember = await KSCurrentUser.KSGetCurrentMemberAsync(_context, _userManager, HttpContext);
            var accountType = await KSCurrentUser.KSGetAccountType(_context, _userManager, HttpContext);
            //If the currently logged in user is not a member they can not access the store.
            if (accountType != KSCurrentUser.AccountType.MEMBER)
            {
                TempData["sysMessage"] = "Error: You're not signed in as a member.";
                return RedirectToAction("Index", "Home");
            }

            //Check that the requested item exists
            var item = _context.Item.FirstOrDefault(i => i.ItemId.Equals(id));
            if (item == null) return RedirectToAction("Index", "Home");

            var cart = await _context.Cart
                .Where(c => c.MemberId.Equals(currentMember.MemberId))
                .FirstOrDefaultAsync();

            var cartItem = await _context.CartItem
                .Where(ci => ci.CartId.Equals(cart.CartId))
                .Where(ci => ci.ItemId.Equals(id))
                .FirstOrDefaultAsync();

            _context.CartItem.Remove(cartItem);
            await _context.SaveChangesAsync();

            if (!_context.CartItem.Where(ci => ci.CartId.Equals(cart.CartId)).Any())
            {
                _context.Cart.Remove(cart);
                _context.SaveChanges();
            }

            return Redirect(HttpContext.Request.Headers["Referer"]);
        }

        [Route("ClearCart")]
        public async Task<IActionResult> CartClearAsync()
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

            var cart = _context.Cart
                .Where(c => c.MemberId.Equals(currentMember.MemberId))
                .Include(c => c.CartItem)
                .FirstOrDefault();

            _context.CartItem.RemoveRange(cart.CartItem);
            _context.Cart.Remove(cart);
            _context.SaveChanges();

            return Redirect(HttpContext.Request.Headers["Referer"]);
        }

        [Route("Checkout")]
        public async Task<IActionResult> Checkout()
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

            var cart = _context.Cart
                .Where(c => c.MemberId.Equals(currentMember.MemberId))
                .Include(c => c.Member)
                .Include(c => c.Business)
                .Include(c => c.CartItem)
                .ThenInclude(ci => ci.Item)
                .FirstOrDefault();

            if (cart == null || !cart.CartItem.Any())
            {
                return RedirectToAction("Index", "Store");
            }

            return View(cart);
        }


    }
}