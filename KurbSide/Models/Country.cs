using System;
using System.Collections.Generic;

namespace KurbSide.Models
{
    public partial class Country
    {
        public Country()
        {
            Business = new HashSet<Business>();
            Province = new HashSet<Province>();
        }

        public string CountryCode { get; set; }
        public string FullName { get; set; }

        public virtual ICollection<Business> Business { get; set; }
        public virtual ICollection<Province> Province { get; set; }
    }
}
