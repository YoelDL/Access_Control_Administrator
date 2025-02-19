using ACA.Domain.Entities.Workers;
using ACA.Application.Abstract;

namespace ACA.Application.Commads.Update.UpdateSupervisor
{
    public record UpdateSupervisorCommand(Supervisor Supervisor) : ICommand;
}
