using System;
using System.Collections.Generic;
using Bounteous.xUnit.Accelerator.Factory;
using Xunit;

namespace Bounteous.xUnit.Accelerator.Tests
{
    [Collection("FactoryGirl")]
    public class FactoryGirlTest : FactoryGirlTestBase
    {
        protected override void SetupCommonFactories()
        {
            // Don't set up any factories by default for these tests
            // Each test will define its own as needed
        }

        [Fact]
        public void BuildSimpleEntity()
        {
            FactoryGirl.Define(() => new Customer());
            var customer = FactoryGirl.Build<Customer>();
            
            Assert.NotNull(customer);
            Assert.NotEqual(Guid.Empty, customer.Guid);
        }

        [Fact]
        public void BuildWithPropertyUpdates()
        {
            FactoryGirl.Define(() => new Customer());
            var expectedName = "John Doe";
            
            var customer = FactoryGirl.Build<Customer>(c => c.Name = expectedName);
            
            Assert.NotNull(customer);
            Assert.Equal(expectedName, customer.Name);
        }

        [Fact]
        public void BuildMultipleEntities()
        {
            FactoryGirl.Define(() => new Customer());
            var count = 5;
            
            var customers = FactoryGirl.Build<Customer>(count, c => { });
            
            Assert.NotNull(customers);
            Assert.Equal(count, customers.Count);
        }

        [Fact]
        public void BuildMultipleEntitiesWithPropertyUpdates()
        {
            FactoryGirl.Define(() => new Customer());
            var count = 3;
            var expectedName = "Test Customer";
            
            var customers = FactoryGirl.Build<Customer>(count, c => c.Name = expectedName);
            
            Assert.NotNull(customers);
            Assert.Equal(count, customers.Count);
            Assert.All(customers, c => Assert.Equal(expectedName, c.Name));
        }

        [Fact]
        public void UniqueIdGeneratesSequentialIds()
        {
            var id1 = FactoryGirl.UniqueId();
            var id2 = FactoryGirl.UniqueId();
            var id3 = FactoryGirl.UniqueId();
            
            Assert.Equal(1, id1);
            Assert.Equal(2, id2);
            Assert.Equal(3, id3);
        }

        [Fact]
        public void UniqueIdWithKeyGeneratesSequentialIds()
        {
            var id1 = FactoryGirl.UniqueId("customer");
            var id2 = FactoryGirl.UniqueId("customer");
            var id3 = FactoryGirl.UniqueId("order");
            var id4 = FactoryGirl.UniqueId("customer");
            
            Assert.Equal(1, id1);
            Assert.Equal(2, id2);
            Assert.Equal(1, id3);
            Assert.Equal(3, id4);
        }

        [Fact]
        public void UniqueIdStrReturnsStringValue()
        {
            var idStr = FactoryGirl.UniqueIdStr();
            
            Assert.NotNull(idStr);
            Assert.Equal("1", idStr);
        }

        [Fact]
        public void UniqueIdStrWithKeyReturnsStringValue()
        {
            var idStr1 = FactoryGirl.UniqueIdStr("test");
            var idStr2 = FactoryGirl.UniqueIdStr("test");
            
            Assert.Equal("1", idStr1);
            Assert.Equal("2", idStr2);
        }

        [Fact]
        public void BuildThrowsExceptionForUndefinedType()
        {
            FactoryGirl.Clear();
            
            var exception = Assert.Throws<ArgumentException>(() => FactoryGirl.Build<Customer>());
            
            Assert.Contains("Unknown entity type requested: Customer", exception.Message);
        }

        [Fact]
        public void DefineReturnsTestFactory()
        {
            var factory = FactoryGirl.Define(() => new Customer());
            
            Assert.NotNull(factory);
            Assert.IsAssignableFrom<ITestFactory>(factory);
        }

        [Fact]
        public void ClearRemovesAllDefinitions()
        {
            FactoryGirl.Define(() => new Customer());
            var customer = FactoryGirl.Build<Customer>();
            Assert.NotNull(customer);
            
            FactoryGirl.Clear();
            
            Assert.Throws<ArgumentException>(() => FactoryGirl.Build<Customer>());
        }

        [Fact]
        public void BuildWithNoUpdatesUsesDefaultAction()
        {
            FactoryGirl.Define(() => new Customer { Name = "Default Name" });
            
            var customer = FactoryGirl.Build<Customer>();
            
            Assert.NotNull(customer);
            Assert.Equal("Default Name", customer.Name);
        }

        [Fact]
        public void MultipleDefinesOverwritePreviousDefinition()
        {
            FactoryGirl.Define(() => new Customer { Name = "First" });
            FactoryGirl.Define(() => new Customer { Name = "Second" });
            
            var customer = FactoryGirl.Build<Customer>();
            
            Assert.Equal("Second", customer.Name);
        }
    }
}
