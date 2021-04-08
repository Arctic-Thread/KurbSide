using System;
using System.Collections.Generic;
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

        /// <summary>
        /// Checks if the <see cref="Item"/> is in the list of <see cref="Sale"/>s
        /// <br/>
        /// <code>Example: KSCheckIfItemInSale(cartItem.Item, businessSales)</code>
        /// </summary>
        /// <remarks>Liam De Rivers</remarks>
        /// <param name="item">The item to check.</param>
        /// <param name="sales">The list of sales to check.</param>
        /// <returns>The Guid of the sale, if the item is present in it. Otherwise new Guid()</returns>
        public static Guid KSCheckIfItemInSale(Item item, List<Sale> sales)
        {
            var activeSales = sales
                .Where(s => s.Active)
                .Where(sale => item.SaleItem.Any(s => s.SaleId.Equals(sale.SaleId)));
            
            foreach (var sale in activeSales)
            {
                return sale.SaleId;
            }

            return new Guid();
        }

        /// <summary>
        /// Returns the discounted price of an item if it is currently in a sale.
        /// If it is not in a sale, the original price is returned.
        /// <br/>
        /// <code>Example: GetDiscountPrice(item, sales)</code>
        /// </summary>
        /// <remarks>Liam De Rivers</remarks>
        /// <param name="item">The item to check.</param>
        /// <param name="sales">The list of sales currently being run by the business.</param>
        /// <returns>The current* price of an item.</returns>
        public static decimal GetDiscountPrice(Item item, List<Sale> sales)
        {
            var activeSales = sales
                .Where(s => s.Active)
                .Where(sale => item.SaleItem.Any(s => s.SaleId.Equals(sale.SaleId)))
                .OrderByDescending(s => s.SaleDiscountPercentage);
            
            foreach (var sale in activeSales)
            {
                return item.Price - (item.Price * sale.SaleDiscountPercentage);
            }

            return (decimal)item.Price;
        }
    }
}