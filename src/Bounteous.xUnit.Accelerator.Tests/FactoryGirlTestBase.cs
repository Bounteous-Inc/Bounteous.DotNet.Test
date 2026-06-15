using System;
using Bounteous.xUnit.Accelerator.Factory;

namespace Bounteous.xUnit.Accelerator.Tests
{
    /// <summary>
    /// Base class for FactoryGirl-related tests that provides common setup and teardown.
    /// Automatically clears FactoryGirl state before each test and sets up common factory definitions.
    /// </summary>
    public abstract class FactoryGirlTestBase : IDisposable
    {
        protected FactoryGirlTestBase()
        {
            FactoryGirl.Clear();
            SetupCommonFactories();
        }

        /// <summary>
        /// Override this method to define custom factory definitions for specific test classes.
        /// The default implementation defines Customer and Request factories.
        /// </summary>
        protected virtual void SetupCommonFactories()
        {
            FactoryGirl.Define(() => new Customer());
            FactoryGirl.Define(() => new Request());
        }

        public virtual void Dispose()
        {
            FactoryGirl.Clear();
        }
    }
}
