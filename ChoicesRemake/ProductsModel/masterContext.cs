using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace ProductsModel
{
    public partial class masterContext : DbContext
    {
        public masterContext()
        {
        }

        public masterContext(DbContextOptions<masterContext> options)
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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=127.0.0.1;Database=master;User Id=SA;Password=Uxz5#2@1+7");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.CatId)
                    .HasName("PK_38");

                entity.ToTable("Category");

                entity.Property(e => e.CatId)
                    .ValueGeneratedNever()
                    .HasColumnName("cat_id");

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("value");
            });

            modelBuilder.Entity<Color>(entity =>
            {
                entity.ToTable("Color");

                entity.Property(e => e.ColorId)
                    .ValueGeneratedNever()
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
                    .ValueGeneratedNever()
                    .HasColumnName("desc_id");

                entity.Property(e => e.LongDescription)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("long_description");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("title");
            });

            modelBuilder.Entity<Image>(entity =>
            {
                entity.ToTable("Image");

                entity.Property(e => e.ImageId)
                    .ValueGeneratedNever()
                    .HasColumnName("image_id");

                entity.Property(e => e.Location)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("location");

                entity.Property(e => e.MiniDesc)
                    .HasMaxLength(50)
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
                    .ValueGeneratedNever()
                    .HasColumnName("mass_id");

                entity.Property(e => e.MassInKg).HasColumnName("massInKg");
            });

            modelBuilder.Entity<MiscDetail>(entity =>
            {
                entity.HasKey(e => e.DetailId)
                    .HasName("PK_42");

                entity.ToTable("Misc_Detail");

                entity.Property(e => e.DetailId)
                    .ValueGeneratedNever()
                    .HasColumnName("detail_id");

                entity.Property(e => e.Key)
                    .HasMaxLength(50)
                    .HasColumnName("key");

                entity.Property(e => e.Value)
                    .HasMaxLength(50)
                    .HasColumnName("value");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => new { e.ProdId, e.ImageId, e.SizeId, e.CatId, e.ColorId, e.MassId })
                    .HasName("PK_5");

                entity.ToTable("Product");

                entity.HasIndex(e => e.ImageId, "fkIdx_50");

                entity.HasIndex(e => e.DescId, "fkIdx_53");

                entity.HasIndex(e => e.SizeId, "fkIdx_56");

                entity.HasIndex(e => e.CatId, "fkIdx_59");

                entity.HasIndex(e => e.MassId, "fkIdx_62");

                entity.HasIndex(e => e.ColorId, "fkIdx_65");

                entity.HasIndex(e => e.DetailId, "fkIdx_68");

                entity.Property(e => e.ProdId).HasColumnName("prod_id");

                entity.Property(e => e.ImageId).HasColumnName("image_id");

                entity.Property(e => e.SizeId).HasColumnName("size_id");

                entity.Property(e => e.CatId).HasColumnName("cat_id");

                entity.Property(e => e.ColorId).HasColumnName("color_id");

                entity.Property(e => e.MassId).HasColumnName("mass_id");

                entity.Property(e => e.Brand)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("brand");

                entity.Property(e => e.DescId).HasColumnName("desc_id");

                entity.Property(e => e.Designer)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("designer");

                entity.Property(e => e.DetailId).HasColumnName("detail_id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
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
                    .ValueGeneratedNever()
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
