using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using KurbSide.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace KurbSide.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class MemberRegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly KSContext _context;

        public MemberRegisterModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            KSContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            //General Account Information
            [Required]
            [EmailAddress]
            [Display(Name = "Email", Prompt = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password", Prompt = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password", Prompt = "Confirm Password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            //Member Information
            [Required(ErrorMessage = "You must enter a Phone Number.")]
            [RegularExpression(@"^\(?([2-9][0-9]{2})\)?[-. ]?([2-9](?!11)[0-9]{2})[-. ]?([0-9]{4})(x[0-9]{1,4})?$", ErrorMessage = "You must enter a valid Phone Number. e.g. (519)-123-1234x1234")]
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

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = Input.Email, Email = Input.Email };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    //Create the member
                    {
                        var newMember = new Member
                        {
                            AspNetId = user.Id,

                            FirstName = Input.FirstName,
                            LastName = Input.LastName,
                            Street = Input.Street,
                            StreetLn2 = Input.StreetLn2,
                            City = Input.City,
                            Postal = Input.Postal,
                            ProvinceCode = Input.ProvinceCode,
                            CountryCode = Input.CountryCode,
                            PhoneNumber = Input.Phone,
                            Gender = Input.Gender,
                            Birthday = Input.Birthday
                        };

                        _context.Member.Add(newMember);
                        await _context.SaveChangesAsync();
                    }

                    //Force EST for now.
                    var est = _context.TimeZones
                        .Where(tz => tz.Offset.Equals("-05:00"))
                        .Select(tz => tz.TimeZoneId)
                        .FirstOrDefault();
                    var userPrefs = new AccountSettings
                    {
                        AspNetId = user.Id,
                        TimeZoneId = est
                    };
                    _context.AccountSettings.Add(userPrefs);
                    await _context.SaveChangesAsync();

                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    TempData["sysMessage"] = $"We've sent an email to {Input.Email}, Please confirm your account to continue.";
                    return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });

                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form

            ViewData["CountryCode"] = new SelectList(_context.Country, "CountryCode", "FullName", "CA");
            ViewData["ProvinceCode"] = new SelectList(_context.Province, "ProvinceCode", "FullName", "ON");
            return Page();
        }
    }
}
