using ACA.Application.Abstract;
using ACA.Application.Commands.Create.CreateOperator;
using ACA.Contracts;
using ACA.Contracts.Workers;
using ACA.Domain.Entities.Workers;

namespace ACA.Application.Commands.CreateOperator
{
    public class CreateOperatorCommandHandler : ICommandHandler<CreateOperatorCommand, Operator>
    {
        private readonly IOperatorRepository _operatorRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateOperatorCommandHandler(
            IOperatorRepository operatorRepository,
            IUnitOfWork unitOfWork)
        {
            _operatorRepository = operatorRepository;
            _unitOfWork = unitOfWork;
        }

        public Task<Operator> Handle(CreateOperatorCommand request, CancellationToken cancellationToken)
        {
            // Crear una nueva instancia de Operator usando los datos del comando
            var operatorEntity = new Operator(
                operatorName: request.OperatorName,
                ci: request.CI,
                id: Guid.NewGuid(),
                School_Level: request.SchoolLevel,
                asgined_supervisor: null,
                processes_operators: null
            );

            // Agregar la entidad al repositorio
            _operatorRepository.AddOperator(operatorEntity);

            // Guardar los cambios en la base de datos
            _unitOfWork.SaveChangesAsync();

            // Devolver la entidad creada
            return Task.FromResult(operatorEntity);
        }
    }
}
