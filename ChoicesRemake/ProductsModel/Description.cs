using System.Collections.Generic;

#nullable disable

namespace ProductsModel
{
    public partial class Description
    {
        public Description()
        {
            Products = new HashSet<Product>();
        }

        public long DescId { get; set; }
        public string Title { get; set; }
        public string LongDescription { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}