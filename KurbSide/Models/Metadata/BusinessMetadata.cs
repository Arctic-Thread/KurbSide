using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KurbSide.Models
{
    public class BusinessMetadata
    {
        public string AspNetId { get; set; }
        [Display(Name = "KurbSide ID")]
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
    }

    [ModelMetadataType(typeof(BusinessMetadata))]
    public partial class Business { }
}
