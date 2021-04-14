using System;
using System.Linq;
using System.Threading.Tasks;
using KurbSide.Models;
using Microsoft.EntityFrameworkCore;

namespace KurbSide.Utilities
{
    public class KSCatalogueUtilities
    {
        /// <summary>
        /// Returns a <see cref="Sale"/> that matches specified saleId, for the given <see cref="Business"/>.
        /// <br/>
        /// <code>Example: GetSpecificSale(_context, business, saleId)</code>
        /// </summary>
        /// <remarks>
        /// Liam De Rivers
        /// </remarks>
        /// <param name="KSContext">The KurbSide context.</param>
        /// <param name="business">The <see cref="Business"/> to check.</param>
        /// <param name="saleId">The ID of the <see cref="Sale"/> to check.</param>
        /// <returns></returns>
        public static async Task<Sale> GetSpecificSale(KSContext KSContext, Business business, Guid saleId)
        {
            return await KSContext.Sale
                .Where(s => s.BusinessId.Equals(business.BusinessId))
                .Where(s => s.SaleId.Equals(saleId))
                .Include(si => si.SaleItem)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Returns a <see cref="Item"/> that matches specified itemId, for the given <see cref="Business"/>.
        /// <br/>
        /// <code>Example: GetSpecificItem(_context, business, itemId);</code>
        /// </summary>
        /// <remarks>
        /// Liam De Rivers
        /// </remarks>
        /// <param name="KSContext">The KurbSide context.</param>
        /// <param name="business">The <see cref="Business"/> to check.</param>
        /// <param name="itemId">The ID of the <see cref="Item"/> to check.</param>
        /// <returns></returns>
        public static async Task<Item> GetSpecificItem(KSContext KSContext, Business business, Guid itemId)
        {
            return await KSContext.Item
                .Where(i => i.BusinessId.Equals(business.BusinessId))
                .Where(i => i.ItemId.Equals(itemId))
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Returns a <see cref="SaleItem"/>; an <see cref="Item"/>, specified by saleId that is present in a
        /// <see cref="Sale"/>, specified by saleId
        /// <br/>
        /// <code>Example: GetSpecificSaleItem(_context, saleId, itemId);</code>
        /// </summary>
        /// <remarks>
        /// Liam De Rivers
        /// </remarks>
        /// <param name="KSContext">The KurbSide context.</param>
        /// <param name="saleId">The ID of the <see cref="Sale"/> to check.</param>
        /// <param name="itemId">The ID of the <see cref="Item"/> to check.</param>
        /// <returns></returns>
        public static async Task<SaleItem> GetSpecificSaleItem(KSContext KSContext, Guid saleId, Guid itemId)
        {
            return await KSContext.SaleItem
                .Where(s => s.SaleId.Equals(saleId))
                .Where(i => i.ItemId.Equals(itemId))
                .FirstOrDefaultAsync();
        }
    }
}