using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BOs.Models;

public partial class VietNongContext : DbContext
{
    public VietNongContext()
    {
    }

    public VietNongContext(DbContextOptions<VietNongContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AgriculturalNews> AgriculturalNews { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<FarmingHandbook> FarmingHandbooks { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<OrderHistory> OrderHistories { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Revenue> Revenues { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Seller> Sellers { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<WeatherForecast> WeatherForecasts { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AgriculturalNews>(entity =>
        {
            entity.HasKey(e => e.NewsId).HasName("PK__Agricult__4C27CCD8504A9ACF");

            entity.Property(e => e.NewsId).HasColumnName("news_id");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.PublishedDate).HasColumnName("published_date");
            entity.Property(e => e.Source)
                .HasMaxLength(255)
                .HasColumnName("source");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Categori__D54EE9B462AA7A33");

            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(255)
                .HasColumnName("category_name");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Image)
                .HasMaxLength(255)
                .HasColumnName("image");
        });

        modelBuilder.Entity<FarmingHandbook>(entity =>
        {
            entity.HasKey(e => e.HandbookId).HasName("PK__FarmingH__B0CC3E02D49519EB");

            entity.ToTable("FarmingHandbook");

            entity.Property(e => e.HandbookId).HasColumnName("handbook_id");
            entity.Property(e => e.Category)
                .HasMaxLength(255)
                .HasColumnName("category");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Orders__46596229A1B870D8");

            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.BuyerId).HasColumnName("buyer_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.SellerId).HasColumnName("seller_id");
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .HasDefaultValue("pending")
                .HasColumnName("status");
            entity.Property(e => e.TotalAmount)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("total_amount");

            entity.HasOne(d => d.Buyer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.BuyerId)
                .HasConstraintName("FK__Orders__buyer_id__4CA06362");

            entity.HasOne(d => d.Seller).WithMany(p => p.Orders)
                .HasForeignKey(d => d.SellerId)
                .HasConstraintName("FK__Orders__seller_i__4D94879B");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.OrderDetailId).HasName("PK__OrderDet__3C5A40805FFB0AA3");

            entity.Property(e => e.OrderDetailId).HasColumnName("order_detail_id");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("price");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__OrderDeta__order__5070F446");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__OrderDeta__produ__5165187F");
        });

        modelBuilder.Entity<OrderHistory>(entity =>
        {
            entity.HasKey(e => e.OrderHistoryId).HasName("PK__OrderHis__6CC25C5A1E86D98A");

            entity.ToTable("OrderHistory");

            entity.Property(e => e.OrderHistoryId).HasColumnName("orderHistory_id");
            entity.Property(e => e.OrderDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("order_date");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderHistories)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__OrderHist__order__571DF1D5");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderHistories)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__OrderHist__produ__5629CD9C");

            entity.HasOne(d => d.User).WithMany(p => p.OrderHistories)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__OrderHist__user___5535A963");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Products__47027DF58EFE52FE");

            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("price");
            entity.Property(e => e.SellerId).HasColumnName("seller_id");
            entity.Property(e => e.StockQuantity).HasColumnName("stock_quantity");
            entity.Property(e => e.Weight)
                .HasDefaultValueSql("(NULL)")
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("weight");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__Products__catego__47DBAE45");

            entity.HasOne(d => d.Seller).WithMany(p => p.Products)
                .HasForeignKey(d => d.SellerId)
                .HasConstraintName("FK__Products__seller__46E78A0C");
        });

        modelBuilder.Entity<Revenue>(entity =>
        {
            entity.HasKey(e => e.RevenueId).HasName("PK__Revenue__3DF902E94FAC2699");

            entity.ToTable("Revenue");

            entity.Property(e => e.RevenueId).HasColumnName("revenue_id");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.RevenueDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("revenue_date");
            entity.Property(e => e.SellerId).HasColumnName("seller_id");
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .HasDefaultValue("pending")
                .HasColumnName("status");
            entity.Property(e => e.TotalAmount)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("total_amount");

            entity.HasOne(d => d.Order).WithMany(p => p.Revenues)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__Revenue__order_i__5CD6CB2B");

            entity.HasOne(d => d.Seller).WithMany(p => p.Revenues)
                .HasForeignKey(d => d.SellerId)
                .HasConstraintName("FK__Revenue__seller___5BE2A6F2");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Roles__760965CC50744EAC");

            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.RoleDescription)
                .HasMaxLength(255)
                .HasColumnName("role_description");
            entity.Property(e => e.RoleName)
                .HasMaxLength(50)
                .HasColumnName("role_name");
        });

        modelBuilder.Entity<Seller>(entity =>
        {
            entity.HasKey(e => e.SellerId).HasName("PK__Sellers__780A0A977B25D513");

            entity.Property(e => e.SellerId).HasColumnName("seller_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(15)
                .HasColumnName("phone_number");
            entity.Property(e => e.Rating)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(3, 2)")
                .HasColumnName("rating");
            entity.Property(e => e.ShopAddress)
                .HasMaxLength(255)
                .HasColumnName("shop_address");
            entity.Property(e => e.ShopName)
                .HasMaxLength(255)
                .HasColumnName("shop_name");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Sellers)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Sellers__user_id__403A8C7D");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__B9BE370F3B7CF372");

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .HasColumnName("address");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DateOfBirth).HasColumnName("date_of_birth");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.FullName)
                .HasMaxLength(255)
                .HasColumnName("full_name");
            entity.Property(e => e.Gender)
                .HasMaxLength(10)
                .HasColumnName("gender");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .HasColumnName("password_hash");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(15)
                .HasColumnName("phone_number");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.Username)
                .HasMaxLength(255)
                .HasColumnName("username");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__Users__role_id__3B75D760");
        });

        modelBuilder.Entity<WeatherForecast>(entity =>
        {
            entity.HasKey(e => e.ForecastId).HasName("PK__WeatherF__9E5731547602283D");

            entity.Property(e => e.ForecastId).HasColumnName("forecast_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.ForecastDate).HasColumnName("forecast_date");
            entity.Property(e => e.Humidity)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("humidity");
            entity.Property(e => e.Location)
                .HasMaxLength(255)
                .HasColumnName("location");
            entity.Property(e => e.Temperature)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("temperature");
            entity.Property(e => e.WeatherCondition)
                .HasMaxLength(255)
                .HasColumnName("weather_condition");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
