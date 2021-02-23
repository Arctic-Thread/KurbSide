using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace KurbSideUtils
{
    public enum PostalFormat
    {
        DASH,
        SPACE,
        NOTHING
    }

    public static class KSValidation
    {
        /// <summary>
        /// Validates if the postal code provided is a valid Canadian postal code.
        /// </summary>
        /// <param name="value">The postal code to be validated.</param>
        /// <returns></returns>
        public static bool KSPostalCodeValidation(this string value)
        {
            if (string.IsNullOrEmpty(value)) // Invalid - postal codes are mandatory
            {
                return false;
            }
            else if (value.All(c => c.Equals(' '))) // Invalid - just spaces are not valid
            {
                return false;
            }
            else
            {
                value = value.KSRemoveWhitespace();
                var postalCodeRegEx = new Regex(@"^(?i)[ABCEGHJKLMNPRSTVXY][0-9][ABCEGHJKLMNPRSTVWXYZ]([ ]|[-])?[0-9][ABCEGHJKLMNPRSTVWXYZ][0-9]$");
                if (postalCodeRegEx.IsMatch(value))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Validates if the phone number provided is valid
        /// If an extension is provided it must be seperated with an 'x'
        /// </summary>
        /// <param name="value">The phone number to be validated</param>
        /// <returns></returns>
        public static bool KSPhoneNumberValidation(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }
            else if(value.All(c => c.Equals(' ')))
            {
                return false;
            }
            else
            {
                var phoneNumberRegEx = new Regex(@"^\(?([2-9][0-9]{2})\)?[-. ]?([2-9](?!11)[0-9]{2})[-. ]?([0-9]{4})(x[0-9]{1,4})?$");
                var abc = phoneNumberRegEx.IsMatch(value);
                return abc;
            }
        }
    }
}
