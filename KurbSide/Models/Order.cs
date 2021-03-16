﻿using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace KurbSide.Models
{
    public partial class Order
    {
        public Guid OrderId { get; set; }
        public Guid MemberId { get; set; }
        public decimal SubTotal { get; set; }
        public decimal? DiscountTotal { get; set; }
        public decimal Tax { get; set; }
        public decimal GrandTotal { get; set; }
        public string Status { get; set; }
        public DateTime? CreationDate { get; set; }

        public virtual Member Member { get; set; }
    }
}