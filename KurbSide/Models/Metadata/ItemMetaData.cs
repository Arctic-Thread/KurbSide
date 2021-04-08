using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using KurbSideUtils;
using KurbSide.Annotations;

namespace KurbSide.Models
{
    /// <summary>
    /// Annotation validation for the item table
    /// </summary>
    public class ItemMetaData
    {
        public Guid ItemId { get; set; }
        public Guid BusinessId { get; set; }

        [Display(Name = "Product Name")]
        [RegularExpression("^[a-zA-Z0-9\\s]*$", ErrorMessage = "Only Alpha-Numeric characters allowed.")]
        [MinLength(2, ErrorMessage = "The entered Product Name is too short. A minimum of 2 characters is required.")]
        [MaxLength(75, ErrorMessage = "The entered Product Name is too long. 75 characters max.")]
        [Required(ErrorMessage = "You must enter a product name.")]
        public string ItemName { get; set; }

        [Display(Name = "Product Details")]
        [MaxLength(500, ErrorMessage = "The entered Product Details is too long. 500 characters max.")]
        public string Details { get; set; }

        [KSMinValue(0.01, ErrorMessage = "The entered price is too low. The minimum price is $0.01.")]
        [KSMaxValue(2147483647, ErrorMessage = "The entered Price is too high. $2,147,483,647 max. ")]
        [Range(0.0, 2147483647, ErrorMessage = "The entered price is too low. The minimum price is $0.01.")]
        [DisplayFormat(DataFormatString = "{0:C0}")]
        [Required(ErrorMessage = "You must enter the products price.")]
        public decimal Price { get; set; }

        [Display(Name = "SKU")]
        [MaxLength(50, ErrorMessage = "The entered SKU is too long. 50 characters max.")]
        public string Sku { get; set; }

        [Display(Name = "UPC/EAN")]
        [MaxLength(13, ErrorMessage = "The entered UPC/EAN is too long. 13 numbers max.")]
        [MinLength(11, ErrorMessage = "The entered UPC/EAN is too short. 11 numbers min.")]
        //[RegularExpression(@"^(?=.*0)[0-9]{11,12}$", ErrorMessage = "Please enter valid UPC (11 or 12 digits, with at least one 0)")]
        public string Upc { get; set; }

        public string ImageLocation { get; set; }

        [Display(Name = "Product Category")]
        [MinLength(2, ErrorMessage = "The entered Product Category is too short. A minimum of 2 characters is required.")]
        [MaxLength(50, ErrorMessage = "The entered Product Category is too long. 50 characters max.")]
        [Required(ErrorMessage = "You must enter a Category for your product.")]
        public string Category { get; set; }
    }
    /// <summary>
    /// Validates and updates the fields 
    /// </summary>
    [ModelMetadataType(typeof(ItemMetaData))]
    public partial class Item : IValidatableObject
    {
        KSContext _context = new KSContext();
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {

            if (!string.IsNullOrEmpty(Sku))
            {
                Sku = Sku.KSRemoveWhitespace();
            }

            if (!string.IsNullOrEmpty(Upc))
            {
                Upc = Upc.KSRemoveWhitespace();
            }

            if (!string.IsNullOrEmpty(Details))
            {
                Details = Details.Trim();
            }

            if (string.IsNullOrEmpty(ItemName))
            {
                yield return new ValidationResult($"You must enter a product name.", new[] { nameof(ItemName) });
            }
            else
            {
                ItemName = ItemName.Trim().KSTitleCase();
            }

            bool IsNumber = true;
            try 
            {
                Convert.ToInt32(Price);
            }
            catch
            {
                IsNumber = false;
            }

            if (!IsNumber)
            {
                yield return new ValidationResult($"Product Price must be a number.", new[] { nameof(Price) });
            }
            else if (Price == 0)
            {
                yield return new ValidationResult($"Product Price cannot be zero.", new[] { nameof(Price) });
            }
            else if(Price < 0)
            {
                yield return new ValidationResult($"Product Price cannot be negative.", new[] { nameof(Price) });
            }
            
            if (string.IsNullOrEmpty(Category))
            {
                yield return new ValidationResult($"You must enter a Product Category.", new[] { nameof(Category) });
            }
            else
            {
                Category = Category.Trim().KSTitleCase();
            }
        }
    }
}
