using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KurbSide.Models
{
    public class MemberMetadata
    {
        public string AspNetId { get; set; }

        [Display(Name = "KurbSide ID")]
        public Guid MemberId { get; set; }

        [Display(Name = "First Name")]
        [MaxLength(100, ErrorMessage = "The entered First Name is too long. 100 characters max.")]
        [Required(ErrorMessage = "You must enter your First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [MaxLength(100, ErrorMessage = "The entered Last Name is too long. 100 characters max.")]
        [Required(ErrorMessage = "You must enter your Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Address Line 1")]
        [MaxLength(250, ErrorMessage = "The entered Address is too long. 250 characters max.")]
        [Required(ErrorMessage = "You must enter your Address.")]
        public string Street { get; set; }

        [Display(Name = "Address Line 2 (Optional)")]
        [MaxLength(250, ErrorMessage = "The entered Address Line 2 can be no longer than 250 characters.")]
        public string StreetLn2 { get; set; }

        //TODO Change this. Longest city name in the world is 176 characters. Longest in Canada is 68.
        [Display(Name = "City")]
        [MaxLength(50, ErrorMessage = "The entered City is too long. 50 characters max.")]
        [Required(ErrorMessage = "You must enter your City.")]
        public string City { get; set; }

        [Display(Name = "Postal Code")]
        [Required(ErrorMessage = "You must enter your Postal Code.")]
        [RegularExpression(@"^(?i)[ABCEGHJKLMNPRSTVXY][0-9][ABCEGHJKLMNPRSTVWXYZ]([ ]|[-])?[0-9][ABCEGHJKLMNPRSTVWXYZ][0-9]$", ErrorMessage = "You must enter a valid postal code. e.g. K1A 0A9")]
        public string Postal { get; set; }

        [Display(Name = "Province")]
        [MaxLength(2, ErrorMessage = "The entered Province is too long. 2 characters max.")]
        [Required(ErrorMessage = "You must enter your Province.")]
        public string ProvinceCode { get; set; }

        [Display(Name = "Country")]
        [MaxLength(2, ErrorMessage = "The entered Country Code is too long. 2 characters max.")]
        [Required(ErrorMessage = "You must enter your Country.")]
        public string CountryCode { get; set; }

        [Display(Name = "Phone Number")]
        [Required(ErrorMessage = "You must enter your Phone Number.")]
        [RegularExpression(@"^\(?([2-9][0-9]{2})\)?[-. ]?([2-9](?!11)[0-9]{2})[-. ]?([0-9]{4})$", ErrorMessage = "You must enter a valid Phone Number. e.g. (519)-885-0300")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Gender")]
        //TODO should probably change this in DB, theres some long pronouns out there.
        [MaxLength(10, ErrorMessage = "The entered Gender is too long. 10 characters max.")]
        [Required(ErrorMessage = "You must enter your Gender.")]
        public string Gender { get; set; }

        [Display(Name = "Birthday")]
        [Required(ErrorMessage = "You must enter your Birthday")]
        public DateTime Birthday { get; set; }

        [Display(Name = "Current Latitude")]
        public double Lat { get; set; }

        [Display(Name = "Current Longitude")]
        public double Lng { get; set; }
    }

    [ModelMetadataType(typeof(MemberMetadata))]
    public partial class Member : IValidatableObject
    {
        KSContext _context = new KSContext();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException("Member Validation Not Implemented");
        }
    }
}
