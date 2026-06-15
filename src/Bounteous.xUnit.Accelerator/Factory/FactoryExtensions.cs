using System;
using System.Numerics;

namespace Bounteous.xUnit.Accelerator.Factory
{
    public static class FactoryExtensions
    {
        public static int NextId(this object item)
            => FactoryGirl.UniqueId();

        public static Guid NewId<T>(this T item)
            => Guid.NewGuid();

        public static string UniqueName(this object item, string prefix = "Name")
            => $"{prefix} {FactoryGirl.UniqueId()}";
        
        public static TId NextId<TId>(this object item) where TId : INumber<TId>
            => FactoryGirl.NextId<TId>();
        
        public static TId NextId<TEntity, TId>(this TEntity entity) where TId : INumber<TId>
            => FactoryGirl.NextId<TId>(typeof(TEntity));
        
        public static Guid NextGuid<T>(this T item)
            => FactoryGirl.NextGuid();
    }
}