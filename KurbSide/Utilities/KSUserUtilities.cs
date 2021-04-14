using System;
using System.Linq;
using System.Threading.Tasks;
using KurbSide.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;

namespace KurbSide.Utilities
{
    public class KSUserUtilities
    {
        /// <summary>
        /// Account types of a user on the KurbSide website.
        /// </summary>
        public enum AccountType
        {
            /// <summary>
            /// A Business Account
            /// </summary>
            BUSINESS,

            /// <summary>
            /// A Member Account
            /// </summary>
            MEMBER,

            /// <summary>
            /// A Visitor (Not logged in)
            /// </summary>
            VISITOR
        }

        /// <summary>
        /// Gets the current users account information.
        /// <br/>
        /// <code>Example: KSGetCurrentUserAsync(_userManager, HttpContext)</code>
        /// </summary>
        /// <remarks>
        /// Liam De Rivers
        /// </remarks>
        /// <param name="userManager">The IdentityUser UserManager.</param>
        /// <param name="httpContext">The HttpContext of the current session.</param>
        /// <returns>The currently logged in user, of type <see cref="IdentityUser"/></returns>
        public static async Task<IdentityUser> KSGetCurrentUserAsync(UserManager<IdentityUser> userManager,
            HttpContext httpContext)
        {
            var currentUser = httpContext.User;
            return await userManager.GetUserAsync(currentUser);
        }

        /// <summary>
        /// Gets the current <see cref="Member"/>s information.
        /// <br/>
        /// <code>Example: KSGetCurrentMemberAsync(_context, _userManager, HttpContext)</code>
        /// </summary>
        /// <remarks>
        /// Liam De Rivers
        /// </remarks>
        /// <param name="KSContext">The KurbSide context.</param>
        /// <param name="userManager">The IdentityUser UserManager.</param>
        /// <param name="httpContext">The HttpContext of the current session.</param>
        /// <returns></returns>
        public static async Task<Member> KSGetCurrentMemberAsync(KSContext KSContext,
            UserManager<IdentityUser> userManager,
            HttpContext httpContext)
        {
            var currentUser = await KSGetCurrentUserAsync(userManager, httpContext);
            var currentMember =
                await KSContext.Member.Where(m => m.AspNetId.Equals(currentUser.Id)).FirstOrDefaultAsync();
            return currentMember;
        }

        /// <summary>
        /// Gets the current <see cref="Business"/>s information.
        /// <br/>
        /// <code>Example: KSGetCurrentBusinessAsync(_context, _userManager, HttpContext)</code>
        /// </summary>
        /// <remarks>
        /// Liam De Rivers
        /// </remarks>
        /// <param name="KSContext">The KurbSide context.</param>
        /// <param name="userManager">The IdentityUser UserManager.</param>
        /// <param name="httpContext">The HttpContext of the current session.</param>
        /// <returns></returns>
        public static async Task<Business> KSGetCurrentBusinessAsync(KSContext KSContext,
            UserManager<IdentityUser> userManager,
            HttpContext httpContext)
        {
            var currentUser = await KSGetCurrentUserAsync(userManager, httpContext);
            var currentBusiness = await KSContext.Business
                .Where(b => b.AspNetId.Equals(currentUser.Id))
                .FirstOrDefaultAsync();
            return currentBusiness;
        }

        /// <summary>
        /// Gets the current users email.
        /// If the current user is not logged, returns an empty string.
        /// <br/>
        /// <code>Example: KSGetLoggedInEmailAsync(_userManager, HttpContext)</code>
        /// </summary>
        /// <remarks>
        /// Liam De Rivers
        /// </remarks>
        /// <param name="userManager">The IdentityUser UserManager.</param>
        /// <param name="httpContext">The HttpContext of the current session.</param>
        /// <returns>The currently logged in users email address.</returns>
        public static async Task<string> KSGetLoggedInEmailAsync(UserManager<IdentityUser> userManager,
            HttpContext httpContext)
        {
            var currentUser = await KSGetCurrentUserAsync(userManager, httpContext);
            return currentUser == null ? "" : currentUser.Email;
        }

        /// <summary>
        /// Gets the current users account type.
        /// <br/>
        /// <code>Example: KSGetAccountType(_context, _userManager, HttpContext)
        /// </code>
        /// </summary>
        /// <remarks>
        /// Liam De Rivers
        /// </remarks>
        /// <param name="KSContext">The KurbSide context.</param>
        /// <param name="userManager">The IdentityUser UserManager.</param>
        /// <param name="httpContext">The HttpContext of the current session.</param>
        /// <returns>The currently logged in users <see cref="AccountType"/>.</returns>
        public static async Task<AccountType> KSGetAccountType(KSContext KSContext,
            UserManager<IdentityUser> userManager,
            HttpContext httpContext)
        {
            var currentUser = await KSGetCurrentUserAsync(userManager, httpContext);

            if (currentUser == null) return AccountType.VISITOR; // If the current user is not logged in.
            if (KSContext.Business.Any(b => b.AspNetId.Equals(currentUser.Id)))
                return AccountType.BUSINESS; // The current user is present in the business table.
            if (KSContext.Member.Any(b => b.AspNetId.Equals(currentUser.Id)))
                return AccountType.MEMBER; // The current user is present in the member table.
            throw new Exception(
                "KurbSide.Service.KSUserUtilities.KSGetAccountType - Current user is neither a Business or Member"); // The current user is logged in, but not present in either the business or member table.
        }
    }
}