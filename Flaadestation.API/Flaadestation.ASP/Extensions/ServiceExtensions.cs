using Flaadestation.Service.Interfaces;
using Flaadestation.Service.Services;
using System.Data;

namespace Flaadestation.ASP.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddBusinessServices(this IServiceCollection services)
        {
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<IOccupationService, OccupationService>();
            services.AddScoped<IBaseService, BaseService>();
            services.AddScoped<IStorageService, StorageService>();
            services.AddScoped<IStorageItemService, StorageItemService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IJobService, JobService>();
            services.AddScoped<IToolService, ToolService>();
            services.AddScoped<IVehicleService, VehicleService>();
            services.AddScoped<IMachineryService, MachineryService>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            return services;
        }
    }
}
