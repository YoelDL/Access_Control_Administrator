using ACA.Application.Abstract;
using ACA.Application.Commands.Delete.DeleteOperator;
using ACA.Contracts.Workers;
using ACA.Contracts;
using ACA.Domain.Entities.Workers;

namespace ACA.Application.Delete.DeleteOperator
{
    public class DeleteOperatorCommandHandler
        : ICommandHandler<DeleteOperatorCommand>
    {
        private readonly IOperatorRepository _operatorRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteOperatorCommandHandler(
            IOperatorRepository operatorRepository,
            IUnitOfWork unitOfWork)
        {
            _operatorRepository = operatorRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(DeleteOperatorCommand request, CancellationToken cancellationToken)
        {
            // Buscar el operador por su ID
            var operatorToDelete = _operatorRepository.GetOperatorById<Operator>(request.Id);

            // Si no se encuentra, no se realiza ninguna acción
            if (operatorToDelete is null)
                return;

            // Eliminar el operador utilizando el repositorio
            _operatorRepository.DeleteOperator(operatorToDelete);

            // Guardar los cambios en la base de datos (sin CancellationToken)
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
