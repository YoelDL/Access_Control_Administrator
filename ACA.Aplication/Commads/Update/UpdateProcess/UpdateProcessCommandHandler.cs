using ACA.Application.Abstract;
using ACA.Application.Contracts.Processes;
using ACA.Contracts;

namespace ACA.Application.Commads.Update.UpdateProcess
{
    public class UpdateProcessCommandHandler : ICommandHandler<UpdateProcessCommand>
    {
        private readonly IProcessRepository _processRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateProcessCommandHandler(
            IProcessRepository processRepository,
            IUnitOfWork unitOfWork)
        {
            _processRepository = processRepository;
            _unitOfWork = unitOfWork;
        }

        public Task Handle(UpdateProcessCommand request, CancellationToken cancellationToken)
        {
            _processRepository.UpdateProcess(request.Process);
            _unitOfWork.SaveChanges();
            return Task.CompletedTask;
        }
    }
}
