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

        public NotificationsController(KSContext context, UserManager<IdentityUser> userManager, ILogger<Notification> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<IActionResult> IndexAsync(int page = 1, int perPage = 25)
        {
            var currentUser = await KSCurrentUser.KSGetCurrentUserAsync(_userManager, HttpContext);
            var accountType = await KSCurrentUser.KSGetAccountType(_context, _userManager, HttpContext);

            List<Notification> notifications = new List<Notification>();

            //TODO maybe this can be condensed instead of if/else
            if (accountType == KSCurrentUser.AccountType.MEMBER)
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

        public async Task<IActionResult> ViewNotificationOrder(Guid notificationId)
        {
            var currentUser = await KSCurrentUser.KSGetCurrentUserAsync(_userManager, HttpContext);

            var notification = await _context.Notification
                .Where(n => n.NotificationId.Equals(notificationId))
                .Where(n => n.RecipientId.Equals(currentUser.Id))
                .FirstOrDefaultAsync();

            //TODO try/catch
            notification.Read = true;
            _context.Update(notification);
            await _context.SaveChangesAsync();

            return RedirectToAction("ViewOrder", "Order", new {id = notification.OrderId});
        }

        public async Task<IActionResult> MarkNotificationAsRead(Guid notificationId)
        {
            var currentUser = await KSCurrentUser.KSGetCurrentUserAsync(_userManager, HttpContext);

            var notification = await _context.Notification
                .Where(n => n.NotificationId.Equals(notificationId))
                .Where(n => n.RecipientId.Equals(currentUser.Id))
                .FirstOrDefaultAsync();

            //TODO try/catch
            notification.Read = true;
            _context.Update(notification);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}