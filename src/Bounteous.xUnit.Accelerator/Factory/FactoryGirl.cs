using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Numerics;
using System.Reflection;
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
    
    public static ITestFactory Define<T, TId>(Expression<Func<T, TId>> idProperty, Func<T> factory) 
        where TId : INumber<TId>
        => Instance.Define(idProperty, factory);
    
    public static int UniqueId(string key = "anonymous")
    {
        return Instance.UniqueId(key);
    }

    public static string UniqueIdStr(string key = "anonymous")
    {
        return UniqueId(key).ToString();
    }
    
    public static TId NextId<TId>() where TId : INumber<TId>
    {
        return Instance.NextId<TId>();
    }
    
    public static TId NextId<TId>(Type entityType) where TId : INumber<TId>
    {
        return Instance.NextId<TId>(entityType);
    }
    
    public static Guid NextGuid()
    {
        return Guid.NewGuid();
    }
    
    public static void Clear() => LazyFactory.Value.Clear();
}

public interface ITestFactory
{
    ITestFactory Define<T>(Func<T> factory);
    ITestFactory Define<T, TId>(Expression<Func<T, TId>> idProperty, Func<T> factory) where TId : INumber<TId>;
}

public class TestFactory : ITestFactory
{
    private readonly ConcurrentDictionary<Type, Func<object>> factories = new();
    private readonly ConcurrentDictionary<Type, object> idPropertySetters = new();
    private Dictionary<string, int> uniqueIds = new();
    private readonly ConcurrentDictionary<string, long> sequenceCounters = new();

    public T Build<T>(Action<T> propertyUpdates)
    {
        if (factories.ContainsKey(typeof(T)) == false)
            throw new ArgumentException($"Unknown entity type requested: {typeof(T).Name}");

        var entity = (T)factories[typeof(T)]();
        
        // Auto-assign ID if factory was defined with ID property
        if (idPropertySetters.TryGetValue(typeof(T), out var setter))
        {
            var idSetter = (IIdPropertySetter)setter;
            idSetter.SetId(entity);
        }
        
        propertyUpdates(entity);
        return entity;
    }

    public ITestFactory Define<T>(Func<T> factory)
    {
        factories[typeof(T)] = () => factory();
        uniqueIds = new Dictionary<string, int>();
        return this;
    }
    
    public ITestFactory Define<T, TId>(Expression<Func<T, TId>> idProperty, Func<T> factory) 
        where TId : INumber<TId>
    {
        factories[typeof(T)] = () => factory();
        uniqueIds = new Dictionary<string, int>();
        
        // Extract property setter from expression
        // Handle both direct property access and conversions (e.g., int -> long)
        var expression = idProperty.Body;
        
        // Unwrap Convert expressions if present
        if (expression is UnaryExpression unaryExpression && 
            (unaryExpression.NodeType == ExpressionType.Convert || 
             unaryExpression.NodeType == ExpressionType.ConvertChecked))
        {
            expression = unaryExpression.Operand;
        }
        
        var memberExpression = expression as MemberExpression 
            ?? throw new ArgumentException("Expression must be a property access", nameof(idProperty));
        
        var propertyInfo = memberExpression.Member as PropertyInfo 
            ?? throw new ArgumentException("Expression must access a property", nameof(idProperty));
        
        // Store the ID property setter
        idPropertySetters[typeof(T)] = new IdPropertySetter<T, TId>(propertyInfo, this);
        
        return this;
    }
    
    public int UniqueId(string key= "anonymous")
    {
        uniqueIds.TryAdd(key, 0);
        return uniqueIds[key] += 1;
    }
    
    public TId NextId<TId>() where TId : INumber<TId>
    {
        var key = $"global_{typeof(TId).Name}";
        return GetNextSequenceValue<TId>(key);
    }
    
    public TId NextId<TId>(Type entityType) where TId : INumber<TId>
    {
        var key = $"{entityType.Name}_{typeof(TId).Name}";
        return GetNextSequenceValue<TId>(key);
    }
    
    private TId GetNextSequenceValue<TId>(string key) where TId : INumber<TId>
    {
        var nextValue = sequenceCounters.AddOrUpdate(
            key,
            1L,
            (_, current) => current + 1
        );
        
        return TId.CreateChecked(nextValue);
    }
    
    public void Clear()
    {
        factories.Clear();
        idPropertySetters.Clear();
        sequenceCounters.Clear();
        uniqueIds?.Clear();
    }
}

internal interface IIdPropertySetter
{
    void SetId(object entity);
}

internal class IdPropertySetter<T, TId> : IIdPropertySetter where TId : INumber<TId>
{
    private readonly PropertyInfo _propertyInfo;
    private readonly TestFactory _factory;
    
    public IdPropertySetter(PropertyInfo propertyInfo, TestFactory factory)
    {
        _propertyInfo = propertyInfo;
        _factory = factory;
    }
    
    public void SetId(object entity)
    {
        var nextId = _factory.NextId<TId>(typeof(T));
        
        // Convert the ID to the property's actual type if needed
        var propertyType = _propertyInfo.PropertyType;
        object valueToSet = nextId;
        
        if (propertyType != typeof(TId))
        {
            // Use Convert.ChangeType for numeric conversions
            valueToSet = Convert.ChangeType(nextId, propertyType);
        }
        
        _propertyInfo.SetValue(entity, valueToSet);
    }
}