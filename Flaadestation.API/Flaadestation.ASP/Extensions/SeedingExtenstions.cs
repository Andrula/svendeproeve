using Flaadestation.Repository.Database.Entities;
using Flaadestation.Repository.Database;
using Microsoft.AspNetCore.Identity;
using static Flaadestation.Shared.Constants;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Flaadestation.ASP.Extensions
{
    public static class SeedingExtenstions
    {
        public static async Task<WebApplication> SeedDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;

            try
            {
                var context = services.GetRequiredService<ApplicationDBContext>();
                var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

                await context.Database.MigrateAsync();

                await SeedDataAsync(context, userManager);
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred while seeding the database.");
                throw;
            }

            return app;
        }

        private static async Task SeedDataAsync(ApplicationDBContext context, UserManager<ApplicationUser> userManager)
        {
            await SeedBaseDataAsync(context);

            await SeedUsersAsync(context, userManager);
        }

        private static async Task SeedBaseDataAsync(ApplicationDBContext context)
        {
            // Seed Companies
            if (!context.Companies.Any())
            {
                var companies = new[]
                {
                    new Company
                    {
                        CompanyId = new Guid("2432B27A-08AB-4623-B4E9-12834F822C47"),
                        Name = "Hvirts Entrepenør A/S",
                        AddressId = new Guid("DC863FC9-A5B6-4832-839D-6862E8F2014D")
                    },
                    new Company
                    {
                        CompanyId = new Guid("D74F0E90-EDB4-4A6C-A282-F3CC9D7D613A"),
                        Name = "Byggecenter Fyn A/S",
                        AddressId = new Guid("3D1D79C6-E5E6-4E44-9293-37ACCC1FE28F")
                    }
                };

                context.Companies.AddRange(companies);
                await context.SaveChangesAsync();
            }

            // Seed Storages
            if (!context.Storages.Any())
            {
                var storages = new[]
                {
                    new Storage { StorageId = new Guid("A4111111-1111-1111-1111-111111111111") },
                    new Storage { StorageId = new Guid("A4222222-2222-2222-2222-222222222222") },
                    new Storage { StorageId = new Guid("A4222341-2222-2222-2222-222222222222") },
                    new Storage { StorageId = new Guid("A4222341-2222-2255-9988-222222222222") }
                };

                context.Storages.AddRange(storages);
                await context.SaveChangesAsync();
            }

            // Seed Occupations
            if (!context.Occupations.Any())
            {
                var occupations = new[]
                {
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
                };

                context.Occupations.AddRange(occupations);
                await context.SaveChangesAsync();
            }

            // Seed Bases
            if (!context.Bases.Any())
            {
                var bases = new[]
                {
                    new Base
                    {
                        BaseId = new Guid("B1111111-1111-1111-1111-111111111111"),
                        Name = "Hovedlager Odense",
                        CompanyId = new Guid("2432B27A-08AB-4623-B4E9-12834F822C47"),
                        StorageId = new Guid("A4111111-1111-1111-1111-111111111111"),
                        AddressId = new Guid("0A3F50B4-B83E-32B8-E044-0003BA298018")
                    },
                    new Base
                    {
                        BaseId = new Guid("B2222222-2222-2222-2222-222222222222"),
                        Name = "Lager København",
                        CompanyId = new Guid("2432B27A-08AB-4623-B4E9-12834F822C47"),
                        StorageId = new Guid("A4222222-2222-2222-2222-222222222222"),
                        AddressId = new Guid("DC863FC9-A5B6-4832-839D-6862E8F2014D")
                    }
                };

                context.Bases.AddRange(bases);
                await context.SaveChangesAsync();
            }

            await SeedCustomersAsync(context);
            await SeedJobsAsync(context);
            await SeedItemsAsync(context);
        }

        private static async Task SeedUsersAsync(ApplicationDBContext context, UserManager<ApplicationUser> userManager)
        {
            var users = new[]
            {
                new {
                    Id = "67CE63C5-B13F-45A1-9990-581CC06C8FFB",
                    CompanyId = new Guid("2432B27A-08AB-4623-B4E9-12834F822C47"),
                    IsCompanyOwner = true,
                    UserName = "admin@hvirts.dk",
                    Email = "admin@hvirts.dk",
                    Password = "Admin123!",
                },
                new {
                    Id = "A281B565-0154-4CF3-9556-59004B7D2F46",
                    CompanyId = new Guid("2432B27A-08AB-4623-B4E9-12834F822C47"),
                    IsCompanyOwner = false,
                    UserName = "staff@hvirts.dk",
                    Email = "staff@hvirts.dk",
                    Password = "Staff123!",
                },
                new {
                    Id = "57A924E7-CE11-4C46-BEB5-41FD91793F06",
                    CompanyId = new Guid("D74F0E90-EDB4-4A6C-A282-F3CC9D7D613A"),
                    IsCompanyOwner = true,
                    UserName = "admin@bf.dk",
                    Email = "admin@bf.dk",
                    Password = "Admin123!",
                },
                new {
                    Id = "5F8B806B-045A-4FCF-94EA-5C12F7DEA35A",
                    CompanyId = new Guid("D74F0E90-EDB4-4A6C-A282-F3CC9D7D613A"),
                    IsCompanyOwner = false,
                    UserName = "staff@bf.dk",
                    Email = "staff@bf.dk",
                    Password = "Staff123!",
                }
            };

            foreach (var userData in users)
            {
                var existingUser = await userManager.FindByEmailAsync(userData.Email);
                if (existingUser == null)
                {
                    var user = new ApplicationUser
                    {
                        Id = userData.Id,
                        UserName = userData.UserName,
                        Email = userData.Email,
                        EmailConfirmed = true,
                        CompanyId = userData.CompanyId,
                        IsCompanyOwner = userData.IsCompanyOwner
                    };

                    var result = await userManager.CreateAsync(user, userData.Password);

                    if (result.Succeeded)
                    {
                        await userManager.AddClaimAsync(user, new Claim("IsCompanyOwner", user.IsCompanyOwner.ToString().ToLower(), ClaimValueTypes.Boolean));
                        await userManager.AddClaimAsync(user, new Claim("CompanyId", user.CompanyId.ToString().ToUpper()));
                        await CreateLicenseForUser(context, user);
                    }
                    else
                    {
                        throw new InvalidOperationException($"Failed to create user {userData.Email}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                    }
                }
            }
        }

        private static async Task CreateLicenseForUser(ApplicationDBContext context, ApplicationUser user)
        {
            var licenseData = user.Id switch
            {
                "67CE63C5-B13F-45A1-9990-581CC06C8FFB" => (new Guid("3B955728-1526-400D-A859-299850E74E3E"), new Guid("29EA26BD-02C9-41D1-BCB6-9FC72F2C44EF")),
                "A281B565-0154-4CF3-9556-59004B7D2F46" => (new Guid("DED3BB29-A60D-4102-AEE7-E6D6F2839E4B"), new Guid("E79DEF5B-18AB-4DE0-8ACD-949D0B473683")),
                "57A924E7-CE11-4C46-BEB5-41FD91793F06" => (new Guid("61B79B8A-3D50-4739-9E7A-3A3A0E8AFBCF"), new Guid("21047F2E-27B6-46C4-8AD4-840AB6CFB7D8")),
                "5F8B806B-045A-4FCF-94EA-5C12F7DEA35A" => (new Guid("2E115BDD-753F-4B98-A578-C95A2C95CF41"), new Guid("D4BB261A-B7EB-4879-A16C-E4B100F21E86")),
                _ => (Guid.NewGuid(), Guid.NewGuid())
            };

            var license = new License
            {
                LicenseId = licenseData.Item1,
                LicenseKey = licenseData.Item2,
                CompanyId = user.CompanyId,
                UserId = user.Id,
                ValidFrom = DateTime.Today.AddDays(-1),
                ValidTo = DateTime.Today.AddYears(1),
            };

            context.Licenses.Add(license);
            await context.SaveChangesAsync();
        }

        private static async Task SeedCustomersAsync(ApplicationDBContext context)
        {
            if (!context.Customers.Any())
            {
                var customers = new[]
                {
                    new Customer
                    {
                        CustomerId = new Guid("C1111111-1111-1111-1111-111111111111"),
                        Name = "Københavns Kommune",
                        Email = "kontakt@kk.dk",
                        Phone = "33663366",
                        CompanyId = new Guid("2432B27A-08AB-4623-B4E9-12834F822C47"),
                        AddressId = new Guid("7141865A-85C3-460D-AA30-DD3E273EBB9E")
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
                };

                context.Customers.AddRange(customers);
                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedJobsAsync(ApplicationDBContext context)
        {
            if (!context.Jobs.Any())
            {
                var jobs = new[]
                {
                    new Job
                    {
                        JobId = new Guid("21111111-1111-1111-1111-111111111111"),
                        Title = "Renovering af Københavns Rådhus",
                        Description = "Omfattende renovering af den historiske bygning med fokus på træværk og elektrisk installation",
                        ScheduledStart = new DateTime(2025, 9, 15, 8, 0, 0),
                        ScheduledEnd = new DateTime(2025, 12, 20, 16, 0, 0),
                        CompanyId = new Guid("2432B27A-08AB-4623-B4E9-12834F822C47"),
                        StorageId = new Guid("A4222341-2222-2222-2222-222222222222"),
                        AddressId = new Guid("70865C44-D570-44E7-A6F5-6F7C90ADD725")
                    },
                    new Job
                    {
                        JobId = new Guid("42222222-2222-2222-2222-222222222222"),
                        Title = "Anlægsarbejde i Odense Park",
                        Description = "Etablering af nye stier og parkering, inkluderer gravearbejde og asfaltering",
                        ScheduledStart = new DateTime(2025, 10, 1, 7, 0, 0),
                        ScheduledEnd = new DateTime(2025, 11, 30, 15, 0, 0),
                        CompanyId = new Guid("2432B27A-08AB-4623-B4E9-12834F822C47"),
                        StorageId = new Guid("A4222341-2222-2255-9988-222222222222"),
                        AddressId = new Guid("E5430358-D5D7-48D6-BCBC-324AF7476BCB")
                    }
                };

                context.Jobs.AddRange(jobs);
                await context.SaveChangesAsync();

                var job1 = await context.Jobs.FirstAsync(j => j.JobId == new Guid("21111111-1111-1111-1111-111111111111"));
                var job2 = await context.Jobs.FirstAsync(j => j.JobId == new Guid("42222222-2222-2222-2222-222222222222"));
                var customer1 = await context.Customers.FirstAsync(c => c.CustomerId == new Guid("C1111111-1111-1111-1111-111111111111"));
                var customer2 = await context.Customers.FirstAsync(c => c.CustomerId == new Guid("C2222222-2222-2222-2222-222222222222"));

                job1.Customers.Add(customer1);
                job2.Customers.Add(customer2);

                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedItemsAsync(ApplicationDBContext context)
        {
            if (!context.Employees.Any())
            {
                var employees = new[]
                {
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
                };

                context.Employees.AddRange(employees);
                await context.SaveChangesAsync();
            }

            if (!context.Tools.Any())
            {
                var tools = new[]
                {
                    new Tool
                    {
                        ItemId = new Guid("A2111111-1111-1111-1111-111111111111"),
                        Name = "Hilti Boremaskine",
                        ItemType = ItemType.Tool,
                        Note = "Professionel boremaskine",
                        DefaultStorageId = new Guid("A4111111-1111-1111-1111-111111111111"),
                        CompanyId = new Guid("2432B27A-08AB-4623-B4E9-12834F822C47"),
                        ImageId = null
                    },
                    new Tool
                    {
                        ItemId = new Guid("A2111111-3333-3333-3333-111111111111"),
                        Name = "Deers Boltsakt",
                        ItemType = ItemType.Tool,
                        Note = "Boltsaks til store grene",
                        DefaultStorageId = new Guid("A4222222-2222-2222-2222-222222222222"),
                        CompanyId = new Guid("2432B27A-08AB-4623-B4E9-12834F822C47"),
                        ImageId = null
                    }
                };

                context.Tools.AddRange(tools);
                await context.SaveChangesAsync();
            }

            if (!context.Machines.Any())
            {
                var machinery = new[]
                {
                    new Machinery
                    {
                        ItemId = new Guid("A3111111-4123-6342-1111-111111111111"),
                        Name = "Liebherr Minikran",
                        ItemType = ItemType.Machine,
                        Note = "Meget stor kran 100 l",
                        DefaultStorageId = new Guid("A4222341-2222-2255-9988-222222222222"),
                        CompanyId = new Guid("2432B27A-08AB-4623-B4E9-12834F822C47"),
                        ImageId = null
                    },
                    new Machinery
                    {
                        ItemId = new Guid("A3111111-1111-3211-1111-523111111111"),
                        Name = "John Deere Traktor",
                        ItemType = ItemType.Machine,
                        Note = "Traktor 100 l",
                        DefaultStorageId = new Guid("A4222341-2222-2222-2222-222222222222"),
                        CompanyId = new Guid("2432B27A-08AB-4623-B4E9-12834F822C47"),
                        ImageId = null
                    }
                };

                context.Machines.AddRange(machinery);
                await context.SaveChangesAsync();
            }

            if (!context.Vehicles.Any())
            {
                var vehicles = new[]
                {
                    new Vehicle
                    {
                        ItemId = new Guid("F1111111-2312-1111-1111-136111111111"),
                        Model = "Ford Transit",
                        LicensePlate = "AB12345",
                        ItemType = ItemType.Vehicle,
                        DefaultStorageId = new Guid("A4111111-1111-1111-1111-111111111111"),
                        CompanyId = new Guid("2432B27A-08AB-4623-B4E9-12834F822C47"),
                        ImageId = null
                    }
                };

                context.Vehicles.AddRange(vehicles);
                await context.SaveChangesAsync();
            }

            if (!context.StorageItems.Any())
            {
                var storageItems = new[]
                {
                    new StorageItem
                    {
                        StorageItemId = new Guid("61111112-1111-1111-1111-111111111111"),
                        ScheduledStart = new DateTime(2025, 9, 15, 0, 0, 0),
                        ScheduledEnd = new DateTime(2025, 12, 20, 23, 59, 59),
                        Note = "Tildelt Københavns Rådhus projekt",
                        ItemId = new Guid("E1111111-1111-1111-1111-111111111111"),
                        StorageId = new Guid("A4222341-2222-2222-2222-222222222222")
                    },
                    new StorageItem
                    {
                        StorageItemId = new Guid("B6222222-1111-1111-1111-111111111111"),
                        ScheduledStart = new DateTime(2025, 10, 1, 0, 0, 0),
                        ScheduledEnd = new DateTime(2025, 12, 20, 23, 59, 59),
                        Note = "Tildelt Københavns Rådhus projekt - elektrisk arbejde",
                        ItemId = new Guid("E2222222-2222-2222-2222-222222222222"),
                        StorageId = new Guid("A4222341-2222-2222-2222-222222222222")
                    },
                    new StorageItem
                    {
                        StorageItemId = new Guid("B0333332-1111-1111-1111-111111111111"),
                        ScheduledStart = new DateTime(2025, 9, 15, 0, 0, 0),
                        ScheduledEnd = new DateTime(2025, 12, 20, 23, 59, 59),
                        Note = "Udstationeret til Københavns Rådhus projekt",
                        ItemId = new Guid("A2111111-1111-1111-1111-111111111111"),
                        StorageId = new Guid("A4222341-2222-2222-2222-222222222222")
                    },
                    new StorageItem
                    {
                        StorageItemId = new Guid("B7944442-1111-1111-1111-111111111111"),
                        ScheduledStart = new DateTime(2025, 10, 1, 0, 0, 0),
                        ScheduledEnd = new DateTime(2025, 11, 30, 23, 59, 59),
                        Note = "Tildelt Odense Park projekt",
                        ItemId = new Guid("A3111111-1111-3211-1111-523111111111"),
                        StorageId = new Guid("A4222341-2222-2255-9988-222222222222")
                    },
                    new StorageItem
                    {
                        StorageItemId = new Guid("B3255552-1111-1111-1111-111111111111"),
                        ScheduledStart = new DateTime(2025, 9, 15, 0, 0, 0),
                        ScheduledEnd = new DateTime(2025, 12, 20, 23, 59, 59),
                        Note = "Transport til Københavns Rådhus projekt",
                        ItemId = new Guid("F1111111-2312-1111-1111-136111111111"),
                        StorageId = new Guid("A4222341-2222-2222-2222-222222222222")
                    }
                };

                context.StorageItems.AddRange(storageItems);
                await context.SaveChangesAsync();
            }
        }
    }
}
