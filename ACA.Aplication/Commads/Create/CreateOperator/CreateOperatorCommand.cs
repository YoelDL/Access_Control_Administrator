using ACA.Application.Abstract;
using ACA.Domain.Entities.Types;
using ACA.Domain.Entities.Workers;

namespace ACA.Application.Commands.Create.CreateOperator
{
    public class CreateOperatorCommand : ICommand<Operator>
    {
        public string OperatorName { get; set; }
        public string CI { get; set; }
        public SchoolLevel SchoolLevel { get; set; }
        public Guid? AsignedSupervisorId { get; set; }

        public CreateOperatorCommand(string operatorName, string ci, SchoolLevel schoolLevel, Guid? asignedSupervisorId)
        {
            OperatorName = operatorName;
            CI = ci;
            SchoolLevel = schoolLevel;
            AsignedSupervisorId = asignedSupervisorId;
        }
    }
}
