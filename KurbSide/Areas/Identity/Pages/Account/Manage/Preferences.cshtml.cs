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

namespace KurbSide.Areas.Identity.Pages.Account.Manage
{
    public partial class PreferencesModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly KSContext _context;

        public PreferencesModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            KSContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Display(Name = "Time Zone")]
            public Guid? TimeZoneID { get; set; }
        }

        private async Task LoadAsync(IdentityUser iUser)
        {
            var user = await _context.AccountSettings
                .Where(m => m.AspNetId.Equals(iUser.Id))
                .FirstOrDefaultAsync();

            Input = new InputModel
            {
                TimeZoneID = user.TimeZoneId,
            };

            ViewData["TimeZoneID"] = new SelectList(_context.TimeZones, "TimeZoneId", "Label", user.TimeZoneId);
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                _lo
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            {
                var accountSettings = await _context.AccountSettings
                    .Where(a => a.AspNetId.Equals(user.Id))
                    .FirstOrDefaultAsync();

                accountSettings.TimeZoneId = Input.TimeZoneID;

                StatusMessage = "Your profile has been updated";
                _context.AccountSettings.Update(accountSettings);
                await _context.SaveChangesAsync();
            }

            await _signInManager.RefreshSignInAsync(user);
            return RedirectToPage();
        }
    }
}
