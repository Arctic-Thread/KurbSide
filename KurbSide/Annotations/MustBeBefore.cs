using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KurbSide.Annotations
{
    public class MustBeBefore : ValidationAttribute
    {
        RequiredAttribute _innerAttribute = new RequiredAttribute();
        public string _before { get; set; }
        public string _after { get; set; }

        public string _beforeName { get; set; }
        public string _afterName { get; set; }

        /// <summary>
        /// Check if one (DateTime/Timespan) is before the other.
        /// -sv
        /// </summary>
        /// <param name="before">Before Prop</param>
        /// <param name="after">After Prop</param>
        /// <param name="beforeName">Error Message Naming</param>
        /// <param name="afterName">Error Message Naming</param>
        public MustBeBefore(string before, string after, string beforeName, string afterName)
        {
            this._before = before;
            this._after = after;
            this._beforeName = beforeName;
            this._afterName = afterName;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var field = validationContext.ObjectType.GetProperty(_before);
            var dependant = validationContext.ObjectType.GetProperty(_after);

            if (dependant != null)
            {
                if (field.GetValue(validationContext.ObjectInstance) == null || dependant.GetValue(validationContext.ObjectInstance) == null)
                {
                    return ValidationResult.Success;
                }

                var before = (DateTime) field.GetValue(validationContext.ObjectInstance);
                var after = (DateTime) dependant.GetValue(validationContext.ObjectInstance);

                if (before == null || after == null)
                {
                    return ValidationResult.Success;
                }

                if (before > after)
                {
                    return new ValidationResult(ErrorMessage = _beforeName + " Must be before " + _afterName);
                }
                return ValidationResult.Success;

            }
            else
            {
                return new ValidationResult(FormatErrorMessage(_after));
            }
        }
    }
}
