using ACA.Application.Abstract;
using ACA.Application.Contracts.Processes;
using ACA.Contracts;
using ACA.Domain.Entities.Processes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACA.Application.Commads.Delete.DeleteProcess
{
    public class DeleteProcessCommandHandler
         : ICommandHandler<DeleteProcessCommand>
    {
        private readonly IProcessRepository _processRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteProcessCommandHandler(
            IProcessRepository processRepository,
            IUnitOfWork unitOfWork)
        {
            _processRepository = processRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(DeleteProcessCommand request, CancellationToken cancellationToken)
        {
            // Buscar el proceso por su ID
            var processToDelete = _processRepository.GetProcessById<Process>(request.Id);

            // Si no se encuentra, no se realiza ninguna acción
            if (processToDelete is null)
                return;

            // Eliminar el proceso utilizando el repositorio
            _processRepository.DeleteProcess(processToDelete);

            // Guardar los cambios en la base de datos (sin CancellationToken)
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
