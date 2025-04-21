namespace portfolium.Core.Constants;

public static class HealthCheckEndpoints {
    public const string Health = "/health";
    public const string HealthReady = "/health/ready";
    public const string HealthLive = "/health/live";
    public const string OptionsUi = "/healthchecks-ui";
    public const string OptionsApi = "/healthchecks-api";
}