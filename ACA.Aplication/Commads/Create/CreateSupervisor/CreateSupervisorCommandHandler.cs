using ACA.Application.Abstract;
using ACA.Contracts;
using ACA.Contracts.Workers;
using ACA.Domain.Entities.Workers;

namespace ACA.Application.Commands.Create.CreateSupervisor
{
    public class CreateSupervisorCommandHandler : ICommandHandler<CreateSupervisorCommand, Supervisor>
    {
        private readonly ISupervisorRepository _supervisorRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateSupervisorCommandHandler(
            ISupervisorRepository supervisorRepository,
            IUnitOfWork unitOfWork)
        {
            _supervisorRepository = supervisorRepository;
            _unitOfWork = unitOfWork;
        }

        public Task<Supervisor> Handle(CreateSupervisorCommand request, CancellationToken cancellationToken)
        {
            // Crear una nueva instancia de Supervisor usando los datos del comando
            var supervisorEntity = new Supervisor(
                supervisor_name: request.SupervisorName,
                ci: request.CI,
                school_level: request.SchoolLevel,
                id: Guid.NewGuid(),
                operators: null, // Inicializa según tu lógica
                processes_supervisors: null // Inicializa según tu lógica
            );

            // Agregar la entidad al repositorio
            _supervisorRepository.AddSupervisor(supervisorEntity); // Asegúrate de que el método sea asincrónico

            // Guardar los cambios en la base de datos
            _unitOfWork.SaveChangesAsync();

            // Devolver la entidad creada
            return Task.FromResult(supervisorEntity);
        }
    }
}
