using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace KurbSideUtils
{
    /// <summary>
    /// Convert a queryable collection to a paginated list.
    /// </summary>
    /// <remarks>
    /// Seth VanNiekerk
    /// </remarks>
    /// <typeparam name="T">object type</typeparam>
    public class KSPaginatedList<T> : List<T>
    {
        /// <summary>
        /// The index of the current returning page.
        /// </summary>
        public int PageIndex { get; private set; }
        /// <summary>
        /// The total number of pages in the collection.
        /// </summary>
        public int TotalPages { get; private set; }

        public KSPaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            this.AddRange(items);
        }

        /// <summary>
        /// Check if within the defined bounds,
        /// there is a previous accesable page.
        /// </summary>
        public bool HasPreviousPage => (PageIndex > 1);

        /// <summary>
        /// Check if within the defined bounds,
        /// there is a nexr accesable page.
        /// </summary>
        public bool HasNextPage => (PageIndex < TotalPages);

        /// <summary>
        /// Create the paginated collection
        /// </summary>
        /// <param name="source">Queryable colletion to be paginated</param>
        /// <param name="pageIndex">Page to be displayed</param>
        /// <param name="pageSize">Number of objects to display per page</param>
        /// <returns>Specified page of given collection</returns>
        public static KSPaginatedList<T> Create(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return new KSPaginatedList<T>(items, count, pageIndex, pageSize);
        }

        /// <summary>
        /// Create the paginated collection in an async context
        /// </summary>
        /// <remarks>
        /// ASYNC NOT IMPLEMENTED, Requires a revisit if we want to use this
        /// </remarks>
        /// <param name="source">Queryable colletion to be paginated</param>
        /// <param name="pageIndex">Page to be displayed</param>
        /// <param name="pageSize">Number of objects to display per page</param>
        /// <returns>Specified page of given collection</returns>
        //public static async Task<KSPaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        //{
        //    throw new NotImplementedException();
        //    //    var count = await source.CountAsync();
        //    //    var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
        //    //    return new KSPaginatedList<T>(items, count, pageIndex, pageSize);
        //}
    }
}
