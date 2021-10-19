using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProductsModel
{
    public class Product
    {
        [Key]
        public int id { get; set; }

        [Required]
        [MaxLength(256)]
        public string name { get; set; }

        [Required]
        [MaxLength(256)]
        public string brand { get; set; }

        public string designer { get; set; }

        [Required]
        [Range(double.Epsilon, double.MaxValue)]
        public double price { get; set; }

        [Required]
        public string localImageLocation { get; set; }

        [Required]
        public List<string> descriptions { get; set; }

        [Required]
        public string category { get; set; }

        [Required]
        public List<string> sub_categories { get; set; }

        [Required]
        public List<double> sizes { get; set; }

        [Required]
        public List<string> colors { get; set; }

        [Required]
        public List<double> masses { get; set; }

        public Dictionary<string, string> misc_details {get;set;} 

    }
}
