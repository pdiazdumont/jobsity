using System.Threading.Tasks;
using Jobsity.Events.Messages;

namespace Jobsity.Web.Application.Publishers
{
	public interface IPublisher
    {
        Task PublishAsync(MessagePostedEvent @event);
    }
}
