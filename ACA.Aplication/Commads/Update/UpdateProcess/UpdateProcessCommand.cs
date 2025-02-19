using ACA.Application.Abstract;
using ACA.Domain.Entities.Processes;

namespace ACA.Application.Commads.Update.UpdateProcess
{
    public record UpdateProcessCommand(Process Process) : ICommand;
}