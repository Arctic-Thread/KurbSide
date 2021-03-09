using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace KurbSide.Models
{
    public partial class BusinessHours
    {
        public Guid BusinessId { get; set; }
        public TimeSpan? MonOpen { get; set; }
        public TimeSpan? MonClose { get; set; }
        public TimeSpan? TuesOpen { get; set; }
        public TimeSpan? TuesClose { get; set; }
        public TimeSpan? WedOpen { get; set; }
        public TimeSpan? WedClose { get; set; }
        public TimeSpan? ThuOpen { get; set; }
        public TimeSpan? ThuClose { get; set; }
        public TimeSpan? FriOpen { get; set; }
        public TimeSpan? FriClose { get; set; }
        public TimeSpan? SatOpen { get; set; }
        public TimeSpan? SatClose { get; set; }
        public TimeSpan? SunOpen { get; set; }
        public TimeSpan? SunClose { get; set; }

        public virtual Business Business { get; set; }
    }
}
