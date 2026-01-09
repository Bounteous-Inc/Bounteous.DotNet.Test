using System;
using Bounteous.xUnit.Accelerator.Factory;
using Xunit;

namespace Bounteous.xUnit.Accelerator.Tests
{
    /// <summary>
    /// xUnit collection definition for FactoryGirl tests.
    /// Ensures all tests in this collection run sequentially to avoid state conflicts.
    /// </summary>
    [CollectionDefinition("FactoryGirl")]
    public class FactoryGirlCollectionFixture : ICollectionFixture<FactoryGirlFixture>
    {
        // This class has no code, and is never created.
        // Its purpose is simply to be the place to apply [CollectionDefinition]
    }

    /// <summary>
    /// Shared fixture for FactoryGirl tests.
    /// Provides shared setup and cleanup across all tests in the collection.
    /// </summary>
    public class FactoryGirlFixture : IDisposable
    {
        public FactoryGirlFixture()
        {
            // Shared setup if needed in the future
        }

        public void Dispose()
        {
            // Ensure clean state after all tests in collection complete
            FactoryGirl.Clear();
        }
    }
}
