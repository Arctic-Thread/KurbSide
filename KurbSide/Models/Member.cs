using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace KurbSide.Models
{
    public partial class Member
    {
        public Member()
        {
            Cart = new HashSet<Cart>();
            Order = new HashSet<Order>();
        }

        public string AspNetId { get; set; }
        public Guid MemberId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string StreetLn2 { get; set; }
        public string City { get; set; }
        public string Postal { get; set; }
        public string ProvinceCode { get; set; }
        public string CountryCode { get; set; }
        public string PhoneNumber { get; set; }
        public string Gender { get; set; }
        public DateTime Birthday { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }

        public virtual AspNetUsers AspNet { get; set; }
        public virtual Country CountryCodeNavigation { get; set; }
        public virtual Province ProvinceCodeNavigation { get; set; }
        public virtual ICollection<Cart> Cart { get; set; }
        public virtual ICollection<Order> Order { get; set; }
    }
}
