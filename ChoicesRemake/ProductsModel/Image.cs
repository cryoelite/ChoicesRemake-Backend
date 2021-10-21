﻿using System;
using System.Collections.Generic;

#nullable disable

namespace ProductsModel
{
    public partial class Image
    {
        public Image()
        {
            Products = new HashSet<Product>();
        }

        public string Name { get; set; }
        public string Location { get; set; }
        public string MiniDesc { get; set; }
        public long ImageId { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
