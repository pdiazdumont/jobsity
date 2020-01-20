using System.Threading.Tasks;

namespace Jobsity.Bots.Stock.Api.Services
{
	public interface IJobsityClient
	{
		Task NewPost(string text);
	}
}
