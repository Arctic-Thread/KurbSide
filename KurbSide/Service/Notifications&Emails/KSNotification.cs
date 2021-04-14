using System;
using System.Linq;
using System.Threading.Tasks;
using KurbSide.Models;
using KurbSide.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace KurbSide.Service
{
    public class KSNotification
    {
        /// <summary>
        /// Creates a notification for the designated recipient.
        /// </summary>
        /// <param name="KSContext">The KurbSide context.</param>
        /// <param name="senderId">The Id of the user who initiated the notification creation.</param>
        /// <param name="recipientId">The Id of the user to receive the notification.</param>
        /// <param name="details">The details for the notification, generated via <see cref="KSNotificationAndEmails.CreateMessage"/></param>
        /// <param name="saleId">If applicable* The Id of the sale associated with the notification.</param>
        /// <param name="orderId">If applicable* The Id of the order associated with the notification.</param>
        public static async Task CreateNotification(KSContext KSContext,
            string senderId,
            string recipientId,
            string details,
            Guid saleId = new Guid(),
            Guid orderId = new Guid())
        {
            Notification notification = new Notification
            {
                SenderId = senderId,
                RecipientId = recipientId,
                NotificationDetails = details,
                Read = false
            };

            if (saleId != new Guid())
            {
                notification.SaleId = saleId;
            }

            if (orderId != new Guid())
            {
                notification.OrderId = orderId;
            }

            await KSContext.Notification.AddAsync(notification);
            await KSContext.SaveChangesAsync();
        }

        /// <summary>
        /// Gets the number of unread notifications for the current user.
        /// </summary>
        /// <param name="KSContext">The KurbSide context.</param>
        /// <param name="userManager">The IdentityUser UserManager.</param>
        /// <param name="httpContext">The HttpContext of the current session.</param>
        /// <returns>An int for the number of unread notifications for the current user.</returns>
        public static async Task<int> GetUnreadNotificationCount(KSContext KSContext,
            UserManager<IdentityUser> userManager,
            HttpContext httpContext)
        {
            var currentUser = await KSCurrentUser.KSGetCurrentUserAsync(userManager, httpContext);
            return await KSContext.Notification
                .Where(n => n.RecipientId.Equals(currentUser.Id))
                .Where(n => n.Read == false)
                .CountAsync();
        }
    }
}