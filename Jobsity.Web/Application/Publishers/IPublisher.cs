using System.Threading.Tasks;
using Jobsity.Events;

namespace Jobsity.Web.Application.Publishers
{
	public interface IPublisher
    {
        Task PublishAsync(Event @event);
    }
}
