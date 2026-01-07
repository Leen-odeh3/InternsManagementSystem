using IMS.Application.Abstractions;
using IMS.Application.Mapper;
using IMS.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace IMS.Application.DI;
public static class addApplicationDependancy
{
    public static IServiceCollection AddApplicationDependancy(this IServiceCollection service)
    {
        service.AddSingleton<UserMapper>();
        service.AddScoped<IAuthService, AuthService>();

        return service;

    }
}
