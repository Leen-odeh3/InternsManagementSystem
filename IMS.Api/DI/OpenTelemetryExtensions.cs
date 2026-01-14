
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
namespace IMS.Api.DI;
public static class OpenTelemetryExtensions
{
    public static IServiceCollection AddObservability(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddOpenTelemetry()
    .WithTracing(builder =>
    {
        builder
            .SetResourceBuilder(
                ResourceBuilder.CreateDefault()
                    .AddService("IMS.API")).AddSource("IMS.API")
            .SetSampler(new AlwaysOnSampler())
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddOtlpExporter(o =>
            {
                o.Endpoint = new Uri(configuration["Otlp:Endpoint"]);
            });
    });

        return services;
    }
}
