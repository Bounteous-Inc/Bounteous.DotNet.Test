using Bounteous.xUnit.Accelerator.Factory;

namespace Bounteous.xUnit.Accelerator.Tests
{
    /// <summary>
    /// Reusable factory definitions for common test entities.
    /// Reduces duplication and ensures consistency across tests.
    /// </summary>
    public static class TestFactoryDefinitions
    {
        /// <summary>
        /// Defines a Customer factory with automatic ID assignment.
        /// </summary>
        public static void DefineCustomerWithAutoId()
        {
            FactoryGirl.Define<Customer, int>(
                idProperty: c => c.Id,
                factory: () => new Customer
                {
                    Name = "John Doe",
                    Email = "john@example.com"
                });
        }

        /// <summary>
        /// Defines a Request factory with automatic ID assignment.
        /// </summary>
        public static void DefineRequestWithAutoId()
        {
            FactoryGirl.Define<Request, long>(
                idProperty: r => r.Id,
                factory: () => new Request());
        }

        /// <summary>
        /// Defines all common factories with automatic ID assignment.
        /// </summary>
        public static void DefineAllCommonFactories()
        {
            DefineCustomerWithAutoId();
            DefineRequestWithAutoId();
        }

        /// <summary>
        /// Defines a basic Customer factory without automatic ID assignment.
        /// </summary>
        public static void DefineBasicCustomer()
        {
            FactoryGirl.Define(() => new Customer());
        }

        /// <summary>
        /// Defines a basic Request factory without automatic ID assignment.
        /// </summary>
        public static void DefineBasicRequest()
        {
            FactoryGirl.Define(() => new Request());
        }
    }
}
