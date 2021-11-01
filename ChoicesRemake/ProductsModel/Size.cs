using System.Collections.Generic;

#nullable disable

namespace ProductsModel
{
    public partial class Size
    {
        public Size()
        {
            Products = new HashSet<Product>();
        }

        public long SizeId { get; set; }
        public double? WidthInMm { get; set; }
        public double? LengthInMm { get; set; }
        public double? HeightInMm { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}