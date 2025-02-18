using System.Threading.Tasks;

namespace Bounteous.xUnit.Accelerator.Tests
{
    public class Client(IService service)
    {
        public async Task Go() => await service.Go();

        public async Task Go(Request request)
            => await service.Go(request);
    }
}