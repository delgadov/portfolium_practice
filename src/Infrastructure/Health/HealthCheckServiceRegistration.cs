using Microsoft.Extensions.Diagnostics.HealthChecks;
using portfolium.Core.Configuration;
using portfolium.Core.Constants;

namespace portfolium.Infrastructure.Health;

public static class HealthCheckServiceRegistration {
    public static void AddHealthChecksService(this IServiceCollection services, IConfiguration configuration) {
        var healthChecks = configuration.GetSection("HealthChecks").Get<HealthCheckSettings>()
            ?? throw new InvalidOperationException("HealthChecks section is missing or malformed in configuration");

        services.AddHealthChecks()
                .AddDatabaseCheck(configuration)
                .AddInfraCheck(healthChecks)
                .AddProcessCheck(healthChecks);
    }

    private static IHealthChecksBuilder AddDatabaseCheck(this IHealthChecksBuilder builder,
                                                         IConfiguration configuration) {
        return builder.AddSqlServer(
            configuration.GetConnectionString("DefaultConnection"),
            name: "SQL Server",
            tags: [HealthCheckTags.Database, HealthCheckTags.Sql, HealthCheckTags.SqlServer, HealthCheckTags.Ready]);
    }


    private static IHealthChecksBuilder
        AddInfraCheck(this IHealthChecksBuilder builder, HealthCheckSettings settings) {
        builder.AddDiskStorageHealthCheck(options => {
                var drive = Environment.OSVersion.Platform == PlatformID.Win32NT
                    ? @"C:\"
                    : @"/";
                options.AddDrive(drive, settings.DiskSpaceThreshold);
            },
            "Disk Storage", HealthStatus.Degraded,
            [HealthCheckTags.Memory, HealthCheckTags.Performance, HealthCheckTags.Infrastructure]);

        builder.AddProcessAllocatedMemoryHealthCheck(
                   settings.MemoryThreshold,
                   "Process Allocated Memory",
                   tags: [HealthCheckTags.Memory, HealthCheckTags.Performance, HealthCheckTags.Infrastructure]);
        return builder;
    }

    private static IHealthChecksBuilder AddProcessCheck(this IHealthChecksBuilder builder, HealthCheckSettings settings) {
        builder.AddProcessHealthCheck("dotnet",
            p => p.Length > 0,
            "ASP.NET Process",
            tags: [HealthCheckTags.Process, HealthCheckTags.Ready]);

        return builder;
    }
}