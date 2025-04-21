using portfolium.Core.Configuration;
using portfolium.Core.Constants;

namespace portfolium.Infrastructure.Health;

public static class HealthCheckUiServiceRegistration {
    public static void AddHealthCheckUiService(this IServiceCollection service, IConfiguration configuration) {
        var healthCheckSettings = configuration.GetSection("HealthChecks").Get<HealthCheckSettings>()
                                  ?? throw new InvalidOperationException(
                                      "HealthChecks section is missing or malformed in configuration");

        service.AddUiOptions(healthCheckSettings);
    }

    private static void AddUiOptions(this IServiceCollection service, HealthCheckSettings settings) {
        service.AddHealthChecksUI(options => {
            options.SetEvaluationTimeInSeconds(settings.EvaluationTimeInSeconds);
            options.MaximumHistoryEntriesPerEndpoint(settings.MaxHistoryEntries);
            options.SetApiMaxActiveRequests(settings.ApiMaxActiveRequests);
            options.AddHealthCheckEndpoint("API Health", HealthCheckEndpoints.Health);
            options.SetNotifyUnHealthyOneTimeUntilChange();
            options.SetMinimumSecondsBetweenFailureNotifications(settings.NotificationsInterval);
        }).AddInMemoryStorage();
    }
}