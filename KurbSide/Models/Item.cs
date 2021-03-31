using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace KurbSide.Models
{
    public partial class Item
    {
        public Item()
        {
            CartItem = new HashSet<CartItem>();
            OrderItem = new HashSet<OrderItem>();
            SaleItem = new HashSet<SaleItem>();
        }

        public Guid ItemId { get; set; }
        public Guid BusinessId { get; set; }
        public string ItemName { get; set; }
        public string Details { get; set; }
        public double? Price { get; set; }
        public string Sku { get; set; }
        public string Upc { get; set; }
        public string ImageLocation { get; set; }
        public string Category { get; set; }
        public bool? Removed { get; set; }

        public virtual Business Business { get; set; }
        public virtual ICollection<CartItem> CartItem { get; set; }
        public virtual ICollection<OrderItem> OrderItem { get; set; }
        public virtual ICollection<SaleItem> SaleItem { get; set; }
    }
}
