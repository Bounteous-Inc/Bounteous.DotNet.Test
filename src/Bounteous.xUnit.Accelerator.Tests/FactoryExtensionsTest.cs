using System;
using Bounteous.xUnit.Accelerator.Factory;
using Xunit;

namespace Bounteous.xUnit.Accelerator.Tests
{
    public class FactoryExtensionsTest
    {
        public FactoryExtensionsTest()
        {
            FactoryGirl.Clear();
            FactoryGirl.Define(() => new Customer());
        }

        [Fact]
        public void NextIdReturnsSequentialIds()
        {
            var customer = new Customer();
            
            var id1 = customer.NextId();
            var id2 = customer.NextId();
            var id3 = customer.NextId();
            
            Assert.Equal(1, id1);
            Assert.Equal(2, id2);
            Assert.Equal(3, id3);
        }

        [Fact]
        public void NextIdWorksWithDifferentObjects()
        {
            var customer = new Customer();
            var request = new Request();
            
            var id1 = customer.NextId();
            var id2 = request.NextId();
            
            Assert.Equal(1, id1);
            Assert.Equal(2, id2);
        }

        [Fact]
        public void NewIdGeneratesUniqueGuids()
        {
            var customer = new Customer();
            
            var guid1 = customer.NewId();
            var guid2 = customer.NewId();
            var guid3 = customer.NewId();
            
            Assert.NotEqual(Guid.Empty, guid1);
            Assert.NotEqual(Guid.Empty, guid2);
            Assert.NotEqual(Guid.Empty, guid3);
            Assert.NotEqual(guid1, guid2);
            Assert.NotEqual(guid2, guid3);
            Assert.NotEqual(guid1, guid3);
        }

        [Fact]
        public void UniqueNameWithDefaultPrefix()
        {
            var customer = new Customer();
            
            var name1 = customer.UniqueName();
            var name2 = customer.UniqueName();
            
            Assert.Equal("Name 1", name1);
            Assert.Equal("Name 2", name2);
        }

        [Fact]
        public void UniqueNameWithCustomPrefix()
        {
            var customer = new Customer();
            
            var name1 = customer.UniqueName("Customer");
            var name2 = customer.UniqueName("Customer");
            var name3 = customer.UniqueName("Order");
            
            Assert.Equal("Customer 1", name1);
            Assert.Equal("Customer 2", name2);
            Assert.Equal("Order 3", name3);
        }

        [Fact]
        public void UniqueNameWorksWithDifferentObjects()
        {
            var customer = new Customer();
            var request = new Request();
            
            var name1 = customer.UniqueName("Entity");
            var name2 = request.UniqueName("Entity");
            
            Assert.Equal("Entity 1", name1);
            Assert.Equal("Entity 2", name2);
        }

        [Fact]
        public void NewIdWorksWithDifferentTypes()
        {
            var customer = new Customer();
            var request = new Request();
            
            var guid1 = customer.NewId();
            var guid2 = request.NewId();
            
            Assert.NotEqual(Guid.Empty, guid1);
            Assert.NotEqual(Guid.Empty, guid2);
            Assert.NotEqual(guid1, guid2);
        }
    }
}
