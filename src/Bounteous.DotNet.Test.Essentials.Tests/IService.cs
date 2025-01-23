using System;
using System.Threading.Tasks;

namespace Bounteous.DotNet.Test.Essentials.Tests
{
    public interface IService
    {
        Task Go();
        Task Go(Request request);
    }

    public class Service : IService
    {
        public Task Go()
            => Task.CompletedTask;

        public Task Go(Request request)
            => Task.CompletedTask;

        public virtual Task<Customer> GetCustomer(Request request)
            => throw new Exception("should be mocked in the Partial Mock Test");
    }
}