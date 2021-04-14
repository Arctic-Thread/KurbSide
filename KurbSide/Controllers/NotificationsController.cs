using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KurbSide.Models;
using KurbSide.Utilities;
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

        public async Task<IActionResult> IndexAsync()
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
                    .Where(n => n.RecipientId.Equals(currentUser.Id))
                    .ToListAsync();
            }
            else
            {
                notifications = await _context.Notification
                    .Include(s => s.Sender.Member)
                    .Include(r => r.Recipient.Business)
                    .Where(n => n.RecipientId.Equals(currentUser.Id))
                    .ToListAsync();
            }

            return View(notifications);
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            var accountType = await KSCurrentUser.KSGetAccountType(_context, _userManager, HttpContext);

            Notification notification;

            //TODO maybe this can be condensed instead of if/else
            if (accountType == KSCurrentUser.AccountType.MEMBER)
            {
                notification = await _context.Notification
                    .Include(n => n.Order)
                    .ThenInclude(n => n.OrderItem)
                    .ThenInclude(n => n.Item)
                    .Include(n => n.Sender)
                    .ThenInclude(n => n.Business)
                    .Include(n => n.Sale)
                    .Where(n => n.NotificationId.Equals(id))
                    .FirstOrDefaultAsync(m => m.NotificationId == id);
            }
            else
            {
                notification = await _context.Notification
                    .Include(n => n.Order)
                    .ThenInclude(n => n.OrderItem)
                    .ThenInclude(n => n.Item)
                    .Include(n => n.Sender)
                    .ThenInclude(n => n.Member)
                    .Include(n => n.Sale)
                    .Where(n => n.NotificationId.Equals(id))
                    .FirstOrDefaultAsync();
            }

            //TODO
            if (notification == null)
            {
                return NotFound();
            }

            //TODO try/catch
            notification.Read = true;
            _context.Update(notification);
            await _context.SaveChangesAsync();
            
            return View(notification);
        }
    }
}