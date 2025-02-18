using System;

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
    }
}