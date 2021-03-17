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
            //Member Information
            [Required(ErrorMessage = "You must enter a Phone Number.")]
            [RegularExpression(@"^\(?([2-9][0-9]{2})\)?[-. ]?([2-9](?!11)[0-9]{2})[-. ]?([0-9]{4})(x[0-9]{1,4})?$", ErrorMessage = "You must enter a valid Phone Number. e.g. (519)-885-0300x1234")]
            [Phone]
            [Display(Name = "Phone Number", Prompt = "Phone Number")]
            public string Phone { get; set; }

            [MaxLength(100, ErrorMessage = "The entered First Name is too long. 100 characters max.")]
            [Required(ErrorMessage = "You must enter your First Name.")]
            [Display(Name = "First Name", Prompt = "First Name")]
            public string FirstName { get; set; }

            [MaxLength(100, ErrorMessage = "The entered Last Name is too long. 100 characters max.")]
            [Required(ErrorMessage = "You must enter your Last Name.")]
            [Display(Name = "Last Name", Prompt = "Last Name")]
            public string LastName { get; set; }

            [MaxLength(10, ErrorMessage = "The entered Gender is too long. 10 characters max.")]
            [Required(ErrorMessage = "You must enter your Gender")]
            [Display(Name = "Gender", Prompt = "Gender")]
            public string Gender { get; set; }

            [Required(ErrorMessage = "You must enter your Birthday")]
            [Display(Name = "Birthday", Prompt = "Birthday")]
            public DateTime Birthday { get; set; }

            //Business Address
            [MaxLength(250, ErrorMessage = "The entered Address is too long. 250 characters max.")]
            [Required(ErrorMessage = "You must enter your Businesses Address.")]
            [Display(Name = "Street Address", Prompt = "Street Address")]
            public string Street { get; set; }

            [MaxLength(250, ErrorMessage = "The entered Address Line 2 can be no longer than 250 characters.")]
            [Display(Name = "Street Address Line 2", Prompt = "Street Address Line 2")]
            public string StreetLn2 { get; set; }

            //TODO Change this. Longest city name in the world is 176 characters. Longest in Canada is 68.
            [MaxLength(50, ErrorMessage = "The entered City is too long. 50 characters max.")]
            [Display(Name = "City", Prompt = "City")]
            public string City { get; set; }

            [Required(ErrorMessage = "You must enter your Postal Code.")]
            [RegularExpression(@"^(?i)[ABCEGHJKLMNPRSTVXY][0-9][ABCEGHJKLMNPRSTVWXYZ]([ ]|[-])?[0-9][ABCEGHJKLMNPRSTVWXYZ][0-9]$", ErrorMessage = "You must enter a valid postal code. e.g. K1A 0A9")]
            [Display(Name = "Postal Code", Prompt = "Postal Code")]
            public string Postal { get; set; }

            [MaxLength(2, ErrorMessage = "The entered Province is too long. 2 characters max.")]
            [Required(ErrorMessage = "You must enter your Businesses Province.")]
            [Display(Name = "Province", Prompt = "Province")]
            public string ProvinceCode { get; set; }

            [MaxLength(2, ErrorMessage = "The entered Country Code is too long. 2 characters max.")]
            [Required(ErrorMessage = "You must enter your Businesses Country.")]
            [Display(Name = "Country", Prompt = "Country")]
            public string CountryCode { get; set; }
        }

        private async Task LoadAsync(IdentityUser user)
        {
            var member = await _context.Member
                .Where(m => m.AspNetId.Equals(user.Id))
                .FirstOrDefaultAsync();

            Input = new InputModel
            {
                Phone = member.PhoneNumber,
                FirstName = member.FirstName,
                LastName = member.LastName,
                Gender = member.Gender,
                Birthday = member.Birthday,
                Street = member.Street,
                StreetLn2 = member.StreetLn2,
                City = member.City,
                Postal = member.Postal
            };

            ViewData["CountryCode"] = new SelectList(_context.Country, "CountryCode", "FullName", member.CountryCode);
            ViewData["ProvinceCode"] = new SelectList(_context.Province, "ProvinceCode", "FullName", member.ProvinceCode);
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
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            {
                var member = await _context.Member
                    .Where(m => m.AspNetId.Equals(user.Id))
                    .FirstOrDefaultAsync();

                string address = $"{Input.Street} {Input.City} {Input.ProvinceCode} {Input.CountryCode} {Input.Postal}";
                Service.Location location = await Service.GeoCode.GetLocationAsync(address);

                member.PhoneNumber = Input.Phone;
                member.FirstName = Input.FirstName;
                member.LastName = Input.LastName;
                member.Gender = Input.Gender;
                member.Birthday = Input.Birthday;
                member.Street = Input.Street;
                member.StreetLn2 = Input.StreetLn2;
                member.City = Input.City;
                member.Postal = Input.Postal;
                member.CountryCode = Input.CountryCode;
                member.ProvinceCode = Input.ProvinceCode;
                member.Lng = location.lng;
                member.Lat = location.lat;

                StatusMessage = "Your profile has been updated";
                _context.Member.Update(member);
                await _context.SaveChangesAsync();
            }

            await _signInManager.RefreshSignInAsync(user);
            return RedirectToPage();
        }
    }
}
