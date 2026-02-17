
using Prometheus;

namespace IMS.Api.DI;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseObservability(
        this IApplicationBuilder app)
    {
        app.UseHttpMetrics();
        return app;
    }
}
