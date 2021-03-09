using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace KurbSide.Models
{
    public partial class AccountSettings
    {
        public string AspNetId { get; set; }
        public int? TimeZone { get; set; }

        public virtual AspNetUsers AspNet { get; set; }
    }
}
