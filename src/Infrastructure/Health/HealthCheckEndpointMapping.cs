using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using portfolium.Core.Constants;

namespace portfolium.Infrastructure.Health;

public static class HealthCheckEndpointMapping {
    public static IEndpointRouteBuilder MapHealthCheckEndpoint(this IEndpointRouteBuilder app) {
        app.MapHealthChecks(HealthCheckEndpoints.Health, new HealthCheckOptions {
            Predicate = _ => true,
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
            AllowCachingResponses = false,
            ResultStatusCodes = {
                [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable,
                [HealthStatus.Degraded] = StatusCodes.Status503ServiceUnavailable,
                [HealthStatus.Healthy] = StatusCodes.Status200OK
            }
        });
        app.MapHealthChecks(HealthCheckEndpoints.HealthReady, new HealthCheckOptions {
            Predicate = check => check.Tags.Contains(HealthCheckTags.Ready),
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
            ResultStatusCodes = {
                [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable,
                [HealthStatus.Degraded] = StatusCodes.Status503ServiceUnavailable,
                [HealthStatus.Healthy] = StatusCodes.Status200OK
            }
        });
        app.MapHealthChecks(HealthCheckEndpoints.HealthLive, new HealthCheckOptions {
            Predicate = _ => false,
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        return app;
    }

    public static IApplicationBuilder MapHealthCheckUiEndpoints(this IApplicationBuilder app) {
        app.UseHealthChecksUI(options => {
            options.UIPath = HealthCheckEndpoints.OptionsUi;
            options.ApiPath = HealthCheckEndpoints.OptionsApi;
        });

        return app;
    }
}