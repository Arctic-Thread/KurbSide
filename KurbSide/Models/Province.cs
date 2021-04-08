using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace KurbSide.Models
{
    public partial class Province
    {
        public Province()
        {
            Business = new HashSet<Business>();
            Member = new HashSet<Member>();
        }

        public string ProvinceCode { get; set; }
        public string CountryCode { get; set; }
        public string FullName { get; set; }
        public decimal TaxRate { get; set; }
        public string TaxCode { get; set; }

        public virtual Country CountryCodeNavigation { get; set; }
        public virtual ICollection<Business> Business { get; set; }
        public virtual ICollection<Member> Member { get; set; }
    }
}
