﻿using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace KurbSide.Models
{
    public partial class SaleItem
    {
        public Guid SaleId { get; set; }
        public Guid ItemId { get; set; }

        public virtual Item Item { get; set; }
        public virtual Sale Sale { get; set; }
    }
}
