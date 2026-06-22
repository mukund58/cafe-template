namespace backend.Data;

using Microsoft.EntityFrameworkCore;
using backend.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Floor> Floors => Set<Floor>();
    public DbSet<Table> Tables => Set<Table>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<Coupon> Coupons => Set<Coupon>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Core constraints
        modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
        modelBuilder.Entity<Coupon>().HasIndex(c => c.Code).IsUnique();
        modelBuilder.Entity<Order>().HasIndex(o => o.OrderNumber).IsUnique();

        // Floor -> Tables (Cascade)
        modelBuilder.Entity<Table>()
            .HasOne(t => t.Floor)
            .WithMany(f => f.Tables)
            .HasForeignKey(t => t.FloorId)
            .OnDelete(DeleteBehavior.Cascade);

        // Category -> Products (Restrict)
        modelBuilder.Entity<Product>()
            .HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        // Order relationships (Restrict protects historical transactional logs)
        modelBuilder.Entity<Order>()
            .HasOne(o => o.Table)
            .WithMany(t => t.Orders)
            .HasForeignKey(o => o.TableId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Order>()
            .HasOne(o => o.Customer)
            .WithMany(c => c.Orders)
            .HasForeignKey(o => o.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Order>()
            .HasOne(o => o.Employee)
            .WithMany(u => u.HandledOrders)
            .HasForeignKey(o => o.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        // Order -> OrderItems (Cascade)
        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Order)
            .WithMany(o => o.OrderItems)
            .HasForeignKey(oi => oi.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        // Order -> Payments (Cascade)
        modelBuilder.Entity<Payment>()
            .HasOne(p => p.Order)
            .WithMany(o => o.Payments)
            .HasForeignKey(p => p.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
        // Coupon -> Orders relationship (One Coupon can be used across Many Orders)
        modelBuilder.Entity<Order>()
            .HasOne(o => o.Coupon)
            .WithMany() // Leaves the collection empty if you don't need Coupon.Orders
            .HasForeignKey(o => o.AppliedCouponId)
            .OnDelete(DeleteBehavior.Restrict); // Keeps order records safe if a coupon code is deleted

        // --- USER VALIDATIONS ---
        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(u => u.Name).IsRequired().HasMaxLength(100);
            entity.Property(u => u.Email).IsRequired().HasMaxLength(256);
            entity.Property(u => u.ProfileImagePath).HasMaxLength(500);
            entity.HasIndex(u => u.Email).IsUnique();
        });

        // --- CUSTOMER VALIDATIONS ---
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.Property(c => c.Name).IsRequired().HasMaxLength(100);
            entity.Property(c => c.Email).HasMaxLength(256);
            entity.Property(c => c.Phone).HasMaxLength(20);
        });


        // --- CATEGORY & PRODUCT VALIDATIONS ---
        modelBuilder.Entity<Category>(entity =>
        {
            entity.Property(c => c.Name).IsRequired().HasMaxLength(50);
            entity.Property(c => c.Color).HasMaxLength(7); // Stores #FFFFFF formats
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.Property(p => p.Name).IsRequired().HasMaxLength(150);
            entity.Property(p => p.Price).HasPrecision(18, 2);

            // Database level check constraint to prevent negative pricing
            entity.ToTable(t => t.HasCheckConstraint("CK_Product_Price", "\"Price\"  > 0"));
        });

        // --- FLOOR & TABLE VALIDATIONS ---
        modelBuilder.Entity<Floor>()
            .Property(f => f.Name).IsRequired().HasMaxLength(50);

        modelBuilder.Entity<Table>(entity =>
        {
            entity.Property(t => t.TableNumber).IsRequired().HasMaxLength(10);
            entity.ToTable(t => t.HasCheckConstraint("CK_Table_Seats", "\"Seats\"  > 0"));
        });

        // --- COUPON VALIDATIONS ---
        modelBuilder.Entity<Coupon>(entity =>
        {
            entity.Property(c => c.Code).IsRequired().HasMaxLength(20);
            entity.Property(c => c.Value).HasPrecision(18, 2);
            entity.HasIndex(c => c.Code).IsUnique();
            entity.ToTable(t => t.HasCheckConstraint("CK_Coupon_Value", "\"Value\" > 0"));
        });

        // --- ORDER & TRANSACTION VALIDATIONS ---
        modelBuilder.Entity<Order>(entity =>
        {
            entity.Property(o => o.OrderNumber).IsRequired().HasMaxLength(50);
            entity.HasIndex(o => o.OrderNumber).IsUnique();
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.Property(oi => oi.UnitPrice).HasPrecision(18, 2);
            entity.ToTable(t => t.HasCheckConstraint("CK_OrderItem_Quantity", "\"Quantity\"  > 0"));
            entity.ToTable(t => t.HasCheckConstraint("CK_OrderItem_UnitPrice", "\"UnitPrice\"  >= 0"));
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.Property(p => p.Amount).HasPrecision(18, 2);
            entity.ToTable(t => t.HasCheckConstraint("CK_Payment_Amount", "\"Amount\"  > 0"));
        });


    }
}
