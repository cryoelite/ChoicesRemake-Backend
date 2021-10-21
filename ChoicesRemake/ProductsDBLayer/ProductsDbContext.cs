using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using ProductsModel;

#nullable disable

namespace ProductsDBLayer
{
    public partial class ProductsDBContext : DbContext
    {


        public ProductsDBContext(DbContextOptions<ProductsDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Color> Colors { get; set; }
        public virtual DbSet<Description> Descriptions { get; set; }
        public virtual DbSet<Image> Images { get; set; }
        public virtual DbSet<Mass> Masses { get; set; }
        public virtual DbSet<MiscDetail> MiscDetails { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Size> Sizes { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.CatId)
                    .HasName("PK_38");

                entity.ToTable("Category");

                entity.Property(e => e.CatId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("cat_id");

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("value");
            });

            modelBuilder.Entity<Color>(entity =>
            {
                entity.ToTable("Color");

                entity.Property(e => e.ColorId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("color_id");

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("value");
            });

            modelBuilder.Entity<Description>(entity =>
            {
                entity.HasKey(e => e.DescId)
                    .HasName("PK_27");

                entity.ToTable("Description");

                entity.Property(e => e.DescId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("desc_id");

                entity.Property(e => e.LongDescription)
                    .IsRequired()
                    .HasColumnName("long_description");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(4000)
                    .HasColumnName("title");
            });

            modelBuilder.Entity<Image>(entity =>
            {
                entity.ToTable("Image");

                entity.Property(e => e.ImageId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("image_id");

                entity.Property(e => e.Location)
                    .IsRequired()
                    .HasMaxLength(512)
                    .HasColumnName("location");

                entity.Property(e => e.MiniDesc)
                    .HasMaxLength(4000)
                    .HasColumnName("mini_desc");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Mass>(entity =>
            {
                entity.ToTable("Mass");

                entity.Property(e => e.MassId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("mass_id");

                entity.Property(e => e.MassInKg).HasColumnName("massInKg");
            });

            modelBuilder.Entity<MiscDetail>(entity =>
            {
                entity.HasKey(e => e.DetailId)
                    .HasName("PK_42");

                entity.ToTable("Misc_Detail");

                entity.Property(e => e.DetailId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("detail_id");

                entity.Property(e => e.Key)
                    .HasMaxLength(512)
                    .HasColumnName("key");

                entity.Property(e => e.Value)
                    .HasMaxLength(1024)
                    .HasColumnName("value");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => new { e.ProdId, e.ImageId, e.SizeId, e.CatId, e.ColorId, e.MassId })
                    .HasName("PK_69");

                entity.ToTable("Product");

                entity.HasIndex(e => e.ImageId, "fkIdx_50");

                entity.HasIndex(e => e.DescId, "fkIdx_53");

                entity.HasIndex(e => e.SizeId, "fkIdx_56");

                entity.HasIndex(e => e.CatId, "fkIdx_59");

                entity.HasIndex(e => e.MassId, "fkIdx_62");

                entity.HasIndex(e => e.ColorId, "fkIdx_65");

                entity.HasIndex(e => e.DetailId, "fkIdx_68");

                entity.Property(e => e.ProdId).ValueGeneratedOnAdd().HasColumnName("prod_id");

                entity.Property(e => e.ImageId).HasColumnName("image_id");

                entity.Property(e => e.SizeId).HasColumnName("size_id");

                entity.Property(e => e.CatId).HasColumnName("cat_id");

                entity.Property(e => e.ColorId).HasColumnName("color_id");

                entity.Property(e => e.MassId).HasColumnName("mass_id");

                entity.Property(e => e.Brand)
                    .IsRequired()
                    .HasMaxLength(512)
                    .HasColumnName("brand");

                entity.Property(e => e.DescId).HasColumnName("desc_id");

                entity.Property(e => e.Designer)
                    .IsRequired()
                    .HasMaxLength(512)
                    .HasColumnName("designer");

                entity.Property(e => e.DetailId).HasColumnName("detail_id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(512)
                    .HasColumnName("name");

                entity.Property(e => e.Price)
                    .HasColumnType("money")
                    .HasColumnName("price");

                entity.HasOne(d => d.Cat)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.CatId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_57");

                entity.HasOne(d => d.Color)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.ColorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_63");

                entity.HasOne(d => d.Desc)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.DescId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_51");

                entity.HasOne(d => d.Detail)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.DetailId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_66");

                entity.HasOne(d => d.Image)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.ImageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_48");

                entity.HasOne(d => d.Mass)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.MassId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_60");

                entity.HasOne(d => d.Size)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.SizeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_54");
            });

            modelBuilder.Entity<Size>(entity =>
            {
                entity.ToTable("Size");

                entity.Property(e => e.SizeId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("size_id");

                entity.Property(e => e.HeightInMm).HasColumnName("HeightInMM");

                entity.Property(e => e.LengthInMm).HasColumnName("LengthInMM");

                entity.Property(e => e.WidthInMm).HasColumnName("WidthInMM");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
