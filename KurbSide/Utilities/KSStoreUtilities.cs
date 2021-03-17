using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KurbSide.Models;
using Microsoft.EntityFrameworkCore;

namespace KurbSide.Utilities
{
    public class KSStoreUtilities
    {
        public static async Task<string> CheckIfOpenForBusiness(BusinessHours businessHours)
        {

            DayOfWeek currentDayOfWeek = DateTime.Now.DayOfWeek;

            switch (currentDayOfWeek)
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