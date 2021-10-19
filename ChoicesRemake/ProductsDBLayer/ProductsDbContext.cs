using Microsoft.EntityFrameworkCore;
using ProductsModel;
using System;

namespace ProductsDBLayer
{
    public class ProductsDbContext : DbContext
    {
        public ProductsDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Product> products { get; set; }
    }
}
