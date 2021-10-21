using System;
using System.Collections.Generic;

#nullable disable

namespace ProductsModel
{
    public partial class MiscDetail
    {
        public MiscDetail()
        {
            Products = new HashSet<Product>();
        }

        public long DetailId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
