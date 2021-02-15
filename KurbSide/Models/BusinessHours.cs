using System;
using System.Collections.Generic;

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
