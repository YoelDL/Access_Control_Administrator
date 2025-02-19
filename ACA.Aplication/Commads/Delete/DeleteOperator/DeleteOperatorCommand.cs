using ACA.Application.Abstract;

namespace ACA.Application.Commands.Delete.DeleteOperator
{
    public record DeleteOperatorCommand(Guid Id) : ICommand;
}
