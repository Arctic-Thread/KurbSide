using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace KurbSide.Models
{
    public partial class Cart
    {
        public Cart()
        {
            CartItem = new HashSet<CartItem>();
        }

        public Guid CartId { get; set; }
        public Guid MemberId { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public Guid BusinessId { get; set; }

        public virtual Business Business { get; set; }
        public virtual Member Member { get; set; }
        public virtual ICollection<CartItem> CartItem { get; set; }
    }
}
