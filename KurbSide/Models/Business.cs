using System;
using System.Collections.Generic;

namespace KurbSide.Models
{
    public partial class Business
    {
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

        public virtual AspNetUsers AspNet { get; set; }
        public virtual Country CountryCodeNavigation { get; set; }
        public virtual Province ProvinceCodeNavigation { get; set; }
    }
}
