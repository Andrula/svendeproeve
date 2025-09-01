using Flaadestation.Repository.Database.Entities;
using Microsoft.AspNetCore.Identity;
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
using static Flaadestation.Shared.Constants;

namespace Flaadestation.Repository.Database
{
    public class ApplicationDBContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
            : base(options)
        {

        }

        public DbSet<License> Licenses { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Tool> Tools { get; set; }
        public DbSet<Machinery> Machines { get; set; }

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
            // License ↔ User
            // ============================
            builder.Entity<License>()
                .HasOne(l => l.User)
                .WithOne(u => u.License)
                .HasForeignKey<License>(l => l.UserId);

            // ============================
            // License ↔ Company
            // ============================
            builder.Entity<License>()
                .HasOne(l => l.Company)
                .WithMany(c => c.Licenses)
                .HasForeignKey(l => l.CompanyId);

            // ============================
            // TPT inheritance mapping
            // ============================
            builder.Entity<Item>().ToTable("Items");
            builder.Entity<Employee>().ToTable("Employees");
            builder.Entity<Vehicle>().ToTable("Vehicles");
            builder.Entity<Tool>().ToTable("Tools");
            builder.Entity<Machinery>().ToTable("Machines");

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

            // ============================
            // Items ↔ DefaultStorage
            // ============================
            builder.Entity<Item>()
                .HasOne(i => i.DefaultStorage)
                .WithMany(s => s.ItemsWithThisStorageAsDefault)
                .HasForeignKey(i => i.DefaultStorageId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            // ============================
            // SEEDING AF DATA
            // ============================

            #region DataSeeding

            builder.Entity<Storage>().HasData(
                new Storage
                {
                    StorageId = new Guid("A4111111-1111-1111-1111-111111111111")
                },
                new Storage
                {
                    StorageId = new Guid("A4222222-2222-2222-2222-222222222222")
                },
                new Storage
                {
                    StorageId = new Guid("A4222341-2222-2222-2222-222222222222")
                },
                new Storage
                {
                    StorageId = new Guid("A4222341-2222-2255-9988-222222222222")
                }
            
            );

            /////////////////////
            // Companies
            /////////////////////

            builder.Entity<Company>().HasData(
                new Company
                {
                    CompanyId = new Guid("2432B27A-08AB-4623-B4E9-12834F822C47"),
                    Name = "Hvirts Entrepenør A/S",
                    AddressId = new Guid("0A3F507B-321E-32B8-E044-0003BA298018")
                },
                new Company
                {
                    CompanyId = new Guid("D74F0E90-EDB4-4A6C-A282-F3CC9D7D613A"),
                    Name = "Byggecenter Fyn A/S",
                    AddressId = new Guid("A48646AD-BE62-4A15-97DE-AFD124056462")
                }
            );

            /////////////////////
            // Users
            /////////////////////

            var hasher = new PasswordHasher<ApplicationUser>();

            builder.Entity<ApplicationUser>().HasData(
                new ApplicationUser
                {
                    Id = "67CE63C5-B13F-45A1-9990-581CC06C8FFB",
                    CompanyId = new Guid("2432B27A-08AB-4623-B4E9-12834F822C47"),

                    IsCompanyOwner = true,

                    UserName = "Erik CEO",
                    NormalizedUserName = "ERIK CEO",

                    Email = "admin@hvirts.dk",
                    NormalizedEmail = "ADMIN@HVIRTS.DK",

                    EmailConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString("D"),
                    ConcurrencyStamp = Guid.NewGuid().ToString("D"),

                    PasswordHash = hasher.HashPassword(null, "Admin123!")
                },
                new ApplicationUser
                {
                    Id = "A281B565-0154-4CF3-9556-59004B7D2F46",
                    CompanyId = new Guid("2432B27A-08AB-4623-B4E9-12834F822C47"),

                    IsCompanyOwner = false,

                    UserName = "staff@hvirts.dk",
                    NormalizedUserName = "STAFF@HVIRTS.DK",

                    Email = "staff@hvirts.dk",
                    NormalizedEmail = "STAFF@HVIRTS.DK",

                    EmailConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString("D"),
                    ConcurrencyStamp = Guid.NewGuid().ToString("D"),

                    PasswordHash = hasher.HashPassword(null, "Staff123!")
                },
                new ApplicationUser
                {
                    Id = "57A924E7-CE11-4C46-BEB5-41FD91793F06",
                    CompanyId = new Guid("D74F0E90-EDB4-4A6C-A282-F3CC9D7D613A"),

                    IsCompanyOwner = true,

                    UserName = "admin@bf.dk",
                    NormalizedUserName = "ADMIN@BF.DK",

                    Email = "admin@bf.dk",
                    NormalizedEmail = "ADMIN@BF.DK",

                    EmailConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString("D"),
                    ConcurrencyStamp = Guid.NewGuid().ToString("D"),

                    PasswordHash = hasher.HashPassword(null, "Admin123!")
                },
                new ApplicationUser
                {
                    Id = "5F8B806B-045A-4FCF-94EA-5C12F7DEA35A",
                    CompanyId = new Guid("D74F0E90-EDB4-4A6C-A282-F3CC9D7D613A"),

                    IsCompanyOwner = false,

                    UserName = "staff@bf.dk",
                    NormalizedUserName = "STAFF@BF.DK",

                    Email = "staff@bf.dk",
                    NormalizedEmail = "STAFF@BF.DK",

                    EmailConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString("D"),
                    ConcurrencyStamp = Guid.NewGuid().ToString("D"),

                    PasswordHash = hasher.HashPassword(null, "Staff123!")
                }
            );

            /////////////////////
            // Licenses
            /////////////////////

            builder.Entity<License>().HasData(
                new License
                {
                    LicenseId = new Guid("3B955728-1526-400D-A859-299850E74E3E"),
                    LicenseKey = new Guid("29EA26BD-02C9-41D1-BCB6-9FC72F2C44EF"),
                    CompanyId = new Guid("2432B27A-08AB-4623-B4E9-12834F822C47"),
                    UserId = "67CE63C5-B13F-45A1-9990-581CC06C8FFB",
                    ValidFrom = DateTime.Today.AddDays(-1),
                    ValidTo = DateTime.Today.AddYears(1),
                },
                new License
                {
                    LicenseId = new Guid("DED3BB29-A60D-4102-AEE7-E6D6F2839E4B"),
                    LicenseKey = new Guid("E79DEF5B-18AB-4DE0-8ACD-949D0B473683"),
                    CompanyId = new Guid("2432B27A-08AB-4623-B4E9-12834F822C47"),
                    UserId = "A281B565-0154-4CF3-9556-59004B7D2F46",
                    ValidFrom = DateTime.Today.AddDays(-1),
                    ValidTo = DateTime.Today.AddYears(1),
                },
                new License
                {
                    LicenseId = new Guid("61B79B8A-3D50-4739-9E7A-3A3A0E8AFBCF"),
                    LicenseKey = new Guid("21047F2E-27B6-46C4-8AD4-840AB6CFB7D8"),
                    CompanyId = new Guid("D74F0E90-EDB4-4A6C-A282-F3CC9D7D613A"),
                    UserId = "57A924E7-CE11-4C46-BEB5-41FD91793F06",
                    ValidFrom = DateTime.Today.AddDays(-1),
                    ValidTo = DateTime.Today.AddYears(1),
                },
                new License
                {
                    LicenseId = new Guid("2E115BDD-753F-4B98-A578-C95A2C95CF41"),
                    LicenseKey = new Guid("D4BB261A-B7EB-4879-A16C-E4B100F21E86"),
                    CompanyId = new Guid("D74F0E90-EDB4-4A6C-A282-F3CC9D7D613A"),
                    UserId = "5F8B806B-045A-4FCF-94EA-5C12F7DEA35A",
                    ValidFrom = DateTime.Today.AddDays(-1),
                    ValidTo = DateTime.Today.AddYears(1),
                }
            );
            /////////////////////
            // Occupation
            /////////////////////

            builder.Entity<Occupation>().HasData(
                new Occupation
                {
                    OccupationId = new Guid("F1234567-1234-1234-1234-123456789012"),
                    Name = "Tømrer"
                },
                new Occupation
                {
                    OccupationId = new Guid("F2345678-2345-2345-2345-234567890123"),
                    Name = "Elektriker"
                },
                new Occupation
                {
                    OccupationId = new Guid("F3456789-3456-3456-3456-345678901234"),
                    Name = "Murer"
                },
                new Occupation
                {
                    OccupationId = new Guid("F4567890-4567-4567-4567-456789012345"),
                    Name = "Kranfører"
                }
            );

            /////////////////////
            // Baser
            /////////////////////

            builder.Entity<Base>().HasData(
                new Base
                {
                    BaseId = new Guid("B1111111-1111-1111-1111-111111111111"),
                    Name = "Hovedlager Odense",
                    CompanyId = new Guid("2432B27A-08AB-4623-B4E9-12834F822C47"),
                    StorageId = new Guid("A4111111-1111-1111-1111-111111111111"),
                    AddressId = new Guid("0A3F507B-321E-32B8-E044-0003BA298018")
                },
                new Base
                {
                    BaseId = new Guid("B2222222-2222-2222-2222-222222222222"),
                    Name = "Lager København",
                    CompanyId = new Guid("2432B27A-08AB-4623-B4E9-12834F822C47"),
                    StorageId = new Guid("A4222222-2222-2222-2222-222222222222"),
                    AddressId = new Guid("A48646AD-BE62-4A15-97DE-AFD124056462")
                }
            );

            /////////////////////
            // Employees
            /////////////////////
         
            builder.Entity<Employee>().HasData(
                new Employee
                {
                    ItemId = new Guid("E1111111-1111-1111-1111-111111111111"),
                    FirstName = "Lars",
                    LastName = "Nielsen",
                    Email = "lars.nielsen@hvirts.dk",
                    Phone = "12345678",
                    OccupationId = new Guid("F1234567-1234-1234-1234-123456789012"),
                    VehicleId = null,
                    ItemType = ItemType.Employee,
                    Note = "Erfaren tømrer",
                    DefaultStorageId = new Guid("A4111111-1111-1111-1111-111111111111"),
                    CompanyId = new Guid("2432B27A-08AB-4623-B4E9-12834F822C47"),
                    ImageId = null
                },
                new Employee
                {
                    ItemId = new Guid("E2222222-2222-2222-2222-222222222222"),
                    FirstName = "Mette",
                    LastName = "Hansen",
                    Email = "mette.hansen@hvirts.dk",
                    Phone = "87654321",
                    OccupationId = new Guid("F2345678-2345-2345-2345-234567890123"),
                    VehicleId = null,
                    ItemType = ItemType.Employee,
                    Note = "Junior elektriker",
                    DefaultStorageId = new Guid("A4111111-1111-1111-1111-111111111111"),
                    CompanyId = new Guid("2432B27A-08AB-4623-B4E9-12834F822C47"),
                    ImageId = null
                }
            );

            /////////////////////
            // Tools
            /////////////////////

            builder.Entity<Tool>().HasData(
                new
                {
                    ItemId = new Guid("A2111111-1111-1111-1111-111111111111"),
                    Name = "Hilti Boremaskine",
                    ItemType = ItemType.Tool,
                    Note = "Professionel boremaskine",
                    DefaultStorageId = new Guid("A4111111-1111-1111-1111-111111111111"),
                    CompanyId = new Guid("2432B27A-08AB-4623-B4E9-12834F822C47"),
                    ImageId = (Guid?)null
                }
            );

            builder.Entity<Tool>().HasData(
                new
                {
                    ItemId = new Guid("A2111111-3333-3333-3333-111111111111"),
                    Name = "Deers Boltsakt",
                    ItemType = ItemType.Tool,
                    Note = "Boltsaks til store grene",
                    DefaultStorageId = new Guid("A4222222-2222-2222-2222-222222222222"),
                    CompanyId = new Guid("2432B27A-08AB-4623-B4E9-12834F822C47"),
                    ImageId = (Guid?)null
                }
            );

            /////////////////////
            // Machinery
            /////////////////////

            builder.Entity<Machinery>().HasData(
                new Machinery
                {
                    ItemId = new Guid("A3111111-4123-6342-1111-111111111111"), // Changed M to A3
                    Name = "Liebherr Minikran",
                    ItemType = ItemType.Machine,
                    Note = "Meget stor kran 100 l",
                    DefaultStorageId = new Guid("A4222341-2222-2255-9988-222222222222"),
                    CompanyId = new Guid("2432B27A-08AB-4623-B4E9-12834F822C47"),
                    ImageId = (Guid?)null
                }
            );

            builder.Entity<Machinery>().HasData(
                new Machinery
                {
                    ItemId = new Guid("A3111111-1111-3211-1111-523111111111"), 
                    Name = "John Deere Traktor",
                    ItemType = ItemType.Machine,
                    Note = "Traktor 100 l",
                    DefaultStorageId = new Guid("A4222341-2222-2222-2222-222222222222"),
                    CompanyId = new Guid("2432B27A-08AB-4623-B4E9-12834F822C47"),
                    ImageId = (Guid?)null
                }
            );

            /////////////////////
            // Vehicles
            /////////////////////
            
            builder.Entity<Vehicle>().HasData(
                new Vehicle
                {
                    ItemId = new Guid("F1111111-2312-1111-1111-136111111111"), 
                    Model = "Ford Transit",
                    LicensePlate = "AB12345",
                    DefaultStorageId = new Guid("A4111111-1111-1111-1111-111111111111"),
                    CompanyId = new Guid("2432B27A-08AB-4623-B4E9-12834F822C47"),
                    ImageId = (Guid?)null
                }
            );

            /////////////////////
            // Customers
            /////////////////////

            builder.Entity<Customer>().HasData(
                new Customer
                {
                    CustomerId = new Guid("C1111111-1111-1111-1111-111111111111"),
                    Name = "Københavns Kommune",
                    Email = "kontakt@kk.dk",
                    Phone = "33663366",
                    CompanyId = new Guid("2432B27A-08AB-4623-B4E9-12834F822C47"),
                    AddressId = new Guid("0A3F507B-321E-32B8-E044-0003BA298018")
                },
                new Customer
                {
                    CustomerId = new Guid("C2222222-2222-2222-2222-222222222222"),
                    Name = "Odense Kommune",
                    Email = "info@odense.dk",
                    Phone = "65513000",
                    CompanyId = new Guid("2432B27A-08AB-4623-B4E9-12834F822C47"),
                    AddressId = new Guid("A48646AD-BE62-4A15-97DE-AFD124056462")
                }
            );

            /////////////////////
            // Jobs
            /////////////////////

            builder.Entity<Job>().HasData(
                new Job
                {
                    JobId = new Guid("21111111-1111-1111-1111-111111111111"),
                    Title = "Renovering af Københavns Rådhus",
                    Description = "Omfattende renovering af den historiske bygning med fokus på træværk og elektrisk installation",
                    ScheduledStart = new DateTime(2025, 9, 15, 8, 0, 0),
                    ScheduledEnd = new DateTime(2025, 12, 20, 16, 0, 0),
                    CompanyId = new Guid("2432B27A-08AB-4623-B4E9-12834F822C47"),
                    StorageId = new Guid("A4222222-2222-2222-2222-222222222222"),
                    AddressId = new Guid("0A3F507B-321E-32B8-E044-0003BA298018")
                },
                new Job
                {
                    JobId = new Guid("42222222-2222-2222-2222-222222222222"),
                    Title = "Anlægsarbejde i Odense Park",
                    Description = "Etablering af nye stier og parkering, inkluderer gravearbejde og asfaltering",
                    ScheduledStart = new DateTime(2025, 10, 1, 7, 0, 0),
                    ScheduledEnd = new DateTime(2025, 11, 30, 15, 0, 0),
                    CompanyId = new Guid("2432B27A-08AB-4623-B4E9-12834F822C47"),
                    StorageId = new Guid("A4111111-1111-1111-1111-111111111111"), 
                    AddressId = new Guid("A48646AD-BE62-4A15-97DE-AFD124056462")
                }
            );

            builder.Entity("CustomerJob").HasData(
     new { CustomerId = new Guid("C1111111-1111-1111-1111-111111111111"), JobId = new Guid("21111111-1111-1111-1111-111111111111") },
     new { CustomerId = new Guid("C2222222-2222-2222-2222-222222222222"), JobId = new Guid("42222222-2222-2222-2222-222222222222") }
 );

            builder.Entity<StorageItem>().HasData(
                new StorageItem
                {
                    StorageItemId = new Guid("61111112-1111-1111-1111-111111111111"),
                    ScheduledStart = new DateTime(2025, 9, 15, 0, 0, 0),
                    ScheduledEnd = new DateTime(2025, 12, 20, 23, 59, 59),
                    Note = "Tildelt Københavns Rådhus projekt",
                    ItemId = new Guid("E1111111-1111-1111-1111-111111111111"),
                    StorageId = new Guid("A4222222-2222-2222-2222-222222222222")
                },
                new StorageItem
                {
                    StorageItemId = new Guid("B6222222-1111-1111-1111-111111111111"),
                    ScheduledStart = new DateTime(2025, 10, 1, 0, 0, 0),
                    ScheduledEnd = new DateTime(2025, 12, 20, 23, 59, 59),
                    Note = "Tildelt Københavns Rådhus projekt - elektrisk arbejde",
                    ItemId = new Guid("E2222222-2222-2222-2222-222222222222"),
                    StorageId = new Guid("A4222222-2222-2222-2222-222222222222")
                },
                new StorageItem
                {
                    StorageItemId = new Guid("B0333332-1111-1111-1111-111111111111"),
                    ScheduledStart = new DateTime(2025, 9, 15, 0, 0, 0),
                    ScheduledEnd = new DateTime(2025, 12, 20, 23, 59, 59),
                    Note = "Udstationeret til Københavns Rådhus projekt",
                    ItemId = new Guid("A2111111-1111-1111-1111-111111111111"),
                    StorageId = new Guid("A4222222-2222-2222-2222-222222222222")
                },
                new StorageItem
                {
                    StorageItemId = new Guid("B7944442-1111-1111-1111-111111111111"),
                    ScheduledStart = new DateTime(2025, 10, 1, 0, 0, 0),
                    ScheduledEnd = new DateTime(2025, 11, 30, 23, 59, 59),
                    Note = "Tildelt Odense Park projekt",
                    ItemId = new Guid("A3111111-1111-3211-1111-523111111111"),
                    StorageId = new Guid("A4111111-1111-1111-1111-111111111111")
                },
                new StorageItem
                {
                    StorageItemId = new Guid("B3255552-1111-1111-1111-111111111111"),
                    ScheduledStart = new DateTime(2025, 9, 15, 0, 0, 0),
                    ScheduledEnd = new DateTime(2025, 12, 20, 23, 59, 59),
                    Note = "Transport til Københavns Rådhus projekt",
                    ItemId = new Guid("F1111111-2312-1111-1111-136111111111"),
                    StorageId = new Guid("A4222222-2222-2222-2222-222222222222")
                }
            );


            #endregion

            base.OnModelCreating(builder);
        }
    }
}
