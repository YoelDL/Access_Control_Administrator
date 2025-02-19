using ACA.Application.Abstract;
using ACA.Contracts.Workers;
using ACA.Contracts;

namespace ACA.Application.Commads.Update.UpdateSupervisor
{
    public class UpdateSupervisorCommandHandler : ICommandHandler<UpdateSupervisorCommand>
    {
        private readonly ISupervisorRepository _supervisorRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateSupervisorCommandHandler(
            ISupervisorRepository supervisorRepository,
            IUnitOfWork unitOfWork)
        {
            _supervisorRepository = supervisorRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(UpdateSupervisorCommand request, CancellationToken cancellationToken)
        {
            // Actualizar el supervisor utilizando el repositorio
            _supervisorRepository.UpdateSupervisor(request.Supervisor);

            // Guardar los cambios en la base de datos
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
