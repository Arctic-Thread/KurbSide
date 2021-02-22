using System;
using System.Collections.Generic;

namespace KurbSide.Models
{
    public partial class Category
    {
        public Category()
        {
            Item = new HashSet<Item>();
        }

        public string CategoryCode { get; set; }
        public string EnglishName { get; set; }
        public bool? Taxable { get; set; }

        public virtual ICollection<Item> Item { get; set; }
    }
}
