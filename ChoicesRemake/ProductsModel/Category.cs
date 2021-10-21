using System;
using System.Collections.Generic;

#nullable disable

namespace ProductsModel
{
    public partial class Category
    {
        public Category()
        {
            Products = new HashSet<Product>();
        }

        public long CatId { get; set; }
        public string Value { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
