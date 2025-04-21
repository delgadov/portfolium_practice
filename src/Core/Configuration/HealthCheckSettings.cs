namespace portfolium.Core.Configuration;

public class HealthCheckSettings {
    public int DiskSpaceThreshold { get; set; }
    public int MemoryThreshold { get; set; }
    public int EvaluationTimeInSeconds { get; set; }
    public int MaxHistoryEntries { get; set; }
    public int NotificationsInterval { get; set; }
    public int ApiMaxActiveRequests { get; set; }
}