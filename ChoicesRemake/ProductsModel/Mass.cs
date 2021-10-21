using System;
using System.Collections.Generic;

#nullable disable

namespace ProductsModel
{
    public partial class Mass
    {
        public Mass()
        {
            Products = new HashSet<Product>();
        }

        public long MassId { get; set; }
        public double? MassInKg { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
