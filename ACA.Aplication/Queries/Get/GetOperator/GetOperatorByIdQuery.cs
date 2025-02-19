using ACA.Application.Abstract;
using ACA.Domain.Entities.Workers;

namespace ACA.Application.Queries.Get.GetOperator
{
    public record GetOperatorByIdQuery(Guid Id) : IQuery<Operator?>;
}
