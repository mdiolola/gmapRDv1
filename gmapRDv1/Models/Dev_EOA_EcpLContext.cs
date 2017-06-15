using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using gmapRDv1.Models;
using Microsoft.Extensions.DependencyInjection;

namespace gmapRDv1.Models
{

    public partial class Dev_EOA_EcpLContext : DbContext
    {

        public virtual DbSet<Brands> Brands { get; set; }
        public virtual DbSet<Practices> Practices { get; set; }

        public Dev_EOA_EcpLContext(DbContextOptions<Dev_EOA_EcpLContext> options)
         : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Brands>(entity =>
            {
                entity.HasKey(e => e.BrandId)
                    .HasName("PK_BrandId");

                entity.Property(e => e.BrandId)
                    .HasColumnName("BrandID")
                    .HasColumnType("char(10)");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("nchar(10)");

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            });

            modelBuilder.Entity<Practices>(entity =>
            {
                entity.HasKey(e => e.PracticeId)
                    .HasName("PK_Practices");

                entity.Property(e => e.PracticeId).HasColumnName("PracticeID");

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasColumnType("varchar(200)");

                entity.Property(e => e.BrandId)
                    .IsRequired()
                    .HasColumnName("BrandID")
                    .HasColumnType("char(10)");

                entity.Property(e => e.ContactNumbers)
                    .IsRequired()
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.Hours)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.Info)
                    .IsRequired()
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.Lat).HasColumnType("decimal(9, 6)");

                entity.Property(e => e.Lon).HasColumnType("decimal(9, 6)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.Promotion)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.Provider1).HasColumnType("varchar(50)");

                entity.Property(e => e.Provider2).HasColumnType("varchar(50)");

                entity.Property(e => e.Services)
                    .IsRequired()
                    .HasColumnType("varchar(200)");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.ZipCode)
                    .IsRequired()
                    .HasColumnType("char(10)");

            });
        }

    }
}