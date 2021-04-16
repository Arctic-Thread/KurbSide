using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KurbSide.Models;
using KurbSide.Utilities;
using KurbSideUtils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace KurbSide.Controllers
{
    [Authorize]
    public class NotificationsController : Controller
    {
        private readonly KSContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<Notification> _logger;

        public NotificationsController(KSContext context, UserManager<IdentityUser> userManager,
            ILogger<Notification> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        /// <summary>
        /// Displays a list of unread notifications for the business/member.
        /// </summary>
        /// <param name="page">The page the business/member is currently on.</param>
        /// <param name="perPage">The number of notifications to be viewed per page.</param>
        /// <returns></returns>
        public async Task<IActionResult> IndexAsync(int page = 1, int perPage = 25)
        {
            var currentUser = await KSUserUtilities.KSGetCurrentUserAsync(_userManager, HttpContext);
            var accountType = await KSUserUtilities.KSGetAccountType(_context, _userManager, HttpContext);

            List<Notification> notifications = new List<Notification>();

            if (accountType == KSUserUtilities.AccountType.MEMBER)
            {
                notifications = await _context.Notification
                    .Include(s => s.Sender.Business)
                    .Include(r => r.Recipient.Member)
                    .Include(o => o.Order)
                    .ThenInclude(b => b.Business)
                    .Where(n => n.RecipientId.Equals(currentUser.Id))
                    .Where(n => n.Read == false)
                    .ToListAsync();
            }
            else
            {
                notifications = await _context.Notification
                    .Include(s => s.Sender.Member)
                    .Include(r => r.Recipient.Business)
                    .Where(n => n.RecipientId.Equals(currentUser.Id))
                    .Where(n => n.Read == false)
                    .ToListAsync();
            }

            var paginatedList = KSPaginatedList<Notification>.Create(notifications.AsQueryable(), page, perPage);

            TempData["currentPage"] = page;
            TempData["totalPage"] = paginatedList.TotalPages;
            TempData["perPage"] = perPage;
            TempData["hasNextPage"] = paginatedList.HasNextPage;
            TempData["hasPrevPage"] = paginatedList.HasPreviousPage;


            return View(paginatedList);
        }

        /// <summary>
        /// Marks a notification as read and then redirects the user to the order associated
        /// with the notification.
        /// </summary>
        /// <param name="notificationId">The Id of the notification</param>
        /// <returns>A redirect to the view order page.</returns>
        public async Task<IActionResult> ViewNotificationOrder(Guid notificationId)
        {
            var currentUser = await KSUserUtilities.KSGetCurrentUserAsync(_userManager, HttpContext);

            var notification = await _context.Notification
                .Where(n => n.NotificationId.Equals(notificationId))
                .Where(n => n.RecipientId.Equals(currentUser.Id))
                .FirstOrDefaultAsync();

            try
            {
                notification.Read = true;
                _context.Update(notification);
                await _context.SaveChangesAsync();

                return RedirectToAction("ViewOrder", "Order", new {id = notification.OrderId});
            }
            catch (DbUpdateConcurrencyException)
            {
                _logger.LogError("DbUpdateConcurrencyException while updating notification.");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.GetBaseException().Message}.");
                return RedirectToAction("Index");
            }
        }

        /// <summary>
        /// Marks a notification as read.
        /// </summary>
        /// <param name="notificationId">The Id of the notification</param>
        public async Task<IActionResult> MarkNotificationAsRead(Guid notificationId)
        {
            var currentUser = await KSUserUtilities.KSGetCurrentUserAsync(_userManager, HttpContext);

            var notification = await _context.Notification
                .Where(n => n.NotificationId.Equals(notificationId))
                .Where(n => n.RecipientId.Equals(currentUser.Id))
                .FirstOrDefaultAsync();

            try
            {
                notification.Read = true;
                _context.Update(notification);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                _logger.LogError("DbUpdateConcurrencyException while updating notification.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.GetBaseException().Message}.");
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> MarkAllNotificationAsRead()
        {
            var currentUser = await KSUserUtilities.KSGetCurrentUserAsync(_userManager, HttpContext);

            var notifications = await _context.Notification
                .Where(n => n.RecipientId.Equals(currentUser.Id))
                .ToListAsync();

            try
            {
                _context.Notification.RemoveRange(notifications);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                _logger.LogError("DbUpdateConcurrencyException while updating notification.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.GetBaseException().Message}.");
            }

            return RedirectToAction("Index");
        }
    }
}