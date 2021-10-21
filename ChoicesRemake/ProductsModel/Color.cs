using System;
using System.Collections.Generic;

#nullable disable

namespace ProductsModel
{
    public partial class Color
    {
        public Color()
        {
            Products = new HashSet<Product>();
        }

        public long ColorId { get; set; }
        public string Value { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
