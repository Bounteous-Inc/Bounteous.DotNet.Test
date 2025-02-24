using Bounteous.Data;

namespace Bounteous.xUnit.Accelerator.Containers;

public class ConnectionStringProvider : IConnectionStringProvider
{
    private static string connectionString = string.Empty;

    public static void Configure(string value) => connectionString = value;
    public string ConnectionString => connectionString;
}