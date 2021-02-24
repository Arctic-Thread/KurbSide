using KurbSide.Annotations;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KurbSide.Models
{
    public class BusinessHoursMetadata
    {
        public Guid BusinessId { get; set; }
        [Display(Name = "Monday Open Time")]
        public TimeSpan? MonOpen { get; set; }
        [Display(Name = "Monday Closing Time")]
        [MustBeBefore("MonOpen", "MonClose", "Open Time", "Closing Time")]
        public TimeSpan? MonClose { get; set; }
        [Display(Name = "Tuesday Open Time")]
        public TimeSpan? TuesOpen { get; set; }
        [Display(Name = "Tuesday Closing Open Time")]
        [MustBeBefore("TuesOpen", "TuesClose", "Open Time", "Closing Time")]
        public TimeSpan? TuesClose { get; set; }
        [Display(Name = "Wednesday Open Time")]
        public TimeSpan? WedOpen { get; set; }
        [Display(Name = "Wednesday Closing Open Time")]
        [MustBeBefore("WedOpen", "WedClose", "Open Time", "Closing Time")]
        public TimeSpan? WedClose { get; set; }
        [Display(Name = "Thursday Open Time")]
        public TimeSpan? ThuOpen { get; set; }
        [Display(Name = "Thursday Closing Time")]
        [MustBeBefore("ThuOpen", "ThuClose", "Open Time", "Closing Time")]
        public TimeSpan? ThuClose { get; set; }
        [Display(Name = "Friday Open Time")]
        public TimeSpan? FriOpen { get; set; }
        [Display(Name = "Friday Closing Open Time")]
        [MustBeBefore("FriOpen", "FriClose", "Open Time", "Closing Time")]
        public TimeSpan? FriClose { get; set; }
        [Display(Name = "Saturday Open Time")]
        public TimeSpan? SatOpen { get; set; }
        [Display(Name = "Saturday Closing Open Time")]
        [MustBeBefore("SatOpen", "SatClose", "Open Time", "Closing Time")]
        public TimeSpan? SatClose { get; set; }
        [Display(Name = "Sunday Open Time")]
        public TimeSpan? SunOpen { get; set; }
        [Display(Name = "Sunday Closing Open Time")]
        [MustBeBefore("SunOpen", "SunClose", "Open Time", "Closing Time")]
        public TimeSpan? SunClose { get; set; }
    }

    [ModelMetadataType(typeof(BusinessHoursMetadata))]
    public partial class BusinessHours : IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            /*
             * ^ is the XOR operator. only one side can be true.
             * If one is set and the other is not, it is invalid.
             * If both times are the same, it is invalid.
             */

            if (MonOpen == null ^ MonClose == null)
            {
                yield return new ValidationResult("You must specify both open and closing times", new[] { nameof(MonOpen) });
            }
            else if (MonOpen == MonClose && MonOpen != null && MonClose != null)
            {
                yield return new ValidationResult("Both Open and Closing time can not be the same.", new[] { nameof(MonOpen) });
            }

            if (TuesOpen == null ^ TuesClose == null)
            {
                yield return new ValidationResult("You must specify both open and closing times", new[] { nameof(TuesOpen) });
            }
            else if (TuesOpen == TuesClose && TuesOpen != null && TuesClose != null)
            {
                yield return new ValidationResult("Both Open and Closing time can not be the same.", new[] { nameof(TuesOpen) });
            }

            if (WedOpen == null ^ WedClose == null)
            {
                yield return new ValidationResult("You must specify both open and closing times", new[] { nameof(WedOpen) });
            }
            else if (WedOpen == WedClose && WedOpen != null && WedClose != null)
            {
                yield return new ValidationResult("Both Open and Closing time can not be the same.", new[] { nameof(WedOpen) });
            }

            if (ThuOpen == null ^ ThuClose == null)
            {
                yield return new ValidationResult("You must specify both open and closing times", new[] { nameof(ThuOpen) });
            }
            else if (ThuOpen == ThuClose && ThuOpen != null && ThuClose != null)
            {
                yield return new ValidationResult("Both Open and Closing time can not be the same.", new[] { nameof(ThuOpen) });
            }

            if (FriOpen == null ^ FriClose == null)
            {
                yield return new ValidationResult("You must specify both open and closing times", new[] { nameof(FriOpen) });
            }
            else if (FriOpen == FriClose && FriOpen != null && FriClose != null)
            {
                yield return new ValidationResult("Both Open and Closing time can not be the same.", new[] { nameof(FriOpen) });
            }

            if (SatOpen == null ^ SatClose == null)
            {
                yield return new ValidationResult("You must specify both open and closing times", new[] { nameof(SatOpen) });
            }
            else if (SatOpen == SatClose && SatOpen != null && SatClose != null)
            {
                yield return new ValidationResult("Both Open and Closing time can not be the same.", new[] { nameof(SatOpen) });
            }

            if (SunOpen == null ^ SunClose == null)
            {
                yield return new ValidationResult("You must specify both open and closing times", new[] { nameof(SunOpen) });
            }
            else if (SunOpen == SunClose && SunOpen != null && SunClose != null)
            {
                yield return new ValidationResult("Both Open and Closing time can not be the same.", new[] { nameof(SunOpen) });
            }
        }
    }
}
