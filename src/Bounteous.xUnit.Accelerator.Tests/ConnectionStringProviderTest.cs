using Bounteous.xUnit.Accelerator.Containers;
using Xunit;

namespace Bounteous.xUnit.Accelerator.Tests
{
    public class ConnectionStringProviderTest
    {
        [Fact]
        public void ConfigureAndRetrieveConnectionString()
        {
            var expectedConnectionString = "Server=localhost;Database=TestDb;";
            
            ConnectionStringProvider.Configure(expectedConnectionString);
            var provider = new ConnectionStringProvider();
            
            Assert.Equal(expectedConnectionString, provider.ConnectionString);
        }

        [Fact]
        public void ConnectionStringIsEmptyByDefault()
        {
            ConnectionStringProvider.Configure(string.Empty);
            var provider = new ConnectionStringProvider();
            
            Assert.Equal(string.Empty, provider.ConnectionString);
        }

        [Fact]
        public void ConfigureOverwritesPreviousValue()
        {
            ConnectionStringProvider.Configure("First Connection String");
            ConnectionStringProvider.Configure("Second Connection String");
            var provider = new ConnectionStringProvider();
            
            Assert.Equal("Second Connection String", provider.ConnectionString);
        }

        [Fact]
        public void MultipleInstancesShareSameConnectionString()
        {
            var connectionString = "Shared=Connection;String=Value;";
            ConnectionStringProvider.Configure(connectionString);
            
            var provider1 = new ConnectionStringProvider();
            var provider2 = new ConnectionStringProvider();
            
            Assert.Equal(connectionString, provider1.ConnectionString);
            Assert.Equal(connectionString, provider2.ConnectionString);
            Assert.Equal(provider1.ConnectionString, provider2.ConnectionString);
        }
    }
}
