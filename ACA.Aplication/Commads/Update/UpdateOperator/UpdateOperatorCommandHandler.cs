using ACA.Application.Abstract;
using ACA.Contracts.Workers;
using ACA.Contracts;

namespace ACA.Application.Commads.Update.UpdateOperator
{
    public class UpdateOperatorCommandHandler : ICommandHandler<UpdateOperatorCommand>
    {
        private readonly IOperatorRepository _operatorRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateOperatorCommandHandler(
            IOperatorRepository operatorRepository,
            IUnitOfWork unitOfWork)
        {
            _operatorRepository = operatorRepository ?? throw new ArgumentNullException(nameof(operatorRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task Handle(UpdateOperatorCommand request, CancellationToken cancellationToken)
        {
            // Actualizar el operador utilizando el repositorio
            _operatorRepository.UpdateOperator(request.Operator);

            // Guardar los cambios en la base de datos
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
