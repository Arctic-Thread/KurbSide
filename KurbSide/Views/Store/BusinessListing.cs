using System.Collections.Generic;
using KurbSide.Models;

namespace KurbSide.Views.Store
{
    /// <summary>
    /// An enumerable list of <see cref="Business"/>es and their distances from the members location.
    /// </summary>
    public class BusinessListing
    {
        public IEnumerable<Business> Businesses { get; set; }
        public IEnumerable<float> DistanceToBusiness { get; set; }

        /// <summary>
        /// An enumerable list of <see cref="Business"/>es and their distances from the members location.
        /// </summary>
        public BusinessListing(IEnumerable<Business> business, IEnumerable<float> distanceToBusinesses)
        {
            Businesses = business;
            DistanceToBusiness = distanceToBusinesses;
        }
    }
}
