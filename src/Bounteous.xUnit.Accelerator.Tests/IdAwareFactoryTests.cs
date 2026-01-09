using System;
using Bounteous.xUnit.Accelerator.Factory;
using Xunit;

namespace Bounteous.xUnit.Accelerator.Tests
{
    [Collection("FactoryGirl")]
    public class IdAwareFactoryTests : FactoryGirlTestBase
    {
        protected override void SetupCommonFactories()
        {
            // Don't set up any factories by default for these tests
            // Each test will define its own as needed
        }

        [Fact]
        public void Define_WithIdProperty_AutoAssignsSequentialIds()
        {
            FactoryGirl.Define<Customer, int>(
                idProperty: c => c.Id,
                factory: () => new Customer 
                { 
                    Name = "John Doe",
                    Email = "john@example.com"
                });

            var customer1 = FactoryGirl.Build<Customer>();
            var customer2 = FactoryGirl.Build<Customer>();
            var customer3 = FactoryGirl.Build<Customer>();

            Assert.Equal(1, customer1.Id);
            Assert.Equal(2, customer2.Id);
            Assert.Equal(3, customer3.Id);
            Assert.Equal("John Doe", customer1.Name);
            Assert.Equal("John Doe", customer2.Name);
        }

        [Fact]
        public void Define_WithIdProperty_AllowsPropertyOverrides()
        {
            FactoryGirl.Define<Customer, int>(
                idProperty: c => c.Id,
                factory: () => new Customer 
                { 
                    Name = "John Doe",
                    Email = "john@example.com"
                });

            var customer = FactoryGirl.Build<Customer>(c => c.Name = "Jane Doe");

            Assert.Equal(1, customer.Id);
            Assert.Equal("Jane Doe", customer.Name);
            Assert.Equal("john@example.com", customer.Email);
        }

        [Fact]
        public void Define_WithIdProperty_SupportsLongIds()
        {
            FactoryGirl.Define<Request, long>(
                idProperty: r => r.Id,
                factory: () => new Request());

            var request1 = FactoryGirl.Build<Request>();
            var request2 = FactoryGirl.Build<Request>();

            Assert.Equal(1L, request1.Id);
            Assert.Equal(2L, request2.Id);
        }

        [Fact]
        public void Define_WithIdProperty_MaintainsPerTypeSequences()
        {
            FactoryGirl.Define<Customer, int>(
                idProperty: c => c.Id,
                factory: () => new Customer { Name = "Customer" });

            FactoryGirl.Define<Request, long>(
                idProperty: r => r.Id,
                factory: () => new Request());

            var customer1 = FactoryGirl.Build<Customer>();
            var request1 = FactoryGirl.Build<Request>();
            var customer2 = FactoryGirl.Build<Customer>();
            var request2 = FactoryGirl.Build<Request>();

            Assert.Equal(1, customer1.Id);
            Assert.Equal(1L, request1.Id);
            Assert.Equal(2, customer2.Id);
            Assert.Equal(2L, request2.Id);
        }

        [Fact]
        public void Define_WithIdProperty_WorksWithBuildMany()
        {
            FactoryGirl.Define<Customer, int>(
                idProperty: c => c.Id,
                factory: () => new Customer { Name = "John Doe" });

            var customers = FactoryGirl.Build<Customer>(5, c => { });

            Assert.Equal(5, customers.Count);
            Assert.Equal(1, customers[0].Id);
            Assert.Equal(2, customers[1].Id);
            Assert.Equal(3, customers[2].Id);
            Assert.Equal(4, customers[3].Id);
            Assert.Equal(5, customers[4].Id);
        }

        [Fact]
        public void Define_WithIdProperty_ClearsSequencesOnClear()
        {
            FactoryGirl.Define<Customer, int>(
                idProperty: c => c.Id,
                factory: () => new Customer { Name = "John Doe" });

            var customer1 = FactoryGirl.Build<Customer>();
            Assert.Equal(1, customer1.Id);

            FactoryGirl.Clear();

            FactoryGirl.Define<Customer, int>(
                idProperty: c => c.Id,
                factory: () => new Customer { Name = "John Doe" });

            var customer2 = FactoryGirl.Build<Customer>();
            Assert.Equal(1, customer2.Id);
        }

        [Fact]
        public void Define_WithIdProperty_CanOverrideIdInPropertyUpdates()
        {
            FactoryGirl.Define<Customer, int>(
                idProperty: c => c.Id,
                factory: () => new Customer { Name = "John Doe" });

            var customer = FactoryGirl.Build<Customer>(c => c.Id = 999);

            Assert.Equal(999, customer.Id);
            Assert.Equal("John Doe", customer.Name);
        }

        [Fact]
        public void Define_WithoutIdProperty_StillWorks()
        {
            FactoryGirl.Define(() => new Customer 
            { 
                Id = 42,
                Name = "John Doe" 
            });

            var customer = FactoryGirl.Build<Customer>();

            Assert.Equal(42, customer.Id);
            Assert.Equal("John Doe", customer.Name);
        }

        [Fact]
        public void Define_WithIdProperty_SupportsShortIds()
        {
            FactoryGirl.Define<CustomerWithShortId, short>(
                idProperty: c => c.Id,
                factory: () => new CustomerWithShortId { Name = "Test" });

            var customer1 = FactoryGirl.Build<CustomerWithShortId>();
            var customer2 = FactoryGirl.Build<CustomerWithShortId>();

            Assert.Equal((short)1, customer1.Id);
            Assert.Equal((short)2, customer2.Id);
        }

        [Fact]
        public void Define_WithIdProperty_SupportsUIntIds()
        {
            FactoryGirl.Define<CustomerWithUIntId, uint>(
                idProperty: c => c.Id,
                factory: () => new CustomerWithUIntId { Name = "Test" });

            var customer1 = FactoryGirl.Build<CustomerWithUIntId>();
            var customer2 = FactoryGirl.Build<CustomerWithUIntId>();

            Assert.Equal(1u, customer1.Id);
            Assert.Equal(2u, customer2.Id);
        }
    }

    public class CustomerWithShortId
    {
        public short Id { get; set; }
        public string Name { get; set; }
    }

    public class CustomerWithUIntId
    {
        public uint Id { get; set; }
        public string Name { get; set; }
    }
}
