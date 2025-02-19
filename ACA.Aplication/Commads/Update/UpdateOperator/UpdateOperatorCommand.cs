using ACA.Domain.Entities.Workers;
using ACA.Application.Abstract;

namespace ACA.Application.Commads.Update.UpdateOperator
{
    public record UpdateOperatorCommand(Operator Operator) : ICommand;
}
