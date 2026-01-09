using System;
using Bounteous.xUnit.Accelerator.Factory;
using Xunit;

namespace Bounteous.xUnit.Accelerator.Tests
{
    [Collection("FactoryGirl")]
    public class GenericNextIdTests : FactoryGirlTestBase
    {

        [Fact]
        public void NextId_Int_ReturnsSequentialIds()
        {
            var customer = new Customer();
            
            var id1 = customer.NextId<int>();
            var id2 = customer.NextId<int>();
            var id3 = customer.NextId<int>();
            
            Assert.Equal(1, id1);
            Assert.Equal(2, id2);
            Assert.Equal(3, id3);
        }

        [Fact]
        public void NextId_Long_ReturnsSequentialIds()
        {
            var customer = new Customer();
            
            var id1 = customer.NextId<long>();
            var id2 = customer.NextId<long>();
            var id3 = customer.NextId<long>();
            
            Assert.Equal(1L, id1);
            Assert.Equal(2L, id2);
            Assert.Equal(3L, id3);
        }

        [Fact]
        public void NextId_Short_ReturnsSequentialIds()
        {
            var customer = new Customer();
            
            var id1 = customer.NextId<short>();
            var id2 = customer.NextId<short>();
            var id3 = customer.NextId<short>();
            
            Assert.Equal((short)1, id1);
            Assert.Equal((short)2, id2);
            Assert.Equal((short)3, id3);
        }

        [Fact]
        public void NextId_Byte_ReturnsSequentialIds()
        {
            var customer = new Customer();
            
            var id1 = customer.NextId<byte>();
            var id2 = customer.NextId<byte>();
            var id3 = customer.NextId<byte>();
            
            Assert.Equal((byte)1, id1);
            Assert.Equal((byte)2, id2);
            Assert.Equal((byte)3, id3);
        }

        [Fact]
        public void NextId_UInt_ReturnsSequentialIds()
        {
            var customer = new Customer();
            
            var id1 = customer.NextId<uint>();
            var id2 = customer.NextId<uint>();
            var id3 = customer.NextId<uint>();
            
            Assert.Equal(1u, id1);
            Assert.Equal(2u, id2);
            Assert.Equal(3u, id3);
        }

        [Fact]
        public void NextId_ULong_ReturnsSequentialIds()
        {
            var customer = new Customer();
            
            var id1 = customer.NextId<ulong>();
            var id2 = customer.NextId<ulong>();
            var id3 = customer.NextId<ulong>();
            
            Assert.Equal(1ul, id1);
            Assert.Equal(2ul, id2);
            Assert.Equal(3ul, id3);
        }

        [Fact]
        public void NextId_DifferentTypes_ShareGlobalSequence()
        {
            var customer = new Customer();
            
            var intId1 = customer.NextId<int>();
            var longId1 = customer.NextId<long>();
            var intId2 = customer.NextId<int>();
            
            Assert.Equal(1, intId1);
            Assert.Equal(1L, longId1);
            Assert.Equal(2, intId2);
        }

        [Fact]
        public void NextId_PerType_CustomerAndRequest_HaveSeparateSequences()
        {
            var customer = new Customer();
            var request = new Request();
            
            var customerId1 = customer.NextId<Customer, int>();
            var customerId2 = customer.NextId<Customer, int>();
            var requestId1 = request.NextId<Request, int>();
            var requestId2 = request.NextId<Request, int>();
            
            Assert.Equal(1, customerId1);
            Assert.Equal(2, customerId2);
            Assert.Equal(1, requestId1);
            Assert.Equal(2, requestId2);
        }

        [Fact]
        public void NextId_PerType_Long_CustomerAndRequest_HaveSeparateSequences()
        {
            var customer = new Customer();
            var request = new Request();
            
            var customerId1 = customer.NextId<Customer, long>();
            var customerId2 = customer.NextId<Customer, long>();
            var requestId1 = request.NextId<Request, long>();
            var requestId2 = request.NextId<Request, long>();
            
            Assert.Equal(1L, customerId1);
            Assert.Equal(2L, customerId2);
            Assert.Equal(1L, requestId1);
            Assert.Equal(2L, requestId2);
        }

        [Fact]
        public void NextId_PerType_SameEntityDifferentIdTypes_HaveSeparateSequences()
        {
            var customer = new Customer();
            
            var intId1 = customer.NextId<Customer, int>();
            var longId1 = customer.NextId<Customer, long>();
            var intId2 = customer.NextId<Customer, int>();
            var longId2 = customer.NextId<Customer, long>();
            
            Assert.Equal(1, intId1);
            Assert.Equal(1L, longId1);
            Assert.Equal(2, intId2);
            Assert.Equal(2L, longId2);
        }

        [Fact]
        public void NextGuid_ReturnsUniqueGuids()
        {
            var customer = new Customer();
            
            var guid1 = customer.NextGuid();
            var guid2 = customer.NextGuid();
            var guid3 = customer.NextGuid();
            
            Assert.NotEqual(Guid.Empty, guid1);
            Assert.NotEqual(Guid.Empty, guid2);
            Assert.NotEqual(Guid.Empty, guid3);
            Assert.NotEqual(guid1, guid2);
            Assert.NotEqual(guid2, guid3);
            Assert.NotEqual(guid1, guid3);
        }

        [Fact]
        public void FactoryGirl_NextId_Int_ReturnsSequentialIds()
        {
            var id1 = FactoryGirl.NextId<int>();
            var id2 = FactoryGirl.NextId<int>();
            var id3 = FactoryGirl.NextId<int>();
            
            Assert.Equal(1, id1);
            Assert.Equal(2, id2);
            Assert.Equal(3, id3);
        }

        [Fact]
        public void FactoryGirl_NextId_Long_ReturnsSequentialIds()
        {
            var id1 = FactoryGirl.NextId<long>();
            var id2 = FactoryGirl.NextId<long>();
            var id3 = FactoryGirl.NextId<long>();
            
            Assert.Equal(1L, id1);
            Assert.Equal(2L, id2);
            Assert.Equal(3L, id3);
        }

        [Fact]
        public void FactoryGirl_NextId_WithEntityType_ReturnsSequentialIds()
        {
            var id1 = FactoryGirl.NextId<int>(typeof(Customer));
            var id2 = FactoryGirl.NextId<int>(typeof(Customer));
            var id3 = FactoryGirl.NextId<int>(typeof(Request));
            var id4 = FactoryGirl.NextId<int>(typeof(Request));
            
            Assert.Equal(1, id1);
            Assert.Equal(2, id2);
            Assert.Equal(1, id3);
            Assert.Equal(2, id4);
        }

        [Fact]
        public void FactoryGirl_NextGuid_ReturnsUniqueGuids()
        {
            var guid1 = FactoryGirl.NextGuid();
            var guid2 = FactoryGirl.NextGuid();
            var guid3 = FactoryGirl.NextGuid();
            
            Assert.NotEqual(Guid.Empty, guid1);
            Assert.NotEqual(Guid.Empty, guid2);
            Assert.NotEqual(Guid.Empty, guid3);
            Assert.NotEqual(guid1, guid2);
            Assert.NotEqual(guid2, guid3);
            Assert.NotEqual(guid1, guid3);
        }

        [Fact]
        public void Clear_ResetsAllSequences()
        {
            var customer = new Customer();
            
            var id1 = customer.NextId<int>();
            var id2 = customer.NextId<Customer, long>();
            
            Assert.Equal(1, id1);
            Assert.Equal(1L, id2);
            
            FactoryGirl.Clear();
            FactoryGirl.Define(() => new Customer());
            
            var id3 = customer.NextId<int>();
            var id4 = customer.NextId<Customer, long>();
            
            Assert.Equal(1, id3);
            Assert.Equal(1L, id4);
        }
    }
}
