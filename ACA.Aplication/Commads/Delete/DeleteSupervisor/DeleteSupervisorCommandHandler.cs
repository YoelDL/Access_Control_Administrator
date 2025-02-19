using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACA.Application.Abstract;
using ACA.Contracts.Workers;
using ACA.Contracts;
using ACA.Application.Commads.Delete.DeleteSupervisor;

namespace ACA.Application.Commads.Delete.DeleteSupervisor
{
    public class DeleteSupervisorCommandHandler
        : ICommandHandler<DeleteSupervisorCommand>
    {
        private readonly ISupervisorRepository _supervisorRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteSupervisorCommandHandler(ISupervisorRepository supervisorRepository, IUnitOfWork unitOfWork)
        {
            _supervisorRepository = supervisorRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(DeleteSupervisorCommand request, CancellationToken cancellationToken)
        {
            // Buscar el supervisor por su ID
            var supervisorToDelete = _supervisorRepository.GetSupervisorById(request.Id);

            // Si no se encuentra, no se realiza ninguna acción
            if (supervisorToDelete is null)
                return;

            // Eliminar el supervisor
            _supervisorRepository.DeleteSupervisor(supervisorToDelete);

            // Guardar los cambios en la base de datos
            await _unitOfWork.SaveChangesAsync();
        }
    }

}
