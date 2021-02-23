using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace KurbSideUtils
{
    public static class KSStringManipulation
    {
        /// <summary>
        /// Changes the provided string to title case.
        /// quIcK bRoWN FOX -> Quick Brown Fox
        /// </summary>
        /// <param name="value">The string to be converted to title case.</param>
        /// <returns></returns>
        public static string KSTitleCase(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }
            else
            {
                TextInfo textInfo = new CultureInfo("en-CA", false).TextInfo;
                value = textInfo.ToTitleCase(value);
                return value;
            }
        }

        /// <summary>
        /// Formats the phone number to include brackets and dashes.
        /// 1231231234 -> (123) 123-1234
        /// If the phone number has an extension it is also included.
        /// 1231231234x4321 -> (123) 123-1234x4321
        /// </summary>
        /// <param name="value">The phone number to be validated</param>
        /// <returns></returns>
        public static string KSFormatAsPhoneNumber(this string value)
        {
            string first = value.Substring(0, 3);
            string second = value.Substring(3, 3);
            string third = value.Substring(6, 4);

            if (value.Length == 10)
            {
                return new string($"({first}) {second}-{third}");
            }
            else if(value.Length >= 11 && value.Length <= 15)
            {
                string ext = value.Substring(10, value.Length-10);
                return new string($"({first}) {second}-{third}{ext}");
            }
            else
            {
                return "Invalid";
            }
        }

        /// <summary>
        /// Removes any white space, tabs or new lines from the provided string.
        /// </summary>
        /// <param name="value">The string to be manipulated.</param>
        /// <returns></returns>
        public static string KSRemoveWhitespace(this string value)
        {
            return new string(value.ToCharArray()
                .Where(c => !Char.IsWhiteSpace(c))
                .ToArray());
        }

        /// <summary>
        /// Extracts all numbers from the provided string.
        /// </summary>
        /// <param name="value">The string to be manipulated.</param>
        /// <returns></returns>
        public static string KSExtractDigit(this string value)
        {
            return new string(value.Where(char.IsDigit).ToArray());
        }

        /// <summary>
        /// Extracts all numbers or letters from the provided string.
        /// </summary>
        /// <param name="value">The string to be manipulated.</param>
        /// <returns></returns>
        public static string KSExtractNumbersAndLetters(this string value)
        {
            return new string(value.Where(char.IsLetterOrDigit).ToArray());
        }

        /// <summary>
        /// Formats the provided postal code to include a dash, space or nothing.
        /// </summary>
        /// <param name="postalCode">The postal code to be formated.</param>
        /// <param name="format">Using PostalFormat enum. DASH = '-', SPACE = ' ', NOTHING = ''</param>
        /// <returns></returns>
        public static string KSPostalCodeFormat(this string postalCode, PostalFormat format = PostalFormat.NOTHING)
        {
            if (postalCode.KSPostalCodeValidation() == true)
            {
                postalCode = postalCode.ToUpper().KSRemoveWhitespace();
                postalCode = postalCode.KSExtractNumbersAndLetters();

                switch (format)
                {
                    case PostalFormat.DASH:
                        postalCode = postalCode.Insert(3, "-");
                        break;
                    case PostalFormat.SPACE:
                        postalCode = postalCode.Insert(3, " ");
                        break;
                    case PostalFormat.NOTHING:
                        // Do nothing
                        break;
                    default:
                        throw new NotImplementedException("KurbSideUtils.KSValidation.KSPostalCodeFormat: Postal Code Format Not Implemented");
                }

                return postalCode;
            }
            else
            {
                return "Invalid";
            }
        }
    }
}
