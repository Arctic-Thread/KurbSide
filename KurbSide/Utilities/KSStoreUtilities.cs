using System;
using KurbSide.Models;

namespace KurbSide.Utilities
{
    public class KSStoreUtilities
    {
        /// <summary>
        /// Checks if a business is currently open or closed on the specified day.
        /// </summary>
        /// <remarks>
        /// TODO might go crazy with some for loops allowing the ability to check
        ///  different days of the week in the future if we're feeling up to it.
        /// -Liam De Rivers
        /// </remarks>
        /// <param name="businessHours">The business hours of the business</param>
        /// <param name="dayToCheck">The day of the week to check</param>
        /// <returns>The status of the business at the current time, on the specified day time.</returns>
        public static string CheckIfOpenForBusiness(BusinessHours businessHours, DayOfWeek dayToCheck)
        {
            switch (dayToCheck)
            {
                case DayOfWeek.Sunday:
                    if (businessHours.SunOpen < DateTime.Now.TimeOfDay &&
                        DateTime.Now.TimeOfDay < businessHours.SunClose)
                    {
                        return "Open Until: " + Convert.ToDateTime(businessHours.SunClose.ToString()).ToString("t");
                    }
                    else
                    {
                        return "Closed.";
                    }

                case DayOfWeek.Monday:
                    if (businessHours.MonOpen < DateTime.Now.TimeOfDay &&
                        DateTime.Now.TimeOfDay < businessHours.MonClose)
                    {
                        return "Open Until: " + Convert.ToDateTime(businessHours.MonClose.ToString()).ToString("t");
                    }
                    else
                    {
                        return "Closed.";
                    }
                
                case DayOfWeek.Tuesday:
                    if (businessHours.TuesOpen < DateTime.Now.TimeOfDay &&
                        DateTime.Now.TimeOfDay < businessHours.TuesClose)
                    {
                        return "Open Until: " + Convert.ToDateTime(businessHours.TuesClose.ToString()).ToString("t");
                    }
                    else
                    {
                        return "Closed.";
                    }
                
                case DayOfWeek.Wednesday:
                    if (businessHours.WedOpen < DateTime.Now.TimeOfDay &&
                        DateTime.Now.TimeOfDay < businessHours.WedClose)
                    {
                        return "Open Until: " + Convert.ToDateTime(businessHours.WedClose.ToString()).ToString("t");
                    }
                    else
                    {
                        return "Closed.";
                    }
                
                case DayOfWeek.Thursday:
                    if (businessHours.ThuOpen < DateTime.Now.TimeOfDay &&
                        DateTime.Now.TimeOfDay < businessHours.ThuClose)
                    {
                        return "Open Until: " + Convert.ToDateTime(businessHours.WedClose.ToString()).ToString("t");
                    }
                    else
                    {
                        return "Closed.";
                    }
                
                case DayOfWeek.Friday:
                    if (businessHours.FriOpen < DateTime.Now.TimeOfDay &&
                        DateTime.Now.TimeOfDay < businessHours.FriClose)
                    {
                        return "Open Until " + Convert.ToDateTime(businessHours.FriClose.ToString()).ToString("t");
                    }
                    else
                    {
                        return "Closed.";
                    }
                
                case DayOfWeek.Saturday:
                    if (businessHours.SatOpen < DateTime.Now.TimeOfDay &&
                        DateTime.Now.TimeOfDay < businessHours.SatClose)
                    {
                        return "Open Until " + Convert.ToDateTime(businessHours.SatClose.ToString()).ToString("t");
                    }
                    else
                    {
                        return "Closed.";
                    }
                
                default:
                    return "Closed.";
            }
        }
    }
}