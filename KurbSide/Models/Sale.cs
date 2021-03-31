using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace KurbSide.Models
{
    public partial class Sale
    {
        public Sale()
        {
            Notification = new HashSet<Notification>();
            SaleItem = new HashSet<SaleItem>();
        }

        public Guid SaleId { get; set; }
        public string SaleName { get; set; }
        public string SaleDescription { get; set; }
        public decimal SaleDiscountPercentage { get; set; }

        public virtual ICollection<Notification> Notification { get; set; }
        public virtual ICollection<SaleItem> SaleItem { get; set; }
    }
}
