using System.Threading.Tasks;

namespace Jobsity.Bots.Stock.Api.Services
{
    public class FakeStooqClient : IStooqClient
    {
        public Task<decimal> GetStockValue(string name)
        {
            return Task.FromResult(93.42m);
        }
    }
}
