using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace Bounteous.xUnit.Accelerator.Factory;

public static class FactoryGirl
{
    private static readonly Lazy<TestFactory> LazyFactory =
        new(() => new TestFactory(), LazyThreadSafetyMode.ExecutionAndPublication);

    private static TestFactory Instance => LazyFactory.Value; 

    public static T Build<T>() => Build<T>(_ => {});
    
    public static List<T> Build<T>(int howMany, Action<T> propertyUpdates)
    {
        var itemsCreated = new List<T>();
        for (var i = 0; i < howMany; i++)
            itemsCreated.Add(Build(propertyUpdates));
        return itemsCreated;
    }

    public static T Build<T>(Action<T> propertyUpdates) => Instance.Build(propertyUpdates);

    public static ITestFactory Define<T>(Func<T> factory) => Instance.Define(factory);
    
    public static int UniqueId(string key = "anonymous")
    {
        return Instance.UniqueId(key);
    }

    public static string UniqueIdStr(string key = "anonymous")
    {
        return UniqueId(key).ToString();
    }
    
    public static void Clear() => LazyFactory.Value.Clear();
}

public interface ITestFactory
{
    ITestFactory Define<T>(Func<T> factory);
}

public class TestFactory : ITestFactory
{
    private readonly ConcurrentDictionary<Type, Func<object>> factories = new();
    private Dictionary<string, int> uniqueIds;

    public T Build<T>(Action<T> propertyUpdates)
    {
        if (factories.ContainsKey(typeof(T)) == false)
            throw new ArgumentException($"Unknown entity type requested: {typeof(T).Name}");

        var entity = (T)factories[typeof(T)]();
        propertyUpdates(entity);
        return entity;
    }

    public ITestFactory Define<T>(Func<T> factory)
    {
        factories[typeof(T)] = () => factory();
        uniqueIds = new Dictionary<string, int>();
        return this;
    }
    
    public int UniqueId(string key= "anonymous")
    {
        uniqueIds.TryAdd(key, 0);
        return uniqueIds[key] += 1;
    }
    
    public void Clear() => factories.Clear();
}