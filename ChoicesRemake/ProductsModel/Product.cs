#nullable disable

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductsModel
{
    public partial class Product
    {
        [NotMapped]
        public List<Category> local_categories = new List<Category>();

        [NotMapped]
        public List<Color> local_colors = new List<Color>();

        [NotMapped]
        public List<Description> local_descs = new List<Description>();

        [NotMapped]
        public List<MiscDetail> local_details = new List<MiscDetail>();

        [NotMapped]
        public List<Image> local_images = new List<Image>();

        [NotMapped]
        public List<Mass> local_masses = new List<Mass>();

        [NotMapped]
        public List<Size> local_sizes = new List<Size>();

        public string Brand { get; set; }
        public virtual Category Cat { get; set; }
        public long CatId { get; set; }
        public virtual Color Color { get; set; }
        public long ColorId { get; set; }
        public virtual Description Desc { get; set; }
        public long DescId { get; set; }
        public string Designer { get; set; }
        public virtual MiscDetail Detail { get; set; }
        public long DetailId { get; set; }
        public virtual Image Image { get; set; }
        public long ImageId { get; set; }
        public virtual Mass Mass { get; set; }
        public long MassId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public long ProdId { get; set; }
        public virtual Size Size { get; set; }
        public long SizeId { get; set; }
    }
}