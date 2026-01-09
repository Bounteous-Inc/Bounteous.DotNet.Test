using System;
using Bounteous.xUnit.Accelerator.Factory;
using Xunit;

namespace Bounteous.xUnit.Accelerator.Tests
{
    [Collection("FactoryGirl")]
    public class GenericNextIdTests : FactoryGirlTestBase
    {

        [Theory]
        [InlineData(1, 2, 3)]  // int
        public void NextId_Int_ReturnsSequentialIds(int expected1, int expected2, int expected3)
        {
            FactoryTestHelpers.AssertSequentialIds(() => new Customer().NextId<int>(), expected1, expected2, expected3);
        }

        [Theory]
        [InlineData(1L, 2L, 3L)]  // long
        public void NextId_Long_ReturnsSequentialIds(long expected1, long expected2, long expected3)
        {
            FactoryTestHelpers.AssertSequentialIds(() => new Customer().NextId<long>(), expected1, expected2, expected3);
        }

        [Theory]
        [InlineData((short)1, (short)2, (short)3)]  // short
        public void NextId_Short_ReturnsSequentialIds(short expected1, short expected2, short expected3)
        {
            FactoryTestHelpers.AssertSequentialIds(() => new Customer().NextId<short>(), expected1, expected2, expected3);
        }

        [Theory]
        [InlineData((byte)1, (byte)2, (byte)3)]  // byte
        public void NextId_Byte_ReturnsSequentialIds(byte expected1, byte expected2, byte expected3)
        {
            FactoryTestHelpers.AssertSequentialIds(() => new Customer().NextId<byte>(), expected1, expected2, expected3);
        }

        [Theory]
        [InlineData(1u, 2u, 3u)]  // uint
        public void NextId_UInt_ReturnsSequentialIds(uint expected1, uint expected2, uint expected3)
        {
            FactoryTestHelpers.AssertSequentialIds(() => new Customer().NextId<uint>(), expected1, expected2, expected3);
        }

        [Theory]
        [InlineData(1ul, 2ul, 3ul)]  // ulong
        public void NextId_ULong_ReturnsSequentialIds(ulong expected1, ulong expected2, ulong expected3)
        {
            FactoryTestHelpers.AssertSequentialIds(() => new Customer().NextId<ulong>(), expected1, expected2, expected3);
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
        public void NextId_PerType_Int_CustomerAndRequest_HaveSeparateSequences()
        {
            FactoryTestHelpers.AssertPerTypeSequenceIsolation<Customer, Request, int>(new Customer(), new Request());
        }

        [Fact]
        public void NextId_PerType_Long_CustomerAndRequest_HaveSeparateSequences()
        {
            FactoryTestHelpers.AssertPerTypeSequenceIsolation<Customer, Request, long>(new Customer(), new Request());
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
            FactoryTestHelpers.AssertUniqueGuids(() => customer.NextGuid());
        }

        [Theory]
        [InlineData(1, 2, 3)]  // int
        public void FactoryGirl_NextId_Int_ReturnsSequentialIds(int expected1, int expected2, int expected3)
        {
            FactoryTestHelpers.AssertSequentialIds(() => FactoryGirl.NextId<int>(), expected1, expected2, expected3);
        }

        [Theory]
        [InlineData(1L, 2L, 3L)]  // long
        public void FactoryGirl_NextId_Long_ReturnsSequentialIds(long expected1, long expected2, long expected3)
        {
            FactoryTestHelpers.AssertSequentialIds(() => FactoryGirl.NextId<long>(), expected1, expected2, expected3);
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
            FactoryTestHelpers.AssertUniqueGuids(() => FactoryGirl.NextGuid());
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
