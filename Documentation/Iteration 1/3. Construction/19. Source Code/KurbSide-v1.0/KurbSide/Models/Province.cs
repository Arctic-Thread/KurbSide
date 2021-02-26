using System;
using System.Collections.Generic;

namespace KurbSide.Models
{
    public partial class Province
    {
        public Province()
        {
            Business = new HashSet<Business>();
        }

        public string ProvinceCode { get; set; }
        public string CountryCode { get; set; }
        public string FullName { get; set; }
        public double? TaxRate { get; set; }
        public string TaxCode { get; set; }

        public virtual Country CountryCodeNavigation { get; set; }
        public virtual ICollection<Business> Business { get; set; }
    }
}
