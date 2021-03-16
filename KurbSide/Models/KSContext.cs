﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace KurbSide.Models
{
    public partial class KSContext : DbContext
    {
        public KSContext()
        {
        }

        public KSContext(DbContextOptions<KSContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AccountSettings> AccountSettings { get; set; }
        public virtual DbSet<AspNetRoleClaims> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUserTokens> AspNetUserTokens { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<Business> Business { get; set; }
        public virtual DbSet<BusinessHours> BusinessHours { get; set; }
        public virtual DbSet<Country> Country { get; set; }
        public virtual DbSet<Item> Item { get; set; }
        public virtual DbSet<Member> Member { get; set; }
        public virtual DbSet<Province> Province { get; set; }
        public virtual DbSet<TimeZones> TimeZones { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=KurbSide;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccountSettings>(entity =>
            {
                entity.HasKey(e => e.AspNetId)
                    .HasName("PK__AccountS__9C3F232B129AECC6");

                entity.Property(e => e.TimeZoneId).HasColumnName("TimeZoneID");

                entity.HasOne(d => d.AspNet)
                    .WithOne(p => p.AccountSettings)
                    .HasForeignKey<AccountSettings>(d => d.AspNetId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AccountSe__AspNe__4D94879B");

                entity.HasOne(d => d.TimeZone)
                    .WithMany(p => p.AccountSettings)
                    .HasForeignKey(d => d.TimeZoneId)
                    .HasConstraintName("FK__AccountSe__TimeZ__6E01572D");
            });

            modelBuilder.Entity<AspNetRoleClaims>(entity =>
            {
                entity.HasIndex(e => e.RoleId);

                entity.Property(e => e.RoleId).IsRequired();

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetRoles>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName)
                    .HasName("RoleNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedName] IS NOT NULL)");

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserClaims>(entity =>
            {
                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogins>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.ProviderKey).HasMaxLength(128);

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserRoles>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.HasIndex(e => e.RoleId);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.RoleId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserTokens>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.Name).HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserTokens)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUsers>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail)
                    .HasName("EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName)
                    .HasName("UserNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedUserName] IS NOT NULL)");

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);
            });

            modelBuilder.Entity<Business>(entity =>
            {
                entity.HasKey(e => new { e.AspNetId, e.BusinessId })
                    .HasName("PK__Business__6321891DC7252BB6");

                entity.HasIndex(e => e.BusinessId)
                    .HasName("PK_Business_Unique")
                    .IsUnique();

                entity.Property(e => e.BusinessId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.BusinessName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.BusinessNumber).HasMaxLength(255);

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.CloseTime).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ContactFirst)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.ContactLast)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.ContactPhone)
                    .IsRequired()
                    .HasMaxLength(22);

                entity.Property(e => e.CountryCode)
                    .HasMaxLength(2)
                    .HasDefaultValueSql("('CA')");

                entity.Property(e => e.LogoLocation).HasMaxLength(255);

                entity.Property(e => e.OpenTime).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(22);

                entity.Property(e => e.Postal)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.ProvinceCode)
                    .HasMaxLength(2)
                    .HasDefaultValueSql("('ON')");

                entity.Property(e => e.StoreIdentifier)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Street)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.StreetLn2).HasMaxLength(255);

                entity.HasOne(d => d.AspNet)
                    .WithMany(p => p.Business)
                    .HasForeignKey(d => d.AspNetId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Business__AspNet__5441852A");

                entity.HasOne(d => d.CountryCodeNavigation)
                    .WithMany(p => p.Business)
                    .HasForeignKey(d => d.CountryCode)
                    .HasConstraintName("FK__Business__Countr__5535A963");

                entity.HasOne(d => d.ProvinceCodeNavigation)
                    .WithMany(p => p.Business)
                    .HasForeignKey(d => d.ProvinceCode)
                    .HasConstraintName("FK__Business__Provin__5629CD9C");
            });

            modelBuilder.Entity<BusinessHours>(entity =>
            {
                entity.HasKey(e => e.BusinessId)
                    .HasName("PK__Business__F1EAA36E2F65C2E2");

                entity.Property(e => e.BusinessId).ValueGeneratedNever();

                entity.HasOne(d => d.Business)
                    .WithOne(p => p.BusinessHours)
                    .HasPrincipalKey<Business>(p => p.BusinessId)
                    .HasForeignKey<BusinessHours>(d => d.BusinessId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__BusinessH__Busin__571DF1D5");
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.HasKey(e => e.CountryCode)
                    .HasName("PK__Country__5D9B0D2D8B36BC02");

                entity.Property(e => e.CountryCode).HasMaxLength(2);

                entity.Property(e => e.FullName)
                    .IsRequired()
                    .HasMaxLength(60);
            });

            modelBuilder.Entity<Item>(entity =>
            {
                entity.HasKey(e => new { e.ItemId, e.BusinessId })
                    .HasName("PK__Item__8D6029BD0D3F4FDB");

                entity.Property(e => e.ItemId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Category).HasMaxLength(50);

                entity.Property(e => e.Details).HasMaxLength(500);

                entity.Property(e => e.ImageLocation).HasMaxLength(255);

                entity.Property(e => e.ItemName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Removed).HasDefaultValueSql("('FALSE')");

                entity.Property(e => e.Sku)
                    .HasColumnName("SKU")
                    .HasMaxLength(50);

                entity.Property(e => e.Upc)
                    .HasColumnName("UPC")
                    .HasMaxLength(50);

                entity.HasOne(d => d.Business)
                    .WithMany(p => p.Item)
                    .HasPrincipalKey(p => p.BusinessId)
                    .HasForeignKey(d => d.BusinessId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Item__BusinessId__5812160E");
            });

            modelBuilder.Entity<Member>(entity =>
            {
                entity.HasKey(e => new { e.AspNetId, e.MemberId })
                    .HasName("PK__Member__0CF0279AF1071D9B");

                entity.HasIndex(e => e.MemberId)
                    .HasName("PK_Member_Unique")
                    .IsUnique();

                entity.Property(e => e.MemberId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Birthday)
                    .HasColumnType("date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.CountryCode)
                    .HasMaxLength(2)
                    .HasDefaultValueSql("('CA')");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Gender).HasMaxLength(10);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Postal)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.ProvinceCode)
                    .HasMaxLength(2)
                    .HasDefaultValueSql("('ON')");

                entity.Property(e => e.Street)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.StreetLn2).HasMaxLength(255);

                entity.HasOne(d => d.AspNet)
                    .WithMany(p => p.Member)
                    .HasForeignKey(d => d.AspNetId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Member__AspNetId__59063A47");

                entity.HasOne(d => d.CountryCodeNavigation)
                    .WithMany(p => p.Member)
                    .HasForeignKey(d => d.CountryCode)
                    .HasConstraintName("FK__Member__CountryC__17036CC0");

                entity.HasOne(d => d.ProvinceCodeNavigation)
                    .WithMany(p => p.Member)
                    .HasForeignKey(d => d.ProvinceCode)
                    .HasConstraintName("FK__Member__Province__17F790F9");
            });

            modelBuilder.Entity<Province>(entity =>
            {
                entity.HasKey(e => e.ProvinceCode)
                    .HasName("PK__Province__11D9FAD444C24F17");

                entity.Property(e => e.ProvinceCode).HasMaxLength(2);

                entity.Property(e => e.CountryCode)
                    .IsRequired()
                    .HasMaxLength(2);

                entity.Property(e => e.FullName)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.TaxCode)
                    .HasMaxLength(5)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.TaxRate).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.CountryCodeNavigation)
                    .WithMany(p => p.Province)
                    .HasForeignKey(d => d.CountryCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Province__Countr__59FA5E80");
            });

            modelBuilder.Entity<TimeZones>(entity =>
            {
                entity.HasKey(e => e.TimeZoneId)
                    .HasName("PK__TimeZone__78D387CF3CD4A123");

                entity.Property(e => e.TimeZoneId)
                    .HasColumnName("TimeZoneID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Label)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Offset)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
