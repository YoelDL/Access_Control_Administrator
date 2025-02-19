using ACA.Application.Abstract;
using ACA.Domain.Entities.Types;
using ACA.Domain.Entities.Workers;

namespace ACA.Application.Commands.Create.CreateSupervisor
{
    public class CreateSupervisorCommand : ICommand<Supervisor>
    {
        public string SupervisorName { get; set; }
        public string CI { get; set; }
        public SchoolLevel SchoolLevel { get; set; }

        public CreateSupervisorCommand(string supervisorName, string ci, SchoolLevel schoolLevel)
        {
            SupervisorName = supervisorName;
            CI = ci;
            SchoolLevel = schoolLevel;
        }
    }
}