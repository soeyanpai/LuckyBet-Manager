using LuckyBet.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace LuckyBet.Data;

public class LotteryDbContext : DbContext
{
    public DbSet<Contact> Contacts { get; set; }
    public DbSet<Voucher> Vouchers { get; set; }
    public DbSet<Sale> Sales { get; set; }
    public DbSet<UplineSale> UplineSales { get; set; }
    public DbSet<WinningNumber> WinningNumbers { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Setting> Settings { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=LuckyBetDb;Trusted_Connection=True;");
    }

    // --- NEW CODE SECTION ---
    // Fluent API for defining entity relationships
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Voucher and Contact Relationship
        modelBuilder.Entity<Voucher>()
            .HasOne(v => v.Contact)          // Voucher တစ်ခုမှာ Contact တစ်ယောက် ရှိတယ်
            .WithMany(c => c.Vouchers)       // Contact တစ်ယောက်မှာ Voucher တွေ အများကြီး ရှိနိုင်တယ်
            .HasForeignKey(v => v.ContactId) // သူတို့ကို 'ContactId' column နဲ့ ချိတ်ဆက်ပါ
            .OnDelete(DeleteBehavior.Restrict); // Contact ကိုဖျက်ရင် သူ့ရဲ့ Voucher တွေပါ ပျက်မသွားအောင် ကာကွယ်ပါ

        // Voucher and User Relationship
        modelBuilder.Entity<Voucher>()
            .HasOne(v => v.User)
            .WithMany(u => u.Vouchers)
            .HasForeignKey(v => v.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Sale and Voucher Relationship
        modelBuilder.Entity<Sale>()
            .HasOne(s => s.Voucher)
            .WithMany(v => v.Sales)
            .HasForeignKey(s => s.VoucherId)
            .OnDelete(DeleteBehavior.Cascade); // Voucher ကိုဖျက်ရင် သူ့ထဲက Sale တွေပါ အကုန်ဖျက်ပါ

        // UplineSale and Contact (Upline) Relationship
        modelBuilder.Entity<UplineSale>()
            .HasOne(us => us.Contact)
            .WithMany(c => c.UplineSales)
            .HasForeignKey(us => us.ContactId)
            .OnDelete(DeleteBehavior.Restrict);

        // UplineSale and User Relationship
        modelBuilder.Entity<UplineSale>()
            .HasOne(us => us.User)
            .WithMany(u => u.UplineSales)
            .HasForeignKey(us => us.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}