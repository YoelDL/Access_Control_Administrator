using ACA.Application.Abstract;
using ACA.Contracts.Workers;
using ACA.Domain.Entities.Workers;

namespace ACA.Application.Queries.Get.GetOperator
{
    public class GetOperatorByIdQueryHandler : IQueryHandler<GetOperatorByIdQuery, Operator>
    {
        private readonly IOperatorRepository _operatorRepository;

        public GetOperatorByIdQueryHandler(IOperatorRepository operatorRepository)
        {
            _operatorRepository = operatorRepository;
        }

        public Task<Operator> Handle(GetOperatorByIdQuery request, CancellationToken cancellationToken)
        {
            
            return Task.FromResult(_operatorRepository.GetOperatorById<Operator>(request.Id));
        }
    }
}
