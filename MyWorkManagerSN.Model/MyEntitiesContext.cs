using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace MyWorkManagerSN.Model
{
    public partial class MyEntitiesContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data source=DESKTOP-TGDPFB8\SQLEXPRESS;initial catalog=WorkManagerCoreDB;integrated security=True;");
        }

        //public DbSet<MyProjectGVV> MyProjectGVV { get; set; }
        // public DbSet<MyProject> MyTestC { get; set; }
        public DbSet <Category> Category { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<OrderLine> OrderLines { get; set; }
        public DbSet<PaymentMode> PaymentModes { get; set; }
        public DbSet<SubscriptionWithAmount> SubscriptionWithAmounts { get; set; }

        protected override void OnModelCreating(Microsoft.EntityFrameworkCore.ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().OwnsOne(u => u.Address);
            modelBuilder.Entity<Customer>().OwnsOne(c => c.Address);
            modelBuilder.Entity<User>().OwnsOne(u=>u.AccountOptions);
        }



    }
}
