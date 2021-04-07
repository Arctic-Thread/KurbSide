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
                                   .Select(ci =>
                                   {
                                       ci.Quantity = q;
                                       return ci;
                                   })
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
                                   .Select(ci =>
                                   {
                                       ci.Quantity += q;
                                       return ci;
                                   })
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

            try
            {
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
            }
            catch (Exception){}

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

            if (cart == null) // If the member does not have a cart (e.g. they clear all items while viewing checkout.)
                return View();

            var cartItems = await _context.CartItem
                .Where(m => m.CartId.Equals(cart.CartId))
                .Include(i => i.Item)
                .ThenInclude(si => si.SaleItem)
                .ThenInclude(s => s.Sale)
                .ToListAsync();

            if (cartItems.Count == 0) // If the member has a cart with no items.
                return View();

            decimal discountTotal = 0;
            
            var businessSales = await _context.Sale
                .Where(s => s.BusinessId.Equals(cart.BusinessId))
                .ToListAsync();
            
            foreach (var cartItem in cartItems)
            {
                var saleId = KSOrderUtilities.KSCheckIfItemInSale(cartItem.Item, businessSales);
                if (saleId != new Guid())
                {
                    var sale = await _context.Sale.Where(s => s.SaleId.Equals(saleId)).FirstOrDefaultAsync();
                    discountTotal += (decimal)(cartItem.Item.Price * cartItem.Quantity) * sale.SaleDiscountPercentage;
                }
            }

            ViewData["sales"] = businessSales;
            ViewData["discountTotal"] = discountTotal;
            return View(cart);
        }

        public async Task<IActionResult> PlaceOrder()
        {
            var currentUser = await KSCurrentUser.KSGetCurrentUserAsync(_userManager, HttpContext);
            var currentMember = await _context.Member
                .Where(m => m.AspNetId.Equals(currentUser.Id))
                .Include(p => p.ProvinceCodeNavigation)
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

            var cartItems = await _context.CartItem
                .Where(m => m.CartId.Equals(cart.CartId))
                .Include(i => i.Item)
                .ThenInclude(si => si.SaleItem)
                .ThenInclude(s => s.Sale)
                .ToListAsync();

            if (cart == null || !cart.CartItem.Any())
            {
                return RedirectToAction("Index", "Store");
            }

            var cartSubTotal = (decimal) cartItems.Sum(ci => ci.Quantity * ci.Item.Price).Value;

            decimal discountTotal = 0;
            
            var businessSales = await _context.Sale
                .Where(s => s.BusinessId.Equals(cart.BusinessId))
                .ToListAsync();
            
            foreach (var cartItem in cartItems)
            {
                var saleId = KSOrderUtilities.KSCheckIfItemInSale(cartItem.Item, businessSales);
                if (saleId != new Guid())
                {
                    var sale = await _context.Sale.Where(s => s.SaleId.Equals(saleId)).FirstOrDefaultAsync();
                    discountTotal += (decimal)(cartItem.Item.Price * cartItem.Quantity) * sale.SaleDiscountPercentage;
                }
            }

            cartSubTotal -= discountTotal;
            var taxRate = currentMember.ProvinceCodeNavigation.TaxRate;
            var taxTotal = taxRate * cartSubTotal;
            var pendingOrderStatus =
                await _context.OrderStatus.Where(s => s.StatusName.Equals("Pending")).FirstOrDefaultAsync();

            try
            {

                //for (int i = 0; i < 10; i++)
                //{
                //    var order2 = new Order
                //    {
                //        MemberId = currentMember.MemberId,
                //        SubTotal = cartSubTotal,
                //        DiscountTotal = 0, //TODO Discounts & Sales
                //        Tax = taxTotal,
                //        GrandTotal = cartSubTotal + taxRate,
                //        Status = pendingOrderStatus.StatusId,
                //        CreationDate = DateTime.Now,
                //        BusinessId = cart.BusinessId
                //    };

                //    await _context.Order.AddAsync(order2);
                //    await _context.SaveChangesAsync();

                //    List<OrderItem> orderItems2 = new List<OrderItem>();

                //    foreach (var cartItem in cartItems)
                //    {
                //        var orderItem = new OrderItem
                //        {
                //            OrderId = order2.OrderId,
                //            ItemId = cartItem.ItemId,
                //            Quantity = cartItem.Quantity,
                //            Discount = 0 //TODO Discounts & Sales
                //        };

                //        orderItems2.Add(orderItem);
                //        await _context.OrderItem.AddAsync(orderItem);
                //        await _context.SaveChangesAsync();
                //    }
                //}

                var order = new Order
                {
                    MemberId = currentMember.MemberId,
                    SubTotal = cartSubTotal,
                    DiscountTotal = discountTotal,
                    Tax = taxTotal,
                    GrandTotal = cartSubTotal + cartSubTotal * taxRate,
                    Status = pendingOrderStatus.StatusId,
                    CreationDate = DateTime.Now,
                    BusinessId = cart.BusinessId
                };

                await _context.Order.AddAsync(order);
                await _context.SaveChangesAsync();

                List<OrderItem> orderItems = new List<OrderItem>();

                foreach (var cartItem in cartItems)
                {
                    var orderItem = new OrderItem
                    {
                        OrderId = order.OrderId,
                        ItemId = cartItem.ItemId,
                        Quantity = cartItem.Quantity,
                        Discount = discountTotal
                    };

                    orderItems.Add(orderItem);
                    await _context.OrderItem.AddAsync(orderItem);
                    await _context.SaveChangesAsync();
                }

                _context.CartItem.RemoveRange(cart.CartItem);
                _context.Cart.Remove(cart);
                await _context.SaveChangesAsync();

                var orderDetails = Tuple.Create(order, orderItems);
                return View("OrderConfirmation", orderDetails);
            }
            catch (Exception ex)
            {
                _logger.LogDebug("Error during checkout: " + ex.GetBaseException().Message);
                ViewData["sysMessage"] = "Error: Something went wrong.";
            }

            return RedirectToAction("Index", "Store");
        }
    }
}