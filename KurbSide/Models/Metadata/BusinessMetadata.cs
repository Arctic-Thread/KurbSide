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
        [Required]
        public string BusinessName { get; set; }
        [Display(Name = "Phone Number")]
        [Required]
        [RegularExpression(@"^\(?([2-9][0-9]{2})\)?[-. ]?([2-9](?!11)[0-9]{2})[-. ]?([0-9]{4})(x[0-9]{1,4})?$", ErrorMessage = "You must enter a valid Phone Number. e.g. (519)-123-1234x1234")]
        public string PhoneNumber { get; set; }
        public TimeSpan? OpenTime { get; set; }
        public TimeSpan? CloseTime { get; set; }
        [Display(Name = "Address Line 1")]
        [Required]
        public string Street { get; set; }
        [Display(Name = "Address Line 2 (Optional)")]
        public string StreetLn2 { get; set; }
        [Required]
        public string City { get; set; }
        [Display(Name = "Postal Code")]
        [Required]
        [RegularExpression(@"^(?i)[ABCEGHJKLMNPRSTVXY][0-9][ABCEGHJKLMNPRSTVWXYZ]([ ]|[-])?[0-9][ABCEGHJKLMNPRSTVWXYZ][0-9]$", ErrorMessage = "You must enter a valid postal code. e.g. K1A 0A9")]
        public string Postal { get; set; }
        [Display(Name = "Province")]
        [Required]
        public string ProvinceCode { get; set; }
        [Display(Name = "Country")]
        [Required]
        public string CountryCode { get; set; }
        [Display(Name = "Business Number (BN)")]
        [Required]
        [RegularExpression(@"^([0-9]{9})$", ErrorMessage = "Your Business Number must be 9 digits in length.")]
        public string BusinessNumber { get; set; }
        [Required]
        [RegularExpression(@"^\(?([2-9][0-9]{2})\)?[-. ]?([2-9](?!11)[0-9]{2})[-. ]?([0-9]{4})(x[0-9]{1,4})?$", ErrorMessage = "You must enter a valid Phone Number. e.g. (519)-123-1234x1234")]
        [Display(Name = "Contact Phone Number ")]
        public string ContactPhone { get; set; }
        [Display(Name = "Contact First Name")]
        [Required]
        public string ContactFirst { get; set; }
        [Display(Name = "Contact Last Name")]
        [Required]
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
                yield return new ValidationResult("Your Businessses Postal Code is required.", new[] { nameof(Postal) });
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
                yield return new ValidationResult("Your Business Contacts First Name is is required.", new[] { nameof(ContactFirst) });
            }

            if (string.IsNullOrEmpty(ContactLast))
            {
                yield return new ValidationResult("Your Business Contacts First Name is is required.", new[] { nameof(ContactLast) });
            }
        }
    }
}


