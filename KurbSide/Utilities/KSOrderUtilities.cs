using System;
using System.Linq;
using KurbSide.Models;

namespace KurbSide.Utilities
{
    public class KSOrderUtilities
    {
        /// <summary>
        /// Checks if the <see cref="Item"/> is in the <see cref="Sale"/>
        /// <br/>
        /// <code>Example: KSCheckIfItemInSale(item, saleId)</code>
        /// </summary>
        /// <remarks>Liam De Rivers</remarks>
        /// <param name="item">The item to check.</param>
        /// <param name="saleId">The Guid of the sale to check.</param>
        /// <returns>A bool, if the item is currently in the sale.</returns>
        public static bool KSCheckIfItemInSale(Item item, Guid saleId) =>
            item.SaleItem.Any(s => s.SaleId.Equals(saleId));
    }
}