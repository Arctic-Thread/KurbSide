using KurbSide.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;

namespace KurbSide.Areas.Identity.Pages.Account.Manage
{
    public partial class PreferencesModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly KSContext _context;
        private readonly ILogger<AccountSettings> _logger;

        public PreferencesModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            KSContext context,
            ILogger<AccountSettings> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _logger = logger;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Display(Name = "Time Zone")]
            public Guid? TimeZoneId { get; set; }
            [Display(Name = "Would you like to receive promotional emails?")]
            public bool PromotionalEmails { get; set; }
        }

        private async Task LoadAsync(IdentityUser currentUser)
        {
            var accountSettings = await _context.AccountSettings
                .Where(a => a.AspNetId.Equals(currentUser.Id))
                .FirstOrDefaultAsync();

            Input = new InputModel
            {
                TimeZoneId = accountSettings.TimeZoneId,
                PromotionalEmails = accountSettings.PromotionalEmails
            };

            ViewData["TimeZoneId"] = new SelectList(_context.TimeZones, "TimeZoneId", "Label", accountSettings.TimeZoneId);
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(currentUser);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            try
            {
                if (!ModelState.IsValid)
                {
                    await LoadAsync(currentUser);
                    return Page();
                }

                var accountSettings = await _context.AccountSettings
                    .Where(a => a.AspNetId.Equals(currentUser.Id))
                    .FirstOrDefaultAsync();

                accountSettings.TimeZoneId = Input.TimeZoneId;
                accountSettings.PromotionalEmails = Input.PromotionalEmails;

                StatusMessage = "Your profile has been updated";
                _context.AccountSettings.Update(accountSettings);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogDebug($"{ex.GetBaseException().Message}. Error updating profile");
            }

            await _signInManager.RefreshSignInAsync(currentUser);
            return RedirectToPage();
        }
    }
}
