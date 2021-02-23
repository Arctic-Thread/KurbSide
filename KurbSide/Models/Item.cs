using System;
using System.Collections.Generic;

namespace KurbSide.Models
{
    public partial class Item
    {
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
    }
}
