using ACA.Application.Abstract;
using ACA.Application.Contracts.Processes;
using ACA.Domain.Entities.Processes;

namespace ACA.Application.Queries.Get.GetProcess
{
    public class GetProcessByIdQueryHandler : IQueryHandler<GetProcessByIdQuery, Process>
    {
        private readonly IProcessRepository _processRepository;

        public GetProcessByIdQueryHandler(IProcessRepository processRepository)
        {
            _processRepository = processRepository;
        }

        public Task<Process> Handle(GetProcessByIdQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_processRepository.GetProcessById<Process>(request.Id));
        }
    }
}
