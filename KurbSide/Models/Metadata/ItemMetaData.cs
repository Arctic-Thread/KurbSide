using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using KurbSideUtils;

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
        [MinLength(2)]
        [MaxLength(90)]
        [Required]
        public string ItemName { get; set; }
        [Display(Name ="Product Details")]
        [MaxLength(490)]
        public string Details { get; set; }
        [Required]
        public double? Price { get; set; }
        [Display(Name ="SKU")]
        public string Sku { get; set; }
        [Display(Name ="UPC")]
        //[RegularExpression(@"^(?=.*0)[0-9]{12}$", ErrorMessage ="Please enter valid UPC")]
        public string Upc { get; set; }
        public string ImageLocation { get; set; }
        [Display(Name ="Product Category")]
        [MinLength(2)]
        [MaxLength(90)]
        [Required]
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
                Sku = Sku.Trim();
            }

            if (!string.IsNullOrEmpty(Upc))
            {
                Upc = Upc.Trim();
            }

            if (!string.IsNullOrEmpty(Details))
            {
                Details = Details.Trim();
            }

            if (string.IsNullOrEmpty(ItemName))
            {
                yield return new ValidationResult($"Product must have a name.", new[] { nameof(ItemName) });
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
                yield return new ValidationResult($"Product value must be a number.", new[] { nameof(Price) });
            }
            else if (Price.Value <= 0)
            {
                yield return new ValidationResult($"Product value cannot be negative.", new[] { nameof(Price) });
            }
            
            if (string.IsNullOrEmpty(Category))
            {
                yield return new ValidationResult($"You must have a category.", new[] { nameof(Category) });
            }
            else
            {
                Category = Category.Trim().KSTitleCase();
            }
        }
    }
}
