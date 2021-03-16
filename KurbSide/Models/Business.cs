using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace KurbSide.Models
{
    public partial class Business
    {
        public Business()
        {
            Cart = new HashSet<Cart>();
            Item = new HashSet<Item>();
        }

        public string AspNetId { get; set; }
        public Guid BusinessId { get; set; }
        public string BusinessName { get; set; }
        public string PhoneNumber { get; set; }
        public TimeSpan? OpenTime { get; set; }
        public TimeSpan? CloseTime { get; set; }
        public string Street { get; set; }
        public string StreetLn2 { get; set; }
        public string City { get; set; }
        public string Postal { get; set; }
        public string ProvinceCode { get; set; }
        public string CountryCode { get; set; }
        public string BusinessNumber { get; set; }
        public string ContactPhone { get; set; }
        public string ContactFirst { get; set; }
        public string ContactLast { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
        public string StoreIdentifier { get; set; }
        public string LogoLocation { get; set; }

        public virtual AspNetUsers AspNet { get; set; }
        public virtual Country CountryCodeNavigation { get; set; }
        public virtual Province ProvinceCodeNavigation { get; set; }
        public virtual BusinessHours BusinessHours { get; set; }
        public virtual ICollection<Cart> Cart { get; set; }
        public virtual ICollection<Item> Item { get; set; }
    }
}
