using Microsoft.EntityFrameworkCore;
using ProjectJWTeCommerce.Models.CartAPIs;
using ProjectJWTeCommerce.Models.ProductAPIs;
using ProjectJWTeCommerce.Models.SellerAPIs;
using ProjectJWTeCommerce.Models.UserAPIs;

namespace ProjectJWTeCommerce
{
    public class MyDBContext : DbContext
    {
        public MyDBContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<UserDetails> UserDetails { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<Seller> sellers { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<ItemQuantity> ItemQuantity { get; set; }
        public DbSet<Features> Features { get; set; }
        public DbSet<Cart> Cart { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserDetails>()
                .HasMany(u => u.Addresses)
                .WithOne()
                .IsRequired(false); // 👈 This makes Addresses optional

            modelBuilder.Entity<Address>()
                .HasOne(a => a.user)      // Each Address has one User
                .WithMany(u => u.Addresses) // Each User can have many Addresses
                .HasForeignKey(a => a.userId) // Explicitly specify the FK
                .OnDelete(DeleteBehavior.Cascade); // Optional: Cascade delete

            modelBuilder.Entity<Product>()
                .HasOne(p => p.seller)
                .WithMany(s => s.Products)
                .HasForeignKey(p => p.SId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ItemQuantity>()
                .HasOne(i => i.product)
                .WithMany(p => p.itemQuantities)
                .HasForeignKey(i => i.PId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Features>()
                .HasOne(f => f.product)
                .WithMany(p => p.features)
                .HasForeignKey(f => f.PId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ItemQuantity>()
                .HasOne(i => i.cart)
                .WithMany(c => c.Items)
                .HasForeignKey(i => i.CId)
                .OnDelete(DeleteBehavior.Cascade);




            base.OnModelCreating(modelBuilder);
        

    }

}
}
