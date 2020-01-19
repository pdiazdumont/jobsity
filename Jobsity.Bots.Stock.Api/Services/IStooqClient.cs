using System.Threading.Tasks;

namespace Jobsity.Bots.Stock.Api.Services
{
    public interface IStooqClient
    {
        Task<decimal> GetStockValue(string name);
    }
}
