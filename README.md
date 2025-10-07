# Bounteous.xUnit.Accelerator

A comprehensive testing utilities library for .NET 8+ applications that accelerates unit test development with factory patterns, mock management, and database container support. This library provides essential tools for writing maintainable, efficient, and comprehensive unit tests.

## 📦 Installation

Install the package via NuGet:

```bash
dotnet add package Bounteous.xUnit.Accelerator
```

Or via Package Manager Console:

```powershell
Install-Package Bounteous.xUnit.Accelerator
```

## 🚀 Quick Start

### 1. Basic Test Setup

```csharp
using Bounteous.xUnit.Accelerator;
using Xunit;

public class CustomerServiceTests : MockBase
{
    [Fact]
    public async Task CreateCustomer_ShouldReturnCustomer()
    {
        // Arrange
        var service = Create<ICustomerService>();
        var customer = FactoryGirl.Build<Customer>();
        
        service.Setup(s => s.CreateAsync(It.IsAny<Customer>()))
               .ReturnsAsync(customer);
        
        // Act
        var result = await service.Object.CreateAsync(customer);
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(customer.Name, result.Name);
    }
}
```

### 2. Factory Pattern Usage

```csharp
using Bounteous.xUnit.Accelerator.Factory;

public class ProductTests
{
    [Fact]
    public void ProductFactory_ShouldCreateValidProduct()
    {
        // Define factory for Product
        FactoryGirl.Define<Product>(() => new Product
        {
            Id = FactoryGirl.UniqueId(),
            Name = $"Product {FactoryGirl.UniqueIdStr()}",
            Price = 99.99m,
            CreatedOn = DateTime.UtcNow
        });
        
        // Build product with custom properties
        var product = FactoryGirl.Build<Product>(p => 
        {
            p.Name = "Custom Product";
            p.Price = 149.99m;
        });
        
        // Assert
        Assert.Equal("Custom Product", product.Name);
        Assert.Equal(149.99m, product.Price);
        Assert.NotEqual(Guid.Empty, product.Id);
    }
}
```

### 3. Database Container Testing

```csharp
using Bounteous.xUnit.Accelerator.Containers;

public class DatabaseTests : MockBase
{
    [Fact]
    public async Task CustomerRepository_ShouldSaveCustomer()
    {
        // Configure test connection string
        ConnectionStringProvider.Configure("Server=localhost;Database=TestDb;...");
        
        // Use container for database operations
        var container = Create<ISqlContainer>();
        container.Setup(c => c.GetConnectionString())
                .Returns(ConnectionStringProvider.ConnectionString);
        
        // Test your repository
        var repository = new CustomerRepository(container.Object);
        var customer = FactoryGirl.Build<Customer>();
        
        var result = await repository.SaveAsync(customer);
        
        Assert.NotNull(result);
    }
}
```

## 🏗️ Architecture Overview

Bounteous.xUnit.Accelerator follows modern testing patterns and provides:

- **Factory Pattern**: Test data generation with customizable properties
- **Mock Management**: Simplified mock creation and verification
- **Database Containers**: Test database connection management
- **Test Utilities**: Helper methods for common testing scenarios
- **Integration Support**: Seamless integration with xUnit and Moq

## 🔧 Key Features

### Factory Pattern (FactoryGirl)

The `FactoryGirl` class provides a powerful test data generation system:

```csharp
public static class FactoryGirl
{
    // Build with default values
    public static T Build<T>() => Build<T>(_ => {});
    
    // Build with custom property updates
    public static T Build<T>(Action<T> propertyUpdates);
    
    // Build multiple instances
    public static List<T> Build<T>(int howMany, Action<T> propertyUpdates);
    
    // Define factory for a type
    public static ITestFactory Define<T>(Func<T> factory);
    
    // Generate unique IDs
    public static int UniqueId(string key = "anonymous");
    public static string UniqueIdStr(string key = "anonymous");
    
    // Clear all factories
    public static void Clear();
}
```

**Usage Examples:**

```csharp
// Define a factory
FactoryGirl.Define<Customer>(() => new Customer
{
    Id = Guid.NewGuid(),
    Name = $"Customer {FactoryGirl.UniqueId()}",
    Email = $"customer{FactoryGirl.UniqueId()}@example.com",
    CreatedOn = DateTime.UtcNow
});

// Build with defaults
var customer = FactoryGirl.Build<Customer>();

// Build with custom properties
var vipCustomer = FactoryGirl.Build<Customer>(c => 
{
    c.Name = "VIP Customer";
    c.Email = "vip@example.com";
    c.IsVip = true;
});

// Build multiple instances
var customers = FactoryGirl.Build<Customer>(5, c => c.IsActive = true);
```

### Mock Management (MockBase)

The `MockBase` class simplifies mock creation and verification:

```csharp
public abstract class MockBase : IDisposable
{
    // Create strict mock (default)
    protected Mock<T> Create<T>(MockBehavior behavior = MockBehavior.Strict) where T : class;
    
    // Create strict mock explicitly
    protected Mock<T> Strict<T>() where T : class;
    
    // Create loose mock
    protected Mock<T> Loose<T>() where T : class;
    
    // Create partial mocks
    protected Mock<T> StrictPartial<T>(params object[] args) where T : class;
    protected Mock<T> LoosePartial<T>(params object[] args) where T : class;
    
    // Automatically verify all mocks on disposal
    public virtual void Dispose() => mocks.VerifyAll();
}
```

**Usage Examples:**

```csharp
public class OrderServiceTests : MockBase
{
    [Fact]
    public async Task ProcessOrder_ShouldCallRepository()
    {
        // Arrange
        var repository = Create<IOrderRepository>();
        var emailService = Loose<IEmailService>();
        var order = FactoryGirl.Build<Order>();
        
        repository.Setup(r => r.SaveAsync(It.IsAny<Order>()))
                 .ReturnsAsync(order);
        
        emailService.Setup(e => e.SendConfirmationAsync(It.IsAny<string>()))
                   .Returns(Task.CompletedTask);
        
        var service = new OrderService(repository.Object, emailService.Object);
        
        // Act
        var result = await service.ProcessOrderAsync(order);
        
        // Assert
        Assert.NotNull(result);
        
        // Verification happens automatically in Dispose()
    }
    
    [Fact]
    public void PartialMock_ShouldWorkWithRealImplementation()
    {
        // Arrange
        var service = StrictPartial<OrderService>(repository.Object);
        service.Setup(s => s.ValidateOrder(It.IsAny<Order>()))
               .Returns(true);
        
        var order = FactoryGirl.Build<Order>();
        
        // Act
        var result = service.Object.ProcessOrder(order);
        
        // Assert
        Assert.True(result);
    }
}
```

### Database Container Support

The library provides utilities for database testing:

```csharp
public class ConnectionStringProvider : IConnectionStringProvider
{
    private static string connectionString = string.Empty;
    
    public static void Configure(string value) => connectionString = value;
    public string ConnectionString => connectionString;
}

public interface ISqlContainer
{
    string GetConnectionString();
    // Additional database operations...
}
```

**Usage Examples:**

```csharp
public class RepositoryTests : MockBase
{
    [Fact]
    public async Task CustomerRepository_ShouldWorkWithTestDatabase()
    {
        // Configure test database connection
        ConnectionStringProvider.Configure(
            "Server=localhost;Database=TestDb;Integrated Security=true;");
        
        var container = Create<ISqlContainer>();
        container.Setup(c => c.GetConnectionString())
                .Returns(ConnectionStringProvider.ConnectionString);
        
        var repository = new CustomerRepository(container.Object);
        var customer = FactoryGirl.Build<Customer>();
        
        // Test repository operations
        var savedCustomer = await repository.SaveAsync(customer);
        var retrievedCustomer = await repository.GetByIdAsync(savedCustomer.Id);
        
        Assert.Equal(savedCustomer.Id, retrievedCustomer.Id);
        Assert.Equal(savedCustomer.Name, retrievedCustomer.Name);
    }
}
```

## 📚 Usage Examples

### Complex Test Scenarios

```csharp
public class OrderProcessingTests : MockBase
{
    [Fact]
    public async Task ProcessOrder_ShouldHandleCompleteWorkflow()
    {
        // Arrange
        var inventoryService = Create<IInventoryService>();
        var paymentService = Create<IPaymentService>();
        var shippingService = Loose<IShippingService>();
        var notificationService = Loose<INotificationService>();
        
        var order = FactoryGirl.Build<Order>(o => 
        {
            o.Items = FactoryGirl.Build<OrderItem>(3, item => 
            {
                item.Quantity = FactoryGirl.UniqueId("quantity");
                item.Price = 29.99m;
            });
        });
        
        // Setup mocks
        inventoryService.Setup(i => i.ReserveItemsAsync(It.IsAny<List<OrderItem>>()))
                       .ReturnsAsync(true);
        
        paymentService.Setup(p => p.ProcessPaymentAsync(It.IsAny<PaymentRequest>()))
                     .ReturnsAsync(new PaymentResult { Success = true });
        
        shippingService.Setup(s => s.CreateShipmentAsync(It.IsAny<Order>()))
                      .ReturnsAsync(new Shipment { TrackingNumber = "TRK123" });
        
        var orderService = new OrderService(
            inventoryService.Object,
            paymentService.Object,
            shippingService.Object,
            notificationService.Object);
        
        // Act
        var result = await orderService.ProcessOrderAsync(order);
        
        // Assert
        Assert.True(result.Success);
        Assert.Equal("TRK123", result.TrackingNumber);
        
        // Verify all interactions
        inventoryService.Verify(i => i.ReserveItemsAsync(order.Items), Times.Once);
        paymentService.Verify(p => p.ProcessPaymentAsync(It.IsAny<PaymentRequest>()), Times.Once);
    }
}
```

### Testing with Multiple Factories

```csharp
public class MultiEntityTests
{
    [Fact]
    public void ComplexWorkflow_ShouldWorkWithMultipleEntities()
    {
        // Define multiple factories
        FactoryGirl.Define<Customer>(() => new Customer
        {
            Id = Guid.NewGuid(),
            Name = $"Customer {FactoryGirl.UniqueId()}",
            Email = $"customer{FactoryGirl.UniqueId()}@example.com"
        });
        
        FactoryGirl.Define<Product>(() => new Product
        {
            Id = Guid.NewGuid(),
            Name = $"Product {FactoryGirl.UniqueId()}",
            Price = 99.99m,
            Category = "Electronics"
        });
        
        FactoryGirl.Define<Order>(() => new Order
        {
            Id = Guid.NewGuid(),
            CustomerId = FactoryGirl.Build<Customer>().Id,
            OrderDate = DateTime.UtcNow,
            Status = OrderStatus.Pending
        });
        
        // Build related entities
        var customer = FactoryGirl.Build<Customer>(c => c.IsVip = true);
        var products = FactoryGirl.Build<Product>(5, p => p.Category = "Books");
        var order = FactoryGirl.Build<Order>(o => 
        {
            o.CustomerId = customer.Id;
            o.Items = products.Select(p => new OrderItem 
            { 
                ProductId = p.Id, 
                Quantity = 1, 
                Price = p.Price 
            }).ToList();
        });
        
        // Assert relationships
        Assert.Equal(customer.Id, order.CustomerId);
        Assert.Equal(5, order.Items.Count);
        Assert.All(order.Items, item => Assert.Equal("Books", 
            products.First(p => p.Id == item.ProductId).Category));
    }
}
```

### Integration Testing Support

```csharp
public class IntegrationTests : MockBase
{
    [Fact]
    public async Task EndToEndWorkflow_ShouldCompleteSuccessfully()
    {
        // Configure test database
        ConnectionStringProvider.Configure(GetTestConnectionString());
        
        // Setup real database container
        var container = Create<ISqlContainer>();
        container.Setup(c => c.GetConnectionString())
                .Returns(ConnectionStringProvider.ConnectionString);
        
        // Create services with real dependencies
        var repository = new CustomerRepository(container.Object);
        var emailService = Create<IEmailService>();
        
        emailService.Setup(e => e.SendWelcomeEmailAsync(It.IsAny<string>()))
                   .Returns(Task.CompletedTask);
        
        var customerService = new CustomerService(repository, emailService.Object);
        
        // Test complete workflow
        var customer = FactoryGirl.Build<Customer>(c => 
        {
            c.Email = "test@example.com";
            c.Name = "Test Customer";
        });
        
        var savedCustomer = await customerService.CreateCustomerAsync(customer);
        var retrievedCustomer = await customerService.GetCustomerAsync(savedCustomer.Id);
        
        // Assertions
        Assert.NotNull(savedCustomer);
        Assert.NotNull(retrievedCustomer);
        Assert.Equal(customer.Email, retrievedCustomer.Email);
        
        // Verify email was sent
        emailService.Verify(e => e.SendWelcomeEmailAsync(customer.Email), Times.Once);
    }
    
    private string GetTestConnectionString()
    {
        return "Server=localhost;Database=TestDb;Integrated Security=true;";
    }
}
```

## 🔧 Advanced Features

### Custom Factory Extensions

```csharp
public static class FactoryExtensions
{
    public static T BuildWithDefaults<T>(this ITestFactory factory) where T : class
    {
        return FactoryGirl.Build<T>();
    }
    
    public static List<T> BuildMany<T>(this ITestFactory factory, int count) where T : class
    {
        return FactoryGirl.Build<T>(count, _ => {});
    }
}

// Usage
var customers = FactoryGirl.Define<Customer>(() => new Customer())
    .BuildMany<Customer>(10);
```

### Test Data Builders

```csharp
public class CustomerBuilder
{
    private Customer _customer = FactoryGirl.Build<Customer>();
    
    public CustomerBuilder WithName(string name)
    {
        _customer.Name = name;
        return this;
    }
    
    public CustomerBuilder WithEmail(string email)
    {
        _customer.Email = email;
        return this;
    }
    
    public CustomerBuilder AsVip()
    {
        _customer.IsVip = true;
        return this;
    }
    
    public Customer Build() => _customer;
}

// Usage
var vipCustomer = new CustomerBuilder()
    .WithName("John Doe")
    .WithEmail("john@example.com")
    .AsVip()
    .Build();
```

## 🎯 Target Framework

- **.NET 8.0** and later

## 📋 Dependencies

- **Bounteous.Core** (0.0.14) - Core utilities and patterns
- **Bounteous.Data** (0.0.12) - Data access functionality
- **Moq** (4.20.72) - Mocking framework

## 🔗 Related Projects

- [Bounteous.Core](../Bounteous.Core/) - Core utilities and patterns
- [Bounteous.Data](../Bounteous.Data/) - Data access functionality
- [Bounteous.xUnit.Container.MsSql](../Bounteous.xUnit.Container.MsSql/) - SQL Server container support

## 🤝 Contributing

This library is maintained by Xerris Inc. For contributions, please contact the development team.

## 📄 License

See [LICENSE](LICENSE) file for details.

---

*This comprehensive testing library accelerates unit test development by providing factory patterns, mock management, and database testing utilities that integrate seamlessly with xUnit and modern .NET testing practices.*
