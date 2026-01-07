using System.Threading.Tasks;
using Bounteous.Core.Validations;
using Moq;
using Xunit;

namespace Bounteous.xUnit.Accelerator.Tests
{
    public class MockBaseTest : MockBase
    {
        [Fact]
        public async Task Go()
        {
            var service = Create<IService>(); //defaults to Strict mock
            var client = new Client(service.Object);
            service.Setup(x => x.Go()).Returns(Task.CompletedTask);
            await client.Go();
        }

        [Fact]
        public async Task GoStrict()
        {
            var service = Strict<IService>(); //defaults to Strict mock
            var client = new Client(service.Object);
            var request = new Request {Id = 1};
            service.Setup(x => x.Go(request)).Returns(Task.CompletedTask);
            await client.Go(request);
        }

        [Fact]
        public async Task GoLoose()
        {
            var service = Loose<IService>(); //defaults to Strict mock
            var client = new Client(service.Object);
            var request = new Request {Id = 1};
            service.Setup(x => x.Go(request)).Returns(Task.CompletedTask);
            await client.Go(request);
        }

        [Fact]
        public async Task PartialStrict()
        {
            var request = new Request();
            var service = StrictPartial<Service>();
            service.Setup(x => x.GetCustomer(request)).ReturnsAsync(new Customer());

            var customer = await service.Object.GetCustomer(request);
            Validate.Begin().IsNotNull(customer, "customer").Check();
        }

        [Fact]
        public async Task PartialLoose()
        {
            var request = new Request();
            var service = LoosePartial<Service>();
            service.Setup(x => x.GetCustomer(request)).ReturnsAsync(new Customer());

            var customer = await service.Object.GetCustomer(request);
            Validate.Begin().IsNotNull(customer, "customer").Check();
        }
    }
}