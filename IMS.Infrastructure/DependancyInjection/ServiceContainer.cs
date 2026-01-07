
using IMS.Application.Abstractions;
using IMS.Core.Entities;
using IMS.Infrastructure.Database;
using IMS.Infrastructure.DbInitilizer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IMS.Infrastructure.ServiceContainer;
public static class ServiceContainer
{
    public static IServiceCollection addInfraDependancy(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });

        services.AddScoped<IAppDbContext>(sp =>
    sp.GetRequiredService<AppDbContext>());

        services.AddIdentity<AppUser, AppRole>().AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders();

        services.AddScoped<IDBInitilizer, DBInitilizer>();


        return services;
    }
}
