﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using KurbSide.Annotations;
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
    public class BusinessRegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly KSContext _context;

        public BusinessRegisterModel(
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
            [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            //Business Information
            [Required]
            [Display(Name = "Business name", Prompt = "Business Name")]
            public string BusinessName { get; set; }
            [Required]
            [Phone]
            [Display(Name = "Phone Number", Prompt = "Phone Number")]
            public string PhoneNumber { get; set; }

            //Primary Contact
            [Required]
            [Phone]
            [Display(Name = "Phone Number", Prompt = "Phone Number")]
            public string ContactPhone { get; set; }
            [Required]
            [Display(Name = "First Name", Prompt = "First Name")]
            public string ContactFirst { get; set; }
            [Required]
            [Display(Name = "Last Name", Prompt = "Last Name")]
            public string ContactLast { get; set; }

            //Business Address
            [Required]
            [Display(Name = "Street Address", Prompt = "Street Address")]
            public string Street { get; set; }
            [Display(Name = "Street Address Line 2", Prompt = "Street Address Line 2")]
            public string StreetLn2 { get; set; }
            [Required]
            [Display(Name = "City", Prompt = "City")]
            public string City { get; set; }
            [Required]
            [Display(Name = "Postal Code", Prompt = "Postal Code")]
            public string Postal { get; set; }
            [Required]
            [Display(Name = "Province", Prompt = "Province")]
            public string ProvinceCode { get; set; }
            [Required]
            [Display(Name = "Country", Prompt = "Country")]
            public string CountryCode { get; set; }

            //Future Use
            [Display(Name = "Business Number", Prompt = "Business Number")]
            public string BusinessNumber { get; set; }

            //BusinessHours
            [Display(Name = "Advanced Business Hours")]
            public bool UseGeneric { get; set; }

            [Display(Name = "Open Time")]
            [RequiredIf("UseGeneric", false)]
            [MustBeBefore("GenericOpen", "GenericClose", "Open", "Close")]
            public TimeSpan? GenericOpen { get; set; }

            [Display(Name = "Close Time")]
            [RequiredIf("UseGeneric", false)]
            public TimeSpan? GenericClose { get; set; }

            [Display(Name = "Monday Open")]
            [RequiredIf("UseGeneric", true)]
            [MustBeBefore("MonOpen", "MonClose", "Open", "Close")]
            public TimeSpan? MonOpen { get; set; }

            [Display(Name = "Close")]
            [RequiredIf("UseGeneric", true)]
            public TimeSpan? MonClose { get; set; }

            [Display(Name = "Tuesday Open")]
            [RequiredIf("UseGeneric", true)]
            [MustBeBefore("TuesOpen", "TuesClose", "Open", "Close")]
            public TimeSpan? TuesOpen { get; set; }

            [Display(Name = "Close")]
            [RequiredIf("UseGeneric", true)]
            public TimeSpan? TuesClose { get; set; }

            [Display(Name = "Wednesday Open")]
            [RequiredIf("UseGeneric", true)]
            [MustBeBefore("WedOpen", "WedClose", "Open", "Close")]
            public TimeSpan? WedOpen { get; set; }

            [Display(Name = "Close")]
            [RequiredIf("UseGeneric", true)]
            public TimeSpan? WedClose { get; set; }

            [Display(Name = "Thursday Open")]
            [RequiredIf("UseGeneric", true)]
            [MustBeBefore("ThuOpen", "ThuClose", "Open", "Close")]
            public TimeSpan? ThuOpen { get; set; }

            [Display(Name = "Close")]
            [RequiredIf("UseGeneric", true)]
            public TimeSpan? ThuClose { get; set; }

            [Display(Name = "Friday Open")]
            [MustBeBefore("FriOpen", "FriClose", "Open", "Close")]
            [RequiredIf("UseGeneric", true)]
            public TimeSpan? FriOpen { get; set; }

            [Display(Name = "Close")]
            [RequiredIf("UseGeneric", true)]
            public TimeSpan? FriClose { get; set; }

            [Display(Name = "Saturday Open")]
            [MustBeBefore("SatOpen", "SatClose", "Open", "Close")]
            [RequiredIf("UseGeneric", true)]
            public TimeSpan? SatOpen { get; set; }

            [Display(Name = "Close")]
            [RequiredIf("UseGeneric", true)]
            public TimeSpan? SatClose { get; set; }

            [Display(Name = "Sunday Open")]
            [RequiredIf("UseGeneric", true)]
            [MustBeBefore("SunOpen", "SunClose", "Open", "Close")]
            public TimeSpan? SunOpen { get; set; }

            [Display(Name = "Close")]
            [RequiredIf("UseGeneric", true)]
            public TimeSpan? SunClose { get; set; }

        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ViewData["CountryCode"] = new SelectList(_context.Country, "CountryCode", "FullName", "CA");
            ViewData["ProvinceCode"] = new SelectList(_context.Province, "ProvinceCode", "FullName", "ON");
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
                    //Create the business
                    {
                        var newBusiness = new Business
                        {
                            AspNetId = user.Id,

                            BusinessName = Input.BusinessName,
                            PhoneNumber = Input.PhoneNumber,
                            Street = Input.Street,
                            StreetLn2 = Input.StreetLn2,
                            City = Input.City,
                            Postal = Input.Postal,
                            ProvinceCode = Input.ProvinceCode,
                            CountryCode = Input.CountryCode,
                            BusinessNumber = Input.BusinessNumber,
                            ContactPhone = Input.ContactPhone,
                            ContactFirst = Input.ContactFirst,
                            ContactLast = Input.ContactLast
                        };

                        _context.Business.Add(newBusiness);
                        await _context.SaveChangesAsync();

                        Input.UseGeneric = !Input.UseGeneric;

                        var businessHours = new BusinessHours
                        {
                            BusinessId = newBusiness.BusinessId,

                            MonOpen = Input.UseGeneric ? Input.GenericOpen.Value : Input.MonOpen.Value,
                            MonClose = Input.UseGeneric ? Input.GenericClose.Value : Input.MonClose.Value,

                            TuesOpen = Input.UseGeneric ? Input.GenericOpen.Value : Input.TuesOpen.Value,
                            TuesClose = Input.UseGeneric ? Input.GenericClose.Value : Input.TuesClose.Value,

                            WedOpen = Input.UseGeneric ? Input.GenericOpen.Value : Input.WedOpen.Value,
                            WedClose = Input.UseGeneric ? Input.GenericClose.Value : Input.WedClose.Value,

                            ThuOpen = Input.UseGeneric ? Input.GenericOpen.Value : Input.ThuOpen.Value,
                            ThuClose = Input.UseGeneric ? Input.GenericClose.Value : Input.ThuClose.Value,

                            FriOpen = Input.UseGeneric ? Input.GenericOpen.Value : Input.FriOpen.Value,
                            FriClose = Input.UseGeneric ? Input.GenericClose.Value : Input.FriClose.Value,

                            SatOpen = Input.UseGeneric ? Input.GenericOpen.Value : Input.SatOpen.Value,
                            SatClose = Input.UseGeneric ? Input.GenericClose.Value : Input.SatClose.Value,

                            SunOpen = Input.UseGeneric ? Input.GenericOpen.Value : Input.SunOpen.Value,
                            SunClose = Input.UseGeneric ? Input.GenericClose.Value : Input.SunClose.Value,
                        };

                        _context.BusinessHours.Add(businessHours);
                        await _context.SaveChangesAsync();
                    }

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

            TempData["AdvancedChecked"] = Input.UseGeneric;
            ViewData["CountryCode"] = new SelectList(_context.Country, "CountryCode", "FullName", "CA");
            ViewData["ProvinceCode"] = new SelectList(_context.Province, "ProvinceCode", "FullName", "ON");
            return Page();
        }
    }
}
