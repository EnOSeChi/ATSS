using ATSS.Domain.Common;
using System.Threading.Tasks;

namespace ATSS.Application.Common.Interfaces
{
    public interface IDomainEventService
    {
        Task Publish(DomainEvent domainEvent);
    }
}
