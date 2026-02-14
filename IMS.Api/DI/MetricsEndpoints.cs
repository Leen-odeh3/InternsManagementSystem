using Microsoft.AspNetCore.Builder;
using Prometheus;

namespace IMS.Api.DI;

public static class MetricsEndpoints
{
    public static void MapMetrics(this WebApplication app)
    {
        app.MapMetrics("/metrics");
    }
}