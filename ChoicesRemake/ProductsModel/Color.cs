using System.Collections.Generic;

#nullable disable

namespace ProductsModel
{
    public static class BaseColor
    {
        public const string Black = "black";
        public const string Blue = "blue";
        public const string Brown = "brown";
        public const string Green = "green";
        public const string Indigo = "indigo";
        public const string Orange = "orange";
        public const string Pink = "pink";
        public const string Red = "red";
        public const string Violet = "violet";
        public const string White = "white";
        public const string Yellow = "yellow";
    }

    public partial class Color
    {
        public Color()
        {
            Products = new HashSet<Product>();
        }

        public long ColorId { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public string Value { get; set; }
    }
}