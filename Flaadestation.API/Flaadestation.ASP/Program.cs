
using Flaadestation.Repository.Database;
using Flaadestation.Repository.Repositories;
using Flaadestation.Repository.Repositories.Interfaces;
using Flaadestation.Service.Interfaces;
using Flaadestation.Service.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Flaadestation.ASP
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            // repositories
            builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
            builder.Services.AddScoped<IOccupationRepository, OccupationRepository>();

            // services
            builder.Services.AddScoped<ICompanyService, CompanyService>();
            builder.Services.AddScoped<IOccupationService, OccupationService>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<ApplicationDBContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("Database")));

            builder.Services.AddAuthorization();

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                //options.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<ApplicationDBContext>()
                .AddDefaultTokenProviders();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.SameSite = SameSiteMode.None; // Allow cross-origin cookies
                options.Cookie.HttpOnly = true;
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
