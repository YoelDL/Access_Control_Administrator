using ACA.Application.Abstract;
using ACA.Contracts;
using ACA.Application.Contracts.Processes;
using ACA.Domain.Entities.Processes;
using ACA.Application.Commads.Create.CreateProcess;

namespace ACA.Application.Commands.Create.CreateProcess
{
    public class CreateProcessCommandHandler : ICommandHandler<CreateProcessCommand, Process>
    {
        private readonly IProcessRepository _processRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateProcessCommandHandler(
            IProcessRepository processRepository,
            IUnitOfWork unitOfWork)
        {
            _processRepository = processRepository;
            _unitOfWork = unitOfWork;
        }

        public Task<Process> Handle(CreateProcessCommand request, CancellationToken cancellationToken)
        {
            // Crear una nueva instancia de Process usando los datos del comando
            var processEntity = new Process(
                processName: request.ProcessName,
                productName: request.ProductName,
                id: Guid.NewGuid(),
                processes_operators: null,
                processes_supervisors: null
            );

            // Agregar la entidad al repositorio
            _processRepository.AddProcess(processEntity); // Asegúrate de que el método sea asincrónico

            // Guardar los cambios en la base de datos
            _unitOfWork.SaveChangesAsync(); 

            // Devolver la entidad creada
            return Task.FromResult(processEntity);
        }
    }
}
