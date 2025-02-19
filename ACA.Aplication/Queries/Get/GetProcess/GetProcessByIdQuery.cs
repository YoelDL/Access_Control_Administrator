using ACA.Application.Abstract;
using ACA.Domain.Entities.Processes;

namespace ACA.Application.Queries.Get.GetProcess
{
    public record GetProcessByIdQuery(Guid Id) : IQuery<Process?>;
}
