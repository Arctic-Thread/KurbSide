using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace KurbSide.Models
{
    public partial class OrderStatus
    {
        public OrderStatus()
        {
            Order = new HashSet<Order>();
        }

        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public string StatusText { get; set; }

        public virtual ICollection<Order> Order { get; set; }
    }
}
