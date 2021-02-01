using RecruitmentTask.Domain.Common;
using System.Threading.Tasks;

namespace RecruitmentTask.Application.Common.Interfaces
{
    public interface IDomainEventService
    {
        Task Publish(DomainEvent domainEvent);
    }
}
