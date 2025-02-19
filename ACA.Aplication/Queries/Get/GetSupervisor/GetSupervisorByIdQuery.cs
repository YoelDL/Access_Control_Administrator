using ACA.Application.Abstract;
using ACA.Domain.Entities.Workers;

namespace ACA.Application.Queries.Get.GetSupervisor
{
    public record GetSupervisorByIdQuery(Guid Id) : IQuery<Supervisor?>;
}
