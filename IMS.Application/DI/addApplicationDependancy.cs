using FluentValidation;
using IMS.Application.Abstractions;
using IMS.Application.Mapper;
using IMS.Application.Services;
using IMS.Application.Validations;
using IMS.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace IMS.Application.DI;
public static class addApplicationDependancy
{
    public static IServiceCollection AddApplicationDependancy(this IServiceCollection services)
    {
        services.AddSingleton<UserMapper>();
        services.AddScoped<IAuthService, AuthService>();

        services.AddValidatorsFromAssembly(typeof(AppUserValidator).Assembly);
   
        return services;
    }
}
