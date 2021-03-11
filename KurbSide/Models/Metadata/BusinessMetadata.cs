using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using KurbSideUtils;

namespace KurbSide.Models
{
    public class BusinessMetadata
    {
        public string AspNetId { get; set; }
        [Display(Name = "KurbSide ID")]
        public Guid BusinessId { get; set; }
        [Display(Name = "Business Name")]
        [MaxLength(100, ErrorMessage = "The entered Business Name is too long. 250 characters max.")]
        [Required(ErrorMessage = "You must enter a Business Name.")]
        public string BusinessName { get; set; }
        [MaxLength(30, ErrorMessage = "The entered Store Identifier is too long. 30 characters max.")]
        [Display(Name = "Store Identifier", Prompt = "Store Identifier")]
        public string StoreIdentifier { get; set; }
        [Display(Name = "Phone Number")]
        [Required(ErrorMessage = "You must enter a Phone Number.")]
        [RegularExpression(@"^\(?([2-9][0-9]{2})\)?[-. ]?([2-9](?!11)[0-9]{2})[-. ]?([0-9]{4})(x[0-9]{1,4})?$", ErrorMessage = "You must enter a valid Phone Number. e.g. (519)-885-0300
            ")]
        public string PhoneNumber { get; set; }
        public TimeSpan? OpenTime { get; set; }
        public TimeSpan? CloseTime { get; set; }
        [Display(Name = "Address Line 1")]
        [MaxLength(250, ErrorMessage = "The entered Address is too long. 250 characters max.")]
        [Required(ErrorMessage = "You must enter your Businesses Address.")]
        public string Street { get; set; }
        [Display(Name = "Address Line 2 (Optional)")]
        [MaxLength(250, ErrorMessage = "The entered Address Line 2 can be no longer than 250 characters.")]
        public string StreetLn2 { get; set; }
        //TODO Change this. Longest city name in the world is 176 characters. Longest in Canada is 68.
        [MaxLength(50, ErrorMessage = "The entered City is too long. 50 characters max.")]
        [Required(ErrorMessage = "You must enter your Businesses City.")]
        public string City { get; set; }
        [Display(Name = "Postal Code")]
        [Required(ErrorMessage = "You must enter your Businesses Postal Code.")]
        [RegularExpression(@"^(?i)[ABCEGHJKLMNPRSTVXY][0-9][ABCEGHJKLMNPRSTVWXYZ]([ ]|[-])?[0-9][ABCEGHJKLMNPRSTVWXYZ][0-9]$", ErrorMessage = "You must enter a valid postal code. e.g. K1A 0A9")]
        public string Postal { get; set; }
        [Display(Name = "Province")]
        [MaxLength(2, ErrorMessage = "The entered Province is too long. 2 characters max.")]
        [Required(ErrorMessage = "You must enter your Businesses Province.")]
        public string ProvinceCode { get; set; }
        [Display(Name = "Country")]
        [MaxLength(2, ErrorMessage = "The entered Country Code is too long. 2 characters max.")]
        [Required(ErrorMessage = "You must enter your Businesses Country.")]
        public string CountryCode { get; set; }
        [Display(Name = "Business Number (BN)")]
        [Required(ErrorMessage = "You must enter your Business Number.")]
        [RegularExpression(@"^([0-9]{9})$", ErrorMessage = "Your Business Number must be 9 digits in length.")]
        public string BusinessNumber { get; set; }
        [Required(ErrorMessage = "You must enter your Business Contacts Phone Number.")]
        [RegularExpression(@"^\(?([2-9][0-9]{2})\)?[-. ]?([2-9](?!11)[0-9]{2})[-. ]?([0-9]{4})(x[0-9]{1,4})?$", ErrorMessage = "You must enter a valid Phone Number. e.g. (519)-885-0300x1234")]
        [Display(Name = "Contact Phone Number ")]
        public string ContactPhone { get; set; }
        [Display(Name = "Contact First Name")]
        [MaxLength(100, ErrorMessage = "The entered Contact First Name is too long. 100 characters max.")]
        [Required(ErrorMessage = "You must enter your Business Contacts First Name.")]
        public string ContactFirst { get; set; }
        [Display(Name = "Contact Last Name")]
        [MaxLength(100, ErrorMessage = "The entered Contact Last Name is too long. 100 characters max.")]
        [Required(ErrorMessage = "You must enter your Business Contacts Last Name.")]
        public string ContactLast { get; set; }
    }

    [ModelMetadataType(typeof(BusinessMetadata))]
    public partial class Business : IValidatableObject
    {
        KSContext _context = new KSContext();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(BusinessName))
            {
                yield return new ValidationResult($"Your Businesses Name is required.", new[] { nameof(BusinessName) });
            }
            else
            {
                BusinessName = BusinessName.Trim().KSTitleCase();
            }

            if (string.IsNullOrEmpty(PhoneNumber))
            {
                yield return new ValidationResult("Your Businesses Phone Number is required.", new[] { nameof(PhoneNumber) });
            }
            else if (PhoneNumber.KSPhoneNumberValidation() == false)
            {
                yield return new ValidationResult("Your Businesses Phone Number is invalid.", new[] { nameof(PhoneNumber) });
            }

            if (string.IsNullOrEmpty(Street))
            {
                yield return new ValidationResult("Your Businesses Street Address is required.", new[] { nameof(Street) });
            }
            else
            {
                Street = Street.Trim().KSTitleCase();
            }

            if (string.IsNullOrEmpty(StreetLn2))
            {
                // Street Line 2 is optional
            }
            else
            {
                StreetLn2 = StreetLn2.Trim().KSTitleCase();
            }

            if (string.IsNullOrEmpty(City))
            {
                yield return new ValidationResult("Your Businesses City is required.", new[] { nameof(City) });
            }
            else
            {
                City = City.Trim().KSTitleCase();
            }

            if (string.IsNullOrEmpty(Postal))
            {
                yield return new ValidationResult("Your Businesses Postal Code is required.", new[] { nameof(Postal) });
            }
            else
            {
                if (Postal.KSPostalCodeValidation() == false)
                {
                    yield return new ValidationResult("Your Businesses Postal Code is invalid", new[] { nameof(Postal) });
                }
                else
                {
                    Postal = Postal.KSPostalCodeFormat(PostalFormat.NOTHING);
                }
            }

            if (string.IsNullOrEmpty(ProvinceCode))
            {
                ProvinceCode = ProvinceCode.ToUpper();
                string getProvinceCodeError = ""; // Cannot yield in try catches, so we must use these variables
                bool provinceCodeFound = true; // Cannot yield in try catches, so we must use these variables

                try
                {
                    if (_context.Province.Where(p => p.ProvinceCode == ProvinceCode).Count() <= 0)
                    {
                        provinceCodeFound = false; 
                    }
                }
                catch(Exception ex)
                {
                    getProvinceCodeError = ex.GetBaseException().Message;
                }

                if(getProvinceCodeError != "")
                {
                    yield return new ValidationResult(getProvinceCodeError, new[] { nameof(ProvinceCode) });
                }

                if(provinceCodeFound == false)
                {
                    yield return new ValidationResult("Province Code not found", new[] { nameof(ProvinceCode) });
                }
            }

            if (string.IsNullOrEmpty(CountryCode))
            {
                CountryCode = CountryCode.ToUpper();
                string getCountryCodeError = ""; // Cannot yield in try catches, so we must use these variables
                bool countryCodeFound = true; // Cannot yield in try catches, so we must use these variables

                try
                {
                    if(_context.Country.Where(p => p.CountryCode == CountryCode).Count() > 0)
                    {
                        countryCodeFound = false;  
                    }
                }
                catch (Exception ex)
                {
                    getCountryCodeError = ex.GetBaseException().Message;
                }

                if (getCountryCodeError != "")
                {
                    yield return new ValidationResult(getCountryCodeError, new[] { nameof(ProvinceCode) });
                }

                if (countryCodeFound == false)
                {
                    yield return new ValidationResult("Country Code not found", new[] { nameof(ProvinceCode) });
                }
            }

            if (string.IsNullOrEmpty(BusinessNumber))
            {
                yield return new ValidationResult("Your Business Number (BN) is required.", new[] { nameof(BusinessNumber) });
            }
            else
            {
                if (BusinessNumber.All(c => c < '9' && c > '0'))
                {
                    yield return new ValidationResult("Your Business Identification Number (BIN) can only contain numbers.", new[] { nameof(BusinessNumber) });
                }
            }

            if (string.IsNullOrEmpty(ContactPhone))
            {
                yield return new ValidationResult("The entered Contact Phone Number is required.", new[] { nameof(ContactPhone) });
            }
            else if (ContactPhone.KSPhoneNumberValidation() == false)
            {
                yield return new ValidationResult("The entered Contact Phone Number is invalid.", new[] { nameof(ContactPhone) });
            }

            if (string.IsNullOrEmpty(ContactFirst))
            {
                yield return new ValidationResult("Your Business Contacts First Name is required.", new[] { nameof(ContactFirst) });
            }

            if (string.IsNullOrEmpty(ContactLast))
            {
                yield return new ValidationResult("Your Business Contacts First Name is required.", new[] { nameof(ContactLast) });
            }
        }
    }
}


