using Flaadestation.Repository.Repositories.Interfaces;
using Flaadestation.Repository.Repositories;

namespace Flaadestation.ASP.Extensions
{
    public static class RepositoryExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<IOccupationRepository, OccupationRepository>();
            services.AddScoped<IBaseRepository, BaseRepository>();
            services.AddScoped<IStorageRepository, StorageRepository>();
            services.AddScoped<IStorageItemRepository, StorageItemRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IJobRepository, JobRepository>();
            services.AddScoped<IToolRepository, ToolRepository>();
            services.AddScoped<IVehicleRepository, VehicleRepository>();
            services.AddScoped<IMachineryRepository, MachineryRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<ILicenseRepository, LicenseRepository>();
            return services;
        }
    }
}
