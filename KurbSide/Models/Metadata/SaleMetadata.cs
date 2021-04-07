using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using KurbSideUtils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace KurbSide.Models
{
    public class SaleMetadata
    {
        public Guid SaleId { get; set; }

        public Guid BusinessId { get; set; }

        [Display(Name = "Sale Name")]
        [MinLength(2, ErrorMessage = "The entered Sale Name is too short. A minimum of 2 characters is required.")]
        [MaxLength(50, ErrorMessage = "The entered Sale Name is too long. 50 characters max.")]
        [Required(ErrorMessage = "You must enter a Sale Name.")]
        public string SaleName { get; set; }

        [Display(Name = "Sale Description")]
        [MaxLength(500, ErrorMessage = "The entered Sale Description is too long. 500 characters max.")]
        public string SaleDescription { get; set; }

        [Display(Name = "Category")]
        [MinLength(2, ErrorMessage = "The entered Sale Category is too short. A minimum of 2 characters is required.")]
        [MaxLength(50, ErrorMessage = "The entered Sale Category is too long. 50 characters max.")]
        [Required(ErrorMessage = "You must enter a Category for your sale.")]
        public string SaleCategory { get; set; }

        [Display(Name = "Discount (%)")]
        [Required(ErrorMessage = "You must enter Discount ")]
        [DisplayFormat(DataFormatString = "{0:P2}")]
        public decimal SaleDiscountPercentage { get; set; }

        [Display(Name = "Currently Active")]
        [Required(ErrorMessage = "You must specify if the sale is currently active or not.")]
        public bool Active { get; set; }
    }

    [ModelMetadataType(typeof(SaleMetadata))]
    public partial class Sale : IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(SaleName))
            {
                yield return new ValidationResult("You must enter a Sale Name.", new[] {nameof(SaleName)});
            }
            else
            {
                SaleName = SaleName.Trim().KSTitleCase();
            }

            if (!string.IsNullOrEmpty(SaleDescription))
            {
                SaleDescription = SaleDescription.Trim();
            }

            if (string.IsNullOrEmpty(SaleCategory))
            {
                yield return new ValidationResult("You must enter a Sale Category.", new[] {nameof(SaleCategory)});
            }
            else
            {
                SaleCategory = SaleCategory.Trim().KSTitleCase();
            }

            if (SaleDiscountPercentage < 0)
            {
                yield return new ValidationResult("You must enter a positive Discount Percentage.", new[] {nameof(SaleCategory)});
            }

            if (SaleDiscountPercentage > 100)
            {
                yield return new ValidationResult("Your Discount Percentage cannot be greater than 100.", new[] {nameof(SaleDiscountPercentage)});
            }
        }
    }
}
