﻿using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace KurbSide.Models
{
    public partial class TimeZones
    {
        public TimeZones()
        {
            AccountSettings = new HashSet<AccountSettings>();
        }

        public Guid TimeZoneId { get; set; }
        public string Offset { get; set; }
        public string Label { get; set; }

        public virtual ICollection<AccountSettings> AccountSettings { get; set; }
    }
}
