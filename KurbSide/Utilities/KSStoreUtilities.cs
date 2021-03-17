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
        public static async Task<string> CheckIfOpenForBusiness(KSContext context, Business business)
        {
            var businessHours = await context.BusinessHours
                .FirstOrDefaultAsync(b => b.BusinessId == business.BusinessId);

            DayOfWeek currentDayOfWeek = DateTime.Now.DayOfWeek;

            switch (currentDayOfWeek)
            {
                case DayOfWeek.Sunday:
                    if (businessHours.SunOpen < DateTime.Now.TimeOfDay &&
                        DateTime.Now.TimeOfDay < businessHours.SunClose)
                    {
                        return "Open Until: " + businessHours.SunClose;
                    }
                    else
                    {
                        return "Closed.";
                    }

                case DayOfWeek.Monday:
                    if (businessHours.MonOpen < DateTime.Now.TimeOfDay &&
                        DateTime.Now.TimeOfDay < businessHours.MonClose)
                    {
                        return "Open Until: " + businessHours.MonClose;
                    }
                    else
                    {
                        return "Closed.";
                    }
                
                case DayOfWeek.Tuesday:
                    if (businessHours.TuesOpen < DateTime.Now.TimeOfDay &&
                        DateTime.Now.TimeOfDay < businessHours.TuesClose)
                    {
                        return "Open Until: " + businessHours.TuesClose;
                    }
                    else
                    {
                        return "Closed.";
                    }
                
                case DayOfWeek.Wednesday:
                    if (businessHours.WedOpen < DateTime.Now.TimeOfDay &&
                        DateTime.Now.TimeOfDay < businessHours.WedClose)
                    {
                        return "Open Until: " + businessHours.WedClose;
                    }
                    else
                    {
                        return "Closed.";
                    }
                
                case DayOfWeek.Thursday:
                    if (businessHours.ThuOpen < DateTime.Now.TimeOfDay &&
                        DateTime.Now.TimeOfDay < businessHours.ThuClose)
                    {
                        return "Open Until: " + businessHours.ThuClose;
                    }
                    else
                    {
                        return "Closed.";
                    }
                
                case DayOfWeek.Friday:
                    if (businessHours.FriOpen < DateTime.Now.TimeOfDay &&
                        DateTime.Now.TimeOfDay < businessHours.FriClose)
                    {
                        return "Open Until: " + businessHours.FriClose;
                    }
                    else
                    {
                        return "Closed.";
                    }
                
                case DayOfWeek.Saturday:
                    if (businessHours.SatOpen < DateTime.Now.TimeOfDay &&
                        DateTime.Now.TimeOfDay < businessHours.SatClose)
                    {
                        return "Open Until: " + businessHours.SatClose;
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