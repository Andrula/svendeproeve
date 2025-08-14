using Flaadestation.Repository.Database.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Flaadestation.Repository.Database
{
    public class ApplicationDBContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
            : base(options)
        {

        }

        public DbSet<Item> Items { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Tool> Tools { get; set; }
        public DbSet<Machine> Machines { get; set; }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Storage> Storages { get; set; }
        public DbSet<StorageItem> StorageItems { get; set; }
        public DbSet<Base> Bases { get; set; }
        public DbSet<Occupation> Occupations { get; set; }
        public DbSet<Image> Images { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            // ============================
            // TPT inheritance mapping
            // ============================
            builder.Entity<Item>().ToTable("Items");
            builder.Entity<Employee>().ToTable("Employees");
            builder.Entity<Vehicle>().ToTable("Vehicles");
            builder.Entity<Tool>().ToTable("Tools");
            builder.Entity<Machine>().ToTable("Machines");

            // ============================
            // Customers ↔ Company
            // ============================
            builder.Entity<Customer>()
                .HasOne(c => c.Company)
                .WithMany(co => co.Customers)
                .HasForeignKey(c => c.CompanyId);

            // Customers ↔ Jobs (many-to-many without join entity)
            builder.Entity<Customer>()
                .HasMany(c => c.Jobs)
                .WithMany(j => j.Customers)
                .UsingEntity<Dictionary<string, object>>( // Fjerner CascadeDelete mellem Customer <> Job
                    "CustomerJob",
                    j => j.HasOne<Job>()
                          .WithMany()
                          .HasForeignKey("JobId")
                          .OnDelete(DeleteBehavior.NoAction),
                    c => c.HasOne<Customer>()
                          .WithMany()
                          .HasForeignKey("CustomerId")
                          .OnDelete(DeleteBehavior.NoAction)
                );

            // ============================
            // Jobs ↔ Company
            // ============================
            builder.Entity<Job>()
                .HasOne(j => j.Company)
                .WithMany(c => c.Jobs)
                .HasForeignKey(j => j.CompanyId);

            // Jobs ↔ Storage
            builder.Entity<Job>()
                .HasOne(j => j.Storage)
                .WithOne(s => s.Job)
                .HasForeignKey<Job>(j => j.StorageId)
                .OnDelete(DeleteBehavior.Cascade);

            // ============================
            // Storage_Items ↔ Items
            // ============================
            builder.Entity<StorageItem>()
                .HasOne(si => si.Item)
                .WithMany(i => i.StorageItems)
                .HasForeignKey(si => si.ItemId);

            // Storage_Items ↔ Storage
            builder.Entity<StorageItem>()
                .HasOne(si => si.Storage)
                .WithMany(s => s.StorageItems)
                .HasForeignKey(si => si.StorageId);

            // ============================
            // Bases ↔ Company
            // ============================
            builder.Entity<Base>()
                .HasOne(b => b.Company)
                .WithMany(c => c.Bases)
                .HasForeignKey(b => b.CompanyId);

            // Bases ↔ Storage
            builder.Entity<Base>()
                .HasOne(b => b.Storage)
                .WithOne(s => s.Base)
                .HasForeignKey<Base>(b => b.StorageId)
                .OnDelete(DeleteBehavior.Cascade);

            // ============================
            // Users ↔ Company
            // ============================
            builder.Entity<ApplicationUser>()
                .HasOne(u => u.Company)
                .WithMany(c => c.Users)
                .HasForeignKey(u => u.CompanyId);


            // ============================
            // Employees ↔ Occupations
            // ============================
            builder.Entity<Employee>()
                .HasOne(e => e.Occupation)
                .WithMany(o => o.Employees)
                .HasForeignKey(e => e.OccupationId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            // Employees ↔ Vehicles
            builder.Entity<Employee>()
                .HasOne(e => e.Vehicle)
                .WithMany(v => v.Employees)
                .HasForeignKey(e => e.VehicleId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            // ============================
            // Tools ↔ Vehicles
            // ============================
            builder.Entity<Tool>()
                .HasOne(t => t.Vehicle)
                .WithMany(v => v.Tools)
                .HasForeignKey(t => t.VehicleId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            // ============================
            // Items ↔ Images
            // ============================
            builder.Entity<Item>()
                .HasOne(i => i.Image)
                .WithOne(img => img.Item)
                .HasForeignKey<Item>(i => i.ImageId);

            base.OnModelCreating(builder);
        }
    }
}
