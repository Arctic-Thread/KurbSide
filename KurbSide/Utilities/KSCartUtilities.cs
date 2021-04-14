using System;
using System.Linq;
using KurbSide.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace KurbSide.Utilities
{
    public class KSCartUtilities
    {
        /// <summary>
        /// Checks if the currently logged in members cart has expired.
        /// If it has, the cart and all cart items are removed.
        /// <br/>
        /// <code>
        /// Example: KSCheckIfCartExpired(_context, _userManager, HttpContext);
        /// </code>
        /// </summary>
        /// <remarks>
        /// Liam De Rivers
        /// </remarks>
        /// <param name="ksContext">The KurbSide context.</param>
        /// <param name="userManager">The IdentityUser UserManager.</param>
        /// <param name="httpContext">The HttpContext of the current session.</param>
        public static async void KSCheckIfCartExpired(KSContext ksContext, UserManager<IdentityUser> userManager,
            HttpContext httpContext)
        {
            var currentMember = await KSUserUtilities.KSGetCurrentMemberAsync(ksContext, userManager, httpContext);

            var cart = await ksContext.Cart
                .AsNoTracking()
                .Where(c => c.MemberId.Equals(currentMember.MemberId))
                .Include(c => c.CartItem)
                .FirstOrDefaultAsync();

            if (cart == null)
                return;

            if (cart.ExpiryDate < DateTime.Now)
            {
                ksContext.CartItem.RemoveRange(cart.CartItem);
                ksContext.Cart.Remove(cart);
                await ksContext.SaveChangesAsync();
            }
        }
    }
}