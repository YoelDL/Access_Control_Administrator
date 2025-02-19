using ACA.Application.Abstract;
using ACA.Contracts.Workers;
using ACA.Domain.Entities.Workers;

namespace ACA.Application.Queries.Get.GetSupervisor
{
    public class GetSupervisorByIdQueryHandler : IQueryHandler<GetSupervisorByIdQuery, Supervisor>
    {
        
        private ISupervisorRepository _supervisorRepository;

        public GetSupervisorByIdQueryHandler(ISupervisorRepository supervisorRepository)
        {
            _supervisorRepository = supervisorRepository;
        }

        public Task<Supervisor> Handle(GetSupervisorByIdQuery request, CancellationToken cancellationToken)
        {

            return Task.FromResult(_supervisorRepository.GetSupervisorById(request.Id));
        }
    }
}
