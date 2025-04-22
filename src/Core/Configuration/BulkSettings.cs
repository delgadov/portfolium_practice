namespace portfolium.Core.Configuration;

public class BulkSettings : IBulkSettings{
    public int MaxItemsPerRequest { get; set; }
}