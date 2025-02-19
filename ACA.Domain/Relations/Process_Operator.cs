using ACA.Domain.Common;
using ACA.Domain.Entities.Workers;
using ACA.Domain.Entities.Processes;

namespace ACA.Domain.Relations
{
    public class Process_Operator : Entity
    {
        public Guid OperatorId { get; set; }

        public Operator Operador { get; set; }

        public Guid ProcessId { get; set; }

        public Process Process { get; set; }
    }
}

