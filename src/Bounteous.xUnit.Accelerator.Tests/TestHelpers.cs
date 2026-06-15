using System;
using System.Numerics;
using Bounteous.xUnit.Accelerator.Factory;
using Xunit;

namespace Bounteous.xUnit.Accelerator.Tests
{
    /// <summary>
    /// Helper methods for common test assertion patterns in FactoryGirl tests.
    /// Reduces boilerplate and improves test readability.
    /// </summary>
    public static class FactoryTestHelpers
    {
        /// <summary>
        /// Asserts that an ID generator produces a specific sequence of values.
        /// </summary>
        /// <typeparam name="T">The numeric type of the ID</typeparam>
        /// <param name="idGenerator">Function that generates the next ID</param>
        /// <param name="expectedSequence">Expected sequence of IDs</param>
        public static void AssertSequentialIds<T>(Func<T> idGenerator, params T[] expectedSequence) 
            where T : INumber<T>
        {
            for (int i = 0; i < expectedSequence.Length; i++)
            {
                var actual = idGenerator();
                Assert.Equal(expectedSequence[i], actual);
            }
        }

        /// <summary>
        /// Asserts that a factory builds a valid entity and optionally performs additional assertions.
        /// </summary>
        /// <typeparam name="T">The entity type</typeparam>
        /// <param name="additionalAssertions">Optional additional assertions to perform on the entity</param>
        public static void AssertFactoryBuildsValidEntity<T>(Action<T> additionalAssertions = null) 
            where T : class
        {
            var entity = FactoryGirl.Build<T>();
            Assert.NotNull(entity);
            additionalAssertions?.Invoke(entity);
        }

        /// <summary>
        /// Asserts that two different entity types maintain separate ID sequences.
        /// </summary>
        /// <typeparam name="TEntity1">First entity type</typeparam>
        /// <typeparam name="TEntity2">Second entity type</typeparam>
        /// <typeparam name="TId">ID type</typeparam>
        /// <param name="entity1">Instance of first entity type</param>
        /// <param name="entity2">Instance of second entity type</param>
        public static void AssertPerTypeSequenceIsolation<TEntity1, TEntity2, TId>(
            TEntity1 entity1, 
            TEntity2 entity2) 
            where TId : INumber<TId>
        {
            var id1_1 = entity1.NextId<TEntity1, TId>();
            var id2_1 = entity2.NextId<TEntity2, TId>();
            var id1_2 = entity1.NextId<TEntity1, TId>();
            var id2_2 = entity2.NextId<TEntity2, TId>();
            
            Assert.Equal(TId.CreateChecked(1), id1_1);
            Assert.Equal(TId.CreateChecked(1), id2_1);
            Assert.Equal(TId.CreateChecked(2), id1_2);
            Assert.Equal(TId.CreateChecked(2), id2_2);
        }

        /// <summary>
        /// Asserts that GUIDs generated are unique.
        /// </summary>
        /// <param name="guidGenerator">Function that generates GUIDs</param>
        /// <param name="count">Number of GUIDs to generate and verify</param>
        public static void AssertUniqueGuids(Func<Guid> guidGenerator, int count = 3)
        {
            var guids = new System.Collections.Generic.HashSet<Guid>();
            
            for (int i = 0; i < count; i++)
            {
                var guid = guidGenerator();
                Assert.NotEqual(Guid.Empty, guid);
                Assert.True(guids.Add(guid), $"Duplicate GUID generated: {guid}");
            }
        }

        /// <summary>
        /// Asserts that building multiple entities produces the expected count with sequential IDs.
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <typeparam name="TId">ID type</typeparam>
        /// <param name="count">Number of entities to build</param>
        /// <param name="idSelector">Function to extract ID from entity</param>
        public static void AssertBuildManyWithSequentialIds<T, TId>(
            int count, 
            Func<T, TId> idSelector) 
            where TId : INumber<TId>
        {
            var entities = FactoryGirl.Build<T>(count, _ => { });
            
            Assert.Equal(count, entities.Count);
            
            for (int i = 0; i < count; i++)
            {
                var expectedId = TId.CreateChecked(i + 1);
                var actualId = idSelector(entities[i]);
                Assert.Equal(expectedId, actualId);
            }
        }
    }
}
