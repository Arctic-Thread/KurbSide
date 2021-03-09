using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace KurbSide.Models
{
    public partial class Country
    {
        public Country()
        {
            Business = new HashSet<Business>();
            Member = new HashSet<Member>();
            Province = new HashSet<Province>();
        }

        public string CountryCode { get; set; }
        public string FullName { get; set; }

        public virtual ICollection<Business> Business { get; set; }
        public virtual ICollection<Member> Member { get; set; }
        public virtual ICollection<Province> Province { get; set; }
    }
}
