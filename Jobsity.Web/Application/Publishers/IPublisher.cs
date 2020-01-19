using System.Threading.Tasks;
using Jobsity.Web.Application.Notifications;

namespace Jobsity.Web.Application.Publishers
{
    public interface IPublisher
    {
        Task PublishAsync(Notification notification);
    }
}
